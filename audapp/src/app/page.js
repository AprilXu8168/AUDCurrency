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
    fetch(this.API_URL+"api/Currency/").then(response => response.json())
    .then(data => {
      this.setState({currencies:data});
    })
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