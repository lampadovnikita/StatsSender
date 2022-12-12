package character

import (
	"github.com/jackc/pgx/v5/pgxpool"
	"golang.org/x/net/context"
)

type PGStorage struct {
	PGPool *pgxpool.Pool
}

func (s *PGStorage) FindByUserID(userID int) (rec *Record, err error) {
	rec = &Record{}

	q := `SELECT id, user_id, name, total_exp, strength, wisdom, agility
			FROM characters
			WHERE user_id = $1`

	err = s.PGPool.
		QueryRow(context.Background(), q, userID).
		Scan(&rec.ID, &rec.UserID, &rec.Name, &rec.TotalExp, &rec.Strength, &rec.Wisdom, &rec.Agility)
	if err != nil {
		return nil, err
	}

	return rec, err
}

func (s *PGStorage) Insert(rec *Record) (err error) {
	q := `INSERT INTO characters (user_id, name, total_exp, strength, wisdom, agility)
		  		VALUES ($1, $2, $3, $4, $5, $6)
				RETURNING id`

	err = s.PGPool.
		QueryRow(context.Background(), q, rec.UserID, rec.Name, rec.TotalExp, rec.Strength, rec.Wisdom, rec.Agility).
		Scan(&rec.ID)
	if err != nil {
		return err
	}

	return nil
}
