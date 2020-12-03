using System.Linq;
using System.Text.RegularExpressions;

namespace advent_of_code_2020.Days.Day02 {
    public class Day02 : Day<int> {
        private static readonly Regex regex = new Regex(@"(\d+)-(\d+) (\w): (\w+)");

        public override int FirstStar() => InputLines
            .Select(line => regex.Match(line).Groups.Values.Skip(1).Select(v => v.Value).ToArray())
            .Where(p => {
                int count = p[3].Count(ch => ch == p[2][0]);
                return count >= int.Parse(p[0]) && count <= int.Parse(p[1]);
            })
            .Count();

        public override int SecondStar() => InputLines
            .Select(line => regex.Match(line).Groups.Values.Skip(1).Select(v => v.Value).ToArray())
            .Where(p => new[] {
                    p[3][int.Parse(p[0]) - 1],
                    p[3][int.Parse(p[1]) - 1]
                }.Count(l => l == p[2][0]) == 1)
            .Count();
    }
}
