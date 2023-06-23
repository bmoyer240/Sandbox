using System.Data;

namespace Dapper;

internal class DecimalArrayHandler : SqlMapper.TypeHandler<decimal[]>
{
    public override void SetValue(IDbDataParameter parameter, decimal[] value) =>
        parameter.Value = string.Join(",", value);

    public override decimal[] Parse(object value) =>
        value.ToString()
            ?.Split(",", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(decimal.Parse)
            .ToArray()
        ?? Array.Empty<decimal>();

    public static void Register() => SqlMapper.AddTypeHandler(new DecimalArrayHandler());
}