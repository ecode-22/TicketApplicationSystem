using System;
using System.Collections.Generic;

namespace TicketApplicationSystem
{
    public class TicketCalculator
    {
        private Dictionary<string, double> categoryRates;

        public TicketCalculator()
        {
            // Initialize category pricing rates
            categoryRates = new Dictionary<string, double>
            {
                { "Category One", 20.0 },
                { "Category Two", 35.0 },
                { "Category Three", 50.0 }
            };
        }

        /// <summary>
        /// Calculates the base price based on category and distance
        /// </summary>
        /// <param name="category">Travel category (One, Two, or Three)</param>
        /// <param name="distance">Distance in kilometers</param>
        /// <returns>Base price before discounts</returns>
        /// <exception cref="ArgumentException">Thrown when category is invalid or distance is invalid</exception>
        public double CalculateBasePrice(string category, double distance)
        {
            if (!categoryRates.ContainsKey(category))
                throw new ArgumentException($"Invalid category: {category}. Valid categories are: Category One, Category Two, Category Three");

            if (distance <= 0)
                throw new ArgumentException("Distance must be greater than 0 kilometers");

            if (distance > 10000)
                throw new ArgumentException("Distance cannot exceed 10,000 kilometers (reasonable travel limit)");

            double rate = categoryRates[category];
            return rate * distance;
        }

        /// <summary>
        /// Applies discount rules to the base price
        /// </summary>
        /// <param name="basePrice">Original calculated price</param>
        /// <param name="age">Passenger's age</param>
        /// <param name="isFemale">Whether passenger is female</param>
        /// <returns>Final price after discounts</returns>
        public double ApplyDiscounts(double basePrice, int age, bool isFemale)
        {
            // Rule 1: Age discount - FREE for children under 12
            if (age < 12)
                return 0;

            // Rule 2: Gender discount - 50% off for female passengers
            double finalPrice = basePrice;
            if (isFemale)
                finalPrice *= 0.5;

            return finalPrice;
        }

        /// <summary>
        /// Gets the rate for a specific category
        /// </summary>
        /// <param name="category">The travel category</param>
        /// <returns>Price per kilometer for the category</returns>
        public double GetCategoryRate(string category)
        {
            if (categoryRates.ContainsKey(category))
                return categoryRates[category];
            return 0;
        }

        /// <summary>
        /// Validates if a category exists
        /// </summary>
        /// <param name="category">Category to check</param>
        /// <returns>True if category exists, false otherwise</returns>
        public bool IsValidCategory(string category)
        {
            return categoryRates.ContainsKey(category);
        }

        /// <summary>
        /// Gets all available categories
        /// </summary>
        /// <returns>List of category names</returns>
        public List<string> GetAvailableCategories()
        {
            return new List<string>(categoryRates.Keys);
        }
    }
}