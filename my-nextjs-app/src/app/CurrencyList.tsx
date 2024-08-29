import React, { useState, ChangeEvent } from 'react';
import CurrencyTable from './CurrencyTable'; // Adjust the path as needed
import styles from '../styles/style.module.css'; // Import the CSS module

// Define types for the props
interface Currency {
  id: number;
  name: string;
  value: number;
  timestamp: string;
}

interface CurrencyListProps {
  currencies: Currency[];
  fetchLatest: () => void;
}

const CurrencyList: React.FC<CurrencyListProps> = ({ currencies, fetchLatest }) => {
  const [selectedName, setSelectedName] = useState<string>('');
  const [filteredCurrencies, setFilteredCurrencies] = useState<Currency[]>(currencies);

  // Fetch all unique names for the dropdown
  const uniqueNames = [...new Set(currencies.map((item) => item.name))];

  const handleFilterChange = async (event: ChangeEvent<HTMLSelectElement>) => {
    const name = event.target.value;
    setSelectedName(name);
    try{
      const response = await fetch('http://localhost:5151/graphql/', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({
          query: name
            ? `
              query {
                currencyPairByName(name: "${name}") {
                  id
                  name
                  value
                  timestamp
                }
              }
            `
            : `
              query {
                currencyPairs {
                  id
                  name
                  value
                  timestamp
                }
              }
            `,
        }),
      });
      const result = await response.json();
      console.log('Received JSON is:', result.data);

      if (name) {
        setFilteredCurrencies(result.data.currencyPairByName);
      } else {
        setFilteredCurrencies(result.data.currencyPairs);
      }
    } catch (error) {
      console.error('Error fetching data:', error);
    }
  }
  return (
    <main className={styles.filterContainer}>
      <div className={styles.headerContainer}>
        <h1 className={styles.title}>Currency List</h1>
        <button
          style={{
            position: 'absolute',
            right: '0',
            padding: '10px 20px',
            fontSize: '1rem',
            cursor: 'pointer',
            backgroundColor: '#007bff',
            color: '#fff',
            border: 'none',
            borderRadius: '5px',
          }}
          onClick={fetchLatest}
        >
          Fetch Latest Currency
        </button>
      </div>
      
      {/* Filter Section */}
      <div className={styles.filterContainer}>
        <label htmlFor="currency-filter" className='filterLabel'>Filter by Name:</label>
        <select
          className='filterSelect'
          id="currency-filter"
          value={selectedName}
          onChange={handleFilterChange}
        >
          <option value="">All</option>
          {uniqueNames.map((name) => (
            <option key={name} value={name}>
              {name}
            </option>
          ))}
        </select>
      </div>

      <div className={styles.infoBar}>
        <p>{`Found ${filteredCurrencies.length} currency records from DB`}</p>
      </div>
      
      {/* Currency Table */}
      <CurrencyTable currencies={filteredCurrencies} />
    </main>
  );
};

export default CurrencyList;
