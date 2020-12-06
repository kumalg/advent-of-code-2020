using System.Linq;

namespace advent_of_code_2020.Days.Day06 {
    public class Day06 : Day<int> {

        public override int FirstStar() => InputText
            .Split("\n\n")
            .Sum(l => l
                .Replace("\n", "")
                .Distinct()
                .Count());

        public override int SecondStar() => InputText
            .Split("\n\n")
            .Sum(l => l
                .Split()
                .Select(l => l.ToCharArray())
                .Aggregate((a, b) => a.Intersect(b).ToArray())
                .Length);
    }
}
