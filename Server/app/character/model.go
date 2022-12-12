package character

import (
	"stats-sender/character/levelprogress"
	"stats-sender/character/stats"
)

type Model struct {
	Name          string              `json:"name"`
	LevelProgress levelprogress.Model `json:"level_progress"`
	Stats         stats.Model         `json:"stats"`
}
