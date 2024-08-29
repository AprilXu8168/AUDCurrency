"use client";
import { FC, useState, useEffect, useCallback } from 'react';
import Head from 'next/head';
import styles from '../styles/style.module.css';
import CurrencyList from './CurrencyList'; // Import CurrencyList

interface Currency {
  id: number;
  name: string;
  timestamp: string;
  value: number;
}

interface GraphQLResponse {
  data: {
    refreshCurrency: Currency[];
    fetchnUpdate: Currency[];
  };
}

const Page: FC = () => {
  // State to hold currency data
  const [currencies, setCurrencies] = useState<Currency[]>([]);

  const API_URL = 'http://localhost:5151/graphql/';
  // Fetch function for latest currencies
  const refreshCurrency = async () => {
    try {
      const response = await fetch(API_URL, {
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
      setCurrencies(result.data.currencyPairs);
    } catch (error) {
      console.error('Error fetching currency data:', error);
    }
  };

  const fetchLatest = useCallback(async () => {
    try {
      const response = await fetch(API_URL, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
          // Add other headers like authentication if required
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
        }),
      });

      const data: GraphQLResponse = await response.json();

      // Check if the data has been received successfully
      if (data.data && data.data.fetchnUpdate) {
        alert('Received new records fetched by GraphQL.');
        setCurrencies(data.data.fetchnUpdate);
      } else {
        alert('No new records were received.');
      }
    } catch (error) {
      console.error('Error fetching currency data:', error);
    }
  }, [API_URL]);

  // Fetch latest currencies on initial render
  useEffect(() => {
    refreshCurrency();
  }, []);

  if (!Array.isArray(currencies) || currencies.length === 0) {
    return <p>{typeof currencies === 'string' ? currencies : 'No data available'}</p>;
  }
  
  return (
    <div className={styles.container}>
      <Head>
        <div className={styles.header} >Hello World</div>
        <meta name="description" content="This is a Hello World page in Next.js using TypeScript." />
        <meta name="viewport" content="width=device-width, initial-scale=1" />
        <meta charSet="UTF-8" />
      </Head>
      <div className={styles.container}>
        <CurrencyList currencies={currencies} fetchLatest={fetchLatest} />
      </div>
    </div>
  );
};

export default Page;
