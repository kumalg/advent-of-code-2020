using System.Linq;

namespace advent_of_code_2020.Days {
    public class Day09 : Day<long> {

        public override long FirstStar() {
            var setSize = 25;
            var input = InputLines.Select(int.Parse);
            return input.Skip(setSize).Where((n, i) => {
                var sub = input.Skip(i).Take(setSize);
                var mmm = sub.SelectMany((x, i) => sub.Skip(i + 1), (x, y) => x + y);
                return !mmm.Contains(n);
            }).First();
        }

        public override long SecondStar() {
            var searched = FirstStar();
            var input = InputLines.Select(long.Parse);

            for (var i = 0; i < input.Count(); i++) {
                for (var j = 2; j <= input.Count() - i; j++) {
                    var subset = input.Skip(i).Take(j).ToList();
                    if (subset.Contains(searched))
                        continue;
                    if (subset.Sum() == searched)
                        return subset.Min() + subset.Max();
                }
            }

            return -1;
        }
    }
}
