using System.Linq;
using System.Text.RegularExpressions;

namespace advent_of_code_2020.Days {
    public class Day18 : Day<long> {
        private static readonly Regex Regex = new Regex(@"\([\d\s*+]+\)");

        public long ComputeFlatEquation(string equation) {
            var one = equation.Split(" ");
            var second = one.Select(v => v is "*" or "+"
                ? (Value: null, Operation: v)
                : (Value: long.Parse(v) as long?, Operation: string.Empty));
            var third = second.Aggregate((a, b) => a.Operation switch {
                "*" => (a.Value * b.Value, string.Empty),
                "+" => (a.Value + b.Value, string.Empty),
                _ => (a.Value, b.Operation)
            });
            return third.Value.GetValueOrDefault(0);
        }

        public override long FirstStar() {
            var equations = InputLines;
            var results = equations.Select(equation => {
                var currentEq = equation;
                var rootBrackets = Regex.Match(currentEq).Groups.Values.Where(v => v.Success).Select(v => v.Value).ToList();

                while (rootBrackets.Count > 0) {
                    foreach (var bracket in rootBrackets) {
                        var result = ComputeFlatEquation(bracket[1..^1]);
                        currentEq = currentEq.Replace(bracket, result.ToString());
                    }
                    rootBrackets = Regex.Match(currentEq).Groups.Values.Where(v => v.Success).Select(v => v.Value).ToList();
                }

                return ComputeFlatEquation(currentEq);
            });
            return results.Sum();
        }

        public override long SecondStar() {
            return 0;
        }
    }
}
