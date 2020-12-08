using System.Collections.Generic;
using System.Linq;

namespace advent_of_code_2020.Days.Day08 {
    public class Day08 : Day<int> {

        public override int FirstStar() {
            (string Operation, int Argument)[] instructions = InputLines.Select(l => l.Split()).Select(l => (l[0], int.Parse(l[1]))).ToArray();
            var position = 0;
            var accumulator = 0;
            var hash = new HashSet<int>();

            while(hash.Add(position) && position < instructions.Length) {
                var instruction = instructions[position];
                if (instruction.Operation == "nop") {
                    position++;
                }
                else if (instruction.Operation == "acc") {
                    accumulator += instruction.Argument;
                    position++;
                }
                else if (instruction.Operation == "jmp") {
                    position += instruction.Argument;
                }
            }

            return accumulator;
        }

        public override int SecondStar() {
            (string Operation, int Argument)[] instructions = InputLines.Select(l => l.Split()).Select(l => (l[0], int.Parse(l[1]))).ToArray();


            foreach(var (Index, Operation, Argument) in instructions.Select((i, index) => (Index: index, i.Operation, i.Argument)).Where(i => i.Operation != "acc")) {

                var position = 0;
                var accumulator = 0;
                var hash = new HashSet<int>();

                while (hash.Add(position) && position < instructions.Length) {
                    var instruction = instructions[position];
                    var operation = instruction.Operation;
                    if (Index == position) {
                        operation = Operation switch {
                            "nop" => "jmp",
                            "jmp" => "nop"
                        };
                    }
                    if (operation == "nop") {
                        position++;
                    }
                    else if (operation == "acc") {
                        accumulator += instruction.Argument;
                        position++;
                    }
                    else if (operation == "jmp") {
                        position += instruction.Argument;
                    }
                }

                if (position >= instructions.Length) {
                    return accumulator;
                }
            }

            return -1;
        }
    }
}
