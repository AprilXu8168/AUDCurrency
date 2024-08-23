import React, { useState } from 'react';
import CurrencyTable from './CurrencyTable'; // Adjust the path as needed

const CurrencyList = ({ currencies, fetchLatest }) => {
  const [selectedName, setSelectedName] = useState('');
  const [filteredCurrencies, setFilteredCurrencies] = useState(currencies);

  // Fetch all unique names for the dropdown
  const uniqueNames = [...new Set(currencies.map((item) => item.name))];

  const handleFilterChange = async (event) => {
    const name = event.target.value;
    setSelectedName(name);

    if (name) {
      try {
        const response = await fetch('http://localhost:5151/graphql/', {
          method: 'POST',
          headers: {
            'Content-Type': 'application/json',
          },
          body: JSON.stringify({
            query: `
              query {
                currencyPairByName (name: "${name}" ) {
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
        console.log("received json is: {0}", result.data.currencyPairByName);
        setFilteredCurrencies(result.data.currencyPairByName);
      } catch (error) {
        console.error('Error fetching filtered data:', error);
      }
    } else {
      setFilteredCurrencies(currencies); // Reset to original data if no filter is selected
    }
  };

  return (
    <main className={style.container}>
      <div className={style.headerContainer}>
        <h1 className={style.header}>Currency List</h1>
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
      <div style={style.filterContainer}>
        <label htmlFor="currency-filter" style={style.filterLabel}>Filter by Name:</label>
        <select
          id="currency-filter"
          value={selectedName}
          onChange={handleFilterChange}
          style={style.filterSelect}
        >
          <option value="">All</option>
          {uniqueNames.map((name) => (
            <option key={name} value={name}>
              {name}
            </option>
          ))}
        </select>
      </div>

      <div style={style.infoBar}>
        <p>{`Found ${filteredCurrencies.length} currency records from DB`}</p>
      </div>
      
      {/* Currency Table */}
      <CurrencyTable currencies={filteredCurrencies} />
    </main>
  );
};

const style = {
    filterContainer: {
      marginTop: '10px',
      marginBottom: '20px',
    },
    filterLabel: {
      marginRight: '10px',
    },
    filterSelect: {
      padding: '5px',
      fontSize: '1rem',
    },
    infoBar: {
      padding: '10px',
      backgroundColor: '#f0f0f0',
      borderRadius: '5px',
      marginBottom: '20px',
      fontSize: '1rem',
    }
  };
export default CurrencyList;
