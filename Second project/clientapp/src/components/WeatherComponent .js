import React, { useState, useEffect } from 'react';
import { fetchWeatherData } from './WeatherService'; 


const WeatherComponent = () => {
  const [weatherData, setWeatherData] = useState([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);

  useEffect(() => {
    const fetchData = async () => {
      try {
        setLoading(true);
        const data = await fetchWeatherData();
        setWeatherData(data);
      } catch (error) {
        setError(error.message);
      } finally {
        setLoading(false);
      }
    };

    fetchData();
  }, []);

  if (loading) {
    return <p>Loading weather data...</p>;
  }

  if (error) {
    return <p>Error fetching weather data: {error}</p>;
  }

  return (
    <div>
      <h1>Weather Data</h1>
      <table>
        <thead>
          <tr>
            <th>Country</th>
            <th>City</th>
            <th>Temperature</th>
            <th>Clouds</th>
            <th>Wind Speed</th>
            <th>Last Update Time</th>
          </tr>
        </thead>
        <tbody>
          {weatherData.map((item, index) => (
            <tr key={index}>
              <td>{item.country}</td>
              <td>{item.city}</td>
              <td>{item.temperature}°C</td>
              <td>{item.clouds}%</td>
              <td>{item.windSpeed} km/h</td>
              <td>{new Date(item.lastUpdateTime).toLocaleString()}</td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
};

export default WeatherComponent;
