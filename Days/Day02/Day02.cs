using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace advent_of_code_2020.Days {
    public class Day02 : Day<int> {
        private static readonly Regex regex = new Regex(@"(\d+)-(\d+) (\w): (\w+)");
        private IEnumerable<(int a, int b, char letter, string password)> Input => InputLines
            .Select(line => regex.Match(line).Groups.Values.Skip(1).Select(v => v.Value))
            .Select(x => (int.Parse(x.ElementAt(0)), int.Parse(x.ElementAt(1)), x.ElementAt(2)[0], x.ElementAt(3)));

        public override int FirstStar() => Input
            .Count(group => {
                int count = group.password.Count(ch => ch == group.letter);
                return count >= group.a && count <= group.b;
            });

        public override int SecondStar() => Input
            .Count(group => group.password
                .Where((passwordLetter, index) => (index == group.a - 1 || index == group.b - 1) && passwordLetter == group.letter)
                .Count() == 1);
    }
}
