using System.Text.Json;

namespace Mcc.Extensions;

public static class ObjectExtensions
{
    public static string ToJson(this object obj)
    {
        return JsonSerializer.Serialize(obj);
    }
    public static string ToJson(this object obj, JsonSerializerOptions options)
    {
        return JsonSerializer.Serialize(obj, options);
    }
}