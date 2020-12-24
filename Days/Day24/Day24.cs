using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace advent_of_code_2020.Days {
    public class Day24 : Day<long> {
        private static readonly string[] Directions = new[] { "e", "se", "sw", "w", "nw", "ne" };
        private static readonly Regex LineRegex = new Regex(@"(e|se|sw|w|nw|ne)");

        private static (double X, double Y) Shift(string direction) => direction switch {
            "e" => (1, 0),
            "se" => (0.5, -1),
            "sw" => (-0.5, -1),
            "w" => (-1, 0),
            "nw" => (-0.5, 1),
            "ne" => (0.5, 1),
        };

        private static List<(double X, double Y)> Neighbors((double X, double Y) position) => Directions
                .Select(d => Shift(d))
                .Select(s => (position.X + s.X, position.Y + s.Y))
                .ToList();

        public override long FirstStar() => InputLines
                .Select(l => LineRegex.Matches(l).Select(v => v.Value).ToList())
                .Select(directions => directions
                    .Select(d => Shift(d))
                    .Aggregate((a, b) => (a.X + b.X, a.Y + b.Y))
                )
                .GroupBy(v => v)
                .Count(g => g.Count() % 2 == 1);

        public override long SecondStar() {
            var selected = InputLines
                .Select(l => LineRegex.Matches(l).Select(v => v.Value).ToList())
                .Select(directions => directions
                    .Select(d => Shift(d))
                    .Aggregate((a, b) => (a.X + b.X, a.Y + b.Y))
                )
                .GroupBy(v => v)
                .Where(g => g.Count() % 2 == 1)
                .SelectMany(g => g)
                .ToList();
            
            for (var i = 0; i < 100; i++) {
                var selectedCopy = selected.Select(v => v).ToList();
                var posiibles = selectedCopy.SelectMany(s => Neighbors(s).Concat(new[] { s })).Distinct().ToList();

                foreach(var posiibility in posiibles) {
                    var neighbors = Neighbors(posiibility).Intersect(selected).Count();
                    if (selected.Contains(posiibility)) {
                        if (neighbors is 0 or > 2) {
                            selectedCopy.Remove(posiibility);
                        }
                    }
                    else {
                        if (neighbors == 2) {
                            selectedCopy.Add(posiibility);
                        }
                    }
                }

                selected = selectedCopy;
            }

            return selected.Count;
        }
    }
}
