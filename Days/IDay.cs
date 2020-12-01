using System.Collections.Generic;

namespace advent_of_code_2020.Days {
    public interface IDay {
        IList<string> InputLines { get; }
        string FirstStar();
        string SecondStar();
    }
}