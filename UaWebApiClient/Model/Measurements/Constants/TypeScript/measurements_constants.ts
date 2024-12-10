export const NS = 'urn:opcfoundation.org:2024-10:starterkit:measurements';

export class BrowseNames {
   static readonly MeasurementContainerType: string = 'MeasurementContainerType'
   static readonly Orientation: string = 'Orientation'
   static readonly OrientationDataType: string = 'OrientationDataType'
   static readonly Pressure: string = 'Pressure'
   static readonly Reset: string = 'Reset'
   static readonly Temperature: string = 'Temperature'
}

export class DataTypeIds {
    static readonly OrientationDataType: string = 'nsu=' + NS + ';i=31'
}

export class MethodIds {
    static readonly MeasurementContainerType_Reset: string = 'nsu=' + NS + ';i=24'
}

export class ObjectIds {
    static readonly OrientationDataType_Encoding_DefaultBinary: string = 'nsu=' + NS + ';i=32'
    static readonly OrientationDataType_Encoding_DefaultXml: string = 'nsu=' + NS + ';i=40'
    static readonly OrientationDataType_Encoding_DefaultJson: string = 'nsu=' + NS + ';i=48'
}

export class ObjectTypeIds {
    static readonly MeasurementContainerType: string = 'nsu=' + NS + ';i=1'
}

export class VariableIds {
    static readonly MeasurementContainerType_Temperature: string = 'nsu=' + NS + ';i=2'
    static readonly MeasurementContainerType_Temperature_EngineeringUnits: string = 'nsu=' + NS + ';i=7'
    static readonly MeasurementContainerType_Temperature_EngineeringUnits_EngineeringUnits: string = 'nsu=' + NS + ';i=12'
    static readonly MeasurementContainerType_Pressure: string = 'nsu=' + NS + ';i=13'
    static readonly MeasurementContainerType_Pressure_EngineeringUnits: string = 'nsu=' + NS + ';i=18'
    static readonly MeasurementContainerType_Pressure_EngineeringUnits_EngineeringUnits: string = 'nsu=' + NS + ';i=23'
    static readonly MeasurementContainerType_Reset_InputArguments: string = 'nsu=' + NS + ';i=25'
    static readonly MeasurementContainerType_Reset_OutputArguments: string = 'nsu=' + NS + ';i=26'
    static readonly MeasurementContainerType_Orientation: string = 'nsu=' + NS + ';i=27'
}
