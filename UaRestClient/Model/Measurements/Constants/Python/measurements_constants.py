from enum import Enum

class Namespaces(Enum):
     Uri = "urn:opcfoundation.org:2024-10:starterkit:measurements"

class BrowseNames(Enum):
    MeasurementContainerType = "MeasurementContainerType"
    Orientation = "Orientation"
    OrientationDataType = "OrientationDataType"
    Pressure = "Pressure"
    Reset = "Reset"
    Temperature = "Temperature"

class DataTypeIds(Enum):
    OrientationDataType = "nsu=urn:opcfoundation.org:2024-10:starterkit:measurements;i=31"

def get_DataTypeIds_name(value: str) -> str:
    try:
        return DataTypeIds(value).name
    except ValueError:
        return None


class MethodIds(Enum):
    MeasurementContainerType_Reset = "nsu=urn:opcfoundation.org:2024-10:starterkit:measurements;i=24"

def get_MethodIds_name(value: str) -> str:
    try:
        return MethodIds(value).name
    except ValueError:
        return None


class ObjectIds(Enum):
    OrientationDataType_Encoding_DefaultBinary = "nsu=urn:opcfoundation.org:2024-10:starterkit:measurements;i=32"
    OrientationDataType_Encoding_DefaultXml = "nsu=urn:opcfoundation.org:2024-10:starterkit:measurements;i=40"
    OrientationDataType_Encoding_DefaultJson = "nsu=urn:opcfoundation.org:2024-10:starterkit:measurements;i=48"

def get_ObjectIds_name(value: str) -> str:
    try:
        return ObjectIds(value).name
    except ValueError:
        return None


class ObjectTypeIds(Enum):
    MeasurementContainerType = "nsu=urn:opcfoundation.org:2024-10:starterkit:measurements;i=1"

def get_ObjectTypeIds_name(value: str) -> str:
    try:
        return ObjectTypeIds(value).name
    except ValueError:
        return None


class VariableIds(Enum):
    MeasurementContainerType_Temperature = "nsu=urn:opcfoundation.org:2024-10:starterkit:measurements;i=2"
    MeasurementContainerType_Temperature_EngineeringUnits = "nsu=urn:opcfoundation.org:2024-10:starterkit:measurements;i=7"
    MeasurementContainerType_Temperature_EngineeringUnits_EngineeringUnits = "nsu=urn:opcfoundation.org:2024-10:starterkit:measurements;i=12"
    MeasurementContainerType_Pressure = "nsu=urn:opcfoundation.org:2024-10:starterkit:measurements;i=13"
    MeasurementContainerType_Pressure_EngineeringUnits = "nsu=urn:opcfoundation.org:2024-10:starterkit:measurements;i=18"
    MeasurementContainerType_Pressure_EngineeringUnits_EngineeringUnits = "nsu=urn:opcfoundation.org:2024-10:starterkit:measurements;i=23"
    MeasurementContainerType_Reset_InputArguments = "nsu=urn:opcfoundation.org:2024-10:starterkit:measurements;i=25"
    MeasurementContainerType_Reset_OutputArguments = "nsu=urn:opcfoundation.org:2024-10:starterkit:measurements;i=26"
    MeasurementContainerType_Orientation = "nsu=urn:opcfoundation.org:2024-10:starterkit:measurements;i=27"

def get_VariableIds_name(value: str) -> str:
    try:
        return VariableIds(value).name
    except ValueError:
        return None

