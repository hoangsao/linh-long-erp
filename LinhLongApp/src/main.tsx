import React from 'react';
import ReactDOM from 'react-dom/client';
import AppRoot from './app/index';
import 'antd/dist/reset.css';
import './shared/styles/globals.scss';

ReactDOM.createRoot(document.getElementById('root')!).render(
  <React.StrictMode>
    <AppRoot />
  </React.StrictMode>
);