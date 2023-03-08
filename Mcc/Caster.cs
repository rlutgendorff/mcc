namespace Mcc;

public static class Caster
{
    public static TTo EnumToEnum<TFrom, TTo>(TFrom input)
        where TFrom : Enum
        where TTo : Enum
    {
        return (TTo)System.Enum.Parse(typeof(TTo), input.ToString());
    }
}