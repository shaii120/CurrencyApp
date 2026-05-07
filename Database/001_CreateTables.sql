IF DB_ID('CurrencyDB') IS NULL
BEGIN
    CREATE DATABASE CurrencyDB;
END
GO

USE CurrencyDB;
GO

IF OBJECT_ID('dbo.CurrencyPair', 'U') IS NOT NULL DROP TABLE dbo.CurrencyPair;
IF OBJECT_ID('dbo.Currency', 'U') IS NOT NULL DROP TABLE dbo.Currency;

CREATE TABLE dbo.Currency (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Country NVARCHAR(100) NOT NULL,
    Name NVARCHAR(100) NOT NULL,
    Code NVARCHAR(10) NOT NULL UNIQUE
);

CREATE TABLE dbo.CurrencyPair (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    BaseCurrencyId INT NOT NULL,
    QuoteCurrencyId INT NOT NULL,
    MinValue DECIMAL(18,6) NOT NULL,
    MaxValue DECIMAL(18,6) NOT NULL,

    CONSTRAINT FK_CurrencyPair_Base FOREIGN KEY (BaseCurrencyId)
        REFERENCES dbo.Currency(Id),

    CONSTRAINT FK_CurrencyPair_Quote FOREIGN KEY (QuoteCurrencyId)
        REFERENCES dbo.Currency(Id),

    CONSTRAINT CHK_MinMax CHECK (MinValue <= MaxValue),
    CONSTRAINT CHK_DifferentCurrencies CHECK (BaseCurrencyId <> QuoteCurrencyId),
    CONSTRAINT UQ_CurrencyPair UNIQUE (BaseCurrencyId, QuoteCurrencyId)
);