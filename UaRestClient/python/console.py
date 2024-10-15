from typing import List

from opcua_webapi import *
from opcua_client import OpcUaClient
from Measurements.Measurements import OrientationDataType
from measurements_constants import BrowseNames as Measurements_BrowseNames, DataTypeIds as Measurements_DataTypeIds

def variant_to_argument_list(input : Variant) -> List[Argument]:    
    output = []
    if input is not None and input.type == BuiltInType.ExtensionObject.value:
        if input.body is not None and isinstance(input.body, list):
            if input.type == BuiltInType.ExtensionObject.value:
                eo = [ExtensionObject.model_validate(x) for x in input.body]
                for x in eo:
                    if x.type_id == DataTypeIds.Argument.value:
                        output.append(Argument.model_validate(x.body))
    return output

useLocalServer = False
url = "https://localhost:44429/opcua/" if useLocalServer else "https://opcua-rest-dashboard.azurewebsites.net/opcua/"

configuration = Configuration(
    host=url,
    username='user1',
    password='password1'
)

configuration.verify_ssl = useLocalServer == False

api = OpcUaClient(configuration)    

try:

    print("==== Browsing ObjectsFolder")
    references = api.browse_children(ObjectIds.ObjectsFolder.value);        
    if references is not None:
        for x in references:
            print(f"{x.display_name.text if x.display_name else ''} {NodeClass(x.node_class)}")
    
    data = next((x for x in references if x.browse_name.endswith("Measurements")), None)

    print()
    print("==== Browsing Data")
    references = api.browse_children(data.node_id);        
    if references is not None:
        for x in references:
            print(f"{x.display_name.text if x.display_name else ''} {NodeClass(x.node_class)}")

    nodes = {x.node_id: x for x in references}

    print()
    print("==== Read Data")
    variable_ids = [x.node_id for x in references if x.node_class == NodeClass.Variable]
    values = api.read_values(variable_ids);   

    values_to_write = []
    if values is not None:
        for ii in range(len(variable_ids)):
            value = x = values[ii].value.body if values[ii].value is not None else None
            print(f"{nodes[variable_ids[ii]].display_name.text if nodes[variable_ids[ii]].display_name else None} = {value}")
            if value is not None and values[ii].value.type == BuiltInType.Double.value:
                values_to_write.append(WriteValue(
                    NodeId = variable_ids[ii],
                    AttributeId = Attributes.Value.value,
                    Value = DataValue(
                        Value = Variant(
                            Body = float(values[ii].value.body) + 1.0,
                            Type = int(BuiltInType.Double.value)
                        )
                    )
                ))

    print()
    print("==== Write Data")
    write_results = api.write_values(values_to_write)

    if write_results is not None:
        for ii in range(len(write_results)):
            print(f"{nodes[variable_ids[ii]].display_name.text if nodes[variable_ids[ii]].display_name else None} = {StatusCodes(write_results[ii]).name}")

    print()
    print("==== Read Back Data")
    variable_ids = [x.node_id for x in values_to_write]
    values = api.read_values(variable_ids)
    
    if values is not None:
        for ii in range(len(variable_ids)):
            print(f"{nodes[variable_ids[ii]].display_name.text if nodes[variable_ids[ii]].display_name else None} = {values[ii].value.body}")

    method = [x for x in references if x.node_class == NodeClass.Method][0]

    print()
    print("==== Browsing Method Arguments")
    properties = api.browse_children(method.node_id)
    input_argument_id = next((x.node_id for x in properties if x.browse_name == BrowseNames.InputArguments.value), None)
    output_argument_id = next((x.node_id for x in properties if x.browse_name == BrowseNames.OutputArguments.value), None)

    print()
    print("==== Read Method Arguments")
    arguments = api.read_values([input_argument_id, output_argument_id])
    input_arguments = variant_to_argument_list(arguments[0].value)
    output_arguments = variant_to_argument_list(arguments[1].value)

    print(f"{method.display_name.text if method.display_name else None}(")
    if input_arguments is not None:
        for x in input_arguments:
            print(f"  [in]  {DataTypeIds(x.data_type).name} {x.name}")
    if output_arguments is not None:
        for x in output_arguments:
            print(f"  [out] {DataTypeIds(x.data_type).name} {x.name}")
    print(f");")

    inputs = [
        Variant(Body=float(40),Type=int(BuiltInType.Double.value)),
        Variant(Body=float(80),Type=int(BuiltInType.Double.value))
    ]

    print()
    print("==== Call Method")
    outputs = api.call(data.node_id, method.node_id, inputs)

    if outputs is not None:
        for ii in range(len(output_arguments)):
            print(f"{output_arguments[ii].name if output_arguments[ii].name else None} = {outputs[ii].body}")

    print()
    print("==== Read Back Data")
    values = api.read_values(variable_ids)
    
    if values is not None:
        for ii in range(len(variable_ids)):
            print(f"{references[ii].display_name.text if references[ii].display_name else None} = {values[ii].value.body}")

    print()
    print("==== Read Complex Data")
    variable_ids = [x.node_id for x in references if x.browse_name.endswith(Measurements_BrowseNames.Orientation.value)]
    values = api.read_values(variable_ids);   

    values_to_write = []

    for ii, variable_id in enumerate(variable_ids):
        value = values[ii]
        orientation = OrientationDataType(ProfileName='Default', X=1, Y=1, Rotation=45) 

        if value.value is not None and isinstance(value.value.body, dict):  
            eo = ExtensionObject.model_validate(value.value.body)
            if eo.type_id == Measurements_DataTypeIds.OrientationDataType.value:
                orientation = OrientationDataType.model_validate(eo.body)

                print(f"   ProfileName = {orientation.profile_name}")
                print(f"   X = {orientation.x}")
                print(f"   Y = {orientation.y}")
                print(f"   Rotation = {orientation.rotation}")

        orientation.x += 1.0
        orientation.y += 1.0
        orientation.rotation += 1.0

        values_to_write.append(WriteValue(
            NodeId = variable_id,
            AttributeId=Attributes.Value.value,
            Value=DataValue(
                Value=Variant(
                    Type=BuiltInType.ExtensionObject.value,
                    Body=ExtensionObject(
                        TypeId=Measurements_DataTypeIds.OrientationDataType.value,
                        Body=orientation.to_dict()
                    ) 
                )
            )
        ))

    print()
    print("==== Complex Write Data")
    write_results = api.write_values(values_to_write)

    if write_results is not None:
        for ii in range(len(write_results)):
            print(f"{nodes[variable_ids[ii]].display_name.text if nodes[variable_ids[ii]].display_name else None} = {StatusCodes(write_results[ii]).name}")

    print()
    print("==== Complex Read Back Data")
    values = api.read_values(variable_ids)   

    for ii, complex_var in enumerate(variable_ids):       
       value = x = values[ii].value.body if values[ii].value is not None else None
       print(f"{nodes[variable_ids[ii]].display_name.text if nodes[variable_ids[ii]].display_name else None} = {value}")

except ApiException as e:
    print("Could not connect to server:", e)
