using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickShipParser.Facts
{
    [TestClass()]
    public class ModelParserFacts
    {
        private ModelStructure SetUpModelStructure()
        {
            string json = @"
            {
                ""modelName"": ""Mag meters"",
                ""baseModel"": ""8705"",
                ""elements"": [
                    {
                        ""codeName"": ""Base"",
                        ""length"": 4,
                        ""codes"": [
                            { ""code"": ""8705"", ""description"": ""Magnetic Flowmeter Sensor - Flanged"" }
                        ]
                    }
                ]
            }";
            return ModelStructure.FromJson(json);
        }

        [TestMethod]
        public void Match_WithValidModelString_ReturnsSuccess()
        {
            var modelStructure = SetUpModelStructure();
            var parser = new ModelParser(modelStructure);

            var result = parser.Match("8705"); // Replace with an appropriate valid model string
            Assert.IsFalse(result.Success());
        }

        [TestMethod]
        public void Match_WithInvalidModelString_ReturnsFailure()
        {
            var modelStructure = SetUpModelStructure();
            var parser = new ModelParser(modelStructure);

            var result = parser.Match("InvalidString");
            Assert.IsFalse(result.Success());
        }

        [TestMethod]
        public void Match_WithPartialValidModelString_ReturnsFailure()
        {
            var modelStructure = SetUpModelStructure();
            var parser = new ModelParser(modelStructure);

            var result = parser.Match("8705Partial"); // A partially valid string
            Assert.IsFalse(result.Success());
        }

        [TestMethod]
        public void Match_WithEmptyModelString_ReturnsFailure()
        {
            var modelStructure = SetUpModelStructure();
            var parser = new ModelParser(modelStructure);

            var result = parser.Match("");
            Assert.IsFalse(result.Success());
        }

        [TestMethod]
        public void Match_WithValidModelString_ReturnsCorrectRemainingText()
        {
            var modelStructure = SetUpModelStructure();
            var parser = new ModelParser(modelStructure);

            var result = parser.Match("8705"); // Replace with an appropriate valid model string
            Assert.AreEqual("8705", result.RemainingText()); // Assuming the whole string is used
            Assert.IsFalse(result.Success());
        }
    }
}