using UaRestClient;
using Opc.Ua.WebApi;
using Opc.Ua.WebApi.Model;
using System.Diagnostics.Metrics;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;

try
{
    var client = new OpcUaClient()
    {
        BaseUrl = new Uri("https://opcua-rest-dashboard.azurewebsites.net/opcua/")
        // BaseUrl = new Uri("https://localhost:44429/opcua/")
    };

    client.SetUserName("user1", "password1");

    client.Run().Wait();
}
catch (Exception e)
{
    Console.WriteLine("Exception when calling DefaultApi.ReadAsync: " + e.Message);
}

static class Demo
{
    public static async Task Run(this OpcUaClient client)
    {
        Console.WriteLine("==== Browsing ObjectsFolder");
        var references = await client.BrowseChildren(ObjectIds.ObjectsFolder);
        references?.ForEach(action: x => Console.WriteLine($"{x.DisplayName?.Text} {(NodeClass)x.NodeClass}"));

        var data = references.Where(x => x.BrowseName.EndsWith("Measurements")).FirstOrDefault();

        if (data == null)
        {
            Console.WriteLine("Measurements folder not found");
            return;
        }

        Console.WriteLine(Environment.NewLine);
        Console.WriteLine("==== Browsing Data");
        references = await client.BrowseChildren(data.NodeId);
        references?.ForEach(action: x => Console.WriteLine($"{x.DisplayName?.Text} {(NodeClass)x.NodeClass}"));

        Console.WriteLine(Environment.NewLine);
        Console.WriteLine("==== Read Data");
        var variableIds = references.Where(x => x.NodeClass == (uint)NodeClass.Variable).Select(x => x.NodeId).ToList();
        var values = await client.ReadValues(variableIds);

        List<WriteValue> valuesToWrite = new();
        List<ReferenceDescription> variables = new();

        for (int ii = 0; ii < variableIds.Count; ii++)
        {
            Console.WriteLine($"{references[ii].DisplayName?.Text} = {values[ii].Value.Body}");

            if (values[ii].Value.Body is not double)
            {
                continue;
            }

            valuesToWrite.Add(new WriteValue()
            {
                AttributeId = Attributes.Value,
                NodeId = variableIds[ii],
                Value = new DataValue()
                {
                    Value = new Variant()
                    {
                        Body = Convert.ToDouble(values[ii].Value.Body) + 1.0,
                        Type = (int)BuiltInType.Double
                    }
                }
            });

            variables.Add(references.Find(x => x.NodeId == variableIds[ii]));
        }

        Console.WriteLine(Environment.NewLine);
        Console.WriteLine("==== Write Data");
        var writeResults = await client.WriteValues(valuesToWrite);

        for (int ii = 0; ii < variables.Count; ii++)
        {
            Console.WriteLine($"{variables[ii].DisplayName?.Text}: {StatusCodes.ToName(writeResults[ii])}");
        }

        Console.WriteLine(Environment.NewLine);
        Console.WriteLine("==== Read Back Data");
        values = await client.ReadValues(variableIds);

        for (int ii = 0; ii < variables.Count; ii++)
        {
            Console.WriteLine($"{variables[ii].DisplayName?.Text} = {values[ii].Value.Body}");
        }

        var method = references.Where(x => x.NodeClass == (uint)NodeClass.Method).FirstOrDefault();

        Console.WriteLine(Environment.NewLine);
        Console.WriteLine("==== Browsing Method Arguments");
        var properties = await client.BrowseChildren(method.NodeId);
        var inputArgumentId = properties.Where(x => x.BrowseName == BrowseNames.InputArguments).Select(x => x.NodeId).FirstOrDefault();
        var outputArgumentId = properties.Where(x => x.BrowseName == BrowseNames.OutputArguments).Select(x => x.NodeId).FirstOrDefault();

        Console.WriteLine("==== Read Method Arguments");
        var arguments = await client.ReadValues([inputArgumentId, outputArgumentId]);
        var inputArguments = client.Deserialize<Argument>(arguments[0].Value);
        var outputArguments = client.Deserialize<Argument>(arguments[1].Value);

        Console.WriteLine($"{method.DisplayName?.Text}(");
        inputArguments?.ForEach(action: x => Console.WriteLine($"  [in]  {DataTypeIds.ToName(x.DataType)} {x.Name}"));
        outputArguments?.ForEach(action: x => Console.WriteLine($"  [out] {DataTypeIds.ToName(x.DataType)} {x.Name}"));
        Console.WriteLine($")");

        List<Variant> inputs =
        [
            new Variant() { Body = 40, Type = (int)BuiltInType.Double },
            new Variant() { Body = 90, Type = (int)BuiltInType.Double },
        ];

        var outputs = await client.Call(data.NodeId, method.NodeId, inputs);

        foreach (var output in outputs)
        {
            Console.WriteLine($"Output: {output.Body}");
        }

        Console.WriteLine(Environment.NewLine);
        Console.WriteLine("==== Read Back Data");
        values = await client.ReadValues(variableIds);

        for (int ii = 0; ii < variables.Count; ii++)
        {
            Console.WriteLine($"{variables[ii].DisplayName?.Text} = {values[ii].Value.Body}");
        }

        Console.WriteLine(Environment.NewLine);
        Console.WriteLine("==== Read Complex Data");
        var complexVariables = references.Where(x => x.BrowseName.Contains(Measurements.WebApi.BrowseNames.Orientation)).ToList();

        values = await client.ReadValues(complexVariables.Select(x => x.NodeId).ToList());
        valuesToWrite.Clear();

        for (int ii = 0; ii < complexVariables.Count; ii++)
        {
            if (values[ii].Value.Body is JObject json)
            {
                var orientation = json["Body"].ToObject<Measurements.Model.OrientationDataType>();

                Console.WriteLine($"   ProfileName = {orientation.ProfileName}");
                Console.WriteLine($"   X = {orientation.X}");
                Console.WriteLine($"   Y = {orientation.Y}");
                Console.WriteLine($"   Rotation = {orientation.Rotation}");

                orientation.X += 1.0;
                orientation.Y += 1.0;
                orientation.Rotation += 1.0;

                valuesToWrite.Add(new WriteValue()
                {
                    NodeId = complexVariables[ii].NodeId,
                    AttributeId = Attributes.Value,
                    Value = new DataValue()
                    {
                        Value = new Variant()
                        {
                            Type = (int)BuiltInType.ExtensionObject,
                            Body = JObject.Parse(new ExtensionObject(
                                typeId: Measurements.WebApi.DataTypeIds.OrientationDataType,
                                body: orientation
                            ).ToJson())
                        }
                    }
                });

                continue;
            }
        }

        Console.WriteLine(Environment.NewLine);
        Console.WriteLine("==== Write Complex Data");
        writeResults = await client.WriteValues(valuesToWrite);

        for (int ii = 0; ii < complexVariables.Count; ii++)
        {
            Console.WriteLine($"{complexVariables[ii].DisplayName?.Text}: {StatusCodes.ToName(writeResults[ii])}");
        }

        Console.WriteLine(Environment.NewLine);
        Console.WriteLine("==== Read Back Complex Data");
        values = await client.ReadValues(complexVariables.Select(x => x.NodeId).ToList());

        for (int ii = 0; ii < complexVariables.Count; ii++)
        {
            Console.WriteLine($"{complexVariables[ii].DisplayName?.Text} = {values[ii].Value.Body}");
        }
    }
}