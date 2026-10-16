namespace Radcliffe.Core.Databases
{
    public struct FieldQuery
    {
        private string _field = "";
        private SQLComparator _comparator = SQLComparator.EQUAL;
        private string _value = "";

        public FieldQuery(string field, SQLComparator comparator, string value)
        {
            _field = field;
            _comparator = comparator;
            _value = value;
        }

        public override string ToString()
        {
            return "`" + _field + "` " + _comparator.ToString() + " " + _value;
        }

        public string ToString(ref DatabaseConnection db)
        {
            string comparison = db.ComparatorToString(_comparator);
            return db.QuoteFieldName(_field) + " " + comparison + " " + _value;
        }
    }
}