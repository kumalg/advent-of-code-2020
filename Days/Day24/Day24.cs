using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace advent_of_code_2020.Days {
    public class Day24 : Day<int> {
        private static readonly string[] Directions = new[] { "e", "se", "sw", "w", "nw", "ne" };
        private static readonly Regex LineRegex = new Regex(@"(e|se|sw|w|nw|ne)");

        private static (int X, int Y) Shift(string direction) => direction switch {
            "e" => (2, 0),
            "se" => (1, -2),
            "sw" => (-1, -2),
            "w" => (-2, 0),
            "nw" => (-1, 2),
            "ne" => (1, 2),
        };

        private static List<(int X, int Y)> Neighbors((int X, int Y) position) => Directions
            .Select(d => Shift(d))
            .Select(s => (position.X + s.X, position.Y + s.Y))
            .ToList();
        
        private static List<(int X, int Y)> InitialArrangement(string[] inputLines) => inputLines
            .Select(l => LineRegex.Matches(l).Select(v => v.Value).ToList())
            .Select(directions => directions
                .Select(d => Shift(d))
                .Aggregate((a, b) => (a.X + b.X, a.Y + b.Y))
            )
            .GroupBy(v => v)
            .Where(g => g.Count() % 2 == 1)
            .SelectMany(g => g)
            .ToList();

        public override int FirstStar() => InitialArrangement(InputLines).Count;

        public override int SecondStar() {
            var selected = InitialArrangement(InputLines);


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
