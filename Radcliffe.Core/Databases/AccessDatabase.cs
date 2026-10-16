using System;
using System.Collections.Generic;
using System.Data.OleDb;

namespace Radcliffe.Core.Databases
{
    public class AccessDatabase : DatabaseConnection
    {

        private Dictionary<SQLComparator, string> _comparators = new Dictionary<SQLComparator, string>() {
            { SQLComparator.EQUAL, "=" },
            { SQLComparator.GREATER_THAN, ">"  },
            { SQLComparator.LESS_THAN, "<"  },
            { SQLComparator.GREATER_THAN_OR_EQ, ">="  },
            { SQLComparator.LESS_THAN_OR_EQ, "<=" }
        };

        private readonly string _connectionString = "";

        public AccessDatabase(string databasePath)
        {
            _connectionString =
                $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={databasePath};";
        }

        public override string ComparatorToString(SQLComparator _comparator)
        {
            if (_comparators.ContainsKey(_comparator))
            {
                return _comparators[_comparator];
            }
            else
            {
                return "?";
            }
        }

        public override string EscapeValue(string value)
        {
            return "'" + value.Replace("'", "''") + "'";
        }

        public override SQLResult Execute(string sql)
        {
            SQLResult result = new SQLResult();
#pragma warning disable CA1416 // Validate platform compatibility
            try
            {
                using OleDbConnection connection = new OleDbConnection(_connectionString);

                connection.Open();

                using OleDbCommand command = new OleDbCommand(sql, connection);
                using OleDbDataReader reader = command.ExecuteReader();

                string[] fields = new string[reader.FieldCount];
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    fields[i] = reader.GetName(i);
                }
                result.SetFields(fields);

                while (reader.Read())
                {
                    string[] data = new string[reader.FieldCount];
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        data[i] = reader[i].ToString();
                    }
                    result.AddRow(data);
                }
            }
            catch (OleDbException ex)
            {
                for (int i = 0; i < ex.Errors.Count; i++)
                {
                    OleDbError error = ex.Errors[i];
                    result.AddError(error.NativeError, error.SQLState, error.Message);
                }
            }
#pragma warning restore CA1416 // Validate platform compatibility
            return result;
        }

        public override string QuoteFieldName(string fieldName)
        {
            return "[" + fieldName.Replace("]", "]]") + "]";
        }

        public override string QuoteTableName(string tableName)
        {
            return "[" + tableName.Replace("]", "]]") + "]";
        }

        public override string ValueParser(string value)
        {
            return EscapeValue(value);
        }

        public override string ValueParser(int value)
        {
            return value.ToString();
        }

        public override string ValueParser(double value)
        {
            return value.ToString();
        }

        public override string ValueParser(DateTime value)
        {
            return "#" + value.ToString("yyyy-MM-dd HH:mm:ss") + "#";
        }

        public override string ValueParser(bool value)
        {
            return value ? "True" : "False";
        }
    }
}