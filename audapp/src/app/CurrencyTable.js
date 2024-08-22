import React from 'react';

const CurrencyTable = ({ currencies }) => {
    return (
        <div className="table-container">
          <table className="table">
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
