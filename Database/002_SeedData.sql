
USE CurrencyDB;
GO

INSERT INTO dbo.Currency (Country, Name, Code) VALUES
('United States', 'Dollar', 'USD'),
('European Union', 'Euro', 'EUR'),
('Israel', 'Shekel', 'ILS'),
('United Kingdom', 'Pound Sterling', 'GBP');

INSERT INTO dbo.CurrencyPair (BaseCurrencyId, QuoteCurrencyId, MinValue, MaxValue)
SELECT 
    b.Id,
    q.Id,
    v.MinValue,
    v.MaxValue
FROM (VALUES
    ('USD', 'ILS', 3.500000, 3.800000),
    ('EUR', 'USD', 0.900000, 1.200000),
    ('GBP', 'EUR', 1.100000, 1.400000)
) AS v(BaseCode, QuoteCode, MinValue, MaxValue)
JOIN dbo.Currency b ON
    b.Code = v.BaseCode
JOIN dbo.Currency q ON
    q.Code = v.QuoteCode;