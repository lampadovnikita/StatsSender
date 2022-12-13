package main

import (
	"golang.org/x/net/context"
	"stats-sender/character"
	"stats-sender/handler"
	"stats-sender/levelbounds"
	"stats-sender/postgresql"
	"stats-sender/server"
	"stats-sender/user"
)

const characterLevelCap int = 5

func main() {
	pgCfg, err := postgresql.GetConfig()
	if err != nil {
		panic(err)
	}

	pool, err := postgresql.NewPool(context.Background(), pgCfg)
	if err != nil {
		panic(err)
	}

	serverConfig, err := server.GetConfig()
	if err != nil {
		panic(err)
	}

	userStorage := &user.PGStorage{PGPool: pool}
	characterStorage := &character.PGStorage{PGPool: pool}
	lb := levelbounds.Model{Bounds: levelbounds.CalculateExpBounds(characterLevelCap)}

	h := handler.Handler{
		UserStorage:      userStorage,
		CharacterStorage: characterStorage,
		LevelBounds:      lb,
	}

	serv := server.Server{
		Cfg:     serverConfig,
		Handler: h,
	}

	serv.Run()
}
