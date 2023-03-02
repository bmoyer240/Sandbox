namespace System.Reflection;

public static class ReflectionExtensions
{
    public static T _SetProperty<T>(this T instance, string name, string? value, bool throwOnError = false)
    {
        if (instance is null || value is null)
        {
            return instance;
        }

        var property = instance.GetType().GetProperty(name, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
        if (property is not null)
        {
            // Get the underlying type if nullable
            var type = property.PropertyType.GenericTypeArguments.FirstOrDefault()
                        ?? property.PropertyType;

            try
            {
                property.SetValue(instance, Convert.ChangeType(value, type), null);
            }
            catch
            {
                if (throwOnError)
                {
                    throw;
                }
            }
        }

        return instance;
    }

}
