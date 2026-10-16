namespace Radcliffe.Core.Databases
{
    public struct Sort
    {
        private string field = "";
        private SQLSort sort = SQLSort.ASC;
        public Sort(string _field, SQLSort _sort)
        {
            field = _field;
            sort = _sort;
        }
        public override string ToString()
        {
            return field + " " + sort.ToString().ToUpper();
        }
        public string ToString(ref DatabaseConnection db)
        {
            return db.QuoteFieldName(field) + " " + sort.ToString().ToUpper();
        }
    }
}