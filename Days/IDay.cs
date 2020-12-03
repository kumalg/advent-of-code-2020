namespace advent_of_code_2020.Days {
    public interface IDay<T> {
        string[] InputLines { get; }
        T FirstStar();
        T SecondStar();
    }
}
