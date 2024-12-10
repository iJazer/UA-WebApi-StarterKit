import './App.css'
import React from 'react';
import * as OpcUa from 'opcua-webapi';
import { RunDemo } from './RunDemo';

const testUserName = 'user1';
const testUserPassword = 'password1';
const allowLocalServerDebugging = false;
const defaultUrl = (allowLocalServerDebugging) ? "https://localhost:44428/opcua" : "https://webapi.opcfoundation.org/opcua";

const configuration = new OpcUa.Configuration({
   basePath: defaultUrl,
   fetchApi: (url, init): Promise<Response> => {
      if (init) {
         const authorization = {
            'Authorization': `Basic ${btoa(`${testUserName}:${testUserPassword}`)}`
         };
         init.headers = { ...init.headers, ...authorization };
      }
      return fetch(url, init);
   }
});

function App() {
   const [isRunning, setIsRunning] = React.useState<boolean>(false);
   return (
      <div className="container">
         <h3>OPC UA Web API</h3>
         <div className="container">
            <button onClick={() => setIsRunning(x => !x)}>{(isRunning) ? "Stop" : "Run"}</button>
         </div>
         <RunDemo configuration={configuration} isRunning={isRunning} />
      </div>
   )
}

export default App
