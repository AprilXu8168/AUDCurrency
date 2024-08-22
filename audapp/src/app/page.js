"use client";
import styles from "./page.module.css";
import { React, Component } from 'react';

class App extends Component{

  constructor(props){
    super(props)
    this.state={
      currencies:[],
      newFetchRates:[]
    }
    this.refreshCurrency = this.refreshCurrency.bind(this);
    this.fetchLatest = this.fetchLatest.bind(this);
    this.API_URL = "http://localhost:5151/"
  }

  componentDidMount(){
    this.refreshCurrency();
  }

  async refreshCurrency(){
    try {
      const response = await fetch(this.API_URL + "api/ExchangeRatesService");
      const data = await response.json();

      if (data && data.value) {
        const recordCount = data.value.length; // Count the number of new records
        console.log(`Found ${recordCount} existing records.`);

        this.setState({ currencies: data.value });
      } else {
        alert("No new records were received.");
      }
     } catch (error) {
      console.error("Error fetching currency data from db:", error);
    }
  }

  async fetchLatest(){
    try {
      const response = await fetch(this.API_URL + "api/ExchangeRatesService/update_Rates");
      const data = await response.json();

      // Check if the data has been received successfully
      if (data && data.value) {
        const recordCount = data.value.length; // Count the number of new records
        alert(`Received ${recordCount} new records.`);
        
        this.setState({ newFetchRates: data.value });
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
        <main className={styles.container}>
          <div style={currencyStyles.headerContainer}>
            <h1 style={currencyStyles.header}>Currency List</h1>
            <button style={currencyStyles.button} onClick={this.fetchLatest}>Fetch Latest Currency</button>
          </div>          
          <table className={styles.table}>
            <thead>
              <tr>
                <th>ID</th>
                <th>Name</th>
                <th>Value</th>
                <th>Timestamp</th>
              </tr>
            </thead>
            <tbody>
              {currencies.map((item) => (
                <tr key={item.id}>
                  <td>{item.id}</td>
                  <td>{item.name}</td>
                  <td>{item.value}</td>
                  <td>{item.timestamp.toString()}</td>
                </tr>
              ))}
            </tbody>
          </table>
        </main>
      );
  }
}



const currencyStyles = {
  headerContainer: {
    display: 'flex',
    alignItems: 'center',
    marginBottom: '20px'
  },
  header: {
    margin: 0,
    marginRight: '20px',
    fontSize: '24px'
  },
  button: {
    padding: '10px 15px',
    fontSize: '16px',
    cursor: 'pointer',
    backgroundColor: '#007bff',
    color: '#fff',
    border: 'none',
    borderRadius: '5px'
  }
};
export default App