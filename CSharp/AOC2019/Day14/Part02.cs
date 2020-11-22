using AOC.Resources;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace AOC2019.Day14
{
    /**
     * https://adventofcode.com/2019/day/14
     */

    public static class Part02
    {
        private const string Fuel = "FUEL";
        private const long OreQuantity = 1_000_000_000_000;

        public static void Exec()
        {
            string[] input = General.ReadInput(Day.Day14);
            NanoFactory factory = new NanoFactory(input);

            long remainingOre = OreQuantity;
            int fuelBatch = 1_000_000;
            int fuel = 0;
            do
            {
                long requiredOre = factory.CalculateOreRequirement(Fuel, fuelBatch);
                if (requiredOre <= remainingOre)
                {
                    fuel += fuelBatch;
                    remainingOre -= requiredOre;
                    continue;
                }

                if (fuelBatch > 1)
                {
                    fuelBatch /= 10;
                    continue;
                }

                break;
            }
            while (true);

            General.PrintResult("The maximum amount of fuel you can produce is", fuel);
        }

        private sealed class NanoFactory
        {
            private const string Ore = "ORE";

            private readonly Dictionary<string, Recipe> _recipies = new Dictionary<string, Recipe>();
            private readonly Dictionary<string, long> _storage = new Dictionary<string, long>();

            #region Constructor

            public NanoFactory(string[] input)
            {
                ParseInput(input);
                InitStorage();
            }

            private void ParseInput(string[] input)
            {
                foreach (var line in input)
                {
                    Dictionary<string, int> ingredients = new Dictionary<string, int>();

                    string[] products = line.Split(new[] { ", ", " => " }, StringSplitOptions.None);
                    foreach (var product in products)
                    {
                        string[] productComponents = product.Split(' ');

                        int quantity = int.Parse(productComponents[0]);
                        string productName = productComponents[1];

                        ingredients[productName] = quantity;
                    }

                    // The last entry is the result, not an ingredient.
                    string outputName = ingredients.Keys.Last();
                    ingredients.Remove(outputName, out int outputQuantity);

                    _recipies[outputName] = new Recipe(ingredients, outputQuantity);
                }
            }

            private void InitStorage()
            {
                foreach (var product in _recipies.Keys)
                {
                    _storage[product] = 0;
                }
            }

            #endregion Constructor

            public long CalculateOreRequirement(string product, long quantity)
            {
                if (product == Ore)
                    return quantity;

                long storedQuantity = _storage[product];
                if (storedQuantity >= quantity)
                {
                    // We have enough stored already.
                    _storage[product] -= quantity;
                    return 0;
                }

                quantity -= storedQuantity;
                storedQuantity = 0;

                // We need to make more.
                Recipe recipe = _recipies[product];
                long batchSize = (long)Math.Ceiling(quantity / (double)recipe.OutputQuantity);

                long oreRequirement = 0;
                foreach (var ingredient in recipe.Ingredients)
                {
                    oreRequirement += CalculateOreRequirement(ingredient.Key, checked(ingredient.Value * batchSize));
                }
                storedQuantity += checked(recipe.OutputQuantity * batchSize);

                _storage[product] = storedQuantity - quantity;
                return oreRequirement;
            }

            public void ClearStorage()
            {
                _storage.Clear();
            }
        }

        private sealed class Recipe
        {
            public Recipe(Dictionary<string, int> ingredients, int outputQuantity)
            {
                var copy = ingredients.ToDictionary(pair => pair.Key, pair => pair.Value);

                Ingredients = new ReadOnlyDictionary<string, int>(copy);
                OutputQuantity = outputQuantity;
            }

            public ReadOnlyDictionary<string, int> Ingredients { get; }
            public int OutputQuantity { get; }
        }
    }
}