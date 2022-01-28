using System;
using System.Text;

namespace Json
{
    // Type of Json values
    public enum ValueType 
    {
        None,
        Boolean,
        Number,
        Integer,
        String,
        Array,
        Json   
    }

    public class JsonValue
    {
        public ValueType Type { get; set; }
        object value;

        #region Constructors
        public JsonValue() 
        {
            Type = ValueType.None;
            this.value = null;
        }

        public JsonValue(bool value) 
        {
            Type = ValueType.Boolean;
            this.value = value;
        } 

        public JsonValue(decimal value) 
        {
            Type = ValueType.Number;
            this.value = value;
        }

        public JsonValue(Int64 value) 
        {
            Type = ValueType.Integer;
            this.value = value;
        }

        public JsonValue(string value)
        {
            Type = ValueType.String;
            this.value = value;
        }

        public JsonValue(JsonArray value) {
            Type = ValueType.Array;
            this.value = value;
        }

        public JsonValue(JsonObject value) {
            Type = ValueType.Json;
            this.value = value;
        }

        #endregion
    
        #region Converters

        public bool IsNull()
        {
            return Type == ValueType.None;
        }

        public static explicit operator bool(JsonValue value)
        {
            if (value.Type != ValueType.Boolean)
            {
                throw new Exception("This object is not a boolean object.");
            }
            return (bool)value.value;
        }

        public static explicit operator decimal(JsonValue value)
        {
            if (value.Type != ValueType.Number && value.Type != ValueType.Integer)
            {
                throw new Exception("This object is not a number object or integer object.");
            }
            if (value.Type == ValueType.Number)
            {
                return (decimal)value.value;
            }
            else
            {
                return Convert.ToDecimal((Int64)value.value);
            }
        }

        public static explicit operator float(JsonValue value)
        {
            return (float)(decimal)value;
        }

        public static explicit operator double(JsonValue value)
        {
            return (double)(decimal)value;
        }

        public static explicit operator Int64(JsonValue value)
        {
            if (value.Type != ValueType.Number && value.Type != ValueType.Integer)
            {
                throw new Exception("This object is not a number object or integer object.");
            }
            if (value.Type == ValueType.Integer)
            {
                return (Int64)value.value;
            }
            else
            {
                return Convert.ToInt64((decimal)value.value);
            }
        }

        public static explicit operator byte(JsonValue value)
        {
            return (byte)(Int64)value;
        }

        public static explicit operator sbyte(JsonValue value)
        {
            return (sbyte)(Int64)value;
        }

        public static explicit operator Int16(JsonValue value)
        {
            return (Int16)(Int64)value;
        }

        public static explicit operator UInt16(JsonValue value)
        {
            return (UInt16)(Int64)value;
        }

        public static explicit operator Int32(JsonValue value)
        {
            return (Int32)(Int64)value;
        }

        public static explicit operator UInt32(JsonValue value)
        {
            return (UInt32)(Int64)value;
        }

        public static explicit operator UInt64(JsonValue value)
        {
            return (UInt64)(Int64)value;
        }

        public static explicit operator string(JsonValue value)
        {
            if (value.Type != ValueType.String)
            {
                throw new Exception("This object is not a string object.");
            }
            return (string)value.value;
        }

        public static explicit operator JsonArray(JsonValue value)
        {
            if (value.Type != ValueType.Array)
            {
                throw new Exception("This object is not a array object.");
            }
            return (JsonArray)value.value;
        }

        public static explicit operator JsonObject(JsonValue value)
        {
            if (value.Type != ValueType.Json)
            {
                throw new Exception("This object is not a json object.");
            }
            return (JsonObject)value.value;
        }

        public static implicit operator JsonValue(bool value) 
        {
            return new JsonValue(value);
        }

        public static implicit operator JsonValue(decimal value) 
        {
            return new JsonValue(value);
        }

        public static implicit operator JsonValue(float value) 
        {
            return new JsonValue((decimal)value);
        }

        public static implicit operator JsonValue(double value) 
        {
            return new JsonValue((decimal)value);
        }

        public static implicit operator JsonValue(byte value) 
        {
            return new JsonValue((Int64)value);
        }

        public static implicit operator JsonValue(sbyte value) 
        {
            return new JsonValue((Int64)value);
        }

