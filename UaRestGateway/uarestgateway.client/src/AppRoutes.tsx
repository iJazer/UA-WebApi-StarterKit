import React from 'react';
import HomePage from './pages/HomePage';
import LoginPage from './pages/LoginPage';
import Swagger from './Swagger';
import MonitoringPage from './pages/MonitoringPage';

export interface PageRoute {
   name: string
   path: string
   element: React.ReactNode
   appId: number
}

const AppRoutes : PageRoute[] = [
   {
      name: "main.home",
      path: '/',
      appId: 1,
      element: <HomePage />
   },
   {
      name: "main.monitoring",
      path: '/monitoring',
      appId: 1,
      element: <MonitoringPage />
   },
   {
      name: "main.swagger",
      path: '/swagger',
      appId: 1,
      element: <Swagger />
   },
   {
      name: "main.login",
      path: '/login',
      appId: -1,
      element: <LoginPage />
   }
];

export default AppRoutes;
