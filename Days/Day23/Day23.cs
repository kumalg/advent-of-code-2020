using System;
using System.Collections.Generic;
using System.Linq;

namespace advent_of_code_2020.Days {
    public class Day23 : Day<string> {
        private static int Mod(int x, int m) {
            return (x % m + m) % m;
        }

        private List<int> Arrange(string cupsString, IEnumerable<int> additional, int moves) {
            var cups = cupsString.Select(i => int.Parse(i.ToString())).ToList();
            if (additional != null) {
                cups = cups.Concat(additional).ToList();
            }
            var cupsCount = cups.Count;

            var currentCupIndex = 0;
            for (var i = 0; i < moves; i++) {
                if (i % 100_000 == 0) {
                    Console.WriteLine(i);
                }
                var currentCup = cups.ElementAt(currentCupIndex);
                var pickUpIndexes = Enumerable.Range(currentCupIndex + 1, 3).Select(i => i % cupsCount).ToList();
                var pickUp = pickUpIndexes.Select(i => cups.ElementAt(i)).ToList();

                var orderedPIckupIndexes = pickUpIndexes.OrderByDescending(v => v).ToList();
                foreach (var index in orderedPIckupIndexes) {
                    cups.RemoveAt(index);
                }

                var destination = Mod(currentCup - 2, cupsCount) + 1;

                while (pickUp.Contains(destination)) {
                    destination = Mod(destination - 2, cupsCount) + 1;
                }

                var indexOfDestination = cups.IndexOf(destination);
                cups.InsertRange(indexOfDestination + 1, pickUp);
                currentCupIndex = (cups.IndexOf(currentCup) + 1) % cupsCount;
            }

            return cups;
        }

        public override string FirstStar() => string.Join("", string.Join("", Arrange("925176834", null, 100)).Split("1").Reverse());

        public override string SecondStar() {
            var cups = Arrange("925176834", Enumerable.Range(10, 1_000_000 - 9), 10_000_000);
            var indexOf1 = cups.IndexOf(1);
            long first = cups.ElementAt(indexOf1 + 1);
            long second = cups.ElementAt(indexOf1 + 2);
            return (first * second).ToString();
        }
    }
}
