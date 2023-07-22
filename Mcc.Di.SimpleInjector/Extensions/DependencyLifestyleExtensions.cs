using SimpleInjector;

namespace Mcc.Di.SimpleInjector.Extensions;

public static class DependencyLifestyleExtensions
{
    public static Lifestyle GetLifestyle(this DependencyLifestyle lifestyle) =>
        lifestyle switch
        {
            DependencyLifestyle.Transient => Lifestyle.Transient,
            DependencyLifestyle.Scoped => Lifestyle.Scoped,
            DependencyLifestyle.Singleton => Lifestyle.Singleton,
            _ => throw new ArgumentOutOfRangeException(nameof(lifestyle), lifestyle, null)
        };
}