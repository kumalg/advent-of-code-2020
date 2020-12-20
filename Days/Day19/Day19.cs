using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace advent_of_code_2020.Days {
    public class Day19 : Day<int> {
        private static readonly Regex RuleLineRegex = new Regex(@"(\d+): (.+)");

        private static IList<string> Optionsy(int ruleId, string rule, Dictionary<int, string> rules, Dictionary<int, int> rulesCount, int maxMatch = int.MaxValue) {
            if (ruleId == 8) {
                rulesCount[8] = rulesCount[8] + 1;
                if (rulesCount[8] > maxMatch) {
                    return new List<string>() { "" };
                }
            }
            if (ruleId == 11) {
                rulesCount[11] = rulesCount[8] + 1;
                if (rulesCount[11] > maxMatch) {
                    return new List<string>() { "" };
                }
            }
            if (rule.StartsWith("\"")) {
                return new List<string>() { rule.Replace("\"", "") };
            }
            if (rule.Contains("|")) {
                //var eightCount = eightCounty;
                //var elevenCount = elevenCounty;
                return rule
                    .Split(" | ")
                    .Select(subrule => Optionsy(int.MaxValue, subrule, rules, rulesCount, maxMatch))
                    .SelectMany(v => v)
                    .Where(cc => !string.IsNullOrEmpty(cc))
                    .ToList();
            }

            var jak = rule
                .Split(" ")
                .Select(int.Parse)
                .Select(id => Optionsy(id, rules[id], rules, rulesCount, maxMatch))
                .ToList();
            return jak.Aggregate((sum, current) => sum.Where(ss => !string.IsNullOrEmpty(ss)).Distinct().Select(s => current.Where(cc => !string.IsNullOrEmpty(cc)).Distinct().Select(c => s + c).ToList()).SelectMany(v => v).ToList());
            //return new[] { "a" }.ToList();
        }

        public override int FirstStar() {
            var sections = InputText
                .Split("\n\n")
                .ToList();
            var stringyRules = sections
                .ElementAt(0)
                .Split("\n")
                .Select(line => RuleLineRegex.Match(line).Groups.Values)
                .ToDictionary(
                    v => int.Parse(v.ElementAt(1).Value),
                    v => v.ElementAt(2).Value
                );
            var rulesCount = stringyRules.ToDictionary(v => v.Key, v => 0);
            rulesCount.Add(int.MaxValue, 0);
            var rule0Options = Optionsy(0, stringyRules[0], stringyRules, rulesCount);
            return sections
                .ElementAt(1)
                .Split("\n")
                .Count(s => rule0Options.Contains(s));
        }

        public override int SecondStar() {
            var sections = InputText
                .Split("\n\n")
                .ToList();
            var stringyRules = sections
                .ElementAt(0)
                .Split("\n")
                .Select(line => RuleLineRegex.Match(line).Groups.Values)
                .ToDictionary(
                    v => int.Parse(v.ElementAt(1).Value),
                    v => v.ElementAt(2).Value
                );
            var rulesCount = stringyRules.ToDictionary(v => v.Key, v => 0);
            rulesCount.Add(int.MaxValue, 0);
            var messages = sections
                .ElementAt(1)
                .Split("\n");
            stringyRules[8] = "42 | 42 8";
            stringyRules[11] = "42 31 | 42 11 31";
            var longestMessage = messages.OrderByDescending(m => m.Length).First().Length;

            var rule0Options = Optionsy(0, stringyRules[0], stringyRules, rulesCount, 10);
            return messages.Count(s => rule0Options.Contains(s));
        }
    }
}
