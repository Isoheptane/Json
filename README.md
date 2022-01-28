# Json
This is a simple JSON library for C#.  
This library provided three types: `JsonObject`, `JsonArray`, `JsonValue`.

## JsonObject
Example:
```csharp
JsonObject obj = JsonObject.Parse(File.ReadAllText("file.json"));
// Read value
string name = (string)obj["name"];
JsonObject info = (JsonObject)obj["info"];
JsonValue more = obj["more"];
// Write value
obj["name"] = name;
obj["info"] = info;
obj["more"] = value;
```

## JsonArray
Example:
```csharp
JsonArray array = JsonArray.Parse(File.ReadAllText("file.json"));
// Read value
int element1 = (int)array[0];
// Write value
array[0] = element1;
```

## JsonValue
Example:
```csharp
JsonValue value = 123;
value = "string";
value = JsonObject.Parse(File.ReadAllText("file.json"));
string firstName = value["name"]["firstName"];
```
