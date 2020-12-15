using System;
using System.Collections.Generic;
using System.Linq;

namespace advent_of_code_2020.Days {
    public class Day15 : Day<long> {
        public override long FirstStar() {
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

        public override long SecondStar() {
            var moves = InputLines.First().Split(',').Select(int.Parse).ToList();

            var dic = moves
                .Select((val, ind) => (val, pos: ind + 1))
                .ToDictionary(i => i.val, i => new List<int>() { i.pos });
            var movesCount = moves.Count;
            var last = moves.Last();

            while (movesCount < 30_000_000) {
                if (dic[last].Count < 2) {
                    last = 0;
                }
                else {
                    var latest = dic[last].Skip(dic[last].Count - 2);
                    last = latest.Last() - latest.First();
                }
                if (!dic.ContainsKey(last)) {
                    dic[last] = new List<int>();
                }
                dic[last].Add(++movesCount);
            }

            return last;
        }
    }
}
