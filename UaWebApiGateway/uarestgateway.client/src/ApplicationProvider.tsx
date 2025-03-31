import * as React from 'react';
import { ApplicationContext } from './ApplicationContext';

export interface IApplicationContext {
   requestTimeout: number,
   setRequestTimeout: (requestTimeout: number) => void
}

interface ApplicationProviderProps {
   children?: React.ReactNode
}

export const ApplicationProvider = ({ children }: ApplicationProviderProps) => {
   const [requestTimeout, setRequestTimeout] = React.useState<number>(60000);

   const context = {
      requestTimeout,
      setRequestTimeout
   } as IApplicationContext;

   return (
      <ApplicationContext.Provider value={context}>
         {children}
      </ApplicationContext.Provider>
   );
};

export default ApplicationProvider;