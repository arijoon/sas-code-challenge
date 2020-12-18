using System.Collections.Generic;
using System.Linq;

namespace ConstructionLine.CodingChallenge
{
    public class SearchEngine
    {
        private readonly List<Shirt> _shirts;

        public SearchEngine(List<Shirt> shirts)
        {
            _shirts = shirts;

            // TODO: data preparation and initialisation of additional data structures to improve performance goes here.

        }


        public SearchResults Search(SearchOptions options)
        {
            // TODO: search logic goes here.
            List<Shirt> colorMatch = _shirts.FindAll(s => options.Colors.Contains(s.Color));
            List<Shirt> sizeMatch = _shirts.FindAll(s => options.Sizes.Contains(s.Size));

            List<Shirt> result;
            if (options.Colors == null || options.Colors.Count < 1)
            {
                result = sizeMatch.ToList();
            }
            else if (options.Sizes == null || options.Sizes.Count < 1)
            {
                result = colorMatch.ToList();
            }
            else
            {
                result = colorMatch.Intersect(sizeMatch).ToList();
            }

            var colors = result.GroupBy(s => s.Color)
                .Select(g => new ColorCount()
                {
                    Color = g.Key,
                    Count = g.Count()
                });

            var sizes = result.GroupBy(s => s.Size)
                .Select(g => new SizeCount()
                {
                    Size = g.Key,
                    Count = g.Count()
                });

            var emptyColors = Color.All.Except(colors.Select(c => c.Color))
                .Select(c => new ColorCount() {Color = c, Count = 0});

            var emptySizes = Size.All.Except(sizes.Select(s => s.Size))
                .Select(s => new SizeCount() {Size = s, Count = 0});

            return new SearchResults
            {
                Shirts = result,
                ColorCounts = colors.Union(emptyColors).ToList(),
                SizeCounts = sizes.Union(emptySizes).ToList()
            };
        }
    }
}