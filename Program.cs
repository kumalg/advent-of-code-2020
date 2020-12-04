using System;
using advent_of_code_2020.Days;
using advent_of_code_2020.Days.Day04;

namespace advent_of_code_2020 {
    class Program {
        static void Main(string[] args) => ShowDayResult(new Day04());

        static void ShowDayResult(IDay day) {
            Console.WriteLine($"{ day.GetType().Name }");
            Console.WriteLine($"Part 1: { day.FirstStarString }");
            Console.WriteLine($"Part 2: { day.SecondStarString }");
        }
    }
}
