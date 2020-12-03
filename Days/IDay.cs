namespace advent_of_code_2020.Days {
    public interface IDay {
        string FirstStarString { get; }
        string SecondStarString { get; }
    }

    public interface IDay<T> : IDay {
        string[] InputLines { get; }
        T FirstStar();
        T SecondStar();
    }
}
