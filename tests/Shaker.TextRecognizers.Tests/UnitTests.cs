using TextRecognizers.Units;
using Xunit;
using static TextRecognizers.Tests.Wf;

namespace TextRecognizers.Tests
{
    public class UnitTests
    {
        private static (double value, string unit, bool success) Parse(string text, MeasurementKind kind)
        {
            var outputs = Out(new ParseMeasurement { Text = Lit(text), Kind = kind });
            return ((double)outputs["Value"], (string)outputs["Unit"], (bool)outputs["Success"]);
        }

        [Fact]
        public void Parses_currency_value_and_unit()
        {
            var (value, unit, success) = Parse("it costs $19.99", MeasurementKind.Currency);
            Assert.True(success);
            Assert.Equal(19.99, value, 2);
            Assert.NotEmpty(unit);
        }

        [Fact]
        public void Parses_dimension()
        {
            var (value, unit, _) = Parse("the parcel weighs 5 kg", MeasurementKind.Dimension);
            Assert.Equal(5d, value);
            Assert.NotEmpty(unit);
        }

        [Fact]
        public void Parses_temperature()
        {
            var (value, _, success) = Parse("it is 20 degrees Celsius", MeasurementKind.Temperature);
            Assert.True(success);
            Assert.Equal(20d, value);
        }

        [Fact]
        public void Parses_age()
        {
            var (value, _, success) = Parse("she is 25 years old", MeasurementKind.Age);
            Assert.True(success);
            Assert.Equal(25d, value);
        }
    }
}
