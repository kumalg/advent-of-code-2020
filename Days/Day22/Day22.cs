using System.Collections.Generic;
using System.Linq;

namespace advent_of_code_2020.Days {
    public class Day22 : Day<long> {
        private class Player {
            public int Id { get; set; }
            public List<int> Cards { get; set; }
        }

        private static Player[] ToPlayers(string inputText) => inputText
            .Trim()
            .Split("\n\n")
            .Select((lines, id) => new Player {
                Id = id,
                Cards = lines.Split("\n").Skip(1).Select(int.Parse).ToList()
            })
            .ToArray();

        private static string RoundToString(Player[] players) => string.Join("", players.Select(p => $"({ string.Join("|", p.Cards) })"));

        private (Player Winner, Player Looser) PlayGame(Player[] players, bool withSubGames) {
            var playedGames = new HashSet<string>();

            while (players.All(p => p.Cards.Count > 0)) {
                if (!playedGames.Add(RoundToString(players))) {
                    return (players[0], players[1]);
                }

                (Player Winner, Player Looser) result;

                var topCards = players.ToDictionary(p => p.Id, p => p.Cards.First());

                if (withSubGames && players.All(p => p.Cards.Count > p.Cards.First())) {
                    result = PlayGame(players.Select(p => new Player {
                        Id = p.Id,
                        Cards = p.Cards.Skip(1).Take(topCards[p.Id]).ToList()
                    }).ToArray(), withSubGames);
                }
                else {
                    var orderedPlayers = players.OrderByDescending(p => p.Cards.First()).ToArray();
                    result = (orderedPlayers[0], orderedPlayers[1]);
                }

                foreach (var player in players) {
                    player.Cards.RemoveAt(0);
                }

                players.First(p => p.Id == result.Winner.Id).Cards.AddRange(new[] { topCards[result.Winner.Id], topCards[result.Looser.Id] });
            }

            var resultedOrder = players.OrderByDescending(p => p.Cards.Count).ToArray();
            return (resultedOrder[0], resultedOrder[1]);
        }

        public override long FirstStar() => PlayGame(ToPlayers(InputText), withSubGames: false)
            .Winner
            .Cards
            .Reverse<int>()
            .Select((v, id) => v * (id + 1))
            .Sum();

        public override long SecondStar() => PlayGame(ToPlayers(InputText), withSubGames: true)
            .Winner
            .Cards
            .Reverse<int>()
            .Select((v, id) => v * (id + 1))
            .Sum();
    }
}
