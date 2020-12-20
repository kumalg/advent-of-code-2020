using System;
using System.Linq;

namespace advent_of_code_2020.Days {
    public class Day15 : Day<int> {
        public override int FirstStar() {
            var moves = InputLines.First().Split(',').Select(int.Parse).ToList();

            while (moves.Count < 2020) {
                var last = moves.Last();
                var latest = moves.Select((v, i) => (i, v)).Where(t => t.v == last).Reverse().Take(2);
                if (latest.Count() != 2) {
                    moves.Add(0);
                }
                else {
                    moves.Add(latest.First().i - latest.Last().i);
                }
            }

            return moves.Last();
        }

        public override int SecondStar() {
            var moves = InputLines.First().Split(',').Select(int.Parse).ToList();

            var dic = moves
                .Select((val, ind) => (val, pos: ind + 1))
                .ToDictionary(i => i.val, i => new[] { i.pos });
            var movesCount = moves.Count;
            var last = moves.Last();

            while (movesCount < 30_000_000) {
                last = dic[last].Length < 2
                    ? 0
                    : dic[last].Last() - dic[last].First();
                dic[last] = !dic.ContainsKey(last)
                    ? new[] { ++movesCount }
                    : new[] { dic[last].Last(), ++movesCount };
            }

            return last;
        }
    }
}
