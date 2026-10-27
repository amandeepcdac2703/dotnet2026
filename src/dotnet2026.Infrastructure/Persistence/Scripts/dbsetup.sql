
CREATE DATABASE IF NOT EXISTS DotNet2026Db;

USE DotNet2026Db;

CREATE TABLE IF NOT EXISTS Products
(
    Id BIGINT NOT NULL AUTO_INCREMENT,

    Name VARCHAR(150) NOT NULL,

    Description VARCHAR(1000) NULL,

    Price DECIMAL(18, 2) NOT NULL,

    Stock INT NOT NULL,

    CreatedAtUtc DATETIME(6) NOT NULL,

    UpdatedAtUtc DATETIME(6) NULL,

    CONSTRAINT PK_Products
        PRIMARY KEY (Id),

    CONSTRAINT CK_Products_Price
        CHECK (Price > 0),

    CONSTRAINT CK_Products_Stock
        CHECK (Stock >= 0)
);
