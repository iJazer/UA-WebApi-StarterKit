/* ========================================================================
 * Copyright (c) 2005-2024 The OPC Foundation, Inc. All rights reserved.
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

using Opc.Ua;

namespace Measurements
{
    #region DataType Identifiers
    /// <remarks />
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    public static partial class DataTypes
    {
        /// <remarks />
        public const uint OrientationDataType = 31;
    }
    #endregion

    #region Method Identifiers
    /// <remarks />
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    public static partial class Methods
    {
        /// <remarks />
        public const uint MeasurementContainerType_Reset = 24;
    }
    #endregion

    #region Object Identifiers
    /// <remarks />
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    public static partial class Objects
    {
        /// <remarks />
        public const uint OrientationDataType_Encoding_DefaultBinary = 32;

        /// <remarks />
        public const uint OrientationDataType_Encoding_DefaultXml = 40;

        /// <remarks />
        public const uint OrientationDataType_Encoding_DefaultJson = 48;
    }
    #endregion

    #region ObjectType Identifiers
    /// <remarks />
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    public static partial class ObjectTypes
    {
        /// <remarks />
        public const uint MeasurementContainerType = 1;
    }
    #endregion

    #region Variable Identifiers
    /// <remarks />
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    public static partial class Variables
    {
        /// <remarks />
        public const uint MeasurementContainerType_Temperature = 2;

        /// <remarks />
        public const uint MeasurementContainerType_Temperature_EURange = 6;

        /// <remarks />
        public const uint MeasurementContainerType_Temperature_EngineeringUnits = 7;

        /// <remarks />
        public const uint MeasurementContainerType_Pressure = 13;

        /// <remarks />
        public const uint MeasurementContainerType_Pressure_EURange = 17;

        /// <remarks />
        public const uint MeasurementContainerType_Pressure_EngineeringUnits = 18;

        /// <remarks />
        public const uint MeasurementContainerType_Reset_InputArguments = 25;

        /// <remarks />
        public const uint MeasurementContainerType_Reset_OutputArguments = 26;

        /// <remarks />
        public const uint MeasurementContainerType_Orientation = 27;

        /// <remarks />
        public const uint Measurements_BinarySchema = 49;

        /// <remarks />
        public const uint Measurements_BinarySchema_NamespaceUri = 51;

        /// <remarks />
        public const uint Measurements_BinarySchema_Deprecated = 52;

        /// <remarks />
        public const uint Measurements_BinarySchema_OrientationDataType = 53;

        /// <remarks />
        public const uint Measurements_XmlSchema = 56;

        /// <remarks />
        public const uint Measurements_XmlSchema_NamespaceUri = 58;

        /// <remarks />
        public const uint Measurements_XmlSchema_Deprecated = 59;

        /// <remarks />
        public const uint Measurements_XmlSchema_OrientationDataType = 60;
    }
    #endregion

    #region DataType Node Identifiers
    /// <remarks />
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    public static partial class DataTypeIds
    {
        /// <remarks />
        public static readonly ExpandedNodeId OrientationDataType = new ExpandedNodeId(Measurements.DataTypes.OrientationDataType, Measurements.Namespaces.Measurements);
    }
    #endregion

    #region Method Node Identifiers
    /// <remarks />
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    public static partial class MethodIds
    {
        /// <remarks />
        public static readonly ExpandedNodeId MeasurementContainerType_Reset = new ExpandedNodeId(Measurements.Methods.MeasurementContainerType_Reset, Measurements.Namespaces.Measurements);
    }
    #endregion

    #region Object Node Identifiers
    /// <remarks />
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    public static partial class ObjectIds
    {
        /// <remarks />
        public static readonly ExpandedNodeId OrientationDataType_Encoding_DefaultBinary = new ExpandedNodeId(Measurements.Objects.OrientationDataType_Encoding_DefaultBinary, Measurements.Namespaces.Measurements);

        /// <remarks />
        public static readonly ExpandedNodeId OrientationDataType_Encoding_DefaultXml = new ExpandedNodeId(Measurements.Objects.OrientationDataType_Encoding_DefaultXml, Measurements.Namespaces.Measurements);

        /// <remarks />
        public static readonly ExpandedNodeId OrientationDataType_Encoding_DefaultJson = new ExpandedNodeId(Measurements.Objects.OrientationDataType_Encoding_DefaultJson, Measurements.Namespaces.Measurements);
    }
    #endregion

    #region ObjectType Node Identifiers
    /// <remarks />
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    public static partial class ObjectTypeIds
    {
        /// <remarks />
        public static readonly ExpandedNodeId MeasurementContainerType = new ExpandedNodeId(Measurements.ObjectTypes.MeasurementContainerType, Measurements.Namespaces.Measurements);
    }
    #endregion

    #region Variable Node Identifiers
    /// <remarks />
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    public static partial class VariableIds
    {
        /// <remarks />
        public static readonly ExpandedNodeId MeasurementContainerType_Temperature = new ExpandedNodeId(Measurements.Variables.MeasurementContainerType_Temperature, Measurements.Namespaces.Measurements);

        /// <remarks />
        public static readonly ExpandedNodeId MeasurementContainerType_Temperature_EURange = new ExpandedNodeId(Measurements.Variables.MeasurementContainerType_Temperature_EURange, Measurements.Namespaces.Measurements);

        /// <remarks />
        public static readonly ExpandedNodeId MeasurementContainerType_Temperature_EngineeringUnits = new ExpandedNodeId(Measurements.Variables.MeasurementContainerType_Temperature_EngineeringUnits, Measurements.Namespaces.Measurements);

        /// <remarks />
        public static readonly ExpandedNodeId MeasurementContainerType_Pressure = new ExpandedNodeId(Measurements.Variables.MeasurementContainerType_Pressure, Measurements.Namespaces.Measurements);

        /// <remarks />
        public static readonly ExpandedNodeId MeasurementContainerType_Pressure_EURange = new ExpandedNodeId(Measurements.Variables.MeasurementContainerType_Pressure_EURange, Measurements.Namespaces.Measurements);

        /// <remarks />
        public static readonly ExpandedNodeId MeasurementContainerType_Pressure_EngineeringUnits = new ExpandedNodeId(Measurements.Variables.MeasurementContainerType_Pressure_EngineeringUnits, Measurements.Namespaces.Measurements);

        /// <remarks />
        public static readonly ExpandedNodeId MeasurementContainerType_Reset_InputArguments = new ExpandedNodeId(Measurements.Variables.MeasurementContainerType_Reset_InputArguments, Measurements.Namespaces.Measurements);

        /// <remarks />
        public static readonly ExpandedNodeId MeasurementContainerType_Reset_OutputArguments = new ExpandedNodeId(Measurements.Variables.MeasurementContainerType_Reset_OutputArguments, Measurements.Namespaces.Measurements);

        /// <remarks />
        public static readonly ExpandedNodeId MeasurementContainerType_Orientation = new ExpandedNodeId(Measurements.Variables.MeasurementContainerType_Orientation, Measurements.Namespaces.Measurements);

        /// <remarks />
        public static readonly ExpandedNodeId Measurements_BinarySchema = new ExpandedNodeId(Measurements.Variables.Measurements_BinarySchema, Measurements.Namespaces.Measurements);

        /// <remarks />
        public static readonly ExpandedNodeId Measurements_BinarySchema_NamespaceUri = new ExpandedNodeId(Measurements.Variables.Measurements_BinarySchema_NamespaceUri, Measurements.Namespaces.Measurements);

        /// <remarks />
        public static readonly ExpandedNodeId Measurements_BinarySchema_Deprecated = new ExpandedNodeId(Measurements.Variables.Measurements_BinarySchema_Deprecated, Measurements.Namespaces.Measurements);

        /// <remarks />
        public static readonly ExpandedNodeId Measurements_BinarySchema_OrientationDataType = new ExpandedNodeId(Measurements.Variables.Measurements_BinarySchema_OrientationDataType, Measurements.Namespaces.Measurements);

        /// <remarks />
        public static readonly ExpandedNodeId Measurements_XmlSchema = new ExpandedNodeId(Measurements.Variables.Measurements_XmlSchema, Measurements.Namespaces.Measurements);

        /// <remarks />
        public static readonly ExpandedNodeId Measurements_XmlSchema_NamespaceUri = new ExpandedNodeId(Measurements.Variables.Measurements_XmlSchema_NamespaceUri, Measurements.Namespaces.Measurements);

        /// <remarks />
        public static readonly ExpandedNodeId Measurements_XmlSchema_Deprecated = new ExpandedNodeId(Measurements.Variables.Measurements_XmlSchema_Deprecated, Measurements.Namespaces.Measurements);

        /// <remarks />
        public static readonly ExpandedNodeId Measurements_XmlSchema_OrientationDataType = new ExpandedNodeId(Measurements.Variables.Measurements_XmlSchema_OrientationDataType, Measurements.Namespaces.Measurements);
    }
    #endregion

    #region BrowseName Declarations
    /// <remarks />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    public static partial class BrowseNames
    {
        /// <remarks />
        public const string MeasurementContainerType = "MeasurementContainerType";

        /// <remarks />
        public const string Measurements_BinarySchema = "Measurements";

        /// <remarks />
        public const string Measurements_XmlSchema = "Measurements";

        /// <remarks />
        public const string Orientation = "Orientation";

        /// <remarks />
        public const string OrientationDataType = "OrientationDataType";

        /// <remarks />
        public const string Pressure = "Pressure";

        /// <remarks />
        public const string Reset = "Reset";

        /// <remarks />
        public const string Temperature = "Temperature";
    }
    #endregion

    #region Namespace Declarations
    /// <remarks />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    public static partial class Namespaces
    {
        /// <summary>
        /// The URI for the Measurements namespace (.NET code namespace is 'Measurements').
        /// </summary>
        public const string Measurements = "urn:opcfoundation.org:2024-10:starterkit:measurements";

        /// <summary>
        /// The URI for the OpcUa namespace (.NET code namespace is 'Opc.Ua').
        /// </summary>
        public const string OpcUa = "http://opcfoundation.org/UA/";

        /// <summary>
        /// The URI for the OpcUaXsd namespace (.NET code namespace is 'Opc.Ua').
        /// </summary>
        public const string OpcUaXsd = "http://opcfoundation.org/UA/2008/02/Types.xsd";
    }
    #endregion
}