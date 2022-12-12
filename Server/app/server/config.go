package server

import (
	"gopkg.in/yaml.v3"
	"os"
	"sync"
)

type Config struct {
	IP   string `yaml:"ip"`
	Port string `yaml:"port"`
}

var instance Config
var once sync.Once

func GetConfig() (Config, error) {
	var err error

	once.Do(func() {
		err = fillFromConfigFile()
	})
	if err != nil {
		return Config{}, err
	}

	return instance, nil
}

func fillFromConfigFile() error {
	instance = Config{}

	yamlFile, err := os.ReadFile("../config/server_config.yaml")
	if err != nil {
		return err
	}

	err = yaml.Unmarshal(yamlFile, &instance)

	return err
}
