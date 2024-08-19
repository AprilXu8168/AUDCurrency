CREATE TABLE "CurrencyItems" (
    "ID" SERIAL PRIMARY KEY,
    "Name" VARCHAR(100) NOT NULL,
    "moneyCode" VARCHAR(10) NOT NULL,
    "baseValue" DECIMAL(15, 4) NOT NULL,
    "value" DECIMAL(15, 4) NOT NULL,
    "Timestamp" TIMESTAMPTZ NOT NULL
);

INSERT INTO "CurrencyItems" ("ID", "Name", "moneyCode", "baseValue", "value", "Timestamp") VALUES
(1, 'Chinese', 'CNY', 1, 4.776, to_timestamp(1723688358)),
(2, 'Japanese', 'JPY', 1, 430, to_timestamp(1723688338));