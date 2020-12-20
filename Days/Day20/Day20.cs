using System;
using System.Collections.Generic;
using System.Linq;

namespace advent_of_code_2020.Days {
    public class Day20 : Day<long> {
        private static readonly string[] seaMonsterPattern = new[] {
            "                  # ",
            "#    ##    ##    ###",
            " #  #  #  #  #  #   "
        };
        private static readonly IList<(int X, int Y)> seaMonstersArray = seaMonsterPattern
            .Select((l, y) => l.Select((ll, x) => ((x, y), ll)))
            .SelectMany(ll => ll)
            .Where(ll => ll.ll == '#')
            .Select(ll => ll.Item1)
            .ToList();

        private class Tile {
            public long Id { get; }
            public char[][] Content { get; private set; }
            public string[] Sides { get; }
            public string[] Sides2 => new[] {
                    TopSide,
                    RightSide,
                    BottomSide,
                    LeftSide
            };
            public string TopSide => string.Join("", Content.First());
            public string RightSide => string.Join("", Content.Select(l => l.Last()));
            public string BottomSide => string.Join("", Content.Last());
            public string LeftSide => string.Join("", Content.Select(l => l.First()));

            public char[][] Rotate() {
                return Content = Enumerable
                    .Range(0, Content.Length)
                    .Select(i => Content
                        .Select(c => c[i])
                        .Reverse()
                        .ToArray()
                    )
                    .ToArray();
            }

            public char[][] Flip() {
                return Content = Content.Select(l => l.Reverse().ToArray()).ToArray();
            }

            public char[][] Unbordered() {
                return Content
                    .Skip(1)
                    .SkipLast(1)
                    .Select(l => l.Skip(1).SkipLast(1).ToArray())
                    .ToArray();
            }

            public Tile(long id, char[][] content) {
                Id = id;
                Content = content;
                Sides = new[] {
                    TopSide,
                    RightSide,
                    BottomSide,
                    LeftSide,
                    string.Join("", TopSide.Reverse()),
                    string.Join("", RightSide.Reverse()),
                    string.Join("", BottomSide.Reverse()),
                    string.Join("", LeftSide.Reverse())
                };
            }
        }

        public override long FirstStar() {
            var tiles = InputText
                .Trim()
                .Split("\n\n")
                .Select(l => l.Split("\n"))
                .ToDictionary(
                    v => long.Parse(v.First()[5..^1]),
                    v => v.Skip(1).Select(l => l.ToCharArray()).ToArray()
                )
                .Select(v => new Tile(v.Key, v.Value))
                .ToList();
            var tilesNeighbors = tiles
                .Where(tile => tiles
                    .Where(otherTile =>
                        otherTile.Id != tile.Id &&
                        otherTile.Sides.Intersect(tile.Sides).Any(
                    )).Count() == 2
                )
                .Select(t => t.Id)
                .ToList();
            return tilesNeighbors.Aggregate((a, b) => a * b);
        }

        private (int X, int Y) Shift(Tile from, Tile to) {
            if (from.RightSide == to.LeftSide) return (1, 0);
            if (from.LeftSide == to.RightSide) return (-1, 0);
            if (from.TopSide == to.BottomSide) return (0, 1);
            if (from.BottomSide == to.TopSide) return (0, -1);
            return (0, 0);
        }

        private static List<string> FlipPicture(IList<string> picture) {
            return picture.Reverse().ToList();
        }

        private static List<string> RotatePicture(IList<string> picture) {
            return Enumerable
                .Range(0, picture.Count)
                .Select(i => string.Join("", picture
                    .Select(c => c[i])
                    .Reverse()
                    .ToList())
                )
                .ToList();
        }

        private static int SeaMonstersCount(IList<string> picture) {
            var yMax = picture.Count - seaMonsterPattern.Length;
            var xMax = picture.First().Length - seaMonsterPattern.First().Length;
            var count = 0;

            for (var y = 0; y < yMax; y++) {
                for (var x = 0; x < xMax; x++) {
                    var yak = picture.Skip(y).Select(xx => xx.Substring(x, seaMonsterPattern.First().Length).ToCharArray()).ToArray();
                    if (seaMonstersArray.All(a => yak[a.Y][a.X] == '#')) {
                        count++;
                    }
                }
            }
            return count;
        }

        private Dictionary<(int X, int Y), Tile> GetMap() {
            var tiles = InputText
                .Trim()
                .Split("\n\n")
                .Select(l => l.Split("\n"))
                .ToDictionary(
                    v => long.Parse(v.First()[5..^1]),
                    v => v.Skip(1).Select(l => l.ToCharArray()).ToArray()
                )
                .Select(v => new Tile(v.Key, v.Value))
                .ToList();
                        var firstCorner = tiles
                            .Where(tile => tiles
                                .Where(otherTile =>
                                    otherTile.Id != tile.Id &&
                                    otherTile.Sides.Intersect(tile.Sides).Any(
                                )).Count() == 2
                            ).First();

            var map = new Dictionary<(int X, int Y), Tile>() {
                [(0, 0)] = firstCorner
            };

            while (map.Count < tiles.Count) {
                var remainingTiles = tiles.Where(t => !map.Values.Select(m => m.Id).Contains(t.Id)).ToList();

                foreach (var remainingTile in remainingTiles) {
                    KeyValuePair<(int X, int Y), Tile>? matchMapItem = null;
                    foreach (var mapItem in map) {
                        for (var i = 0; i < 4; i++) {
                            if (Shift(mapItem.Value, remainingTile) != (0, 0)) {
                                matchMapItem = mapItem;
                                break;
                            }
                            remainingTile.Flip();
                            if (Shift(mapItem.Value, remainingTile) != (0, 0)) {
                                matchMapItem = mapItem;
                                break;
                            }
                            else {
                                remainingTile.Flip();
                                remainingTile.Rotate();
                            }
                        }
                        if (matchMapItem != null) {
                            break;
                        }
                    }
                    if (matchMapItem != null) {
                        var shift = Shift(matchMapItem.Value.Value, remainingTile);
                        map.Add((matchMapItem.Value.Key.X + shift.X, matchMapItem.Value.Key.Y + shift.Y), remainingTile);
                        break;
                    }
                }
            }
            return map;
        }

        public override long SecondStar() {
            var map = GetMap();

            var properMap = map
                .GroupBy(v => v.Key.Y)
                .OrderByDescending(v => v.Key)
                .Select(v => v
                    .OrderBy(vv => vv.Key.X)
                    .Select(vv => vv.Value.Unbordered().Select(w => string.Join("", w)).ToList())
                    .ToList())
                .ToList();
            var eee = properMap
                .SelectMany(v => {
                    var hh = Enumerable
                        .Range(0, v.ElementAt(0).Count)
                        .Select(i => {
                            var hmm = v.Select(vv => vv.ElementAt(i)).ToList();
                            return string.Join("", hmm);
                        })
                        .ToList();
                    return hh;
                })
                .Reverse()
                .ToList();

            var currentPicture = eee;

            for (var i = 0; i < 4; i++) {
                var count = SeaMonstersCount(currentPicture);
                if (count > 0) {
                    return string.Join("", currentPicture).Count(e => e == '#') - count * seaMonstersArray.Count;
                }
                count = SeaMonstersCount(FlipPicture(currentPicture));
                if (count > 0) {
                    return string.Join("", currentPicture).Count(e => e == '#') - count * seaMonstersArray.Count;
                }
                currentPicture = RotatePicture(currentPicture);
            }

            return 0;
        }
    }
}
