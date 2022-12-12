package user

type Storage interface {
	FindByID(id int) (rec *Record, err error)
	FindByLogin(login string) (rec *Record, err error)
	Insert(rec *Record) (err error)
}