        public static implicit operator JsonValue(Int16 value) 
        {
            return new JsonValue((Int64)value);
        }

        public static implicit operator JsonValue(UInt16 value) 
        {
            return new JsonValue((Int64)value);
        }

        public static implicit operator JsonValue(Int32 value) 
        {
            return new JsonValue((Int64)value);
        }

        public static implicit operator JsonValue(UInt32 value) 
        {
            return new JsonValue((Int64)value);
        }

        public static implicit operator JsonValue(Int64 value) 
        {
            return new JsonValue((Int64)value);
        }

        public static implicit operator JsonValue(UInt64 value) 
        {
            return new JsonValue((Int64)value);
        }

        public static implicit operator JsonValue(string value) 
        {
            return new JsonValue(value);
        }

        public static implicit operator JsonValue(JsonArray value) 
        {
            return new JsonValue(value);
        }

        public static implicit operator JsonValue(JsonObject value) 
        {
            return new JsonValue(value);
        }

        #endregion
    
        #region Operators

        public JsonValue this[int index] 
        {
            get 
            {
                return ((JsonArray)this.value)[index];
            }
            set
            {
                ((JsonArray)this.value)[index] = value;
            }
        }

        public JsonValue this[string index]
        {
            get
            {
                return ((JsonObject)this.value)[index];
            }
            set
            {
                ((JsonObject)this.value)[index] = value;
            }
        }

        #endregion
        /// <summary>
        /// Parse string to a JsonValue object.
        /// </summary>
        public static JsonValue Parse(string value) {
            value = value.Trim();
            if (value.ToLower() == "null")
            {
                return new JsonValue();
            }
            if (value.ToLower() == "true") 
            {
                return new JsonValue(true);
            }
            else if (value.ToLower() == "false")
            {
                return new JsonValue(false);
            }
            if (value.StartsWith("\"") && value.EndsWith("\""))
            {
                string resolved = Format.Unescape(value.Substring(1, value.Length - 2));
                return new JsonValue(resolved);
            }
            if (value.StartsWith("[") && value.EndsWith("]"))
            {
                return new JsonValue(JsonArray.Parse(value));
            }
            if (value.StartsWith("{") && value.EndsWith("}"))
            {
                return new JsonValue(JsonObject.Parse(value));
            }
            //  Try resolve as number
            if (value.Contains(".") || value.Contains("e") || value.Contains("E"))
            {
                Decimal number;
                
                if 
                (
                    Decimal.TryParse
                    (
                        value, 
                        System.Globalization.NumberStyles.Any, 
                        System.Globalization.CultureInfo.InvariantCulture, 
                        out number
                    )
                )
                {
                    return new JsonValue(number);
                }
            }
            {
                Int64 integer;
                if (Int64.TryParse(value, out integer))
                {
                    return new JsonValue(integer);
                }
            }
            throw new Exception("Can't resolve Json.");
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
            switch (Type)
            {
                case ValueType.None:
                    builder.Append("null");
                    break;
                case ValueType.Boolean:
                    builder.Append((bool)value ? "true" : "false");
                    break;
                case ValueType.Integer:
                    builder.Append(((Int64)value).ToString());
                    break;
                case ValueType.Number:
                    builder.Append(((decimal)value).ToString());
                    break;
                case ValueType.String:
                    builder.Append("\"" + Format.Escape((string)value) + "\"");
                    break;
                case ValueType.Array:
                    ((JsonArray)value).AppendStringTo(builder);
                    break;
                case ValueType.Json:
                    ((JsonObject)value).AppendStringTo(builder);
                    break;
            }
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
            switch (Type)
            {
                case ValueType.None:
                    builder.Append("null");
                    break;
                case ValueType.Boolean:
                    builder.Append((bool)value ? "true" : "false");
                    break;
                case ValueType.Integer:
                    builder.Append(((Int64)value).ToString());
                    break;
                case ValueType.Number:
                    builder.Append(((decimal)value).ToString());
                    break;
                case ValueType.String:
                    builder.Append("\"" + Format.Escape((string)value) + "\"");
                    break;
                case ValueType.Array:
                    ((JsonArray)value).AppendSerializedTo(builder, depth, ref indentation);
                    break;
                case ValueType.Json:
                    ((JsonObject)value).AppendSerializedTo(builder, depth, ref indentation);
                    break;
            }
        }
    }
}
