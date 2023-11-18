using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuickShipParser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickShipParser.Facts
{
    [TestClass()]
    public class ModelStructureFacts
    {
        private string validJson = @"
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

        [TestMethod]
        public void FromJson_ValidJson_CreatesModelWithName()
        {
            var model = ModelStructure.FromJson(validJson);
            Assert.AreEqual("Mag meters", model.ModelName);
        }

        [TestMethod]
        public void FromJson_ValidJson_CreatesModelWithBaseModel()
        {
            var model = ModelStructure.FromJson(validJson);
            Assert.AreEqual("8705", model.BaseModel);
        }

        [TestMethod]
        public void FromJson_ValidJson_CreatesModelWithElements()
        {
            var model = ModelStructure.FromJson(validJson);
            Assert.IsTrue(model.Elements.Any());
        }

        [TestMethod]
        public void FromJson_ValidJson_CorrectElementProperties()
        {
            var model = ModelStructure.FromJson(validJson);
            var element = model.Elements.FirstOrDefault();
            Assert.IsNotNull(element);
            Assert.AreEqual("Base", element.CodeName);
            Assert.AreEqual(4, element.Length);
        }

        [TestMethod]
        [ExpectedException(typeof(System.Text.Json.JsonException))]
        public void FromJson_InvalidJson_ThrowsException()
        {
            string invalidJson = "{ invalid json }";
            ModelStructure.FromJson(invalidJson);
        }
    }
}