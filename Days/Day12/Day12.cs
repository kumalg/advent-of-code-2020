using System;
using System.Linq;

namespace advent_of_code_2020.Days.Day12 {
    public class Day12 : Day<long> {
        private enum Direction {
            North = 0,
            East = 1,
            South = 2,
            West = 3
        }

        private record Ferry {
            public int X { get; init; }
            public int Y { get; init; }
            public Direction Direction { get; init; }

            public Ferry Next(string command) {
                var action = command.First();
                var value = int.Parse(command[1..]);

                return action switch {
                    'N' => this with { Y = Y + value },
                    'S' => this with { Y = Y - value },
                    'E' => this with { X = X + value },
                    'W' => this with { X = X - value },
                    'L' => this with {
                        Direction = (Direction - value / 90) < Direction.North
                            ? (Direction + 4 - value / 90)
                            : (Direction - value / 90)
                    },
                    'R' => this with {
                        Direction = (Direction + value / 90) > Direction.West
                            ? (Direction - 4 + value / 90)
                            : (Direction + value / 90)
                    },
                    'F' => Direction switch {
                        Direction.North => this with { Y = Y + value },
                        Direction.South => this with { Y = Y - value },
                        Direction.East => this with { X = X + value },
                        Direction.West => this with { X = X - value }
                    },
                    _ => this
                };
            }
        }

        private record Ferry2 {
            public int X { get; init; }
            public int Y { get; init; }
            public (int X, int Y) Waypoint { get; init; }

            public Ferry2 Next(string command) {
                var action = command.First();
                var value = int.Parse(command[1..]);

                return action switch {
                    'F' => this with {
                        X = X + value * Waypoint.X,
                        Y = Y + value * Waypoint.Y
                    },
                    'N' => this with { Waypoint = (Waypoint.X, Waypoint.Y + value) },
                    'S' => this with { Waypoint = (Waypoint.X, Waypoint.Y - value) },
                    'E' => this with { Waypoint = (Waypoint.X + value, Waypoint.Y) },
                    'W' => this with { Waypoint = (Waypoint.X - value, Waypoint.Y) },
                    'R' => value switch {
                        90 => this with { Waypoint = (Waypoint.Y, -Waypoint.X) },
                        180 => this with { Waypoint = (-Waypoint.X, -Waypoint.Y) },
                        270 => this with { Waypoint = (-Waypoint.Y, Waypoint.X) },
                        _ => this
                    },
                    'L' => value switch {
                        90 => this with { Waypoint = (-Waypoint.Y, Waypoint.X) },
                        180 => this with { Waypoint = (-Waypoint.X, -Waypoint.Y) },
                        270 => this with { Waypoint = (Waypoint.Y, -Waypoint.X) },
                        _ => this
                    },
                    _ => this
                };
            }
        }

        private record Waypoint {
            public int X { get; init; }
            public int Y { get; init; }
        }

        public override long FirstStar() {
            var ferry = new Ferry { X = 0, Y = 0, Direction = Direction.East };

            foreach (var command in InputLines)
                ferry = ferry.Next(command);

            return Math.Abs(ferry.X) + Math.Abs(ferry.Y);
        }

        public override long SecondStar() {
            var ferry = new Ferry2 { X = 0, Y = 0, Waypoint = (10, 1) };

            foreach (var command in InputLines)
                ferry = ferry.Next(command);

            return Math.Abs(ferry.X) + Math.Abs(ferry.Y);
        }
    }
}
