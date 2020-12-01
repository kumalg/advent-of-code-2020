using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace advent_of_code_2020.Days {
    public abstract class Day : IDay {
        private IList<string> _inputLines;
        public IList<string> InputLines => _inputLines ??= File.ReadAllLines($"../../../Days/{GetType().Name}/input.txt").ToList();
        public abstract string FirstStar();
        public abstract string SecondStar();
    }
}
