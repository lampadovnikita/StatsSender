package main

import (
	"errors"
	"fmt"
	"github.com/gin-gonic/gin"
	"github.com/golang-jwt/jwt/v4"
	"github.com/jackc/pgx/v5"
	"golang.org/x/net/context"
	"net/http"
	"stats-sender/character"
	"stats-sender/character/isCharacterCreated"
	"stats-sender/character/levelprogress"
	"stats-sender/character/stats"
	"stats-sender/levelbounds"
	"stats-sender/postgresql"
	"stats-sender/preload"
	"stats-sender/server"
	"stats-sender/user"
	"strconv"
	"strings"
	"time"
)

const characterLevelCap int = 5

var lvlBounds levelbounds.Model

var userStorage user.Storage
var characterStorage character.Storage

var incorrectLoginOrPassword = errors.New("incorrect login or password")
var authError = errors.New("authentication error")

type TokenResponse struct {
	BearerToken string `json:"bearer_token"`
}

func main() {
	pgCfg, err := postgresql.GetConfig()
	if err != nil {
		panic(err)
	}

	pool, err := postgresql.NewPool(context.Background(), pgCfg)
	if err != nil {
		panic(err)
	}

	userStorage = &user.PGStorage{PGPool: pool}
	characterStorage = &character.PGStorage{PGPool: pool}

	lvlBounds = levelbounds.Model{Bounds: levelbounds.CalculateExpBounds(characterLevelCap)}

	serverConfig, err := server.GetConfig()
	if err != nil {
		panic(err)
	}

	serverAddr := fmt.Sprintf("%s:%s", serverConfig.IP, serverConfig.Port)
	startServer(serverAddr)
}

func startServer(serverAddr string) {
	engine := gin.Default()

	engine.GET("ping", Ping)

	engine.POST("user/authenticate", AuthenticateUser)
	engine.POST("user/register", RegisterUser)

	verified := engine.Group("", Verify)
	{
		verified.GET("preload", SendPreloadData)

		verified.GET("level/bounds", SendCharacterLevelBounds)

		verified.POST("character/create", CreateCharacter)
		verified.GET("character", SendCharacterData)
		verified.GET("character/isCreated", SendIsCharacterCreated)
	}

	err := engine.Run(serverAddr)
	if err != nil {
		fmt.Printf("Error on starting server: %s", err.Error())
	}
}

func Verify(c *gin.Context) {
	fmt.Println("Verification")

	id, err := ParseAuthHeader(c)
	if err != nil {
		c.AbortWithStatusJSON(http.StatusUnauthorized, gin.H{"error": err.Error()})
		return
	}

	c.Set("user_id", id)
}

func Ping(c *gin.Context) {
	c.IndentedJSON(http.StatusOK, "The server is running")
}

func SendPreloadData(c *gin.Context) {
	userID, err := getUserIDFromContext(c)
	if err != nil {
		c.IndentedJSON(http.StatusInternalServerError, gin.H{"error": err.Error()})
		return
	}

	var isCharacterCreated = true
	rec, err := characterStorage.FindByUserID(userID)
	if err != nil {
		if err == pgx.ErrNoRows {
			isCharacterCreated = false
		} else {
			c.IndentedJSON(http.StatusInternalServerError, gin.H{"error": err.Error()})
			return
		}
	}

	var characterModel = character.Model{}
	if isCharacterCreated == true {
		characterModel.Name = rec.Name
		characterModel.Stats = stats.Model{Strength: rec.Strength, Wisdom: rec.Wisdom, Agility: rec.Agility}
		characterModel.LevelProgress = levelprogress.Model{TotalExp: rec.TotalExp}
	}

	preloadData := preload.Model{
		LevelBounds:        lvlBounds,
		IsCharacterCreated: isCharacterCreated,
		Character:          characterModel,
	}

	c.IndentedJSON(http.StatusOK, preloadData)
}

func SendCharacterLevelBounds(c *gin.Context) {
	c.IndentedJSON(http.StatusOK, lvlBounds)
}

func SendCharacterData(c *gin.Context) {
	userID, err := getUserIDFromContext(c)
	if err != nil {
		c.IndentedJSON(http.StatusInternalServerError, gin.H{"error": err.Error()})
		return
	}

	rec, err := characterStorage.FindByUserID(userID)
	if err != nil {
		if err == pgx.ErrNoRows {
			c.Status(http.StatusNoContent)
		} else {
			c.IndentedJSON(http.StatusBadRequest, gin.H{"error": err.Error()})
		}

		return
	}

	characterModel := character.Model{
		Name:          rec.Name,
		LevelProgress: levelprogress.Model{TotalExp: rec.TotalExp},
		Stats:         stats.Model{Strength: rec.Strength, Wisdom: rec.Wisdom, Agility: rec.Agility},
	}

	c.IndentedJSON(http.StatusOK, characterModel)
}

