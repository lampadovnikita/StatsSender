package postgresql

import (
	"gopkg.in/yaml.v3"
	"os"
	"sync"
)

type PGConfig struct {
	Username string `yaml:"username"`
	Password string `yaml:"password"`
	Host     string `yaml:"host"`
	Port     string `yaml:"port"`
	Database string `yaml:"database"`
}

var instance PGConfig
var once sync.Once

func GetConfig() (PGConfig, error) {
	var err error
	once.Do(func() {
		err = fillFromConfigFile()
	})
	if err != nil {
		return PGConfig{}, err
	}

	return instance, nil
}

func fillFromConfigFile() error {
	instance = PGConfig{}

	yamlFile, err := os.ReadFile("../config/pgsql_config.yaml")
	if err != nil {
		return err
	}

	err = yaml.Unmarshal(yamlFile, &instance)

	return err
}
