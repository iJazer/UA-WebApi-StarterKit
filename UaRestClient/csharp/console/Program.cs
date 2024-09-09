using UaRestClient;
using OpenApi.Opc.Ua;
using Org.OpenAPITools.Model;
using Newtonsoft.Json;

try
{
    var client = new OpcUaClient()
    {
        // BaseUrl = new Uri("https://opcua-rest-dashboard.azurewebsites.net/opcua/")
        BaseUrl = new Uri("https://localhost:44429/opcua/")
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

        var data = references.Where(x => x.BrowseName.EndsWith("Data")).FirstOrDefault();

        Console.WriteLine(Environment.NewLine);
        Console.WriteLine("==== Browsing Data");
        references = await client.BrowseChildren(data.NodeId);
        references?.ForEach(action: x => Console.WriteLine($"{x.DisplayName?.Text} {(NodeClass)x.NodeClass}"));

        Console.WriteLine(Environment.NewLine);
        Console.WriteLine("==== Read Data");
        var variableIds = references.Where(x => x.NodeClass == (uint)NodeClass.Variable).Select(x => x.NodeId).ToList();
        var values = await client.ReadValues(variableIds);

        List<WriteValue> valuesToWrite = new();

        for (int ii = 0; ii < variableIds.Count; ii++)
        {
            Console.WriteLine($"{references[ii].DisplayName?.Text} = {values[ii].Value.Body}");

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
        }

        Console.WriteLine(Environment.NewLine);
        Console.WriteLine("==== Write Data");
        var writeResults = await client.WriteValues(valuesToWrite);

        for (int ii = 0; ii < variableIds.Count; ii++)
        {
            Console.WriteLine($"{references[ii].DisplayName?.Text}: {StatusCodes.ToName(writeResults[ii])}");
        }

        Console.WriteLine(Environment.NewLine);
        Console.WriteLine("==== Read Back Data");
        values = await client.ReadValues(variableIds);

        for (int ii = 0; ii < variableIds.Count; ii++)
        {
            Console.WriteLine($"{references[ii].DisplayName?.Text} = {values[ii].Value.Body}");
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

        for (int ii = 0; ii < variableIds.Count; ii++)
        {
            Console.WriteLine($"{references[ii].DisplayName?.Text} = {values[ii].Value.Body}");
        }
    }
}
