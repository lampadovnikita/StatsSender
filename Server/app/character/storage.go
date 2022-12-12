package character

type Storage interface {
	FindByUserID(userID int) (rec *Record, err error)
	Insert(rec *Record) (err error)
}
