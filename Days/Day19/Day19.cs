using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace advent_of_code_2020.Days {
    public class Day19 : Day<int> {
        private static readonly Regex RuleLineRegex = new Regex(@"(\d+): (.+)");

        private static (Dictionary<int, string> Rules, List<string> Messages) GetRulesAndMessages(string inputText) {
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

        private static IList<string> RuleOptions( string rule, Dictionary<int, string> rules) {
            if (rule.StartsWith("\"")) {
                return new List<string>() { rule.Replace("\"", "") };
            }

            if (rule.Contains("|")) {
                return rule
                    .Split(" | ")
                    .SelectMany(subrule => RuleOptions(subrule, rules))
                    .Where(cc => !string.IsNullOrEmpty(cc))
                    .ToList();
            }

            return rule
                .Split(" ")
                .Select(int.Parse)
                .Select(id => RuleOptions(rules[id], rules))
                .ToList()
                .Aggregate((sum, current) => sum
                    .Where(ss => !string.IsNullOrEmpty(ss))
                    .Distinct()
                    .SelectMany(s => current
                        .Where(cc => !string.IsNullOrEmpty(cc))
                        .Distinct()
                        .Select(c => s + c)
                        .ToList()
                    )
                    .ToList()
                );
        }

        public override int FirstStar() {
            var (rules, messages) = GetRulesAndMessages(InputText);

            var rule0Options = RuleOptions(rules[0], rules);
            return messages.Count(s => rule0Options.Contains(s));
        }

        public override int SecondStar() {
            var (rules, messages) = GetRulesAndMessages(InputText);

            // 8: 42 | 42 8
            // 11: 42 31 | 42 11 31

            var rule42Options = RuleOptions(rules[42], rules);
            var rule31Options = RuleOptions(rules[31], rules);

            var county = 0;

            foreach(var message in messages) {
                var count42 = 0;
                var count31 = 0;
                var remainingMessage = message;

                var is42match = rule42Options.FirstOrDefault(o => remainingMessage.StartsWith(o));
                while (is42match != null) {
                    remainingMessage = remainingMessage[is42match.Length..];
                    is42match = rule42Options.FirstOrDefault(o => remainingMessage.StartsWith(o));
                    count42++;
                }

                var is31match = rule31Options.FirstOrDefault(o => remainingMessage.EndsWith(o));
                while (is31match != null) {
                    remainingMessage = remainingMessage[..^is31match.Length];
                    is31match = rule31Options.FirstOrDefault(o => remainingMessage.EndsWith(o));
                    count31++;
                }

                if (remainingMessage == "" && count42 > 0 & count31 > 0 && count42 > count31) {
                    county++;
                }
            }

            return county;
        }
    }
}
