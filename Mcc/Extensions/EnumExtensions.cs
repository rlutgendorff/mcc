namespace Mcc.Extensions;
public static class EnumExtensions
{
    public static T2 ConvertTo<T1, T2>(this T1 enumValue) where T1 : Enum where T2 : Enum
    {
        if (!typeof(T1).IsEnum || !typeof(T2).IsEnum)
        {
            throw new ArgumentException("Both T1 and T2 must be enum types");
        }

        return (T2)Enum.Parse(typeof(T2), enumValue.ToString());
    }
}