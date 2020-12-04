﻿using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace advent_of_code_2020.Days.Day04 {
    public class Day04 : Day<int> {
        private static bool ValidateKey(KeyValuePair<string, string> pair) => pair.Key switch {
            "byr" => int.Parse(pair.Value) is >= 1920 and <= 2002,
            "iyr" => int.Parse(pair.Value) is >= 2010 and <= 2020,
            "eyr" => int.Parse(pair.Value) is >= 2020 and <= 2030,
            "hgt" => pair.Value.EndsWith("cm")
                        ? int.Parse(pair.Value[0..^2]) is >= 150 and <= 193
                        : pair.Value.EndsWith("in") && int.Parse(pair.Value[0..^2]) is >= 59 and <= 76,
            "hcl" => new Regex(@"^#[a-f-0-9]{6}$").IsMatch(pair.Value),
            "ecl" => "amb|blu|brn|gry|grn|hzl|oth".Split("|").Contains(pair.Value),
            "pid" => pair.Value.Length == 9 && new Regex(@"\d{9}").IsMatch(pair.Value),
            "cid" => true,
            _ => true,
        };

        private readonly string[] mandatoryFields = new[] { "byr", "iyr", "eyr", "hgt", "hcl", "ecl", "pid" };

        private IEnumerable<Dictionary<string, string>> Passports => Input
            .Split("\n\n")
            .Select(p => new Regex(@"[\s]")
                .Split(p).Where(p => p != "")
                .Select(kv => kv.Split(":"))
                .ToDictionary(d => d[0], d => d[1]));

        public override int FirstStar() => Passports
            .Count(p => !mandatoryFields.Except(p.Select(l => l.Key)).Any());

        public override int SecondStar() => Passports
            .Count(p => p.Select(ValidateKey).All(k => k));
    }
}
