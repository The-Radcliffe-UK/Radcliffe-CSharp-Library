using System;
using System.IO.Pipes;

namespace Radcliffe.Core.Databases
{
    public abstract class DatabaseConnection
    {
        public abstract SQLResult Execute(string sql);
        public abstract string ComparatorToString(SQLComparator _comparator);

        public abstract string QuoteTableName(string tableName);
        public abstract string QuoteFieldName(string fieldName);
        public abstract string EscapeValue(string value);

        public abstract string ValueParser(string value);
        public abstract string ValueParser(int value);
        public abstract string ValueParser(double value);
        public abstract string ValueParser(DateTime value);
        public abstract string ValueParser(bool value);
    }
}
