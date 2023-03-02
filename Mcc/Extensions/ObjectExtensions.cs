namespace Mcc.Extensions;

public static class ObjectExtensions
{
    public static string ToJson(this object obj)
    {
        return System.Text.Json.JsonSerializer.Serialize(obj);
    }
}