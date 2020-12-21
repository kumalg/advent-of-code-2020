using System.Linq;

namespace advent_of_code_2020.Days {
    public class Day05 : Day<int> {
        private static readonly int Rows = 128;
        private static readonly int Columns = 8;

        private static int Position(string regionDirections, int maxValue, char decreaseLetter) => regionDirections
            .Aggregate((Range: maxValue, Max: maxValue), (a, b) => (a.Range / 2, a.Max - (b == decreaseLetter ? a.Range / 2 : 0)))
            .Max - 1;

        private static int Row(string regionDirections) => Position(regionDirections, Rows, 'F');

        private static int Column(string regionDirections) => Position(regionDirections, Columns, 'L');

        private int Id(string line) => Columns * Row(line[..7]) + Column(line[^3..]);

        public override int FirstStar() => InputLines.Max(Id);

        public override int SecondStar() {
            var freeIds = Enumerable.Range(0, Columns * Rows + Columns).Except(InputLines.Select(Id));
            return freeIds.First(id => id != 0 && !freeIds.Contains(id - 1));
        }
    }
}
