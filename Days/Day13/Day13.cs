using System.Linq;

namespace advent_of_code_2020.Days.Day13 {
    public class Day13 : Day<long> {
        public override long FirstStar() {
            var earliest = int.Parse(InputLines.First());

            return InputLines
                .ElementAt(1)
                .Split(",")
                .Where(l => l != "x")
                .Select(int.Parse)
                .OrderBy(n => n - earliest % n)
                .Select(n => (n - earliest % n) * n)
                .First();
        }

        public override long SecondStar() {
            var scalar = 6953;
            var offset = 17;
            //var scalar = 133;
            //var offset = 7;
            var curr = 99999999994637;
            var nums = InputLines
                .ElementAt(1)
                .Split(",")
                .Select((v, i) => (i, v))
                .Where(t => t.v != "x")
                .Select(t => (i: t.i, v: int.Parse(t.v)))
                .ToList();

            while (!nums.Select(t => (num: t.v, curr: curr - offset + t.i)).All(t => t.curr % t.num == 0)) {
                curr += scalar;
            }

            return curr - offset;
        }
    }
}
