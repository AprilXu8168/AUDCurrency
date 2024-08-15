# AUDCurrency
Store and display AUD currency pair 

### To start up the backend 
```
    cd audBackEnd
    dotnet run dev
```

### To install and start up frontEnd 
```
    cd audapp
    npm install
    npm run dev
```


### To generat controller base on model and db context
```
    dotnet aspnet-codegenerator controller -name CurrencyController -async -api -m CurrencyItem -dc CurrenciesDBContext -outDir Controllers
```

### To update dababase
```
    dotnet ef database update
```

### To start up postgres DB within docker compose

```
    docker compose -f compose.yml up --build
```

### Init data
```
[
  {
    "id": 1,
    "timestamp": 1723688358,
    "name": "Chinese",
    "moneyCode": "CNY",
    "baseValue": 1,
    "value": 4.776
  },
  {
    "id": 2,
    "timestamp": 1723688338,
    "name": "Japanese",
    "moneyCode": "JPY",
    "baseValue": 1,
    "value": 430
  }
]
```