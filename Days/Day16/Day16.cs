using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace advent_of_code_2020.Days {
    public class Day16 : Day<long> {
        public override long FirstStar() {
            var sections = InputText.Split("\n\n");
            var fields = new Regex(@"(\d+-\d+)")
                .Matches(sections.ElementAt(0))
                .Select(v => {
                    var list = v.Value
                        .Split("-")
                        .Select(int.Parse)
                        .ToList();
                    return (Min: list[0], Max: list[1]);
                })
                .ToList();
            var nums = new Regex(@"(\d+)")
                .Matches(sections.ElementAt(2))
                .Select(v => int.Parse(v.Value));
            var which = nums.Where(n => !fields.Any(f => n >= f.Min && n <= f.Max)).ToList();
            return which.Sum();

        }

        private static (int Min, int Max) RangeTuple(string line) {
            var list = line
                .Split("-")
                .Select(int.Parse)
                .ToList();
            return (Min: list[0], Max: list[1]);
        }

        public override long SecondStar() {
            var fieldRegex = new Regex(@"(.+): (\d+-\d+) or (\d+-\d+)");
            var ticketRegex = new Regex(@"(\d+)");
            var sections = InputText.Split("\n\n");
            var fields = sections
                .ElementAt(0)
                .Split("\n")
                .Select(l => fieldRegex.Match(l).Groups.Values)
                .Select(g => (Name: g.ElementAt(1).Value, FirstRange: RangeTuple(g.ElementAt(2).Value), SecondRange: RangeTuple(g.ElementAt(3).Value)))
                .ToList();

            var orderedFields = new List<(int pos, (string Name, (int Min, int Max) FirstRange, (int Min, int Max) SecondRange))>();

            var dic = new Dictionary<int, List<(string Name, (int Min, int Max) FirstRange, (int Min, int Max) SecondRange)>>();

            var myTicket = ticketRegex.Matches(sections.ElementAt(1)).Select(v => long.Parse(v.Value)).ToList();
            var tickets = sections
                .ElementAt(2)
                .Split("\n")
                .Skip(1)
                .Select(t => ticketRegex
                    .Matches(t)
                    .Select(v => long.Parse(v.Value))
                )
                .Where(t => t.All( tn => fields.Any(f => tn >= f.FirstRange.Min && tn <= f.FirstRange.Max || tn >= f.SecondRange.Min && tn <= f.SecondRange.Max)))
                .ToList();

            tickets.Add(myTicket);

            //foreach (var field in fields) {
            var count = fields.Count();
            for (var i = 0; i < count; i++) {

                foreach (var f in fields) {
                    var nums = tickets.Select(t => t.ElementAt(i));
                    var fieldValid = nums.All(tn => tn >= f.FirstRange.Min && tn <= f.FirstRange.Max || tn >= f.SecondRange.Min && tn <= f.SecondRange.Max);

                    if (fieldValid) {
                        //orderedFields.Add(f);
                        //fields.Remove(f);
                        if (dic.ContainsKey(i)) {
                            dic[i].Add(f);
                        }
                        else {
                            dic[i] = new[] { f }.ToList();
                        }
                        //break;
                    }
                }
            }

            while(orderedFields.Count < 20) {
                var eee = dic.First(t => t.Value.Count == 1);
                var fir = eee.Value.First();
                orderedFields.Add((eee.Key, fir));

                foreach (var e in dic) {
                    var containss = e.Value.FirstOrDefault(f => f.Name == fir.Name);
                    if (containss != default) {
                        e.Value.Remove(containss);
                    }
                }
            }

            var hmm = orderedFields
                .Where(t => t.Item2.Name.Contains("departure"))
                .Select(t => myTicket[t.pos])
                .ToList();

            return hmm
                  .Aggregate((a, b) => a * b);
        }
    }
}

//668844166853 too high