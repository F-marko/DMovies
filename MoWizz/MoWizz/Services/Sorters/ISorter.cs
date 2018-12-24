using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoWizz.Services.Sorters
{
    public interface ISorter<T>
    {
        List<T> Sorted(List<T> items);
    }
}
