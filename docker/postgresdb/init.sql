CREATE TABLE currencyItems (
    id SERIAL PRIMARY KEY,
    name VARCHAR(100) NOT NULL,
    money_code VARCHAR(10) NOT NULL,
    base_value DECIMAL(15, 4) NOT NULL,
    value DECIMAL(15, 4) NOT NULL,
    timestamp TIMESTAMPTZ NOT NULL
);

INSERT INTO currencyItems (id, name, money_code, base_value, value, timestamp) VALUES
(1, 'Chinese', 'CNY', 1, 4.776, to_timestamp(1723688358)),
(2, 'Japanese', 'JPY', 1, 430, to_timestamp(1723688338));