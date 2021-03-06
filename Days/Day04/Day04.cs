﻿using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace advent_of_code_2020.Days {
    public class Day04 : Day<int> {
        private static readonly string[] mandatoryFields = new[] { "byr", "iyr", "eyr", "hgt", "hcl", "ecl", "pid" };
        private static readonly Regex eclRegex = new Regex(@"^((amb)|(blu)|(brn)|(gry)|(grn)|(hzl)|(oth))$");
        private static readonly Regex hclRegex = new Regex(@"^#[a-f-0-9]{6}$");
        private static readonly Regex pidRegex = new Regex(@"^\d{9}$");

        private static bool ValidateKey(KeyValuePair<string, string> pair) => pair.Key switch {
            "hgt" => pair.Value.EndsWith("cm") && int.Parse(pair.Value[0..^2]) is >= 150 and <= 193 ||
                     pair.Value.EndsWith("in") && int.Parse(pair.Value[0..^2]) is >= 59 and <= 76,
            "byr" => int.Parse(pair.Value) is >= 1920 and <= 2002,
            "iyr" => int.Parse(pair.Value) is >= 2010 and <= 2020,
            "eyr" => int.Parse(pair.Value) is >= 2020 and <= 2030,
            "ecl" => eclRegex.IsMatch(pair.Value),
            "hcl" => hclRegex.IsMatch(pair.Value),
            "pid" => pidRegex.IsMatch(pair.Value),
            "cid" => true,
            _ => true,
        };

        private IEnumerable<Dictionary<string, string>> Passports => InputText
            .Split("\n\n")
            .Select(p => p
                .Split()
                .Select(kv => kv.Split(":"))
                .ToDictionary(d => d[0], d => d[1]))
            .Where(p => !mandatoryFields.Except(p.Keys).Any());

        public override int FirstStar() => Passports.Count();

        public override int SecondStar() => Passports.Count(p => p.Select(ValidateKey).All(k => k));
    }
}
