using System.Collections.Generic;

namespace Radcliffe.Core.Databases
{
    public class Database
    {
		private DatabaseConnection _database = null;

		public Database(ref DatabaseConnection _db)
		{
			_database = _db;
		}

        public SQLResult SQLQuery(string sql) { 
			return _database.Execute(sql);
		}
		public SQLResult Query(string dbTable, FieldQuery[] query, Sort[] sort, string[] returnFields)
		{
			string sql_query = "";
			if (returnFields.Length == 0)
			{
				sql_query = "SELECT * FROM " + _database.QuoteFieldName(dbTable);
			}
			else
			{
				string fields = string.Join(", ", returnFields);
				sql_query = "SELECT " + fields + " FROM " + _database.QuoteFieldName(dbTable);
			}

			if (query.Length == 1)
			{
				//single query
				sql_query += " WHERE " + query[0].ToString(ref _database);
			}
			else if (query.Length > 1)
			{
				//multiple queryes!
				for (int i = 0; i < query.Length; i++)
				{
					sql_query += " WHERE " + query[i].ToString(ref _database);
					if (i < (query.Length - 1))
					{
						sql_query += " AND ";
					}
				}
			}

			if (sort.Length == 1)
			{
				//We have 1 sort param
				sql_query += " ORDER BY ";
				sql_query += sort[0].ToString(ref _database);
			}
			else if (sort.Length > 1)
			{
				//we have multiple sorts!
				sql_query += " ORDER BY ";
				for (int i = 0; i < sort.Length; i++)
				{
					sql_query += sort[i].ToString(ref _database);
					if (i < (sort.Length - 1))
					{
						sql_query += ", ";
					}
				}
			}
			sql_query += ";";

			return SQLQuery(sql_query);
		}

		public SQLResult Update(string table, string idField, string idValue, List<(string field, string value)> values)
		{
			string sql_query = "UPDATE " + _database.QuoteTableName(table) + " SET ";
			for (int i = 0; i < values.Count; i++)
			{
                (string field, string value) entry = values[i];
                sql_query += _database.QuoteFieldName(entry.field) + " = " + entry.value;
				if (i < (values.Count - 1))
				{
					sql_query += ", ";
				}
            }

			sql_query += " WHERE " + _database.QuoteFieldName( idField) + " " + _database.ComparatorToString( SQLComparator.EQUAL ) + " " + idValue + ";";

			return SQLQuery(sql_query);
        }

        public SQLResult Insert(string table, List<(string field, string value)> values)
        {
			string sql_query = "INSERT INTO " + _database.QuoteTableName(table) + " (";

			for (int i = 0; i < values.Count; i++)
			{
				sql_query += _database.QuoteFieldName( values[i].field );
				if (i < (values.Count - 1))
				{
					sql_query += ", ";
				}
			}

			sql_query += ") VALUES (";

            for (int i = 0; i < values.Count; i++)
            {
                sql_query += values[i].value;
                if (i < (values.Count - 1))
                {
                    sql_query += ", ";
                }
            }

            sql_query += ");";

			return _database.Execute( sql_query );
        }

        public SQLResult Del(string table, string idField, string idValue)
        {
			string sql_query = "DELETE FROM " + _database.QuoteTableName(table) + " WHERE " +  _database.QuoteFieldName( idField) + " " + _database.ComparatorToString(SQLComparator.EQUAL) + " " + idValue;
            return _database.Execute( sql_query );
        }
    }
}