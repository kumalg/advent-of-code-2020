using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace advent_of_code_2020.Days.Day10 {
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

        public void GoDeeper(ref long ways, IList<long> fullInput, long currentAdapterJoints) {
            //var newCurrentPath = currentPath.Add(currentAdapterJoints);
            //if (currentAdapterJoints == fullInput.Last()) {
            if (currentAdapterJoints == 161) {
                    //ways.Add(currentPath.ToArray());
                    ways++;
                return;
            }
            var nextAdapters = fullInput.Where(j => j > currentAdapterJoints && j <= currentAdapterJoints + 3);
            foreach (var next in nextAdapters) {
                GoDeeper(ref ways, fullInput, next);
            }
        }

        public override long SecondStar() {
            //var ways = new List<long[]>();
            long ways = 0;

            var input = InputLines.Select(long.Parse);
            var fullInput = input.Concat(new[] { 0, input.Max() + 3 }).OrderBy(j => j).ToList();

            var last = fullInput.Last();
            GoDeeper(ref ways, fullInput, 0);

            return ways;
        }
    }
}