func CreateCharacter(c *gin.Context) {
	charModel := character.Model{}
	err := c.BindJSON(&charModel)
	if err != nil {
		c.IndentedJSON(http.StatusBadRequest, gin.H{"error": err.Error()})
		return
	}

	userID, err := getUserIDFromContext(c)
	if err != nil {
		c.IndentedJSON(http.StatusInternalServerError, gin.H{"error": err.Error()})
		return
	}

	rec := &character.Record{
		UserID:   userID,
		Name:     charModel.Name,
		TotalExp: 0,
		Strength: charModel.Stats.Strength,
		Wisdom:   charModel.Stats.Wisdom,
		Agility:  charModel.Stats.Agility,
	}
	err = characterStorage.Insert(rec)
	if err != nil {
		c.IndentedJSON(http.StatusUnprocessableEntity, gin.H{"error": err.Error()})
		return
	}

	c.Status(http.StatusCreated)
}

func SendIsCharacterCreated(c *gin.Context) {
	userID, err := getUserIDFromContext(c)
	if err != nil {
		c.IndentedJSON(http.StatusInternalServerError, gin.H{"error": err.Error()})
		return
	}

	isCreated := true

	_, err = characterStorage.FindByUserID(userID)
	if err != nil {
		if err == pgx.ErrNoRows {
			isCreated = false
		} else {
			c.IndentedJSON(http.StatusUnprocessableEntity, gin.H{"error": err.Error()})
			return
		}
	}

	isCreatedModel := isCharacterCreated.Model{IsCreated: isCreated}
	c.IndentedJSON(http.StatusOK, isCreatedModel)
}

func RegisterUser(c *gin.Context) {
	u := user.Model{}
	err := c.BindJSON(&u)
	if err != nil {
		c.IndentedJSON(http.StatusBadRequest, gin.H{"error": err.Error()})
		return
	}

	encPass, err := u.EncryptPassword()
	rec := &user.Record{Login: u.Login, EncryptedPassword: encPass}
	err = userStorage.Insert(rec)
	if err != nil {
		c.IndentedJSON(http.StatusUnprocessableEntity, gin.H{"error": err.Error()})
		return
	}

	c.Status(http.StatusCreated)
}

func AuthenticateUser(c *gin.Context) {
	u := user.Model{}
	err := c.BindJSON(&u)
	if err != nil {
		c.IndentedJSON(http.StatusBadRequest, gin.H{"error": err.Error()})
		return
	}

	rec, err := userStorage.FindByLogin(u.Login)
	if err != nil {
		if err == pgx.ErrNoRows {
			c.IndentedJSON(http.StatusUnauthorized, gin.H{"error": incorrectLoginOrPassword.Error()})
		} else {
			c.IndentedJSON(http.StatusUnprocessableEntity, gin.H{"error": err.Error()})
		}
		return
	}

	if rec.ComparePassword(u.Password) != true {
		c.IndentedJSON(http.StatusUnauthorized, gin.H{"error": incorrectLoginOrPassword.Error()})
		return
	}

	bearerToken, err := CreateToken(rec.ID, time.Hour*12)
	if err != nil {
		c.IndentedJSON(http.StatusInternalServerError, gin.H{"error": err.Error()})
		return
	}

	c.IndentedJSON(http.StatusOK, TokenResponse{BearerToken: bearerToken})
}

func CreateToken(userID int, ttl time.Duration) (string, error) {
	rc := jwt.RegisteredClaims{
		Subject:   strconv.Itoa(userID),
		ExpiresAt: &jwt.NumericDate{Time: time.Now().Add(ttl)},
	}

	token := jwt.NewWithClaims(jwt.SigningMethodHS256, rc)

	return token.SignedString([]byte("CRYPTO_KEY"))
}

func ParseAuthHeader(c *gin.Context) (int, error) {
	authHeader := c.GetHeader("Authorization")

	headerParts := strings.Split(authHeader, " ")
	if len(headerParts) != 2 || headerParts[0] != "Bearer" {
		return 0, errors.New("invalid auth header")
	}

	if len(headerParts[1]) == 0 {
		return 0, errors.New("the header is empty")
	}

	return ParseBearerToken(headerParts[1])
}

func ParseBearerToken(accessToken string) (int, error) {
	claims := jwt.MapClaims{}
	_, err := jwt.ParseWithClaims(accessToken, claims, func(token *jwt.Token) (i interface{}, err error) {
		_, ok := token.Method.(*jwt.SigningMethodHMAC)
		if ok != true {
			return nil, fmt.Errorf("unexpected signing method: %v", token.Header["alg"])
		}

		return []byte("CRYPTO_KEY"), nil
	})
	if err != nil {
		return 0, err
	}

	id, err := strconv.Atoi(claims["sub"].(string))
	if err != nil {
		return 0, err
	}

	return id, nil
}

func getUserIDFromContext(c *gin.Context) (int, error) {
	userIDVal, exists := c.Get("user_id")
	if exists == false {
		return 0, errors.New("can't found user ID")
	}

	userID, ok := userIDVal.(int)
	if ok == false {
		return 0, errors.New("can't convert user ID")
	}

	return userID, nil
}
