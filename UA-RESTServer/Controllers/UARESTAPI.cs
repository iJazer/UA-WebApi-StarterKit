/* ========================================================================
 * Copyright (c) 2005-2023 The OPC Foundation, Inc. All rights reserved.
 *
 * OPC Foundation MIT License 1.00
 *
 * Permission is hereby granted, free of charge, to any person
 * obtaining a copy of this software and associated documentation
 * files (the "Software"), to deal in the Software without
 * restriction, including without limitation the rights to use,
 * copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the
 * Software is furnished to do so, subject to the following
 * conditions:
 *
 * The above copyright notice and this permission notice shall be
 * included in all copies or substantial portions of the Software.
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
 * EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
 * OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
 * NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
 * HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
 * WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
 * FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
 * OTHER DEALINGS IN THE SOFTWARE.
 *
 * The complete license agreement can be found here:
 * http://opcfoundation.org/License/MIT/1.00/
 * ======================================================================*/

namespace Ua.Rest.Server
{
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json;
    using Swashbuckle.AspNetCore.Annotations;
    using System.ComponentModel.DataAnnotations;
    using Opc.Ua;
    using System.Net;
    using Microsoft.AspNetCore.Authorization;

    [Authorize(AuthenticationSchemes = "BasicAuthentication")]
    [ApiController]
    public class UARESTController : ControllerBase
    {
        /// <summary>
        /// Lists the namespace URIs from the server
        /// </summary>
        /// <response code="401">Unauthorized, e.g. the server refused the authorization attempt.</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        /// <response code="0">Default error handling for unmentioned status codes</response>
        [HttpGet]
        [Route("/listNamespaceUris")]
        [SwaggerOperation("ListNamespaceUris")]
        [SwaggerResponse(statusCode: 401, type: typeof(ActionResult), description: "Unauthorized, e.g. the server refused the authorization attempt.")]
        [SwaggerResponse(statusCode: 403, type: typeof(ActionResult), description: "Forbidden")]
        [SwaggerResponse(statusCode: 404, type: typeof(ActionResult), description: "Not Found")]
        [SwaggerResponse(statusCode: 500, type: typeof(ActionResult), description: "Internal Server Error")]
        [SwaggerResponse(statusCode: 0, type: typeof(ActionResult), description: "Default error handling for unmentioned status codes")]
        public virtual IActionResult ListNamespaceUris()
        {
            try
            {
                return new ObjectResult(FactoryStationServer.NodeManagerInstance?.NamespaceUris) { StatusCode = (int)HttpStatusCode.OK };
            }
            catch (Exception ex)
            {
                return new ObjectResult(ex.Message) { StatusCode = (int)HttpStatusCode.BadRequest };
            }
        }

        /// <summary>
        /// Lists the OPC UA nodes from the server
        /// </summary>
        /// <response code="401">Unauthorized, e.g. the server refused the authorization attempt.</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        /// <response code="0">Default error handling for unmentioned status codes</response>
        [HttpGet]
        [Route("/listNodes")]
        [SwaggerOperation("ListNodes")]
        [SwaggerResponse(statusCode: 401, type: typeof(ActionResult), description: "Unauthorized, e.g. the server refused the authorization attempt.")]
        [SwaggerResponse(statusCode: 403, type: typeof(ActionResult), description: "Forbidden")]
        [SwaggerResponse(statusCode: 404, type: typeof(ActionResult), description: "Not Found")]
        [SwaggerResponse(statusCode: 500, type: typeof(ActionResult), description: "Internal Server Error")]
        [SwaggerResponse(statusCode: 0, type: typeof(ActionResult), description: "Default error handling for unmentioned status codes")]
        public virtual IActionResult ListNodes()
        {
            try
            {
                StationNodeManager? manager = FactoryStationServer.NodeManagerInstance;
                if (manager != null)
                {
                    Dictionary<string, string> keyValuePairs = new();
                    foreach (BaseVariableState variable in manager.UANodes.Values)
                    {
                        keyValuePairs.Add(variable.DisplayName.Text, variable.NodeId.ToString());
                    }

                    return new ObjectResult(keyValuePairs) { StatusCode = (int)HttpStatusCode.OK };
                }
                else
                {
                    return new ObjectResult("Not found") { StatusCode = (int)HttpStatusCode.NotFound };
                }
            }
            catch (Exception ex)
            {
                return new ObjectResult(ex.Message) { StatusCode = (int)HttpStatusCode.BadRequest };
            }
        }

        /// <summary>
        /// Returns an OPC UA node value from the server
        /// </summary>
        /// <param name="expandedNodeID">The expanded OPC UA node ID</param>
        /// <response code="200">Requested OPC UA node value</response>
        /// <response code="400">Bad Request, e.g. the format of the parameter is wrong.</response>
        /// <response code="401">Unauthorized, e.g. the server refused the authorization attempt.</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        /// <response code="0">Default error handling for unmentioned status codes</response>
        [HttpPost]
        [Route("/read")]
        [SwaggerOperation("ReadNode")]
        [SwaggerResponse(statusCode: 200, type: typeof(byte[]), description: "Requested OPC UA node value")]
        [SwaggerResponse(statusCode: 400, type: typeof(ActionResult), description: "Bad Request, e.g. the format of the parameter is wrong.")]
        [SwaggerResponse(statusCode: 401, type: typeof(ActionResult), description: "Unauthorized, e.g. the server refused the authorization attempt.")]
        [SwaggerResponse(statusCode: 403, type: typeof(ActionResult), description: "Forbidden")]
        [SwaggerResponse(statusCode: 404, type: typeof(ActionResult), description: "Not Found")]
        [SwaggerResponse(statusCode: 500, type: typeof(ActionResult), description: "Internal Server Error")]
        [SwaggerResponse(statusCode: 0, type: typeof(ActionResult), description: "Default error handling for unmentioned status codes")]
        public virtual IActionResult ReadNode([FromBody][Required]string nodeID)
        {
            try
            {
                BaseDataVariableState? nodeState = FactoryStationServer.NodeManagerInstance?.Find(NodeId.Parse(nodeID)) as BaseDataVariableState;
                if (nodeState != null)
                {
                    return new ObjectResult(new KeyValuePair<string, string?>(nodeState.DisplayName.Text, nodeState.Value?.ToString())) { StatusCode = (int)HttpStatusCode.OK };
                }
                else
                {
                    return new ObjectResult("Not found") { StatusCode = (int)HttpStatusCode.NotFound };
                }
            }
            catch (Exception ex)
            {
                return new ObjectResult(ex.Message) { StatusCode = (int)HttpStatusCode.BadRequest };
            }
        }
    }
}
