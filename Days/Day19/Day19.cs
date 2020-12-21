using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace advent_of_code_2020.Days {
    public class Day19 : Day<int> {
        private static readonly Regex RuleLineRegex = new Regex(@"(\d+): (.+)");

        private static (IDictionary<int, string> Rules, IList<string> Messages) GetRulesAndMessages(string inputText) {
            var sections = inputText
                .Split("\n\n")
                .ToList();

            var rules = sections
                .ElementAt(0)
                .Split("\n")
                .Select(line => RuleLineRegex.Match(line).Groups.Values)
                .ToDictionary(
                    v => int.Parse(v.ElementAt(1).Value),
                    v => v.ElementAt(2).Value
                );

            var messages = sections
                .ElementAt(1)
                .Split("\n")
                .ToList();

            return (rules, messages);
        }

        private static IList<string> RuleOptions(string rule, IDictionary<int, string> rules) {
            if (rule.StartsWith("\"")) {
                return new List<string>() { rule.Replace("\"", "") };
            }

            if (rule.Contains("|")) {
                return rule
                    .Split(" | ")
                    .SelectMany(subrule => RuleOptions(subrule, rules))
                    .ToList();
            }

            return rule
                .Split(" ")
                .Select(int.Parse)
                .Select(id => RuleOptions(rules[id], rules))
                .Aggregate((sum, current) => sum
                    .SelectMany(s => current.Select(c => s + c))
                    .ToList()
                );
        }

        public override int FirstStar() {
            var (rules, messages) = GetRulesAndMessages(InputText);
            return RuleOptions(rules[0], rules).Intersect(messages).Count();
        }

        public override int SecondStar() {
            var (rules, messages) = GetRulesAndMessages(InputText);

            // 8: 42 | 42 8
            // 11: 42 31 | 42 11 31

            var rule42Options = RuleOptions(rules[42], rules);
            var rule31Options = RuleOptions(rules[31], rules);

            return messages.Count(message => {
                var regex = new Regex($"^({ string.Join("|", rule42Options) })+({ string.Join("|", rule31Options) })+$").Match(message);
                return regex.Success && regex.Groups[1].Captures.Count > regex.Groups[2].Captures.Count;
            });
        }
    }
}
