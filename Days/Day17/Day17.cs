using System.Collections.Generic;
using System.Linq;

namespace advent_of_code_2020.Days {
    public class Day17 : Day<int> {
        private static IList<(int X, int Y, int Z)> CubeNeighbors((int X, int Y, int Z) cube) {
            return Enumerable
                .Range(cube.X - 1, 3)
                .SelectMany(X => Enumerable
                    .Range(cube.Y - 1, 3)
                    .SelectMany(Y => Enumerable
                        .Range(cube.Z - 1, 3)
                        .Select(Z => (X, Y, Z)
                )))
                .Where(c => c != cube)
                .ToList();
        }

        private static IList<(int X, int Y, int Z, int W)> Cube4Neighbors((int X, int Y, int Z, int W) cube) {
            return Enumerable
                .Range(cube.X - 1, 3)
                .SelectMany(X => Enumerable
                    .Range(cube.Y - 1, 3)
                    .SelectMany(Y => Enumerable
                        .Range(cube.Z - 1, 3)
                        .SelectMany(Z => Enumerable
                            .Range(cube.W - 1, 3)
                            .Select(W => (X, Y, Z, W)
                ))))
                .Where(c => c != cube)
                .ToList();
        }

        public override int FirstStar() {
            var activeCubes = InputLines
                .SelectMany((line, y) => line
                    .Select((c, x) => (c, x))
                    .Where(t => t.c == '#')
                    .Select(t => (X: t.x, Y: y, Z: 0)))
                .ToList();

            for (var i = 0; i < 6; i++) {
                activeCubes = activeCubes
                    .SelectMany(c => CubeNeighbors(c))
                    .Distinct()
                    .Where(c => {
                        var cubeActiveNeighbors = activeCubes.Intersect(CubeNeighbors(c));
                        return activeCubes.Contains(c)
                            ? cubeActiveNeighbors.Count() is 2 or 3
                            : cubeActiveNeighbors.Count() is 3;
                    })
                    .ToList();
            }

            return activeCubes.Count;
        }

        public override int SecondStar() {
            var activeCubes = InputLines
                .SelectMany((line, y) => line
                    .Select((c, x) => (c, x))
                    .Where(t => t.c == '#')
                    .Select(t => (X: t.x, Y: y, Z: 0, W: 0)))
                .ToList();

            for (var i = 0; i < 6; i++) {
                activeCubes = activeCubes
                    .SelectMany(c => Cube4Neighbors(c))
                    .Distinct()
                    .Where(c => {
                        var cubeActiveNeighbors = activeCubes.Intersect(Cube4Neighbors(c));
                        return activeCubes.Contains(c)
                            ? cubeActiveNeighbors.Count() is 2 or 3
                            : cubeActiveNeighbors.Count() is 3;
                    })
                    .ToList();
            }

            return activeCubes.Count;
        }
    }
}
