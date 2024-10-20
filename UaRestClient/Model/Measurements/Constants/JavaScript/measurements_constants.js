export const NS = 'urn:opcfoundation.org:2024-10:starterkit:measurements';

export const BrowseNames = Object.freeze({
   MeasurementContainerType: 'MeasurementContainerType',
   Orientation: 'Orientation',
   OrientationDataType: 'OrientationDataType',
   Pressure: 'Pressure',
   Reset: 'Reset',
   Temperature: 'Temperature',
});

export const DataTypeIds = Object.freeze({
   OrientationDataType: 'nsu=' + NS + ';i=31',
});

export const MethodIds = Object.freeze({
   MeasurementContainerType_Reset: 'nsu=' + NS + ';i=24',
});

export const ObjectIds = Object.freeze({
   OrientationDataType_Encoding_DefaultBinary: 'nsu=' + NS + ';i=32',
   OrientationDataType_Encoding_DefaultXml: 'nsu=' + NS + ';i=40',
   OrientationDataType_Encoding_DefaultJson: 'nsu=' + NS + ';i=48',
});

export const ObjectTypeIds = Object.freeze({
   MeasurementContainerType: 'nsu=' + NS + ';i=1',
});

export const VariableIds = Object.freeze({
   MeasurementContainerType_Temperature: 'nsu=' + NS + ';i=2',
   MeasurementContainerType_Temperature_EngineeringUnits: 'nsu=' + NS + ';i=7',
   MeasurementContainerType_Temperature_EngineeringUnits_EngineeringUnits: 'nsu=' + NS + ';i=12',
   MeasurementContainerType_Pressure: 'nsu=' + NS + ';i=13',
   MeasurementContainerType_Pressure_EngineeringUnits: 'nsu=' + NS + ';i=18',
   MeasurementContainerType_Pressure_EngineeringUnits_EngineeringUnits: 'nsu=' + NS + ';i=23',
   MeasurementContainerType_Reset_InputArguments: 'nsu=' + NS + ';i=25',
   MeasurementContainerType_Reset_OutputArguments: 'nsu=' + NS + ';i=26',
   MeasurementContainerType_Orientation: 'nsu=' + NS + ';i=27',
});
