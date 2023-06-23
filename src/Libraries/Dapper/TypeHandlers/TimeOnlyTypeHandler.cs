using System.Data;

namespace Dapper;

public sealed class TimeOnlyTypeHandler : SqlMapper.TypeHandler<TimeOnly>
{
    public override TimeOnly Parse(object value) =>
        value switch
        {
            DateTime time => TimeOnly.FromDateTime(time),
            TimeSpan span => TimeOnly.FromTimeSpan(span),
            _             => default
        };

    public override void SetValue(IDbDataParameter parameter, TimeOnly value)
    {
        parameter.DbType = DbType.Time;
        parameter.Value  = value;
    }

    public static void Register() => SqlMapper.AddTypeHandler(new TimeOnlyTypeHandler());
}