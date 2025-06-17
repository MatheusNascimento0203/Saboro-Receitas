using System.Collections.ObjectModel;
using System.Data;
using Microsoft.EntityFrameworkCore;

namespace Saboro.Data.Extensions;

public static class DataReaderExtension
{
    public static T ReadAttr<T>(this IDataReader r, string attrName)
    {
        if (r[attrName] == DBNull.Value || string.IsNullOrEmpty(r[attrName]?.ToString()))
            return default;

        var tipoT = typeof(T);
        var tipoR = r[attrName].GetType();

        try
        {
            return (T)(tipoR == tipoT || (tipoT.GetGenericArguments().Any() && tipoR == tipoT.GenericTypeArguments[0])
                ? r[attrName]
                : Convert.ChangeType(r[attrName], tipoT));
        }
        catch (Exception ex)
        {
            throw new Exception($"{attrName}: {tipoT} to {tipoR} | {ex.Message}");
        }
    }

    public static T Cast<T>(this IDataReader r) where T : class
    {
        var propName = "";
        try
        {
            var obj = (T)Activator.CreateInstance(typeof(T));
            var props = obj.GetType().GetProperties();
            for (var i = 0; i < r.FieldCount; i++)
            {
                var columnName = r.GetName(i);
                if (r[columnName] == DBNull.Value || r[columnName] == null)
                    continue;

                var prop = props.FirstOrDefault(x => EF.Functions.Like(x.Name, columnName));
                if (prop == null)
                    continue;

                propName = prop.Name;
                var propType = prop.PropertyType;
                var columnType = r[columnName].GetType();

                prop.SetValue(obj, propType == columnType || (propType.GetGenericArguments().Any() && propType.GenericTypeArguments[0] == columnType)
                    ? r[columnName]
                    : Convert.ChangeType(r[columnName], propType));
            }

            return obj;
        }
        catch (Exception ex)
        {
            throw new Exception($"{propName}: {ex.Message}");
        }
    }

    public static IEnumerable<T> CastEnumerable<T>(this IDataReader r) where T : class
    {
        var collection = new Collection<T>();
        while (r.Read())
            collection.Add(r.Cast<T>());

        return collection;
    }
}