package postgresql

import (
	"context"
	"fmt"
	"github.com/jackc/pgx/v5/pgxpool"
)

const dsnFormat string = "postgresql://%s:%s@%s:%s/%s"

func NewPool(ctx context.Context, config PGConfig) (pool *pgxpool.Pool, err error) {
	dsn := fmt.Sprintf(dsnFormat, config.Username, config.Password, config.Host, config.Port, config.Database)

	pgxCfg, err := pgxpool.ParseConfig(dsn)
	if err != nil {
		return nil, err
	}

	pool, err = pgxpool.NewWithConfig(ctx, pgxCfg)
	if err != nil {
		return nil, err
	}

	return pool, err
}
