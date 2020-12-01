using System;
using advent_of_code_2020.Days.Day01;

namespace advent_of_code_2020 {
    class Program {
        static void Main(string[] args) {
            ShowResult();
        }

        static async void ShowResult() {
            Console.WriteLine(await Day01.FirstStar());
            Console.WriteLine(await Day01.SecondStar());
        }
    }
}
