using System;

namespace Radcliffe.Core.Databases
{
    public abstract class Table
    {
        private string? _tableName =  null;
		public (string displayName, string dbName)[] _fields;
		public Database _db;

		public Table(string tableName, (string,string)[] fields, ref Database db)
		{
			_tableName = tableName;
			_fields = fields;
			_db = db;
		}

		private string[] NullIfEmpty(string[] array){
			return (array.Length == 0) ? null : array;
		}

		public SQLResult All() {
			return _db.Query(_tableName, Array.Empty<FieldQuery>(), Array.Empty<Sort>(), Array.Empty<string>());
		}

		public SQLResult AllSorted($sortField, $order = "ASC") {
			/*self::EnsureTableDefined();
			return self::DB()->WholeTableSorted(static::$DB_TABLE, $sortField, $order);*/
		}

		public SQLResult CustomFilteredSort($filter, $sort) {
			/*self::EnsureTableDefined();
			return self::NullIfEmpty(
				self::DB()->CustomSortedQuery(static::$DB_TABLE, $filter, $sort)
			);*/
		}

		public SQLResult FilteredSort($filter, $sort) {
			/*self::EnsureTableDefined();
			return self::NullIfEmpty(
				self::DB()->GeneralSortedQuery(static::$DB_TABLE, $filter, $sort)
			);*/
		}
		
		public SQLResult Filtered($filter) {
			/*self::EnsureTableDefined();
			return self::NullIfEmpty(
				self::DB()->GeneralQuery(static::$DB_TABLE, $filter)
			);*/
		}
		
		public SQLResult CustomFiltered($filter) { 
			/*self::EnsureTableDefined();
			return self::NullIfEmpty(
				self::DB()->CustomQuery(static::$DB_TABLE, $filter)
			);*/
		}

		public SQLResult Get(int $id) {
			/*self::EnsureTableDefined();
			$results = self::DB()->GeneralQuery(static::$DB_TABLE, [["id", $id]]);
			return (count($results) === 0) ? null : $results[0];*/
		}

		public SQLResult FilteredGet(array $filter) {
			/*self::EnsureTableDefined();
			return self::NullIfEmpty(
				self::DB()->GeneralQuery(static::$DB_TABLE, $filter)
			);*/
		}

		public SQLResult Add(array $fields, array $values) {
			/*self::EnsureTableDefined();
			return self::DB()->Insert(static::$DB_TABLE, $fields, $values);*/
		}

		public SQLResult Edit($idField, $id, array $fields, array $values) {
			/*self::EnsureTableDefined();
			return self::DB()->Update(static::$DB_TABLE, $fields, $values, $idField, $id);*/
		}

		public SQLResult Del($idField, $id) {
			/*self::EnsureTableDefined();
			return self::DB()->Del(static::$DB_TABLE, $idField, $id);*/
		}
    }
}
