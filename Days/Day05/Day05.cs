using System.Linq;

namespace advent_of_code_2020.Days.Day05 {
    public class Day05 : Day<int> {
        private int Row(string regionDirections) => regionDirections.Aggregate((range: 128, max: 128), (a, b) => {
            return (a.range / 2, a.max - (b == 'F' ? a.range / 2 : 0));
        }).max - 1;

        private int Column(string regionDirections) => regionDirections.Aggregate((range: 8, max: 8), (a, b) => {
            return (a.range / 2, a.max - (b == 'L' ? a.range / 2 : 0));
        }).max - 1;

        private int Id(string line) => 8 * Row(line[0..7]) + Column(line[7..]);

        public override int FirstStar() => InputLines.Max(Id);

        public override int SecondStar() {
            var freeIds = Enumerable.Range(0, 128 * 8 + 8).Except(InputLines.Select(Id));
            return freeIds.First(id => id != 0 && !freeIds.Contains(id - 1));
        }
    }
}
