import React from 'react';
import MinTemperatureGraph from './components/MinTemperatureGraph';
import MaxWindSpeedGraph from './components/MaxWindSpeedGraph';

function App() {
  return (
    <div className="App">
      <h1>Weather Dashboard</h1>
      <MinTemperatureGraph />
      <MaxWindSpeedGraph />
    </div>
  );
}

export default App;
