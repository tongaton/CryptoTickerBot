using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace CryptoTickerBot.Data.Helpers
{
    public class DbQueryReader
    {
        private readonly Dictionary<string, Dictionary<string, List<Query>>> _queryDictionary;
        private static DbQueryReader _dBQueryReader;

        public class Query
        {
            public string QueryName { get; set; }
            public string QueryString { get; set; }


            public List<Clause> Clauses { get; set; }
        }

        public class Clause
        {
            public string ClauseName { get; set; }

            public string ClauseString { get; set; }
        }


        public enum DbNames
        {
            Erky
        }

        private DbQueryReader()
        {
            _queryDictionary = new Dictionary<string, Dictionary<string, List<Query>>>();
            ReadQueries();
        }

        public static DbQueryReader GetInstance()
        {
            if (_dBQueryReader == null)
            {
                _dBQueryReader = new DbQueryReader();
            }

            return _dBQueryReader;
        }

        private void ReadQueries()
        {
            try
            {
                foreach (string dbName in Enum.GetNames(typeof(DbNames)))
                {
                    string fileName = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), dbName + "-queries.json");
                    using StreamReader r = new StreamReader(fileName);
                    string json = r.ReadToEnd();
                    Dictionary<string, List<Query>> queries = JsonConvert.DeserializeObject<Dictionary<string, List<Query>>>(json);
                    _queryDictionary.Add(dbName, queries);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string GetQuery(string dBName, string entity, string queryName)
        {
            try
            {
                return _queryDictionary[dBName][entity].Find(x => x.QueryName == queryName).QueryString;
            }
            catch (Exception ex)
            {
                //no existe la query la query para esa entidad
                throw ex;
            }
        }

        public string GetQueryClause(string dBName, string entity, string queryName, string clauseName)
        {
            try
            {
                List<Clause> listClauses = _queryDictionary[dBName][entity].Find(x => x.QueryName == queryName).Clauses;
                return listClauses.Find(x => x.ClauseName.Equals(clauseName)).ClauseString;
            }
            catch (Exception ex)
            {
                //no existe la query la query para esa entidad
                throw ex;
            }
        }


    }
}
