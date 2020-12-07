using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace advent_of_code_2020.Days.Day07 {
    public class Day07 : Day<int> {
        static readonly Regex MainRegex = new Regex(@"(.+) (?:bags contain) (?:(?: no other bags)|(.+))");
        static readonly Regex LineRegex = new Regex(@"(\d+) (.+) bag(?:s|)");
        static readonly string Key = "shiny gold";

        IDictionary<string, IDictionary<string, int>> _rules;
        IDictionary<string, IDictionary<string, int>> Rules => _rules ??= InputLines
            .Select(line => MainRegex.Match(line).Groups.Values.Skip(1))
            .ToDictionary(
                line => line.ElementAt(0).Value,
                line => line.ElementAt(1).Value.StartsWith("no")
                    ? null
                    : DictioraryFromString(line.ElementAt(1).Value)
            );

        public static IDictionary<string, int> DictioraryFromString(string line) => line
            .Replace(".", "")
            .Split(", ")
            .Select(m => LineRegex.Match(m).Groups.Values.Skip(1))
            .ToDictionary(
                m => m.ElementAt(1).Value,
                m => int.Parse(m.ElementAt(0).Value)
            );

        public void GoDeeper(string key, HashSet<string> keysContains) {
            foreach (var el in Rules.Where(i => i.Value != null && i.Value.ContainsKey(key)))
                if (keysContains.Add(el.Key))
                    GoDeeper(el.Key, keysContains);
        }

        public int Count(string key) {
            var bagsInside = Rules[key];

            return 1 + (bagsInside != null ? bagsInside.Select(b => b.Value * Count(b.Key)).Sum() : 0);
        }

        public override int FirstStar() {
            var keysContains = new HashSet<string>();

            GoDeeper(Key, keysContains);
            
            return keysContains.Count;
        }

        public override int SecondStar() => Count(Key) - 1;
    }
}
