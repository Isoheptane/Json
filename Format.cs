using System;
using System.Text;

namespace Json
{
    public static class Format
    {
        public static string Escape(string original)
        {
            return original
            .Replace("\\", "\\\\")
            .Replace("\a", "\\a")
            .Replace("\b", "\\b")
            .Replace("\f", "\\f")
            .Replace("\n", "\\n")
            .Replace("\r", "\\r")
            .Replace("\t", "\\t")
            .Replace("\v", "\\v")
            .Replace("\'", "\\'")
            .Replace("\"", "\\\"")
            .Replace("\0", "\\\0");
        }

        public static string Unescape(string escaped)
        {
            return escaped
            .Replace("\\a", "\a")
            .Replace("\\b", "\b")
            .Replace("\\f", "\f")
            .Replace("\\n", "\n")
            .Replace("\\r", "\r")
            .Replace("\\t", "\t")
            .Replace("\\v", "\v")
            .Replace("\\'", "'")
            .Replace("\\\"", "\"")
            .Replace("\\\0", "\0")
            .Replace("\\\\", "\\");
        }

        public static void SkipSpace(ref string str, ref int index)
        {
            while (index < str.Length && str[index] <= 32)
            {
                index++;
            }
        }

        public static string ReadUntil(ref string str, ref int index, string charList)
        {
            StringBuilder builder = new StringBuilder();
            while (!charList.Contains(str[index].ToString()))
            {
                builder.Append(str[index]);
                index++;
            }
            return builder.ToString();
        }

        public static JsonValue ReadValue(ref string str, ref int index)
        {
            Format.SkipSpace(ref str, ref index);
            if (str[index] == '{')
            {
                return JsonObject.Parse(ref str, ref index);
            }
            else if (str[index] == '[')
            {
                return JsonArray.Parse(ref str, ref index);
            }
            else
            {
                string valueString = Format.ReadUntil(ref str, ref index, ",}]");
                return JsonValue.Parse(valueString);
            }
        }
    }
}