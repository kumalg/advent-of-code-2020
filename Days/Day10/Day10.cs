using System;
using System.Collections.Generic;
using System.Linq;

namespace advent_of_code_2020.Days {
    public static class ListExtensions {
        public static IEnumerable<IEnumerable<T>> GroupWhile<T>(this IEnumerable<T> source, Func<T, T, bool> condition) {
            T previous = source.First();
            var list = new List<T>() { previous };
            foreach (T item in source.Skip(1)) {
                if (condition(previous, item) == false) {
                    yield return list;
                    list = new List<T>();
                }
                list.Add(item);
                previous = item;
            }
            yield return list;
        }
    }

    public class Day10 : Day<long> {

        public override long FirstStar() {
            var input = InputLines.Select(long.Parse);
            var fullInput = input.Concat(new[] { 0, input.Max() + 3 }).OrderBy(j => j);
            var eee = fullInput
                .Skip(1)
                .Select((jolts, i) => jolts - fullInput.ElementAt(i))
                .GroupBy(j => j)
                .Select(j => (j.Key, j.Count()))
                .ToDictionary(j => j.Key, j => j.Item2);
            return eee[1] * eee[3];
        }

        public override long SecondStar() {
            var input = InputLines.Select(long.Parse);
            var fullInput = input.Concat(new[] { 0, input.Max() + 3 }).OrderBy(j => j).ToList();

            return fullInput
                .Select((jolts, index) => new {
                    Jolts = jolts,
                    Difference = index == 0 ? 0 : jolts - fullInput.ElementAt(index - 1)
                })
                .GroupWhile((a, b) => b.Difference - a.Difference == 0)
                .Where(g => g.First().Difference == 1 && g.Count() > 1)
                .Select(g => g.Count() - 1)
                .Select(x => x > 2 ? (long)Math.Pow(2, x) - Math.Max((x - 2) * (x - 2) - 1, 1) : (long)Math.Pow(2, x))
                .Aggregate((a, b) => a * b);
        }
    }
}
