using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace advent_of_code_2020.Days.Day01 {
    public class Day01 : Day {
        public override string FirstStar() {
            var report = InputLines.Select(int.Parse);
            return report
                    .SelectMany((x, i) => report.Skip(i + 1), Tuple.Create)
                    .Where(t => t.Item1 + t.Item2 == 2020)
                    .Select(t => t.Item1 * t.Item2)
                    .First()
                    .ToString();
        }

        public override string SecondStar() {
            var report = InputLines.Select(int.Parse);
            return report
                .SelectMany((x, i) => report.Skip(i + 1)
                    .SelectMany((w, iw) => report.Skip(iw + 2), Tuple.Create), (x, y) => Tuple.Create(x, y.Item1, y.Item2))
                .Where(t => t.Item1 + t.Item2 + t.Item3 == 2020)
                .Select(t => t.Item1 * t.Item2 * t.Item3)
                .First()
                .ToString();
        }
    }
}
