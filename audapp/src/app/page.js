"use client";
import style  from './style.css';
import { React, Component } from 'react';
import CurrencyList from './CurrencyList';

class App extends Component{

  constructor(props){
    super(props)
    this.state={
      currencies:[],
      newFetchRates:[]
    }
    this.refreshCurrency = this.refreshCurrency.bind(this);
    this.fetchLatest = this.fetchLatest.bind(this);
    this.API_URL = "http://127.0.0.1:5151/"
  }

  componentDidMount(){
    this.refreshCurrency();
  }

  // Fetch REST api endpoint
//   async refreshCurrency(){
//  try {
//       const response = await fetch(this.API_URL + "api/ExchangeRatesService");
//       const data = await response.json();

//       if (data && data.value) {
//         const recordCount = data.value.length; // Count the number of new records
//         console.log(`REST API returned ${recordCount} existing records.`);

//         this.setState({ currencies: data.value });
//       } else {
//         alert("No new records were received.");
//       }
//      } catch (error) {
//       console.error("Error fetching currency data from db:", error);
//     }
//   }

  // Fetch graphql endpoint with graphql Query
  async refreshCurrency(){
    try {
      const response = await fetch(this.API_URL + "graphql", {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({
          query: `
            query {
              currencyPairs {
                id
                name
                timestamp
                value
              }
            }
          `,
        }),
      });
      const result = await response.json();

      if (result.data && result.data.currencyPairs) {
        const currencyPairs = result.data.currencyPairs;
        console.log(`Graphql has returned existing records.`);
        this.setState({ currencies: currencyPairs });
      } else {
        alert("No new records were received.");
      }
    } catch (error) {
      console.error("Error fetching currency data from GraphQL:", error);
    }
  }


  // Fetch REST api endpoint
  // async fetchLatest(){
  //   try {
  //     const response = await fetch(this.API_URL + "api/ExchangeRatesService/update_Rates");
  //     const data = await response.json();

  //     // Check if the data has been received successfully
  //     if (data && data.value) {
  //       alert(`Received new records from RestfulAPI.`);
        
  //       this.setState({ newFetchRates: data.value });
  //     } else {
  //       alert("No new records were received.");
  //     }
  //   } catch (error) {
  //     console.error("Error fetching currency data:", error);
  //   }
  // }

  //Fetch with grapgql Endpoint
    async fetchLatest(){
    try {
      const response = await fetch(this.API_URL + "graphql", {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            // Add any other headers if needed, e.g., authentication tokens
        },
        body: JSON.stringify({
          query: `
            mutation {
              fetchnUpdate {
                id
                name
                timestamp
                value
              }
            }
          `,
        }),      });

      const data = await response.json();

      // Check if the data has been received successfully
      if (data) {
        alert(`Received new records fetched by graphql.`);
        
        this.setState({ newFetchRates: data });
      } else {
        alert("No new records were received.");
      }
    } catch (error) {
      console.error("Error fetching currency data:", error);
    }
  }

  render(){
    const{currencies} = this.state;
    // Ensure data is defined and is an array
    if (!Array.isArray(currencies) || currencies.length === 0) {
      return <p>{currencies}</p>;
    }
    return (
      <><div style={style.headerContainer}>
          <CurrencyList currencies={currencies} fetchLatest={this.fetchLatest}/>
        </div></>
    );
  };
}


export default App