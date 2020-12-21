using System.Linq;

namespace advent_of_code_2020.Days {
    public class Day11 : Day<long> {
        private enum SeatType {
            Occupied,
            Empty,
            Floor
        }

        private static SeatType AsSeatType(char c) => c switch {
            '#' => SeatType.Occupied,
            'L' => SeatType.Empty,
            '.' => SeatType.Floor
        };

        private static (int y, int x)[] AdjacentSeats(int x, int y, int xMax, int yMax) {
            var e = Enumerable
                .Range(y - 1, 3)
                .Select(y => Enumerable
                    .Range(x - 1, 3)
                    .Select(xx => (y, xx)))
                .SelectMany(t => t);
            var h = e
                .Where(t => t.xx >= 0 && t.xx <= xMax && t.y >= 0 && t.y <= yMax && t != (y, x))
                .ToArray();
            return h;
        }

        private int MakeRound(SeatType[][] map) {
            var initialMap = map.Select(l => l.ToArray()).ToArray();
            var changes = 0;

            for (var y = 0; y < initialMap.Length; y++) {
                for (var x = 0; x < initialMap[y].Length; x++) {
                    if (initialMap[y][x] == SeatType.Floor) continue;

                    var occupiedAdjacent = AdjacentSeats(x, y, initialMap[y].Length - 1, initialMap.Length - 1)
                        .Count(t => initialMap[t.y][t.x] == SeatType.Occupied);

                    if (initialMap[y][x] == SeatType.Empty && occupiedAdjacent == 0) {
                        map[y][x] = SeatType.Occupied;
                        changes++;
                    }
                    else if (initialMap[y][x] == SeatType.Occupied && occupiedAdjacent >= 4) {
                        map[y][x] = SeatType.Empty;
                        changes++;
                    }
                }
            }

            return changes;
        }

        private static SeatType? VisibleSeat(SeatType[][] map, (int x, int y) position, (int x, int y) direction) {
            var searched = (x: position.x + direction.x, y: position.y + direction.y);

            while (
                searched.x >= 0 &&
                searched.y >= 0 &&
                searched.x < map[position.y].Length &&
                searched.y < map.Length
            ) {
                if (map[searched.y][searched.x] is SeatType.Occupied or SeatType.Empty) {
                    return map[searched.y][searched.x];
                }
                searched = (x: searched.x + direction.x, y: searched.y + direction.y);
            }
            return null;
        }

        private static int MakeRound2(SeatType[][] map) {
            var initialMap = map.Select(l => l.ToArray()).ToArray();
            var changes = 0;

            for (var y = 0; y < initialMap.Length; y++) {
                for (var x = 0; x < initialMap[y].Length; x++) {
                    if (initialMap[y][x] == SeatType.Floor) continue;

                    var visibleOccupiedSeats = AdjacentSeats(x, y, initialMap[y].Length - 1, initialMap.Length - 1)
                        .Count(a => VisibleSeat(initialMap, (x, y), (a.x - x, a.y - y)) == SeatType.Occupied);

                    if (initialMap[y][x] == SeatType.Empty && visibleOccupiedSeats == 0) {
                        map[y][x] = SeatType.Occupied;
                        changes++;
                    }
                    else if (initialMap[y][x] == SeatType.Occupied && visibleOccupiedSeats >= 5) {
                        map[y][x] = SeatType.Empty;
                        changes++;
                    }
                }
            }

            return changes;
        }

        public override long FirstStar() {
            var map = InputLines.Select(l => l.Select(AsSeatType).ToArray()).ToArray();
            while (MakeRound(map) != 0);
            return map.SelectMany(i => i).Count(t => t == SeatType.Occupied);
        }

        public override long SecondStar() {
            var map = InputLines.Select(l => l.Select(AsSeatType).ToArray()).ToArray();
            while (MakeRound2(map) != 0);
            return map.SelectMany(i => i).Count(t => t == SeatType.Occupied);
        }
    }
}
