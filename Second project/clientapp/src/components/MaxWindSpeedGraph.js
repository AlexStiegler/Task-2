import React, { useState, useEffect } from 'react';
import { Bar } from 'react-chartjs-2';
import { Chart as ChartJS, CategoryScale, LinearScale, BarElement, Title, Tooltip, Legend, } from 'chart.js';

import WeatherService from '../services/WeatherService';

ChartJS.register(
  CategoryScale,
  LinearScale,
  BarElement,
  Title,
  Tooltip,
  Legend
);

const MaxWindSpeedGraph = () => {
  const [weatherData, setWeatherData] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');

  useEffect(() => {
    const fetchData = async () => {
      try {
        const data = await WeatherService.getMaxWindSpeeds();
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

  // Make sure loading and error states are handled after useEffect
  if (loading) {
    return <p>Loading...</p>;
  }

  if (error) {
    return <p>Error: {error}</p>;
  }

  const data = {
    labels: weatherData.map(item => item.country), 
    datasets: [
      {
        label: 'Highest Wind Speed',
        data: weatherData.map(item => item.maxWindSpeed), 
        backgroundColor: 'rgba(53, 162, 235, 0.5)',
        borderColor: 'rgba(53, 162, 235, 0.8)',
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
          text: 'Wind Speed (km/h)',
        },
      },
    },
    responsive: true,
    plugins: {
      legend: {
        position: 'top', // Adjust legend position if needed
      },
      title: {
        display: true,
        text: 'Max Wind Speed by Country',
      },
      tooltip: {
        callbacks: {
          label: function(context) {
            let label = context.dataset.label || '';
            if (label) {
              label += ': ';
            }
            label += Math.round(context.parsed.y * 100) / 100;
            label += ' km/h'; // Removed the LastUpdateTime as it's not present in the provided data
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

export default MaxWindSpeedGraph;
