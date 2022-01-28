using System;
using System.Text;
using System.Collections.Generic;

namespace Json
{
    public class JsonObject
    {
        public Dictionary<string, JsonValue> Maps { get; set; }
        public List<string> Keys { get; set; }

        public JsonObject()
        {
            Maps = new Dictionary<string, JsonValue>();
            Keys = new List<string>();
        }

        public JsonValue this[string index]
        {
            get
            {
                return Maps[index];
            }
            set
            {
                Maps[index] = value;
            }
        }

        public void Add(string key, JsonValue value)
        {
            if (!Maps.ContainsKey(key))
                Keys.Add(key);
            Maps.Add(key, value);
        }

        /// <summary>
        /// Parse string to a JsonObject object.
        /// </summary>
        public static JsonObject Parse(string str)
        {
            str = str.Trim();
            int index = 0;
            return Parse(ref str, ref index);
        }
        public static JsonObject Parse(ref string str, ref int index)
        {
            JsonObject jsonObject = new JsonObject();
            Format.SkipSpace(ref str, ref index);
            if (str[index] != '{')
                throw new Exception("Missing \"{\" at the head of a JsonObject.");
            index++;
            Format.SkipSpace(ref str, ref index);
            if (str[index] == '}')
            {
                index++;
                return jsonObject;
            }
            while (true)
            {
                //  Resolve name
                string key = Format.ReadUntil(ref str, ref index, ":");
                key = key.Trim();
                key = Format.Unescape(key.Substring(1, key.Length - 2));
                index++;
                //  Read Value
                JsonValue readValue = Format.ReadValue(ref str, ref index);
                jsonObject.Add(key, readValue);
                // Check if is the end
                Format.SkipSpace(ref str, ref index);
                if (str[index] == '}')
                {
                    index++;
                    return jsonObject;
                }
                index++;
                Format.SkipSpace(ref str, ref index);
            }
        }

        /// <summary>
        /// Conver this object to a unserialized string.
        /// </summary>
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            AppendStringTo(builder);
            return builder.ToString();
        }
        /// <summary>
        /// Append string to a StringBuilder object.
        /// </summary>
        public void AppendStringTo(StringBuilder builder)
        {
            builder.Append('{');
            int count = 0;
            foreach (var key in Keys)
            {
                builder.Append("\"" + Format.Escape(key) + "\": ");
                Maps[key].AppendStringTo(builder);
                count++;
                if (count != Keys.Count)
                {
                    builder.Append(", ");
                }
            }
            builder.Append('}');
        }

        /// <summary>
        /// Conver this object to a serialized string.
        /// </summary>
        public string Serialize(int depth = 0, string indentation = "  ")
        {
            StringBuilder builder = new StringBuilder();
            AppendSerializedTo(builder, depth, ref indentation);
            return builder.ToString();
        }
        /// <summary>
        /// Append serialized string to a StringBuilder object.
        /// </summary>
        public void AppendSerializedTo(StringBuilder builder, int depth, ref string indentation)
        {
            if (Keys.Count == 0)
            {
                builder.Append("{}");
                return;
            }
            builder.Append("{\n");
            int count = 0;
            foreach (var key in Keys)
            {
                for (int i = 0; i < depth + 1; i++)
                {
                    builder.Append(indentation);
                }
                builder.Append("\"" + Format.Escape(key) + "\": ");
                Maps[key].AppendSerializedTo(builder, depth + 1, ref indentation);
                count++;
                if (count != Keys.Count)
                {
                    builder.Append(',');
                }
                builder.Append('\n');
            }
            for (int i = 0; i < depth; i++)
            {
                builder.Append(indentation);
            }
            builder.Append('}');
        }
    }
}
