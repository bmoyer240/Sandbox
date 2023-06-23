using System.Data;

namespace Dapper;

internal class IntArrayHandler : SqlMapper.TypeHandler<int[]>
{
    public override void SetValue(IDbDataParameter parameter, int[] value) =>
        parameter.Value = string.Join(",", value);

    public override int[] Parse(object value) =>
        value.ToString()
            ?.Split(",", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(int.Parse)
            .ToArray()
        ?? Array.Empty<int>();

    public static void Register() => SqlMapper.AddTypeHandler(new IntArrayHandler());
}