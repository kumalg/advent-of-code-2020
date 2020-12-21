using System.Linq;
using System.Text.RegularExpressions;

namespace advent_of_code_2020.Days {
    public class Day18 : Day<long> {
        private static readonly Regex Regex = new Regex(@"\([\d\s*+]+\)");

        private static long ComputeFlatEquation(string equation, OperationOrder order) {
            equation = Regex.Replace(equation, @"\s+", "");

            Operation operation = new Operation();
            operation.Parse(equation, order);

            var result = operation.Solve();
            return result;
        }

        public override long FirstStar() {
            var equations = InputLines;
            var results = equations.Select(equation => {
                var currentEq = equation;
                var rootBrackets = Regex.Match(currentEq).Groups.Values.Where(v => v.Success).Select(v => v.Value).ToList();

                while (rootBrackets.Count > 0) {
                    foreach (var bracket in rootBrackets) {
                        var result = ComputeFlatEquation(bracket[1..^1], OperationOrder.No);
                        currentEq = currentEq.Replace(bracket, result.ToString());
                    }
                    rootBrackets = Regex.Match(currentEq).Groups.Values.Where(v => v.Success).Select(v => v.Value).ToList();
                }

                return ComputeFlatEquation(currentEq, OperationOrder.No);
            });
            return results.Sum();
        }

        public override long SecondStar() {
            var equations = InputLines;
            var results = equations.Select(equation => {
                var currentEq = equation;
                var rootBrackets = Regex.Match(currentEq).Groups.Values.Where(v => v.Success).Select(v => v.Value).ToList();

                while (rootBrackets.Count > 0) {
                    foreach (var bracket in rootBrackets) {
                        var result = ComputeFlatEquation(bracket[1..^1], OperationOrder.AddFirst);
                        currentEq = currentEq.Replace(bracket, result.ToString());
                    }
                    rootBrackets = Regex.Match(currentEq).Groups.Values.Where(v => v.Success).Select(v => v.Value).ToList();
                }

                return ComputeFlatEquation(currentEq, OperationOrder.AddFirst);
            });
            return results.Sum();
        }

        private enum OperationOrder {
            No,
            MultFirst,
            AddFirst
        }

        private class Operation {
            public Operation LeftNumber { get; set; }
            public string Operator { get; set; }
            public Operation RightNumber { get; set; }

            private readonly Regex additionSubtraction = new Regex("[+-]", RegexOptions.RightToLeft);
            private readonly Regex multiplicationDivision = new Regex("[*/]", RegexOptions.RightToLeft);
            private readonly Regex all = new Regex("[*/+-]", RegexOptions.RightToLeft);

            public void Parse(string equation, OperationOrder order = OperationOrder.MultFirst) {
                Match operatorLocation;
                if (order == OperationOrder.No) {
                    operatorLocation = all.Match(equation);
                }

                else if (order == OperationOrder.AddFirst) {
                    operatorLocation = multiplicationDivision.Match(equation);
                    if (!operatorLocation.Success) {
                        operatorLocation = additionSubtraction.Match(equation);
                    }
                }

                else {
                    operatorLocation = additionSubtraction.Match(equation);
                    if (!operatorLocation.Success) {
                        operatorLocation = multiplicationDivision.Match(equation);
                    }
                }

                if (operatorLocation.Success) {
                    Operator = operatorLocation.Value;

                    LeftNumber = new Operation();
                    LeftNumber.Parse(equation.Substring(0, operatorLocation.Index), order);

                    RightNumber = new Operation();
                    RightNumber.Parse(equation.Substring(operatorLocation.Index + 1), order);
                }
                else {
                    Operator = "v";
                    result = long.Parse(equation);
                }
            }

            private long result;

            public long Solve() => Operator switch {
                "+" => LeftNumber.Solve() + RightNumber.Solve(),
                "-" => LeftNumber.Solve() - RightNumber.Solve(),
                "*" => LeftNumber.Solve() * RightNumber.Solve(),
                "/" => LeftNumber.Solve() / RightNumber.Solve(),
                _ => result
            };
        }
    }
}
