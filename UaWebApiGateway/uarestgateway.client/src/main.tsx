import React from 'react'
import { BrowserRouter } from 'react-router-dom';
import ReactDOM from 'react-dom/client'
import App from './App.tsx'
import './index.css'
import './i18n';

import CssBaseline from '@mui/material/CssBaseline';
import UserProvider from './UserProvider';
import SessionProvider from './SessionProvider.tsx';
import SubscriptionProvider from './SubscriptionProvider.tsx';

ReactDOM.createRoot(document.getElementById('root')!).render(
   <React.StrictMode>
      <BrowserRouter>
         <UserProvider>
            <SessionProvider>
               <SubscriptionProvider>
                  <CssBaseline />
                  <App />
               </SubscriptionProvider>
            </SessionProvider>
         </UserProvider>
      </BrowserRouter>
   </React.StrictMode>
)
