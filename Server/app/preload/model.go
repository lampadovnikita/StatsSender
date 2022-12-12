package preload

import (
	"stats-sender/character"
	"stats-sender/levelbounds"
)

type Model struct {
	LevelBounds        levelbounds.Model `json:"level_bounds"`
	IsCharacterCreated bool              `json:"is_character_created"`
	Character          character.Model   `json:"character"`
}
