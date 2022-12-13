package server

import (
	"fmt"
	"stats-sender/handler"
)

const serverAddrFormat = "%s:%s"

type Server struct {
	Cfg     Config
	Handler handler.Handler
}

func (s *Server) Run() {
	engine := s.Handler.InitRoutes()

	serverAddr := fmt.Sprintf(serverAddrFormat, s.Cfg.IP, s.Cfg.Port)
	err := engine.Run(serverAddr)
	if err != nil {
		fmt.Printf("Error on starting server: %s", err.Error())
	}
}
