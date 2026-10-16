using System;
using System.Collections.Generic;

namespace Radcliffe.Core.Databases
{
    public class Table
    {
        private string _tableName =  "";
		private string _primaryKey = "";
		private (string displayName, string dbName)[] _fields;
		private Database _db;

		public Table(string tableName, string primaryKey, (string,string)[] fields, ref Database db)
		{
			_primaryKey = primaryKey;
			_tableName = tableName;
			_fields = fields;
			_db = db;
		}

		public SQLResult All(string[ ] fields) {
			return _db.Query(_tableName, Array.Empty<FieldQuery>(), Array.Empty<Sort>(), fields );
		}

		public SQLResult All()
		{
			return All(Array.Empty<string>());
		}

		public SQLResult AllSorted(string[ ] fields, string sortField, SQLSort order) {
			Sort[] sort = [new( sortField, order )];
            return _db.Query( _tableName, Array.Empty<FieldQuery>(), sort, fields );
        }

		public SQLResult AllSorted(string sortField, SQLSort order)
		{
			return AllSorted( Array.Empty<string>(), sortField, order );
		}

		public SQLResult FilteredSort(string[ ] fields, FieldQuery[ ] filter, Sort[ ] sort) {
			return _db.Query( _tableName, filter, sort, fields );
		}

		public SQLResult FilteredSort(FieldQuery[ ] filter, Sort[ ] sort)
		{
			return FilteredSort( Array.Empty<string>(), filter, sort );
		}
		
		public SQLResult Filtered(string[ ] fields, FieldQuery[ ] filter) {
			return _db.Query( _tableName, filter, Array.Empty<Sort>(), fields );
        }

		public SQLResult Filtered(FieldQuery[ ] filter)
		{
			return Filtered( Array.Empty<string>(), filter );
		}

        public SQLResult Get(string[ ] fields, string idField, string id) {
			FieldQuery[] fieldQuery = [new(idField, SQLComparator.EQUAL, id)];
            return _db.Query( _tableName, fieldQuery, Array.Empty<Sort>(), fields );
        }

		public SQLResult Get(string id)
		{
			return Get(Array.Empty<string>(), _primaryKey, id);
		}

        public SQLResult Add(List<(string field, string value)> values) {
			return _db.Insert( _tableName, values );
		}

		public SQLResult Edit(string id, List<(string field, string value)> values) {
			return _db.Update( _tableName, _primaryKey, id, values );
		}

		public SQLResult Del(string id) {
			return _db.Del( _tableName, _primaryKey, id);
		}

		public string ToCSV()
		{
			return All().ToCSV();
		}

		public string ToJSON()
		{
			return All().ToJSON();
		}
    }
}
