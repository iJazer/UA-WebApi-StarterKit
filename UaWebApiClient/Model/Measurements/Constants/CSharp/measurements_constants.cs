namespace Measurements.WebApi
{
    /// <summary>
    /// The namespaces used in the model.
    /// </summary>
    public static class Namespaces
    {
        /// <remarks />
        public const string Uri = "urn:opcfoundation.org:2024-10:starterkit:measurements";
    }

    /// <summary>
    /// The browse names defined in the model.
    /// </summary>
    public static class BrowseNames
    {
        /// <remarks />
        public const string MeasurementContainerType = "MeasurementContainerType";
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

    /// <summary>
    /// The well known identifiers for DataType nodes.
    /// </summary>
    public static class DataTypeIds {
        /// <remarks />
        public const string OrientationDataType = "nsu=" + Namespaces.Uri + ";i=31";

        /// <summary>
        /// Converts a value to a name for display.
        /// </summary>
        public static string ToName(string value)
        {
            foreach (var field in typeof(DataTypeIds).GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static))
            {
                if (field.GetValue(null).Equals(value))
                {
                    return field.Name;
                }
            }

            return value.ToString();
        }
    }

    /// <summary>
    /// The well known identifiers for Method nodes.
    /// </summary>
    public static class MethodIds {
        /// <remarks />
        public const string MeasurementContainerType_Reset = "nsu=" + Namespaces.Uri + ";i=24";

        /// <summary>
        /// Converts a value to a name for display.
        /// </summary>
        public static string ToName(string value)
        {
            foreach (var field in typeof(MethodIds).GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static))
            {
                if (field.GetValue(null).Equals(value))
                {
                    return field.Name;
                }
            }

            return value.ToString();
        }
    }

    /// <summary>
    /// The well known identifiers for Object nodes.
    /// </summary>
    public static class ObjectIds {
        /// <remarks />
        public const string OrientationDataType_Encoding_DefaultBinary = "nsu=" + Namespaces.Uri + ";i=32";
        /// <remarks />
        public const string OrientationDataType_Encoding_DefaultXml = "nsu=" + Namespaces.Uri + ";i=40";
        /// <remarks />
        public const string OrientationDataType_Encoding_DefaultJson = "nsu=" + Namespaces.Uri + ";i=48";

        /// <summary>
        /// Converts a value to a name for display.
        /// </summary>
        public static string ToName(string value)
        {
            foreach (var field in typeof(ObjectIds).GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static))
            {
                if (field.GetValue(null).Equals(value))
                {
                    return field.Name;
                }
            }

            return value.ToString();
        }
    }

    /// <summary>
    /// The well known identifiers for ObjectType nodes.
    /// </summary>
    public static class ObjectTypeIds {
        /// <remarks />
        public const string MeasurementContainerType = "nsu=" + Namespaces.Uri + ";i=1";

        /// <summary>
        /// Converts a value to a name for display.
        /// </summary>
        public static string ToName(string value)
        {
            foreach (var field in typeof(ObjectTypeIds).GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static))
            {
                if (field.GetValue(null).Equals(value))
                {
                    return field.Name;
                }
            }

            return value.ToString();
        }
    }

    /// <summary>
    /// The well known identifiers for Variable nodes.
    /// </summary>
    public static class VariableIds {
        /// <remarks />
        public const string MeasurementContainerType_Temperature = "nsu=" + Namespaces.Uri + ";i=2";
        /// <remarks />
        public const string MeasurementContainerType_Temperature_EngineeringUnits = "nsu=" + Namespaces.Uri + ";i=7";
        /// <remarks />
        public const string MeasurementContainerType_Temperature_EngineeringUnits_EngineeringUnits = "nsu=" + Namespaces.Uri + ";i=12";
        /// <remarks />
        public const string MeasurementContainerType_Pressure = "nsu=" + Namespaces.Uri + ";i=13";
        /// <remarks />
        public const string MeasurementContainerType_Pressure_EngineeringUnits = "nsu=" + Namespaces.Uri + ";i=18";
        /// <remarks />
        public const string MeasurementContainerType_Pressure_EngineeringUnits_EngineeringUnits = "nsu=" + Namespaces.Uri + ";i=23";
        /// <remarks />
        public const string MeasurementContainerType_Reset_InputArguments = "nsu=" + Namespaces.Uri + ";i=25";
        /// <remarks />
        public const string MeasurementContainerType_Reset_OutputArguments = "nsu=" + Namespaces.Uri + ";i=26";
        /// <remarks />
        public const string MeasurementContainerType_Orientation = "nsu=" + Namespaces.Uri + ";i=27";

        /// <summary>
        /// Converts a value to a name for display.
        /// </summary>
        public static string ToName(string value)
        {
            foreach (var field in typeof(VariableIds).GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static))
            {
                if (field.GetValue(null).Equals(value))
                {
                    return field.Name;
                }
            }

            return value.ToString();
        }
    }
    
}