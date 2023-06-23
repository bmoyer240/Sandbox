using System.Data;

namespace Dapper;

internal sealed class DateOnlyArrayHandler : SqlMapper.TypeHandler<DateOnly[]>
{
    public override void SetValue(IDbDataParameter parameter, DateOnly[] value) =>
        parameter.Value = string.Join(",", value);

    public override DateOnly[] Parse(object value) =>
        value.ToString()
            ?.Split(",", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(DateOnly.Parse)
            .ToArray()
        ?? Array.Empty<DateOnly>();

    public static void Register() => SqlMapper.AddTypeHandler(new DateOnlyArrayHandler());
}