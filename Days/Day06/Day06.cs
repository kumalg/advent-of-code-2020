﻿using System.Linq;

namespace advent_of_code_2020.Days.Day06 {
    public class Day06 : Day<int> {

        public override int FirstStar() => InputText
            .Split("\n\n")
            .Select(l => l.Replace("\n", "")
                .ToCharArray()
                .Distinct()
                .Count())
            .Sum();

        public override int SecondStar() => InputText
            .Split("\n\n")
            .Select(l => l
                .Split()
                .Select(l => l.ToCharArray())
                .Aggregate((a, b) => a.Intersect(b).ToArray())
                .Length)
            .Sum();
    }
}