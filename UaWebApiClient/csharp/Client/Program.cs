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
            Console.WriteLine($"{references[ii].DisplayName?.Text} = {values[ii].Value}");

            if (values[ii].Value is not double)
            {
                continue;
            }

            valuesToWrite.Add(new WriteValue()
            {
                AttributeId = Attributes.Value,
                NodeId = variableIds[ii],
                Value = new DataValue()
                {
                    UaType = (int)BuiltInType.Double,
                    Value = Convert.ToDouble(values[ii].Value) + 1.0
                }
            });

            variables.Add(references.Find(x => x.NodeId == variableIds[ii]));
        }

        Console.WriteLine(Environment.NewLine);
        Console.WriteLine("==== Write Data");
        var writeResults = await client.WriteValues(valuesToWrite);

        for (int ii = 0; ii < variables.Count; ii++)
        {
            Console.WriteLine($"{variables[ii].DisplayName?.Text}: {StatusUtils.ToText(writeResults[ii])}");
        }

        Console.WriteLine(Environment.NewLine);
        Console.WriteLine("==== Read Back Data");
        values = await client.ReadValues(variableIds);

        for (int ii = 0; ii < variables.Count; ii++)
        {
            Console.WriteLine($"{variables[ii].DisplayName?.Text} = {values[ii].Value}");
        }

        var method = references.Where(x => x.NodeClass == (uint)NodeClass.Method).FirstOrDefault();

        Console.WriteLine(Environment.NewLine);
        Console.WriteLine("==== Browsing Method Arguments");
        var properties = await client.BrowseChildren(method.NodeId);
        var inputArgumentId = properties.Where(x => x.BrowseName == BrowseNames.InputArguments).Select(x => x.NodeId).FirstOrDefault();
        var outputArgumentId = properties.Where(x => x.BrowseName == BrowseNames.OutputArguments).Select(x => x.NodeId).FirstOrDefault();

        Console.WriteLine("==== Read Method Arguments");
        var arguments = await client.ReadValues([inputArgumentId, outputArgumentId]);
        var inputArguments = client.Deserialize<Argument>(arguments[0]);
        var outputArguments = client.Deserialize<Argument>(arguments[1]);

        Console.WriteLine($"{method.DisplayName?.Text}(");
        inputArguments?.ForEach(action: x => Console.WriteLine($"  [in]  {DataTypeIds.ToName(x.DataType)} {x.Name}"));
        outputArguments?.ForEach(action: x => Console.WriteLine($"  [out] {DataTypeIds.ToName(x.DataType)} {x.Name}"));
        Console.WriteLine($")");

        List<Variant> inputs =
        [
            new Variant() { Value = 40, UaType = (int)BuiltInType.Double },
            new Variant() { Value = 90, UaType = (int)BuiltInType.Double },
        ];

        var outputs = await client.Call(data.NodeId, method.NodeId, inputs);

        foreach (var output in outputs)
        {
            Console.WriteLine($"Output: {output.Value}");
        }

        Console.WriteLine(Environment.NewLine);
        Console.WriteLine("==== Read Back Data");
        values = await client.ReadValues(variableIds);

        for (int ii = 0; ii < variables.Count; ii++)
        {
            Console.WriteLine($"{variables[ii].DisplayName?.Text} = {values[ii].Value}");
        }

        Console.WriteLine(Environment.NewLine);
        Console.WriteLine("==== Read Complex Data");
        var complexVariables = references.Where(x => x.BrowseName.Contains(Measurements.WebApi.BrowseNames.Orientation)).ToList();

        values = await client.ReadValues(complexVariables.Select(x => x.NodeId).ToList());
        valuesToWrite.Clear();

        for (int ii = 0; ii < complexVariables.Count; ii++)
        {
            Measurements.Model.OrientationDataType orientation = new();

            if (values[ii].Value is JObject json)
            {
                orientation = json.ToObject<Measurements.Model.OrientationDataType>();

                Console.WriteLine($"   ProfileName = {orientation.ProfileName}");
                Console.WriteLine($"   X = {orientation.X}");
                Console.WriteLine($"   Y = {orientation.Y}");
                Console.WriteLine($"   Rotation = {orientation.Rotation}");
            }

            orientation.X += 1.0;
            orientation.Y += 1.0;
            orientation.Rotation += 1.0;

            var body = JObject.FromObject(orientation);
            body.AddFirst(new JProperty("UaTypeId", Measurements.WebApi.DataTypeIds.OrientationDataType));

            valuesToWrite.Add(new WriteValue()
            {
                NodeId = complexVariables[ii].NodeId,
                AttributeId = Attributes.Value,
                Value = new DataValue()
                {
                    UaType = (int)BuiltInType.ExtensionObject,
                    Value = body
                }
            });
        }

        Console.WriteLine(Environment.NewLine);
        Console.WriteLine("==== Write Complex Data");
        writeResults = await client.WriteValues(valuesToWrite);

        for (int ii = 0; ii < complexVariables.Count; ii++)
        {
            Console.WriteLine($"{complexVariables[ii].DisplayName?.Text}: {StatusUtils.ToText(writeResults[ii])}");
        }

        Console.WriteLine(Environment.NewLine);
        Console.WriteLine("==== Read Back Complex Data");
        values = await client.ReadValues(complexVariables.Select(x => x.NodeId).ToList());

        for (int ii = 0; ii < complexVariables.Count; ii++)
        {
            Console.WriteLine($"{complexVariables[ii].DisplayName?.Text} = {values[ii].Value}");
        }
    }
}