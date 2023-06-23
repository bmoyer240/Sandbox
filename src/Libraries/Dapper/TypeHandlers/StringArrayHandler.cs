using System.Data;

namespace Dapper;

internal sealed class StringArrayHandler : SqlMapper.TypeHandler<string[]>
{
    public override void SetValue(IDbDataParameter parameter, string[] value) =>
        parameter.Value = string.Join(",", value);

    public override string[] Parse(object value) =>
        value.ToString()?.Split(",", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
        ?? Array.Empty<string>();

    public static void Register() => SqlMapper.AddTypeHandler(new StringArrayHandler());
}