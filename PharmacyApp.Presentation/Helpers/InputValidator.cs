// PharmacyApp.Presentation/Helpers/InputValidator.cs
using System;

namespace PharmacyApp.Presentation.Helpers
{
    public static class InputValidator
    {
        public static (bool isValid, string errorMessage) ValidateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return (false, "Medicine name is required and cannot be empty.");
            if (name.Trim().Length > 100)
                return (false, "Medicine name must not exceed 100 characters.");
            return (true, string.Empty);
        }

        public static (bool isValid, string errorMessage) ValidatePrice(string priceText)
        {
            if (!decimal.TryParse(priceText, out decimal price))
                return (false, "Price must be a valid number (e.g. 12.50).");
            if (price <= 0)
                return (false, "Price must be greater than zero.");
            return (true, string.Empty);
        }

        public static (bool isValid, string errorMessage) ValidateStock(string stockText)
        {
            if (!int.TryParse(stockText, out int stock))
                return (false, "Stock quantity must be a whole number (e.g. 50).");
            if (stock < 0)
                return (false, "Stock quantity cannot be negative.");
            return (true, string.Empty);
        }

        public static (bool isValid, string errorMessage) ValidateExpiryDate(DateTime date)
        {
            if (date <= DateTime.Today)
                return (false, "Expiry date must be a future date (after today).");
            return (true, string.Empty);
        }

        public static (bool isValid, string errorMessage) ValidateSaleQuantity(string qtyText, int availableStock)
        {
            if (!int.TryParse(qtyText, out int qty))
                return (false, "Quantity must be a whole number.");
            if (qty <= 0)
                return (false, "Quantity must be greater than zero.");
            if (qty > availableStock)
                return (false, $"Quantity ({qty}) exceeds available stock ({availableStock}).");
            return (true, string.Empty);
        }
    }
}
