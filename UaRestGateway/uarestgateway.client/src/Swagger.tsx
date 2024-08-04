import SwaggerUI from "swagger-ui-react"
import "swagger-ui-react/swagger-ui.css"

export function Swagger() {

   return (
      <div>
         <SwaggerUI url="/data/opc.ua.openapi.allservices.json" />
      </div>
   );
}

export default Swagger;
