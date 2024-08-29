import React from 'react';
import styles from '../styles/style.module.css'
// Define the type for a single currency item
interface Currency {
  id: number;
  name: string;
  value: number;
  timestamp: string; // Assuming timestamp is a string in ISO format
}

// Define the props for the CurrencyTable component
interface CurrencyTableProps {
  currencies: Currency[];
}

// Define the CurrencyTable component
const CurrencyTable: React.FC<CurrencyTableProps> = ({ currencies }) => {
  return (
    <div className={styles.tableContainer}>
      <table className={styles.table} id="CurrencyListTable">
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
              <td>{new Date(item.timestamp).toLocaleString()}</td>
          </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
};

export default CurrencyTable;