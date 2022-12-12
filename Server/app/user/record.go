package user

import "golang.org/x/crypto/bcrypt"

type Record struct {
	ID                int
	Login             string
	EncryptedPassword string
}

func (r *Record) ComparePassword(password string) bool {
	return bcrypt.CompareHashAndPassword([]byte(r.EncryptedPassword), []byte(password)) == nil
}
