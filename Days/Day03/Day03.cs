using System.Linq;

namespace advent_of_code_2020.Days.Day03 {
    public class Day03 : Day {
        private long TreesForSlope((int right, int down) slope) => InputLines
            .Where((line, index) => index % slope.down == 0 && index != 0)
            .Select((line, index) => line.ElementAt((index + 1) * slope.right % line.Length))
            .Count(c => c == '#');

        public override string FirstStar() => TreesForSlope((3, 1)).ToString();

        public override string SecondStar() => new (int right, int down)[] { (1, 1), (3, 1), (5, 1), (7, 1), (1, 2) }
            .Select(TreesForSlope)
            .Aggregate((a, b) => a * b)
            .ToString();
    }
}
