using System;
using System.Text;
using System.Collections.Generic;

namespace Json
{
    public class JsonArray
    {

        public List<JsonValue> List { get; set; }

        public JsonArray()
        {
            List = new List<JsonValue>();
        }

        public JsonValue this[int index] 
        {
            get 
            {
                return List[index]; 
            }
            set
            {
                List[index] = value;
            }
        }

        public void Add(JsonValue value)
        {
            List.Add(value);
        }

        /// <summary>
        /// Parse string to a JsonArray object.
        /// </summary>
        public static JsonArray Parse(string str)
        {
            str = str.Trim();
            int index = 0;
            return Parse(ref str, ref index);
        }
        public static JsonArray Parse(ref string str, ref int index)
        {
            JsonArray jsonArray = new JsonArray();
            Format.SkipSpace(ref str, ref index);
            if (str[index] != '[')
                throw new Exception("Missing \"[\" at the head of a JsonArray.");
            index++;
            Format.SkipSpace(ref str, ref index);
            if (str[index] == ']')
            {
                index++;
                return jsonArray;
            }
            while (true)
            {  
                //  Just directly read value
                JsonValue readValue = Format.ReadValue(ref str, ref index);
                jsonArray.Add(readValue);
                // Check if is the end
                Format.SkipSpace(ref str, ref index);
                if (str[index] == ']')
                {
                    index++;
                    return jsonArray;
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
            builder.Append('[');
            int count = 0;
            foreach (var value in List)
            {
                value.AppendStringTo(builder);
                count++;
                if (count != List.Count)
                {
                    builder.Append(", ");
                }
            }
            builder.Append(']');
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
            if (List.Count == 0)
            {
                builder.Append("[]");
                return;
            }
            builder.Append("[\n");
            int count = 0;
            foreach (var value in List)
            {
                for (int i = 0; i < depth + 1; i++)
                {
                    builder.Append(indentation);
                }
                value.AppendSerializedTo(builder, depth + 1, ref indentation);
                count++;
                if (count != List.Count)
                {
                    builder.Append(",");
                }
                builder.Append('\n');
            }
            for (int i = 0; i < depth; i++)
            {
                builder.Append(indentation);
            }
            builder.Append(']');
        }
    }
}
