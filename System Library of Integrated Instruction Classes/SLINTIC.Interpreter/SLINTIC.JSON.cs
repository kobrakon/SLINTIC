using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

// down with newtonsoft
namespace SLINTIC.Interpreter
{
    public static class JSON
    {
        public static Type DeserializeJSON(string path)
        {
            if (!File.Exists(path)) { throw new NullReferenceException($"JSON Deserialization Error: Couldn't deserialize {path}, the file doesn't exist"); }
            var file = File.ReadAllLines(path); if (file is null) { throw new NullReferenceException($"JSON Deserialization Error: Couldn't deserialize {path}, there is nothing to deserialize."); }
            AssemblyBuilder asm = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName("JsonAsm"), AssemblyBuilderAccess.Run);
            ModuleBuilder mod = asm.DefineDynamicModule("JsonMod");
            TypeBuilder json = mod.DefineType("JSON");
            int shift = 1;

            (from l in file where l.Split(":")[1] is not null && l.Split(":")[1].Contains("{") && l.Split(":")[1].Contains("[") select l).ToList().ForEach((string l) => json.DefineField($"{l.Split(":")[0]}", typeof(object), FieldAttributes.Public | FieldAttributes.Static).SetConstant(l.Split(":")[1]));

            foreach (string s in file)
            {
                if (s.Contains("{") || s == "{" && s != file[0])
                {
                    List<string> d = new();
                    while (file[Array.IndexOf(file, s) + shift] != "}")
                    {
                        string b = file[Array.IndexOf(file, s) + shift];
                        d.Add(b);
                        shift++;
                        if (file[Array.IndexOf(file, s) + shift] == "}") break;
                    }
                    (from l in d where l.Split(":")[1] != null select l).ToList().ForEach((l) => json.DefineField($"{l.Split(":")[0]}", typeof(object), FieldAttributes.Public | FieldAttributes.Static).SetConstant(l.Split(":")[1]));
                }

                if (s.Contains("[") || s == "[")
                {
                    FieldBuilder a = json.DefineField($"{file[Array.IndexOf(file, s) + shift].Split(":")[0]}", typeof(object[]), FieldAttributes.Public | FieldAttributes.Static);
                    List<object> arraycont = new();

                    while (file[Array.IndexOf(file, s) + shift] != "]")
                    {
                        shift++;
                        string e = file[Array.IndexOf(file, s) + shift];
                        arraycont.Add(e);
                    }
                    shift = 1;
                    a.SetConstant(arraycont.ToArray());
                }
            }
            json.CreateType();
            return json;
        }
    }
}


/*/
example of a JSON that can be 

{
    "ExampleString" : "exampleString",
    "ExampleBool" : true,
    "ExampleInt": 32,
    "ExampleArray" : 
    [
        "Example1",
        "Example2"
    ]
    "ExampleBlock": 
    {
        "ExampleBlockBlock": 
        {
            "string": "string",
            "bool": true
        }
        "ExampleBlockArray": 
        [
            "fas": true,
            "int": 32,
            "sfije": "string"
        ]
        "ExampleBlockInt": 69
    }
}

/**/