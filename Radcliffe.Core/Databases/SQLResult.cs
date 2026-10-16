using System;
using System.Collections.Generic;

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
    }
}
