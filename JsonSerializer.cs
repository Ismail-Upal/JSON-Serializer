using System.Collections;
using System.Globalization;
using System.Reflection;
using System.Text;

public static class JsonSerializer
{
    private static StringBuilder Json;

    public static string Serialize(object root)
    {
        Json = new StringBuilder();
        PrintObj(root);
        return Json.ToString();
    }

    private static void PrintObj(object root)
    {
        if(root is null)
        {
            Json.Append("null");
            return;
        }

        Json.Append('{');
        PropertyInfo[] props = root.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

        bool ok = false;
        foreach (var prop in props)
        {
            if (ok) Json.Append(',');

            object value = prop.GetValue(root);

            string key = $"\"{prop.Name}\":";
            Json.Append(key);

            PrintVal(value);

            ok = true;
        }

        Json.Append('}');
    }

    private static void PrintVal(object value)
    {
        if (value is null) Json.Append("null");
        else if (value is string) Json.Append($"\"{value}\"");
        else if (value is char c) Json.Append(c.ToString());
        else if (value is bool b) Json.Append(b ? "true" : "false");
        else if (IsNumber(value)) Json.Append(Convert.ToString(value, CultureInfo.InvariantCulture));
        else if (value is IEnumerable enumerable)
        {
            Json.Append('[');

            bool ok = false;
            foreach (var v in enumerable)
            {
                if (ok) Json.Append(',');
                PrintVal(v);
                ok = true;
            }

            Json.Append(']');
        }
        else PrintObj(value);
    }

    private static bool IsNumber(object value)
    {
        return value is sbyte || value is byte || value is short || value is ushort || value is int || value is uint || value is long || value is ulong || value is float || value is double || value is decimal;
    }
}
