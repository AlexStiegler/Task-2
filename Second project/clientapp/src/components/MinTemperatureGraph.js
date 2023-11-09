import React, { useState, useEffect } from 'react';
import { Bar } from 'react-chartjs-2';
import {
  Chart as ChartJS,
  CategoryScale,
  LinearScale,
  BarElement,
  Title,
  Tooltip,
  Legend,
} from 'chart.js';

import WeatherService from '../services/WeatherService';

ChartJS.register(
  CategoryScale,
  LinearScale,
  BarElement,
  Title,
  Tooltip,
  Legend
);

const MinTemperatureGraph = () => {
  const [weatherData, setWeatherData] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');

  useEffect(() => {
    const fetchData = async () => {
      try {
        const data = await WeatherService.getMinTemperatures(); // Assume this returns data with city and last update time
        setWeatherData(data);
        setLoading(false);
      } catch (error) {
        console.error('Error fetching weather data:', error);
        setError('Failed to fetch data');
        setLoading(false);
      }
    };

    fetchData();
  
    const intervalId = setInterval(fetchData, 60000);
  
    return () => clearInterval(intervalId);
  }, []);

  if (loading) {
    return <p>Loading...</p>;
  }

  if (error) {
    return <p>Error: {error}</p>;
  }

  // Here you can sort the weatherData array if needed, before creating the chart data
  
  const data = {
    labels: weatherData.map(item => `${item.country}`), // As we don't have city data
    datasets: [
      {
        label: 'Minimum Temperature (°C)',
        data: weatherData.map(item => item.minTemperature), // This will populate the bars with temperature data
        backgroundColor: 'rgba(255, 99, 132, 0.2)',
        borderColor: 'rgba(255, 99, 132, 1)',
        borderWidth: 1,
      },
    ],
  };

  const options = {
    scales: {
      y: {
        beginAtZero: true,
        title: {
          display: true,
          text: 'Temperature (°C)',
        },
      },
    },
    responsive: true,
    plugins: {
      legend: {
        position: 'top', 
      },
      title: {
        display: true,
        text: 'Minimum Temperature by Country',
      },
      tooltip: {
        callbacks: {
          label: function(context) {
            // Assuming weatherData[context.dataIndex] has city and lastUpdateTime
            let label = `${weatherData[context.dataIndex].country}: `;
            label += `${context.parsed.y}°C`;
            // Add city and last update time if available
            label += ` - ${weatherData[context.dataIndex].city}`;
            label += ` (Last updated: ${weatherData[context.dataIndex].lastTimeUpdated})`;
            return label;
          },
        },
      },
    },
  };

  return <Bar data={data} options={options} />;
};

export default MinTemperatureGraph;
