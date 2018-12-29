using MoWizz.Models;
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
            switch (sort)
            {
                case "Latest":
                    return new LatestSorter();
                case "Popularity":
                    break;
                case "Rating":
                    return new RatingSorter();
                default:
                    break;
            
            }
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

    class LatestSorter : ISorter<Movie>
    {
        public List<Movie> Sorted(List<Movie> items)
        {
            items.Sort(new LatestMovieComparer());

            return items;
        }
    }

    class LatestMovieComparer : IComparer<Movie>
    {
        public int Compare(Movie x, Movie y)
        {
            if (String.IsNullOrEmpty(x.release_date))
            {
                x.release_date = "0000-00-00";
            }
            if (String.IsNullOrEmpty(y.release_date))
            {
                y.release_date = "0000-00-00";
            }
            try
            {
                return DateTime.Parse(y.release_date).CompareTo(DateTime.Parse(x.release_date));
            }
            catch(Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Err:  x:" + x.release_date + " y:" + y.release_date + " " + x.id + "," + y.id);
                return 0;
            }
        }
    }
}