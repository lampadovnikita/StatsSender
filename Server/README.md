# Server

## Description
The server part is written in the golang language and, in general terms, contains the following modules:
server interaction and configuration, interaction with PostgreSQL database and configuration, work with the JWT access token, interaction with user information, interaction with user character information. 
  
In turn, user data contains information necessary for identification, as well as information about the user's character.
Character information contains name, characteristics, level and experience.

## Database SQL code
``` sql
CREATE TABLE IF NOT EXISTS users (
    id INT PRIMARY KEY GENERATED ALWAYS AS IDENTITY,
    login TEXT UNIQUE NOT NULL,
    encrypted_password TEXT NOT NULL

    CONSTRAINT login_positive_size CHECK (char_length(login) > 0)
    CONSTRAINT encrypted_password_positive_size CHECK (char_length(encrypted_password) > 0)
);
```

```sql
CREATE TABLE IF NOT EXISTS characters (
    id INT PRIMARY KEY GENERATED ALWAYS AS IDENTITY,
    user_id INT UNIQUE NOT NULL,
    name TEXT NOT NULL,
    total_exp INT NOT NULL,
    strength INT NOT NULL,
    wisdom INT NOT NULL,
    agility INT NOT NULL,
    CONSTRAINT fk_user FOREIGN KEY(user_id) REFERENCES users ON DELETE CASCADE,

    CONSTRAINT name_positive_size CHECK (char_length(name) > 0)
);
```

## Used Github libraries
* [logrus](https://github.com/sirupsen/logrus)
* [pgx](https://github.com/jackc/pgx)
* [gin](https://github.com/gin-gonic/gin)
* [jwt](https://github.com/golang-jwt/jwt)
