using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radcliffe.Core.Databases
{
    internal class Enums
    {
    }

    public enum SQLSort
    {
        ASC,
        DESC
    }

    public enum SQLComparator
    {
        EQUAL,
        GREATER_THAN,
        LESS_THAN,
        GREATER_THAN_OR_EQ,
        LESS_THAN_OR_EQ,
        NOT_EQUAL,
        BETWEEN,
        LIKE,
        IN
    }
}
