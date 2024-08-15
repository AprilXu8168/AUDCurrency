"use client";
import styles from "./page.module.css";
import { React, Component } from 'react';

class App extends Component{

  constructor(props){
    super(props)
    this.state={
      currencies:[]
    }
  }

  API_URL = "http://localhost:5151/"

  componentDidMount(){
    this.refreshCurrency();
  }

  async refreshCurrency(){
    try {
      const response = await fetch(this.API_URL + "api/Currency/");
      const data = await response.json();
      console.log("Received data:", data); // Log the received JSON data
      this.setState({ currencies: data });
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
          <h1>Currency List</h1>
          <table className={styles.table}>
            <thead>
              <tr>
                <th>Name</th>
                <th>Money Code</th>
                <th>Base Value</th>
                <th>Value</th>
                <th>Timestamp</th>
              </tr>
            </thead>
            <tbody>
              {currencies.map((item) => (
                <tr key={item.id}>
                  <td>{item.name}</td>
                  <td>{item.moneyCode}</td>
                  <td>{item.baseValue}</td>
                  <td>{item.value}</td>
                  <td>{new Date(item.timestamp * 1000).toLocaleString()}</td>
                </tr>
              ))}
            </tbody>
          </table>
        </main>
      );
  }
}

export default App