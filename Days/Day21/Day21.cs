using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace advent_of_code_2020.Days {
    public class Day21 : Day<string> {
        private static readonly Regex LineRegex = new Regex(@"(.+) \(contains (.+)\)");

        private static List<(int Id, string[] Ingredients, string[] Allergens)> GenerateFoodsList(string[] input) {
            return input
                .Select(l => LineRegex.Match(l).Groups.Values.Select(v => v.Value))
                .Select((v, id) => (id, v.ElementAt(1).Split(" "), v.ElementAt(2).Split(", ")))
                .ToList();
        }

        private static Dictionary<string, string> GenerateIngredientsDictionary(List<(int Id, string[] Ingredients, string[] Allergens)> foods) {
            var ingredients = foods
                .SelectMany(f => f.Ingredients)
                .Distinct()
                .ToDictionary(v => v, _ => string.Empty);
            foreach (var food in foods) {
                foreach (var allergen in food.Allergens) {
                    var restFoods = foods
                        .Where(otherfood => otherfood.Allergens.Contains(allergen) && otherfood.Id != food.Id)
                        .ToList();
                    var matchingIngredient = food.Ingredients;
                    foreach (var restFood in restFoods) {
                        matchingIngredient = matchingIngredient.Intersect(restFood.Ingredients).ToArray();
                    }
                    matchingIngredient = matchingIngredient.Except(ingredients.Where(v => v.Value != string.Empty).Select(v => v.Key).ToList()).ToArray();
                    if (matchingIngredient.Count() == 1) {
                        ingredients[matchingIngredient.First()] = allergen;
                    }
                }
            }
            return ingredients;
        }

        public override string FirstStar() {
            var foods = GenerateFoodsList(InputLines);
            var ingredients = GenerateIngredientsDictionary(foods);
            var emptyIngredients = ingredients.Where(v => v.Value == string.Empty).Select(v => v.Key).ToList();
            return foods.SelectMany(f => f.Ingredients).Count(i => emptyIngredients.Any(a => a == i)).ToString();
        }

        public override string SecondStar() {
            var foods = GenerateFoodsList(InputLines);
            var ingredients = GenerateIngredientsDictionary(foods);
            var fullIngredients = ingredients.Where(v => v.Value != string.Empty).ToList();
            return string.Join(",", fullIngredients.OrderBy(f => f.Value).Select(f => f.Key));
        }
    }
}
