namespace advent_of_code_2020.Days {
    public interface IDay {
        string[] InputLines { get; }
        object FirstStar();
        object SecondStar();
    }
}
