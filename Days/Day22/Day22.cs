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

        private static string RoundToString(Player[] players) => string.Join("", players.Select(p => $"({string.Join("|", p.Cards)})"));

        private ((int Id, int Card) Winner, (int Id, int Card) Looser) PlayGame(Player[] players, bool withSubGames) {
            var playedGames = new HashSet<string>();

            while (players.All(p => p.Cards.Count > 0)) {
                if (!playedGames.Add(RoundToString(players))) {
                    return ((players[0].Id, players[0].Cards.First()), (players[1].Id, players[1].Cards.First()));
                }

                //var topCards = players.Select(p => (playerId: p.Id, card: p.Cards.First())).ToList();
                ((int Id, int Card) winner, (int id, int card) looser) results;

                if (withSubGames && players.All(p => p.Cards.Count > p.Cards.First())) {
                    results = PlayGame(players.Select(p => new Player { Id = p.Id, Cards = p.Cards.Skip(1).Take(p.Cards.First()).ToList() }).ToArray(), withSubGames);

                    //foreach (var player in players) {
                    //    player.Cards.RemoveAt(0);
                    //}

                    //players.ElementAt(subGameWinner.Id).Cards.AddRange(subGameWinner.Id > 0 ? topCards.Select(t => t.card).Reverse() : topCards.Select(t => t.card));
                }
                else {
                    var orderedPlayers = players.OrderByDescending(p => p.Cards.First()).ToArray();
                    results = ((orderedPlayers[0].Id, orderedPlayers[0].Cards.First()), (orderedPlayers[1].Id, orderedPlayers[1].Cards.First()));
                    //var orderedTopCards = topCards.OrderByDescending(v => v.card).ToList();

                    //foreach (var player in players) {
                    //    player.Cards.RemoveAt(0);
                    //}

                    //players.ElementAt(orderedTopCards.First().playerId).Cards.AddRange(orderedTopCards.Select(t => t.card));
                }

                foreach (var player in players) {
                    player.Cards.RemoveAt(0);
                }

                players.ElementAt(results.winner.Id).Cards.AddRange(results.winner.Id > 0 ? topCards.Select(t => t.card).Reverse() : topCards.Select(t => t.card));
            }
            return players
                .Where(p => p.Cards.Count > 0)
                .First();
        }

        public override long FirstStar() => PlayGame(ToPlayers(InputText), withSubGames: false)
                .Cards
                .Reverse<int>()
                .Select((v, id) => v * (id + 1))
                .Sum();

        public override long SecondStar() => PlayGame(ToPlayers(InputText), withSubGames: true)
                .Cards
                .Reverse<int>()
                .Select((v, id) => v * (id + 1))
                .Sum();
    }
}
