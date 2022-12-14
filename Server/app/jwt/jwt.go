package jwt

import (
	"errors"
	"fmt"
	"github.com/gin-gonic/gin"
	"github.com/golang-jwt/jwt/v4"
	"strconv"
	"strings"
	"time"
)

const cryptoKey = "CRYPTO_KEY"

const signingMethodErrorFormat = "unexpected signing method: %v"

var emptyHeaderError = errors.New("the header is empty")
var invalidAuthHeaderError = errors.New("invalid auth header")

func CreateToken(userID int, ttl time.Duration) (string, error) {
	rc := jwt.RegisteredClaims{
		Subject:   strconv.Itoa(userID),
		ExpiresAt: &jwt.NumericDate{Time: time.Now().Add(ttl)},
	}

	token := jwt.NewWithClaims(jwt.SigningMethodHS256, rc)

	return token.SignedString([]byte(cryptoKey))
}

func ParseAuthHeader(c *gin.Context) (int, error) {
	authHeader := c.GetHeader("Authorization")

	headerParts := strings.Split(authHeader, " ")
	if len(headerParts) != 2 || headerParts[0] != "Bearer" {
		return 0, invalidAuthHeaderError
	}

	if len(headerParts[1]) == 0 {
		return 0, emptyHeaderError
	}

	return ParseBearerToken(headerParts[1])
}

func ParseBearerToken(accessToken string) (int, error) {
	claims := jwt.MapClaims{}
	_, err := jwt.ParseWithClaims(accessToken, claims, func(token *jwt.Token) (i interface{}, err error) {
		_, ok := token.Method.(*jwt.SigningMethodHMAC)
		if ok != true {
			return nil, fmt.Errorf(signingMethodErrorFormat, token.Header["alg"])
		}

		return []byte(cryptoKey), nil
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
