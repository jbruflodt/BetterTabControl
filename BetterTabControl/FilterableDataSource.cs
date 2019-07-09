using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetterTabs
{
    class FilterableDataSource
    {
    }
    public interface IFilterableDataSource : System.Collections.IEnumerable
    {
        List<string> GetValuesForProperty(string propertyName);

    }
}
