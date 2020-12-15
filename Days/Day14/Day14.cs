using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace advent_of_code_2020.Days {
    public class Day14 : Day<ulong> {
        private static readonly Regex MemRegex = new Regex(@"mem\[(\d+)\] = (\d+)");
        public static ulong MaskedNumber(string mask, ulong number) {
            var nums = mask.Select((letter, i) => (bitPosition: mask.Length - i - 1, letter)).Where(t => t.letter != 'X');
            foreach(var num in nums) {
                if (num.letter == '0') {
                    number &= ~(1ul << num.bitPosition);
                }
                else if (num.letter == '1') {
                    number |= 1ul << num.bitPosition;
                }
            }
            return number;
        }

        public override ulong FirstStar() {
            var memorySIze = InputLines
                .Where(l => l.StartsWith("mem"))
                .Select(l => MemRegex.Match(l).Groups.Values.ElementAt(1).Value)
                .Select(int.Parse)
                .Max();
            var memory = new Dictionary<ulong, ulong>();
            var currentMask = "";

            foreach (var line in InputLines) {
                if (line.StartsWith("mask")) {
                    currentMask = line[7..];
                }
                else {
                    var mem = MemRegex.Match(line).Groups.Values.Skip(1).Select(v => ulong.Parse(v.Value));
                    memory[mem.First()] = MaskedNumber(currentMask, mem.Last());
                }
            }
            var sum = 0ul;
            foreach(var m in memory) {
                checked {
                    sum += m.Value;
                }
            }

            return sum;
        }

        public static ulong[] Addresses(string mask, ulong address) {
            var addressString = Convert.ToString((long)address, 2);
            addressString = new string('0', 36 - addressString.Length) + addressString;

            var baseAddressString = string.Join("", addressString.Select((c, index) => mask[index] switch {
                '1' => '1',
                'X' => '0',
                _ => c
            }).ToArray());
            var baseAddress = Convert.ToUInt64(baseAddressString, 2);

            var xs = mask
                .Select((letter, i) => (bitPosition: mask.Length - i - 1, letter))
                .Where(t => t.letter == 'X')
                .Select(t => t.bitPosition)
                .ToArray();
            var range = Enumerable.Range(0, 1 << xs.Length);
            var addresses = new ulong[range.Count()];

            foreach (var i in range) {
                var cc = Convert.ToString(i, 2);
                var c = cc.Select((c, i) => (cc.Length - 1 - i, c)).Where(t => t.c == '1').Select(t => t.Item1);

                var thisAddress = baseAddress
                    ;
                foreach (var bit in c) {
                    thisAddress |= 1ul << xs.ElementAt(bit);
                }
                addresses[i] = thisAddress;
            }

            return addresses;
        }

        public override ulong SecondStar() {
            var memorySize = InputLines
                .Where(l => l.StartsWith("mem"))
                .Select(l => MemRegex.Match(l).Groups.Values.ElementAt(1).Value)
                .Select(int.Parse)
                .Max();
            var memory = new Dictionary<ulong, ulong>();
            var currentMask = "";

            var masks = InputLines
                .Where(l => l.StartsWith("mask"))
                .Select(l => l.Count(c => c == 'X'))
                .Max();

            foreach (var line in InputLines) {
                if (line.StartsWith("mask")) {
                    currentMask = line[7..];
                }
                else {
                    var mem = MemRegex.Match(line).Groups.Values.Skip(1).Select(v => ulong.Parse(v.Value));
                    var addresses = Addresses(currentMask, mem.First());
                    foreach(var address in addresses) {
                        memory[address] = mem.Last();
                    }
                }
            }
            var sum = 0ul;
            foreach (var m in memory.Values) {
                checked {
                    sum += m;
                }
            }

            return sum;
        }
    }
}
