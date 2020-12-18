using System.Collections.Generic;
using System.Linq;

namespace ConstructionLine.CodingChallenge
{
    public class SearchEngine
    {
        private readonly List<Shirt> _shirts;
        private readonly Dictionary<Color, Dictionary<Size, List<Shirt>>> _searchIndex;

        public SearchEngine(List<Shirt> shirts)
        {
            _shirts = shirts;
            _searchIndex = _shirts.GroupBy(s => s.Color).
                ToDictionary(g => g.Key, g =>
                    g.GroupBy(s => s.Size)
                    .ToDictionary(gs => gs.Key, gs => gs.ToList()));
        }


        public SearchResults Search(SearchOptions options)
        {

            var colors = options.Colors.Any() ? options.Colors : Color.All;
            var sizes = options.Sizes.Any() ? options.Sizes : Size.All;

            List<List<Shirt>> tmpResult = new List<List<Shirt>>();
            Dictionary<Color, int> colorCount = Color.All.ToDictionary(g => g, g => 0);
            Dictionary<Size, int> sizeCount = Size.All.ToDictionary(g => g, g => 0);

            foreach (Color color in colors)
            {
                foreach (Size size in sizes)
                {
                    List<Shirt> matchingShirts = _searchIndex[color][size];
                    colorCount[color] += matchingShirts.Count;
                    sizeCount[size] += matchingShirts.Count;

                    tmpResult.Add(matchingShirts);
                }
            }

            List<Shirt> result = tmpResult.SelectMany(s => s).ToList();

            List<ColorCount> colorCounts = colorCount
                .Select(c => new ColorCount()
                {
                    Color = c.Key,
                    Count = c.Value
                }).ToList();

            List<SizeCount> sizeCounts = sizeCount
                .Select(s => new SizeCount()
                {
                    Size = s.Key,
                    Count = s.Value
                }).ToList();


            return new SearchResults
            {
                Shirts = result,
                ColorCounts = colorCounts,
                SizeCounts = sizeCounts
            };
        }
    }
}