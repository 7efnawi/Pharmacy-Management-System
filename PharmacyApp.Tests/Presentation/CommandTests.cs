// PharmacyApp.Tests/Presentation/CommandTests.cs
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PharmacyApp.Presentation.Constants;
using System.Linq;

namespace PharmacyApp.Tests.Presentation
{
    [TestClass]
    public class CommandTests
    {
        [TestMethod]
        public void GetAllCommands_ReturnsAtLeastTenCommands()
        {
            var commands = AppCommands.GetAllCommands();
            Assert.IsGreaterThanOrEqualTo(10, commands.Count);
        }

        [TestMethod]
        public void CommandIds_MustBeUnique()
        {
            var commands = AppCommands.GetAllCommands();
            var duplicateIds = commands
                .GroupBy(c => c.CommandId)
                .Where(g => g.Count() > 1)
                .Select(g => g.Key)
                .ToList();

            Assert.IsEmpty(duplicateIds);
        }

        [TestMethod]
        public void KeyboardShortcuts_HasAtLeastFiveConfigured()
        {
            var commands = AppCommands.GetAllCommands();
            int shortcutCount = commands.Count(c => !string.IsNullOrWhiteSpace(c.ShortcutText));
            Assert.IsGreaterThanOrEqualTo(5, shortcutCount);
        }

        [TestMethod]
        public void Shortcuts_MustBeUniqueExceptNone()
        {
            var commands = AppCommands.GetAllCommands()
                .Where(c => !string.IsNullOrWhiteSpace(c.ShortcutText));

            var duplicateShortcuts = commands
                .GroupBy(c => c.ShortcutText)
                .Where(g => g.Count() > 1)
                .Select(g => g.Key)
                .ToList();

            Assert.IsEmpty(duplicateShortcuts);
        }
    }
}
