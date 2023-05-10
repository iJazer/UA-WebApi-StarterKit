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
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json;
    using Opc.Ua;
    using Swashbuckle.AspNetCore.Annotations;
    using System.ComponentModel.DataAnnotations;
    using System.Net;
    using System.Text;
    using UA_RESTServer.Models;

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
        [HttpGet]
        [Route("/listServerNamespaces")]
        [SwaggerOperation("ListServerNamespaces")]
        [SwaggerResponse(statusCode: 401, type: typeof(ActionResult), description: "Unauthorized, e.g. the server refused the authorization attempt.")]
        [SwaggerResponse(statusCode: 403, type: typeof(ActionResult), description: "Forbidden")]
        [SwaggerResponse(statusCode: 404, type: typeof(ActionResult), description: "Not Found")]
        [SwaggerResponse(statusCode: 500, type: typeof(ActionResult), description: "Internal Server Error")]
        public virtual IActionResult ListServerNamespaces()
        {
            try
            {
                return new ObjectResult(FactoryStationServer.NodeManagerInstance?.NamespaceUris) { StatusCode = (int)HttpStatusCode.OK };
            }
            catch (Exception ex)
            {
                return new ObjectResult(ex.Message) { StatusCode = (int)HttpStatusCode.InternalServerError };
            }
        }

        /// <summary>
        /// Lists the OPC UA variables from the server
        /// </summary>
        /// <response code="401">Unauthorized, e.g. the server refused the authorization attempt.</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet]
        [Route("/listVariables")]
        [SwaggerOperation("ListVariables")]
        [SwaggerResponse(statusCode: 401, type: typeof(ActionResult), description: "Unauthorized, e.g. the server refused the authorization attempt.")]
        [SwaggerResponse(statusCode: 403, type: typeof(ActionResult), description: "Forbidden")]
        [SwaggerResponse(statusCode: 404, type: typeof(ActionResult), description: "Not Found")]
        [SwaggerResponse(statusCode: 500, type: typeof(ActionResult), description: "Internal Server Error")]
        public virtual IActionResult ListVariables()
        {
            try
            {
                NamespaceTable namespaceTable = new NamespaceTable(FactoryStationServer.NodeManagerInstance?.NamespaceUris);
                StationNodeManager? manager = FactoryStationServer.NodeManagerInstance;
                if (manager != null)
                {
                    Dictionary<string, string> keyValuePairs = new();
                    foreach (BaseVariableState variable in manager.UAVariableNodes.Values)
                    {
                        keyValuePairs.Add(variable.DisplayName.Text, NodeId.ToExpandedNodeId(variable.NodeId, namespaceTable).ToString());
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
                return new ObjectResult(ex.Message) { StatusCode = (int)HttpStatusCode.InternalServerError };
            }
        }

        /// <summary>
        /// Lists the OPC UA methods from the server
        /// </summary>
        /// <response code="401">Unauthorized, e.g. the server refused the authorization attempt.</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet]
        [Route("/listMethods")]
        [SwaggerOperation("ListMethods")]
        [SwaggerResponse(statusCode: 401, type: typeof(ActionResult), description: "Unauthorized, e.g. the server refused the authorization attempt.")]
        [SwaggerResponse(statusCode: 403, type: typeof(ActionResult), description: "Forbidden")]
        [SwaggerResponse(statusCode: 404, type: typeof(ActionResult), description: "Not Found")]
        [SwaggerResponse(statusCode: 500, type: typeof(ActionResult), description: "Internal Server Error")]
        public virtual IActionResult ListMethods()
        {
            try
            {
                NamespaceTable namespaceTable = new NamespaceTable(FactoryStationServer.NodeManagerInstance?.NamespaceUris);
                StationNodeManager? manager = FactoryStationServer.NodeManagerInstance;
                if (manager != null)
                {
                    Dictionary<string, string> keyValuePairs = new();
                    foreach (MethodState variable in manager.UAMethodNodes.Values)
                    {
                        keyValuePairs.Add(variable.DisplayName.Text, NodeId.ToExpandedNodeId(variable.NodeId, namespaceTable).ToString());
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
                return new ObjectResult(ex.Message) { StatusCode = (int)HttpStatusCode.InternalServerError };
            }
        }

        /// <summary>
        /// Browse an OPC UA node on the server
        /// </summary>
        /// <param name="browsePath">The browse path within the OPC UA Server. If omitted, the browse will start in the root folder of the OPC UA Server.</param>
        /// <response code="200">Requested OPC UA node value</response>
        /// <response code="400">Bad Request, e.g. the format of the parameter is wrong.</response>
        /// <response code="401">Unauthorized, e.g. the server refused the authorization attempt.</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPost]
        [Route("/browse")]
        [SwaggerOperation("Browse")]
        [SwaggerResponse(statusCode: 401, type: typeof(ActionResult), description: "Unauthorized, e.g. the server refused the authorization attempt.")]
        [SwaggerResponse(statusCode: 403, type: typeof(ActionResult), description: "Forbidden")]
        [SwaggerResponse(statusCode: 404, type: typeof(ActionResult), description: "Not Found")]
        [SwaggerResponse(statusCode: 500, type: typeof(ActionResult), description: "Internal Server Error")]
        public virtual IActionResult Browse([FromBody] string browsePath)
        {
            try
            {
                NamespaceTable namespaceTable = new NamespaceTable(FactoryStationServer.NodeManagerInstance?.NamespaceUris);
                if (FactoryStationServer.NodeManagerInstance?.NodeStates != null)
                {
                    NodeState? browseNode = null;
                    if (string.IsNullOrEmpty(browsePath))
                    {
                        browseNode = FactoryStationServer.NodeManagerInstance.NodeStates[0].GetHierarchyRoot();
                    }
                    else
                    {
                        foreach (NodeState node in FactoryStationServer.NodeManagerInstance.NodeStates)
                        {
                            if (node.BrowseName.Name == browsePath)
                            {
                                browseNode = node;
                                break;
                            }
                        }
                    }

                    if (browseNode == null)
                    {
                        return new ObjectResult("Not found") { StatusCode = (int)HttpStatusCode.NotFound };
                    }

                    Dictionary<NodeId, string> hierarchy = new();
                    List<NodeStateHierarchyReference> references = new();
                    browseNode.GetHierarchyReferences(FactoryStationServer.NodeManagerInstance.Server.DefaultSystemContext, "", hierarchy, references);
                    
                    List<string> browseResult = new();
                    foreach (NodeStateHierarchyReference reference in references)
                    {
                        string? browseName = FactoryStationServer.NodeManagerInstance?.Find(ExpandedNodeId.ToNodeId(reference.TargetId, namespaceTable)).BrowseName.Name;
                        browseResult.Add(browseNode.BrowseName.Name + " -> " + browseName);
                    }

                    return new ObjectResult(browseResult) { StatusCode = (int)HttpStatusCode.OK };
                }
                else
                {
                    return new ObjectResult("Not found") { StatusCode = (int)HttpStatusCode.NotFound };
                }
            }
            catch (Exception ex)
            {
                return new ObjectResult(ex.Message) { StatusCode = (int)HttpStatusCode.InternalServerError };
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
        [HttpPost]
        [Route("/read")]
        [SwaggerOperation("ReadNode")]
        [SwaggerResponse(statusCode: 200, type: typeof(byte[]), description: "Requested OPC UA node value")]
        [SwaggerResponse(statusCode: 400, type: typeof(ActionResult), description: "Bad Request, e.g. the format of the parameter is wrong.")]
        [SwaggerResponse(statusCode: 401, type: typeof(ActionResult), description: "Unauthorized, e.g. the server refused the authorization attempt.")]
        [SwaggerResponse(statusCode: 403, type: typeof(ActionResult), description: "Forbidden")]
        [SwaggerResponse(statusCode: 404, type: typeof(ActionResult), description: "Not Found")]
        [SwaggerResponse(statusCode: 500, type: typeof(ActionResult), description: "Internal Server Error")]
        public virtual IActionResult ReadNode([FromBody][Required]string expandedNodeID)
        {
            try
            {
                NamespaceTable namespaceTable = new NamespaceTable(FactoryStationServer.NodeManagerInstance?.NamespaceUris);
                ExpandedNodeId expandedNodeId = ExpandedNodeId.Parse(expandedNodeID);
                BaseDataVariableState? nodeState = FactoryStationServer.NodeManagerInstance?.Find(ExpandedNodeId.ToNodeId(expandedNodeId, namespaceTable)) as BaseDataVariableState;
                if (nodeState != null)
                {
                    return new ObjectResult(new Tuple<string, string?, string, string>(nodeState.DisplayName.Text, nodeState.Value?.ToString(), nodeState.DataType.ToString(), nodeState.Timestamp.ToString())) { StatusCode = (int)HttpStatusCode.OK };
                }
                else
                {
                    return new ObjectResult("Not found") { StatusCode = (int)HttpStatusCode.NotFound };
                }
            }
            catch (Exception ex)
            {
                return new ObjectResult(ex.Message) { StatusCode = (int)HttpStatusCode.InternalServerError };
            }
        }

        /// <summary>
        /// Updates an OPC UA node value in the server
        /// </summary>
        /// <param name="payload">The payload for the write operation containing expanded OPC UA node ID, value and OPC UA data type</param>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request, e.g. the format of the parameter is wrong.</response>
        /// <response code="401">Unauthorized, e.g. the server refused the authorization attempt.</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPost]
        [Route("/write")]
        [SwaggerOperation("WriteNode")]
        [SwaggerResponse(statusCode: 200, type: typeof(byte[]), description: "Success")]
        [SwaggerResponse(statusCode: 400, type: typeof(ActionResult), description: "Bad Request, e.g. the format of the parameter is wrong.")]
        [SwaggerResponse(statusCode: 401, type: typeof(ActionResult), description: "Unauthorized, e.g. the server refused the authorization attempt.")]
        [SwaggerResponse(statusCode: 403, type: typeof(ActionResult), description: "Forbidden")]
        [SwaggerResponse(statusCode: 404, type: typeof(ActionResult), description: "Not Found")]
        [SwaggerResponse(statusCode: 500, type: typeof(ActionResult), description: "Internal Server Error")]
        public virtual IActionResult WriteNode([FromBody][Required] WritePayload payload)
        {
            try
            {
                NamespaceTable namespaceTable = new NamespaceTable(FactoryStationServer.NodeManagerInstance?.NamespaceUris);
                ExpandedNodeId expandedNodeId = ExpandedNodeId.Parse(payload.ExpandedNodeId);
                BaseDataVariableState? nodeState = FactoryStationServer.NodeManagerInstance?.Find(ExpandedNodeId.ToNodeId(expandedNodeId, namespaceTable)) as BaseDataVariableState;
                if (nodeState != null)
                {
                    // see https://reference.opcfoundation.org/v104/Core/docs/Part6/5.1.2/
                    switch (payload.OPCUADataType)
                    {
                        case "i=1": nodeState.Value = bool.Parse(payload.Value); break;
                        case "i=2": nodeState.Value = byte.Parse(payload.Value); break;
                        case "i=3": nodeState.Value = byte.Parse(payload.Value); break;
                        case "i=4": nodeState.Value = short.Parse(payload.Value); break;
                        case "i=5": nodeState.Value = ushort.Parse(payload.Value); break;
                        case "i=6": nodeState.Value = int.Parse(payload.Value); break;
                        case "i=7": nodeState.Value = uint.Parse(payload.Value); break;
                        case "i=8": nodeState.Value = long.Parse(payload.Value); break;
                        case "i=9": nodeState.Value = ulong.Parse(payload.Value); break;
                        case "i=10": nodeState.Value = float.Parse(payload.Value); break;
                        case "i=11": nodeState.Value = double.Parse(payload.Value); break;
                        case "i=12": nodeState.Value = payload.Value; break;
                        case "i=13": nodeState.Value = DateTime.Parse(payload.Value); break;
                        case "i=15": nodeState.Value = Encoding.UTF8.GetBytes(payload.Value); break;
                        case "i=19": nodeState.Value = int.Parse(payload.Value); break;
                        default: return new ObjectResult("OPC UA data type not found!") { StatusCode = (int)HttpStatusCode.BadRequest };
                    }

                    return new ObjectResult("Success") { StatusCode = (int)HttpStatusCode.OK };
                }
                else
                {
                    return new ObjectResult("Not found") { StatusCode = (int)HttpStatusCode.NotFound };
                }
            }
            catch (Exception ex)
            {
                return new ObjectResult(ex.Message) { StatusCode = (int)HttpStatusCode.InternalServerError };
            }
        }

        /// <summary>
        /// Calls an OPC UA method on the server
        /// </summary>
        /// <param name="payload">The call operation payload containing expanded OPC UA node ID and a list of OPC UA Variant input arguments</param>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request, e.g. the format of the parameter is wrong.</response>
        /// <response code="401">Unauthorized, e.g. the server refused the authorization attempt.</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPost]
        [Route("/call")]
        [SwaggerOperation("MethodCall")]
        [SwaggerResponse(statusCode: 200, type: typeof(byte[]), description: "Success")]
        [SwaggerResponse(statusCode: 400, type: typeof(ActionResult), description: "Bad Request, e.g. the format of the parameter is wrong.")]
        [SwaggerResponse(statusCode: 401, type: typeof(ActionResult), description: "Unauthorized, e.g. the server refused the authorization attempt.")]
        [SwaggerResponse(statusCode: 403, type: typeof(ActionResult), description: "Forbidden")]
        [SwaggerResponse(statusCode: 404, type: typeof(ActionResult), description: "Not Found")]
        [SwaggerResponse(statusCode: 500, type: typeof(ActionResult), description: "Internal Server Error")]
        public virtual IActionResult MethodCall([FromBody][Required] CallPayload payload)
        {
            try
            {
                NamespaceTable namespaceTable = new NamespaceTable(FactoryStationServer.NodeManagerInstance?.NamespaceUris);
                ExpandedNodeId expandedNodeId = ExpandedNodeId.Parse(payload.ExpandedNodeId);
                MethodState? nodeState = FactoryStationServer.NodeManagerInstance?.Find(ExpandedNodeId.ToNodeId(expandedNodeId, namespaceTable)) as MethodState;
                if (nodeState != null)
                {
                    List<Variant> outputArguments = new();
                    List<ServiceResult> argumentErrors = new();

                    ServiceResult result = nodeState.Call(FactoryStationServer.NodeManagerInstance?.Server.DefaultSystemContext, null, payload.InputArguments, argumentErrors, outputArguments);
                    if (Opc.Ua.StatusCode.IsBad(result.StatusCode))
                    {
                        string hexString = "0x" + result.StatusCode.Code.ToString("X");
                        throw new Exception("Call failed with: " + hexString);
                    }

                    return new ObjectResult("Success") { StatusCode = (int)HttpStatusCode.OK };
                }
                else
                {
                    return new ObjectResult("Not found") { StatusCode = (int)HttpStatusCode.NotFound };
                }
            }
            catch (Exception ex)
            {
                return new ObjectResult(ex.Message) { StatusCode = (int)HttpStatusCode.InternalServerError };
            }
        }
    }
}
