using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace advent_of_code_2020.Days.Day07 {
    public class Day07 : Day<int> {
        static readonly Regex MainRegex = new Regex(@"(.+) (?:bags contain) (.+)[.]");
        static readonly Regex LineRegex = new Regex(@"(\d+) (.+) bag(?:s|)");
        static readonly string Key = "shiny gold";

        IDictionary<string, IDictionary<string, int>> _rules;
        IDictionary<string, IDictionary<string, int>> Rules => _rules ??= InputLines
            .Select(line => MainRegex.Match(line).Groups.Values.Skip(1).Select(v => v.Value))
            .ToDictionary(
                line => line.ElementAt(0),
                line => line.ElementAt(1) switch {
                    "no other bags" => new Dictionary<string, int>(),
                    _ => DictioraryFromString(line.ElementAt(1))
                }
            );

        public static IDictionary<string, int> DictioraryFromString(string line) => line
            .Split(", ")
            .Select(m => LineRegex.Match(m).Groups.Values.Skip(1).Select(v => v.Value))
            .ToDictionary(
                m => m.ElementAt(1),
                m => int.Parse(m.ElementAt(0))
            );

        public void GoDeeper(string key, HashSet<string> keysContains) {
            foreach (var el in Rules.Where(i => i.Value != null && i.Value.ContainsKey(key)))
                if (keysContains.Add(el.Key))
                    GoDeeper(el.Key, keysContains);
        }

        public override int FirstStar() {
            var keysContains = new HashSet<string>();

            GoDeeper(Key, keysContains);

            return keysContains.Count;
        }

        public int CountBags(string key) => 1 + Rules[key].Sum(b => b.Value * CountBags(b.Key));

        public override int SecondStar() => CountBags(Key) - 1;
    }
}
