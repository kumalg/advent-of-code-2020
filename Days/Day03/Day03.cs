using System.Linq;

namespace advent_of_code_2020.Days.Day03 {
    public class Day03 : Day {
        private long TreesForSlope(int right, int down) => InputLines
            .Where((line, i) => i % down == 0 && i != 0)
            .Select((line, index) => string
                .Concat(Enumerable.Repeat(line, (index + 1) * right / line.Length + 1))
                .ElementAt((index + 1) * right))
            .Count(c => c == '#');

        public override string FirstStar() => TreesForSlope(3, 1).ToString();

        public override string SecondStar() => new[] { (1, 1), (3, 1), (5, 1), (7, 1), (1, 2) }
            .Select(s => TreesForSlope(s.Item1, s.Item2))
            .Aggregate((a, b) => a * b)
            .ToString();
    }
}
