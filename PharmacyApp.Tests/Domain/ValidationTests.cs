// PharmacyApp.Tests/Domain/ValidationTests.cs
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PharmacyApp.Presentation.Helpers;
using System;

namespace PharmacyApp.Tests.Domain
{
    [TestClass]
    public class ValidationTests
    {
        [TestMethod]
        public void ValidateName_EmptyString_ReturnsFalse()
        {
            var (isValid, msg) = InputValidator.ValidateName(string.Empty);
            Assert.IsFalse(isValid);
            Assert.IsFalse(string.IsNullOrWhiteSpace(msg));
        }

        [TestMethod]
        public void ValidateName_ValidString_ReturnsTrue()
        {
            var (isValid, _) = InputValidator.ValidateName("Paracetamol 500mg");
            Assert.IsTrue(isValid);
        }

        [TestMethod]
        public void ValidatePrice_Zero_ReturnsFalse()
        {
            var (isValid, msg) = InputValidator.ValidatePrice("0");
            Assert.IsFalse(isValid);
            Assert.IsFalse(string.IsNullOrWhiteSpace(msg));
        }

        [TestMethod]
        public void ValidatePrice_NegativeNumber_ReturnsFalse()
        {
            var (isValid, _) = InputValidator.ValidatePrice("-5.00");
            Assert.IsFalse(isValid);
        }

        [TestMethod]
        public void ValidatePrice_ValidPositiveDecimal_ReturnsTrue()
        {
            var (isValid, _) = InputValidator.ValidatePrice("25.50");
            Assert.IsTrue(isValid);
        }

        [TestMethod]
        public void ValidateStock_NegativeInteger_ReturnsFalse()
        {
            var (isValid, _) = InputValidator.ValidateStock("-1");
            Assert.IsFalse(isValid);
        }

        [TestMethod]
        public void ValidateStock_Zero_ReturnsTrue()
        {
            var (isValid, _) = InputValidator.ValidateStock("0");
            Assert.IsTrue(isValid);
        }

        [TestMethod]
        public void ValidateExpiryDate_PastDate_ReturnsFalse()
        {
            var (isValid, msg) = InputValidator.ValidateExpiryDate(DateTime.Today.AddDays(-1));
            Assert.IsFalse(isValid);
            Assert.IsFalse(string.IsNullOrWhiteSpace(msg));
        }

        [TestMethod]
        public void ValidateExpiryDate_FutureDate_ReturnsTrue()
        {
            var (isValid, _) = InputValidator.ValidateExpiryDate(DateTime.Today.AddMonths(6));
            Assert.IsTrue(isValid);
        }

        [TestMethod]
        public void ValidateSaleQuantity_ExceedsStock_ReturnsFalse()
        {
            var (isValid, msg) = InputValidator.ValidateSaleQuantity("150", 100);
            Assert.IsFalse(isValid);
            Assert.IsFalse(string.IsNullOrWhiteSpace(msg));
        }

        [TestMethod]
        public void ValidateSaleQuantity_ValidQuantity_ReturnsTrue()
        {
            var (isValid, _) = InputValidator.ValidateSaleQuantity("5", 100);
            Assert.IsTrue(isValid);
        }
    }
}
