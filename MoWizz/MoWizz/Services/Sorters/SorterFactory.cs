using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MoWizz.Services.Sorters
{
    public class SorterFactory
    {
        public ISorter<Movie> Get(string sort)
        {
            return new RatingSorter();
        }
    }

    class RatingSorter : ISorter<Movie>
    {
        public List<Movie> Sorted(List<Movie> items)
        {
            throw new NotImplementedException();
        }
    }
}