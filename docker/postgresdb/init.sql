CREATE TABLE "CurrencyPairs" (
    "ID" SERIAL PRIMARY KEY,
    "Name" VARCHAR(100) NOT NULL,
    "Value" DECIMAL(15, 4) NOT NULL,
    "Timestamp" TIMESTAMPTZ NOT NULL
);

INSERT INTO "CurrencyPairs" ("ID", "Name", "Value", "Timestamp") VALUES
(1, 'CNY', 4.776, to_timestamp(1723688358)),
(2, 'JPY', 430, to_timestamp(1723688338));