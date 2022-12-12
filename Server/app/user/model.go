package user

import "golang.org/x/crypto/bcrypt"

type Model struct {
	Login    string `json:"login"`
	Password string `json:"password"`
}

func (m *Model) EncryptPassword() (string, error) {
	b, err := bcrypt.GenerateFromPassword([]byte(m.Password), bcrypt.MinCost)
	if err != nil {
		return "", err
	}

	return string(b), nil
}
