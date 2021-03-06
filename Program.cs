﻿using System;
using advent_of_code_2020.Days;

namespace advent_of_code_2020 {
    class Program {
        static void Main() => ShowDayResult(new Day25());

        static void ShowDayResult(IDay day) {
            Console.WriteLine($"{ day.GetType().Name }");
            Console.WriteLine($"Part 1: { day.FirstStarString }");
            Console.WriteLine($"Part 2: { day.SecondStarString }");
        }
    }
}
