using System;

namespace TicketApplicationSystem
{
    /// <summary>
    /// Business logic class for ticket price calculation
    /// Handles pricing rules and discount application
    /// </summary>
    public class TicketCalculator
    {
        // Category pricing constants
        private const decimal CATEGORY_ONE_RATE = 20m;
        private const decimal CATEGORY_TWO_RATE = 35m;
        private const decimal CATEGORY_THREE_RATE = 50m;

        /// <summary>
        /// Calculate base ticket price based on category and distance
        /// </summary>
        /// <param name="category">Travel category (One, Two, or Three)</param>
        /// <param name="distance">Distance in kilometers</param>
        /// <returns>Base price before discounts</returns>
        public decimal CalculateBasePrice(string category, decimal distance)
        {
            decimal ratePerKm = 0;

            switch (category)
            {
                case "Category One":
                    ratePerKm = CATEGORY_ONE_RATE;
                    break;
                case "Category Two":
                    ratePerKm = CATEGORY_TWO_RATE;
                    break;
                case "Category Three":
                    ratePerKm = CATEGORY_THREE_RATE;
                    break;
                default:
                    throw new ArgumentException("Invalid category selected");
            }

            return ratePerKm * distance;
        }

        /// <summary>
        /// Apply discount rules based on age and gender
        /// </summary>
        /// <param name="basePrice">Original ticket price</param>
        /// <param name="age">Passenger age</param>
        /// <param name="gender">Passenger gender</param>
        /// <returns>Final price after discounts</returns>
        public decimal ApplyDiscounts(decimal basePrice, int age, string gender)
        {
            decimal finalPrice = basePrice;

            // Rule 1: Children under 12 get free tickets
            if (age < 12)
            {
                return 0m;
            }

            // Rule 2: Female passengers get 50% discount
            if (gender.Equals("Female", StringComparison.OrdinalIgnoreCase))
            {
                finalPrice = finalPrice * 0.5m;
            }

            return finalPrice;
        }

        /// <summary>
        /// Complete ticket price calculation with all rules applied
        /// </summary>
        /// <param name="category">Travel category</param>
        /// <param name="distance">Distance in kilometers</param>
        /// <param name="age">Passenger age</param>
        /// <param name="gender">Passenger gender</param>
        /// <returns>Final ticket price</returns>
        public decimal CalculateFinalPrice(string category, decimal distance, int age, string gender)
        {
            decimal basePrice = CalculateBasePrice(category, distance);
            decimal finalPrice = ApplyDiscounts(basePrice, age, gender);
            return finalPrice;
        }
    }
}
