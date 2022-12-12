package user

import (
	"github.com/jackc/pgx/v5/pgxpool"
	"golang.org/x/net/context"
)

type PGStorage struct {
	PGPool *pgxpool.Pool
}

func (s *PGStorage) FindByID(id int) (rec *Record, err error) {
	rec = &Record{}

	q := `SELECT id, login, encrypted_password
			FROM users
			WHERE id = $1`
	err = s.PGPool.QueryRow(context.Background(), q, id).Scan(&rec.ID, &rec.Login, &rec.EncryptedPassword)
	if err != nil {
		return nil, err
	}

	return rec, err
}

func (s *PGStorage) FindByLogin(login string) (rec *Record, err error) {
	rec = &Record{}

	q := `SELECT id, login, encrypted_password
				FROM users
			  	WHERE login = $1`

	err = s.PGPool.QueryRow(context.Background(), q, login).Scan(&rec.ID, &rec.Login, &rec.EncryptedPassword)
	if err != nil {
		return nil, err
	}

	return rec, err
}

func (s *PGStorage) Insert(rec *Record) (err error) {
	q := `INSERT INTO users (login, encrypted_password)
		  		VALUES ($1, $2)
				RETURNING id`

	err = s.PGPool.QueryRow(context.Background(), q, rec.Login, rec.EncryptedPassword).Scan(&rec.ID)
	if err != nil {
		return err
	}

	return nil
}
