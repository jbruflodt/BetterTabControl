using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace BetterTabs
{
    interface IFilterableDataSource<T> : IEnumerable<T>
    {
        List<string> GetPopertyValueList(string propertyName);
    }
}
