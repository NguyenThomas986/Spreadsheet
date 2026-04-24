using SpreadsheetEngine;

namespace Spreadsheet_Testing
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {

        }

        /// <summary>
        /// Concrete implementation of abstract Cell for testing.
        /// </summary>
        private class TestCell : Cell
        {
            public TestCell(int row, int col) : base(row, col) { }
        }

        /// <summary>
        /// Tests that the default background color is white (0xFFFFFFFF).
        /// </summary>
        [Test]
        public void BGColorDefaultIsWhite()
        {
            var cell = new TestCell(0, 0);
            Assert.That(cell.BGColor, Is.EqualTo(0xFFFFFFFF));
        }

        /// <summary>
        /// Tests that setting BGColor fires PropertyChanged with correct property name.
        /// </summary>
        [Test]
        public void BGColorSetFiresPropertyChanged()
        {
            var cell = new TestCell(0, 0);
            string? firedProperty = null;
            cell.PropertyChanged += (s, e) => firedProperty = e.PropertyName;

            cell.BGColor = 0xFFFF0000;

            Assert.That(firedProperty, Is.EqualTo("BGColor"));
        }

        /// <summary>
        /// Tests that setting BGColor to the same value does not fire PropertyChanged.
        /// </summary>
        [Test]
        public void BGColorSetSameValueDoesNotFirePropertyChanged()
        {
            var cell = new TestCell(0, 0);
            int eventCount = 0;
            cell.PropertyChanged += (s, e) => { if (e.PropertyName == "BGColor") eventCount++; };

            cell.BGColor = 0xFFFFFFFF;

            Assert.That(eventCount, Is.EqualTo(0));
        }
    }
}
