import * as React from 'react';
import { IApplicationContext } from './ApplicationProvider';


export const ApplicationContext = React.createContext<IApplicationContext>({
    requestTimeout: 120000,
    setRequestTimeout: () => { }
});
