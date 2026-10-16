using System.Collections.Generic;
using System.Linq;

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

		public SQLResult Update(string table, string idField, int idValue, Dictionary<string, string> values)
		{
			string sql_query = "UPDATE " + _database.QuoteFieldName(table) + " SET ";
			for (int i = 0; i < values.Count; i++)
			{
				KeyValuePair<string, string> entry = values.ElementAt(i);
                sql_query += entry.Key + " = '" + entry.Value + "'";
				if (i < (values.Count - 1))
				{
					sql_query += ", ";
				}
            }

			sql_query += "WHERE " + idField + " = " + idValue.ToString() + ";";

			return SQLQuery(sql_query);
        }

        public void Insert(string table, string[] fields, string[] values, bool debug = false)
        {
			/*$sql_query = "INSERT INTO `". $table. "` (";

            foreach ($fields as $field) {
				$sql_query.= "`". $field. "`,";
            }
			$sql_query = rtrim($sql_query, ','). ") VALUES (";

            foreach ($values as $val) {
				$sql_query.= $this->Quote($val). ",";
            }
			$sql_query = rtrim($sql_query, ','). ");";

            if ($debug) echo $sql_query;

            reurn $this->query($sql_query);
			*/
        }

        public void Del(string table, string searchField, string searchValue)
        {
			/*$sql_query = "DELETE FROM `". $table. "` WHERE `". $searchField. "` = ". $this->Quote($searchValue);
            return $this->query($sql_query);*/
        }
    }
}
