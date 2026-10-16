using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Radcliffe.Core.Databases
{
    public struct SQLResult
    {
        private List<string[]> _data;
        private string[] _fields;
        private Dictionary<int, (string sqlState,string message)> _errors;

        public void AddRow(string[] row)
        {
            _data.Add(row);
        }

        public void SetFields(string[] fields)
        {
            _fields = fields;
        }

        public void AddError(int code, string sqlState, string reason)
        {
            _errors.Add(code, (sqlState, reason));
        }

        public SQLResult() {
            _data = new List<string[]>();
            _fields = Array.Empty<string>();
            _errors = new Dictionary<int, (string, string)>();
        }

        public string ToCSV()
        {
            StringBuilder csv = new();

            // Header row
            for (int i = 0; i < _fields.Length; i++)
            {
                if (i > 0)
                {
                    csv.Append( "," );
                }

                csv.Append( EscapeCSVValue( _fields[i] ) );
            }

            csv.AppendLine();

            // Data rows
            foreach (string[ ] row in _data)
            {
                for (int i = 0; i < row.Length; i++)
                {
                    if (i > 0)
                    {
                        csv.Append( "," );
                    }

                    csv.Append( EscapeCSVValue( row[i] ) );
                }

                csv.AppendLine();
            }

            return csv.ToString();
        }

        public string ToJSON()
        {
            List<Dictionary<string, string>> data = new();

            foreach (string[ ] row in _data)
            {
                Dictionary<string, string> item = new();

                for (int i = 0; i < _fields.Length && i < row.Length; i++)
                {
                    item[_fields[i]] = row[i];
                }

                data.Add( item );
            }

            return JsonSerializer.Serialize( data );
        }

        private string EscapeCSVValue(string value)
        {
            if (value.Contains( "\"" ) ||
                value.Contains( "," ) ||
                value.Contains( "\r" ) ||
                value.Contains( "\n" ))
            {
                return "\"" + value.Replace( "\"", "\"\"" ) + "\"";
            }

            return value;
        }
    }
}
