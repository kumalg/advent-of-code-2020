using System.IO;

namespace advent_of_code_2020.Days {
    public abstract class Day<T> : IDay<T> {
        private string[] _inputLines;
        public string[] InputLines => _inputLines ??= File.ReadAllLines($"../../../Days/{GetType().Name}/input.txt");
        public string FirstStarString => FirstStar().ToString();
        public string SecondStarString => SecondStar().ToString();
        public abstract T FirstStar();
        public abstract T SecondStar();
    }
}
