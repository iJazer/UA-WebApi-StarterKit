import * as React from 'react';

import SwaggerUI from "swagger-ui-react"
import "swagger-ui-react/swagger-ui.css"

export function Swagger() {

   return (
      <div>
         <SwaggerUI url="/opcfoundation-org-opcua-rest-api-1.0.0-swagger.json" />
      </div>
   );
}

export default Swagger;
