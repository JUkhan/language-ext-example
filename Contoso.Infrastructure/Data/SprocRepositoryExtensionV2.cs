using Microsoft.EntityFrameworkCore;
using SPOLNET.Model.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace SPOLNET.Data.Repository.Ext.V2
{
    public static class SprocRepositoryExtensionV2
    {
        public static SprocHandler MultiSelect(this DbContext context, string storedProcName) =>
            new SprocHandler(context, storedProcName);

    }
    public class SprocHandler
    {
        private readonly DbContext _db;
        private string _spName;
        private List<Sparam> _paramList;
        private bool _hasQuery = false;
        private List<Func<DbDataReader, (string key, IEnumerable value)>> _resultSet;

        public SprocHandler(DbContext db, string spName)
        {
            _db = db;
            _spName = spName;
            _paramList = new List<Sparam>();
            _resultSet = new List<Func<DbDataReader, (string key, IEnumerable value)>>();

        }

        public SprocHandler With<TResult>()
        {
            _resultSet.Add(reader => (typeof(TResult).Name, MapToList<TResult>(reader)));
            return this;
        }

        public SprocHandler WithSqlParam(string paramName, object paramValue)
        {
            _paramList.Add(new Sparam(paramName, paramValue));
            return this;
        }

        public SprocHandler WithSqlOutParam(string paramName, object paramValue)
        {
            _paramList.Add(new Sparam(paramName, paramValue, true));
            return this;
        }

        public SprocHandler WithQuery(string query)
        {
            _spName = query;
            _hasQuery = true;
            return this;
        }

        public async Task<Dictionary<string, dynamic>> ExecuteAsync()
        {
            var res = new Dictionary<string, dynamic>();
            using (var conn = _db.Database.GetDbConnection())
            {
                await conn.OpenAsync();
                var command = conn.CreateCommand();
                command.CommandText = _spName;
                command.CommandType = _hasQuery ? CommandType.Text : CommandType.StoredProcedure;
                _paramList.Iter(it =>
                {
                    var param = command.CreateParameter();
                    param.ParameterName = it.ParamName;
                    param.Value = it.ParamValue;
                    param.Direction = it.IsOutputParam ? ParameterDirection.Output : ParameterDirection.Input;
                    command.Parameters.Add(param);
                });
                using (var reader = await command.ExecuteReaderAsync())
                {
                    _resultSet.Iter(resultSet =>
                    {
                        var result = resultSet(reader);
                        res.Add(result.key, result.value);
                        reader.NextResult();
                    });
                    _paramList.Where(it => it.IsOutputParam).Iter(it =>
                    {
                        res.Add(it.ParamName, command.Parameters[it.ParamName].Value);
                    });
                }
            }
            return res;
        }
        public async Task<Dictionary<string, dynamic>> ExecuteNonQueryAsync()
        {
            var res = new Dictionary<string, dynamic>();
            using (var conn = _db.Database.GetDbConnection())
            {
                await conn.OpenAsync();
                var command = conn.CreateCommand();
                command.CommandText = _spName;
                command.CommandType = _hasQuery ? CommandType.Text : CommandType.StoredProcedure;
                _paramList.Iter(it =>
                {
                    var param = command.CreateParameter();
                    param.ParameterName = it.ParamName;
                    param.Value = it.ParamValue;
                    param.Direction = it.IsOutputParam ? ParameterDirection.Output : ParameterDirection.Input;
                    command.Parameters.Add(param);
                });
                var reader = await command.ExecuteNonQueryAsync();
                res.Add("EffectedRows", reader);
                _paramList.Where(it => it.IsOutputParam).Iter(it =>
                {
                    res.Add(it.ParamName, command.Parameters[it.ParamName].Value);
                });

            }
            return res;
        }

        private IEnumerable<T> MapToList<T>(DbDataReader dr)
        {


            var objList = new List<T>();
            var colMapping = dr.GetColumnSchema()
              .ToDictionary(key => key.ColumnName.ToLower());

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    T obj = Activator.CreateInstance<T>();
                    MapObject(dr, colMapping, obj);
                    objList.Add(obj);
                }
            }
            return objList;

        }

        private static void MapObject(DbDataReader dr, Dictionary<string, DbColumn> colMapping, object ins)
        {
            var props = ins.GetType().GetRuntimeProperties();
            foreach (var prop in props)
            {

                if (colMapping.Any(c => c.Key == prop.Name.ToLower()))
                {
                    var val =
                  dr.GetValue(colMapping[prop.Name.ToLower()].ColumnOrdinal.Value);
                    prop.SetValue(ins, val == DBNull.Value ? null : val);
                }
                else if (prop.GetCustomAttribute<BindToReader>() != null)
                {
                    var _ins = Activator.CreateInstance(prop.PropertyType);
                    prop.SetValue(ins, _ins);
                    MapObject(dr, colMapping, _ins);

                }
            }
        }

        internal struct Sparam
        {
            public Sparam(string paramName, object paramValue, bool isOutputParam = false)
            {
                ParamName = paramName;
                ParamValue = paramValue;
                IsOutputParam = isOutputParam;
            }
            public string ParamName { get; }
            public object ParamValue { get; }

            public bool IsOutputParam { get; }

        }
    }

}
