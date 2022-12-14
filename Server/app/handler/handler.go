package handler

import (
	"errors"
	"fmt"
	"github.com/gin-gonic/gin"
	"github.com/jackc/pgx/v5"
	"github.com/sirupsen/logrus"
	"net/http"
	"stats-sender/character"
	"stats-sender/character/isCharacterCreated"
	"stats-sender/character/levelprogress"
	"stats-sender/character/stats"
	"stats-sender/jwt"
	"stats-sender/levelbounds"
	"stats-sender/preload"
	"stats-sender/user"
	"time"
)

const userIDKey = "user_id"

var incorrectLoginOrPasswordError = errors.New("incorrect login or password")
var authError = errors.New("authentication error")
var userIDNotFoundError = errors.New("can't found user ID")
var cantConvertUserIDError = errors.New("can't convert user ID")

type TokenResponse struct {
	BearerToken string `json:"bearer_token"`
}

type Handler struct {
	UserStorage      user.Storage
	CharacterStorage character.Storage
	LevelBounds      levelbounds.Model
	Logger           *logrus.Logger
}

func (h *Handler) InitRoutes() *gin.Engine {
	engine := gin.Default()

	engine.GET("ping", h.Ping)

	userGroup := engine.Group("user")
	{
		userGroup.POST("authenticate", h.AuthenticateUser)
		userGroup.POST("register", h.RegisterUser)
	}

	verified := engine.Group("", h.Verify)
	{
		verified.GET("preload", h.SendPreloadData)

		verified.GET("level/bounds", h.SendCharacterLevelBounds)

		characterGroup := verified.Group("character")
		{
			characterGroup.GET("", h.SendCharacterData)
			characterGroup.POST("create", h.CreateCharacter)
			characterGroup.GET("isCreated", h.SendIsCharacterCreated)
		}
	}

	return engine
}

func (h *Handler) Verify(c *gin.Context) {
	fmt.Println("Verification")

	id, err := jwt.ParseAuthHeader(c)
	if err != nil {
		h.errorResponse(c, http.StatusUnauthorized, authError)
		return
	}

	c.Set(userIDKey, id)
}

func (h *Handler) Ping(c *gin.Context) {
	c.IndentedJSON(http.StatusOK, "The server is running")
}

func (h *Handler) SendPreloadData(c *gin.Context) {
	userID, err := h.getUserIDFromContext(c)
	if err != nil {
		h.errorResponse(c, http.StatusInternalServerError, err)
		return
	}

	var isCharCreated = true
	rec, err := h.CharacterStorage.FindByUserID(userID)
	if err != nil {
		if err == pgx.ErrNoRows {
			isCharCreated = false
		} else {
			h.errorResponse(c, http.StatusInternalServerError, err)
			return
		}
	}

	var characterModel = character.Model{}
	if isCharCreated == true {
		characterModel.Name = rec.Name
		characterModel.Stats = stats.Model{Strength: rec.Strength, Wisdom: rec.Wisdom, Agility: rec.Agility}
		characterModel.LevelProgress = levelprogress.Model{TotalExp: rec.TotalExp}
	}

	preloadData := preload.Model{
		LevelBounds:        h.LevelBounds,
		IsCharacterCreated: isCharCreated,
		Character:          characterModel,
	}

	c.IndentedJSON(http.StatusOK, preloadData)
}

func (h *Handler) SendCharacterLevelBounds(c *gin.Context) {
	c.IndentedJSON(http.StatusOK, h.LevelBounds)
}

func (h *Handler) SendCharacterData(c *gin.Context) {
	userID, err := h.getUserIDFromContext(c)
	if err != nil {
		h.errorResponse(c, http.StatusInternalServerError, err)
		return
	}

	rec, err := h.CharacterStorage.FindByUserID(userID)
	if err != nil {
		if err == pgx.ErrNoRows {
			c.Status(http.StatusNoContent)
		} else {
			h.errorResponse(c, http.StatusBadRequest, err)
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

func (h *Handler) CreateCharacter(c *gin.Context) {
	charModel := character.Model{}
	err := c.BindJSON(&charModel)
	if err != nil {
		h.errorResponse(c, http.StatusBadRequest, err)
		return
	}

	userID, err := h.getUserIDFromContext(c)
	if err != nil {
		h.errorResponse(c, http.StatusInternalServerError, err)
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
	err = h.CharacterStorage.Insert(rec)
	if err != nil {
		h.errorResponse(c, http.StatusUnprocessableEntity, err)
		return
	}

	c.Status(http.StatusCreated)
}

func (h *Handler) SendIsCharacterCreated(c *gin.Context) {
	userID, err := h.getUserIDFromContext(c)
	if err != nil {
		h.errorResponse(c, http.StatusInternalServerError, err)
		return
	}

	isCreated := true

	_, err = h.CharacterStorage.FindByUserID(userID)
	if err != nil {
		if err == pgx.ErrNoRows {
			isCreated = false
		} else {
			h.errorResponse(c, http.StatusUnprocessableEntity, err)
			return
		}
	}

	isCreatedModel := isCharacterCreated.Model{IsCreated: isCreated}
	c.IndentedJSON(http.StatusOK, isCreatedModel)
}

func (h *Handler) RegisterUser(c *gin.Context) {
	u := user.Model{}
	err := c.BindJSON(&u)
	if err != nil {
		h.errorResponse(c, http.StatusBadRequest, err)
		return
	}

	encPass, err := u.EncryptPassword()
	rec := &user.Record{Login: u.Login, EncryptedPassword: encPass}
	err = h.UserStorage.Insert(rec)
	if err != nil {
		h.errorResponse(c, http.StatusUnprocessableEntity, err)
		return
	}

	c.Status(http.StatusCreated)
}

func (h *Handler) AuthenticateUser(c *gin.Context) {
	u := user.Model{}
	err := c.BindJSON(&u)
	if err != nil {
		h.errorResponse(c, http.StatusBadRequest, err)
		return
	}

	rec, err := h.UserStorage.FindByLogin(u.Login)
	if err != nil {
		if err == pgx.ErrNoRows {
			h.errorResponse(c, http.StatusUnauthorized, incorrectLoginOrPasswordError)
		} else {
			h.errorResponse(c, http.StatusUnprocessableEntity, err)
		}
		return
	}

	if rec.ComparePassword(u.Password) != true {
		h.errorResponse(c, http.StatusUnauthorized, err)
		return
	}

	bearerToken, err := jwt.CreateToken(rec.ID, time.Hour*12)
	if err != nil {
		h.errorResponse(c, http.StatusInternalServerError, err)
		return
	}

	c.IndentedJSON(http.StatusOK, TokenResponse{BearerToken: bearerToken})
}

func (h *Handler) getUserIDFromContext(c *gin.Context) (int, error) {
	userIDVal, exists := c.Get(userIDKey)
	if exists == false {
		return 0, userIDNotFoundError
	}

	userID, ok := userIDVal.(int)
	if ok == false {
		return 0, cantConvertUserIDError
	}

	return userID, nil
}

func (h *Handler) errorResponse(c *gin.Context, statusCode int, err error) {
	h.Logger.Error(err)
	c.AbortWithStatusJSON(statusCode, gin.H{"error": err.Error()})
}
