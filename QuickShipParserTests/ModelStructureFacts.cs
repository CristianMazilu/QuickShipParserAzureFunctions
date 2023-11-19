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
        private ModelStructure SetUpModelStructure()
        {
            var sourceFilePath = "D:\\Projects\\2023.11.18 QuickShipParserAzureFunctions\\QuickShipParser\\ModelConfigurations\\MagCodeComposition.json";
            if (!File.Exists(sourceFilePath))
            {
                throw new FileNotFoundException(sourceFilePath);
            }

            string json = File.ReadAllText(sourceFilePath);

            return ModelStructure.FromJson(json);
        }

        [TestMethod]
        public void FromJson_ValidJson_CreatesModelWithName()
        {
            var model = SetUpModelStructure();;
            Assert.AreEqual("Mag meters", model.ModelName);
        }

        [TestMethod]
        public void FromJson_ValidJson_CreatesModelWithBaseModel()
        {
            var model = SetUpModelStructure();;
            Assert.AreEqual("8705", model.BaseModel);
        }

        [TestMethod]
        public void FromJson_ValidJson_CreatesModelWithElements()
        {
            var model = SetUpModelStructure();;
            Assert.IsTrue(model.Elements.Any());
        }

        [TestMethod]
        public void FromJson_ValidJson_CorrectElementProperties()
        {
            var model = SetUpModelStructure();;
            var element = model.Elements.FirstOrDefault();
            Assert.IsNotNull(element);
            Assert.AreEqual("Base", element.CodeName);
        }

        [TestMethod]
        [ExpectedException(typeof(System.Text.Json.JsonException))]
        public void FromJson_InvalidJson_ThrowsException()
        {
            string invalidJson = "{ invalid json }";
            ModelStructure.FromJson(invalidJson);
        }

        [TestMethod]
        public void ElementMatch_ElementNotMatchingOptions_ReturnsFailure()
        {
            var modelStructure = SetUpModelStructure();;
            var element = modelStructure.Elements.FirstOrDefault(e => e.CodeName == "Electrode Type");
            var modelString = "B";

            var matchResult = element.Match(modelString);

            Assert.IsFalse(matchResult.Success());
            Assert.AreEqual("B", matchResult.RemainingText());
        }

        [TestMethod]
        public void ElementMatch_ElementMatchingNonOptionalFirstCode_ReturnsSuccess()
        {
            var modelStructure = SetUpModelStructure();
            var element = modelStructure.Elements.FirstOrDefault(e => e.CodeName == "Line Size");
            var modelString = "010RT";

            var matchResult = element.Match(modelString);

            Assert.IsTrue(matchResult.Success());
            Assert.AreEqual("RT", matchResult.RemainingText());
        }

        [TestMethod]
        public void ElementMatch_ElementMatchingNonOptionalSecondCode_ReturnsSuccess()
        {
            var modelStructure = SetUpModelStructure();
            var element = modelStructure.Elements.FirstOrDefault(e => e.CodeName == "Line Size");
            var modelString = "015RT";

            var matchResult = element.Match(modelString);

            Assert.IsTrue(matchResult.Success());
            Assert.AreEqual("RT", matchResult.RemainingText());
        }

        [TestMethod]
        public void ElementMatch_ElementMatchingNonOptionalLastCode_ReturnsSuccess()
        {
            var modelStructure = SetUpModelStructure();
            var element = modelStructure.Elements.FirstOrDefault(e => e.CodeName == "Line Size");
            var modelString = "040RT";

            var matchResult = element.Match(modelString);

            Assert.IsTrue(matchResult.Success());
            Assert.AreEqual("RT", matchResult.RemainingText());
        }

        [TestMethod]
        public void ElementMatch_ElementNotMatchingNonOptionalLastCode_ReturnsFailure()
        {
            var modelStructure = SetUpModelStructure();
            var element = modelStructure.Elements.FirstOrDefault(e => e.CodeName == "Line Size");
            var modelString = "050RT";

            var matchResult = element.Match(modelString);

            Assert.IsFalse(matchResult.Success());
            Assert.AreEqual("050RT", matchResult.RemainingText());
        }

        [TestMethod]
        public void ElementMatch_ElementNotMatchingOptionalCodes_ReturnsSuccess()
        {
            var modelStructure = SetUpModelStructure();
            var element = modelStructure.Elements.FirstOrDefault(e => e.CodeName == "Flange Type and Material");
            var modelString = "RTR";

            var matchResult = element.Match(modelString);

            Assert.IsTrue(matchResult.Success());
            Assert.AreEqual("RTR", matchResult.RemainingText());
        }

        [TestMethod]
        public void ElementMatch_ElementMatchingOptionalFirstCode_ReturnsSuccess()
        {
            var modelStructure = SetUpModelStructure();
            var element = modelStructure.Elements.FirstOrDefault(e => e.CodeName == "Flange Type and Material");
            var modelString = "CTR";

            var matchResult = element.Match(modelString);

            Assert.IsTrue(matchResult.Success());
            Assert.AreEqual("TR", matchResult.RemainingText());
        }

        [TestMethod]
        public void ElementMatch_ElementMatchingOptionalLastCode_ReturnsSuccess()
        {
            var modelStructure = SetUpModelStructure();
            var element = modelStructure.Elements.FirstOrDefault(e => e.CodeName == "Flange Type and Material");
            var modelString = "STR";

            var matchResult = element.Match(modelString);

            Assert.IsTrue(matchResult.Success());
            Assert.AreEqual("TR", matchResult.RemainingText());
        }

        [TestMethod]
        public void CodeDescriptionMatch_ValidCodeWithDescription_OptionalFalse_ReturnsSuccess()
        {
            var modelStructure = SetUpModelStructure();
            var element = modelStructure.Elements.FirstOrDefault(e => e.CodeName == "Electrode Type");
            var codeDescription = element?.Codes.FirstOrDefault(c => c.Code == "E");

            var matchResult = codeDescription?.Match("E");

            Assert.IsNotNull(matchResult);
            Assert.IsTrue(matchResult.Success());
            Assert.AreEqual("", matchResult.RemainingText());
        }

        [TestMethod]
        public void CodeDescriptionMatch_ValidCodeWithDescriptionWithRemaininText_OptionalTrue_ReturnsSuccess()
        {
            var modelStructure = SetUpModelStructure();
            var element = modelStructure.Elements.FirstOrDefault(e => e.CodeName == "Flange Rating");
            var codeDescription = element?.Codes.FirstOrDefault(c => c.Code == "H");

            var matchResult = codeDescription?.Match("H23");

            Assert.IsNotNull(matchResult);
            Assert.IsTrue(matchResult.Success());
            Assert.AreEqual("23", matchResult.RemainingText());
        }

        [TestMethod]
        public void CodeDescriptionMatch_ValidCodeWithDescriptionWithoutRemaininText_OptionalTrue_ReturnsSuccess()
        {
            var modelStructure = SetUpModelStructure();
            var element = modelStructure.Elements.FirstOrDefault(e => e.CodeName == "Flange Rating");
            var codeDescription = element?.Codes.FirstOrDefault(c => c.Code == "H");

            var matchResult = codeDescription?.Match("H");

            Assert.IsNotNull(matchResult);
            Assert.IsTrue(matchResult.Success());
            Assert.AreEqual("", matchResult.RemainingText());
        }

        [TestMethod]
        public void CodeDescriptionMatch_ValidCodeWithDescriptionWithoutRemaininText_OptionalFalse_ReturnsSuccess()
        {
            var modelStructure = SetUpModelStructure();
            var element = modelStructure.Elements.FirstOrDefault(e => e.CodeName == "Line Size");
            var codeDescription = element?.Codes.FirstOrDefault(c => c.Code == "010");

            var matchResult = codeDescription?.Match("010");

            Assert.IsNotNull(matchResult);
            Assert.IsTrue(matchResult.Success());
            Assert.AreEqual("", matchResult.RemainingText());
        }

        [TestMethod]
        public void CodeDescriptionMatch_ValidCodeWithDescriptionWithRemaininText_OptionalFalse_ReturnsSuccess()
        {
            var modelStructure = SetUpModelStructure();
            var element = modelStructure.Elements.FirstOrDefault(e => e.CodeName == "Line Size");
            var codeDescription = element?.Codes.FirstOrDefault(c => c.Code == "020");

            var matchResult = codeDescription?.Match("02023");

            Assert.IsNotNull(matchResult);
            Assert.IsTrue(matchResult.Success());
            Assert.AreEqual("23", matchResult.RemainingText());
        }

        [TestMethod]
        public void CodeDescriptionMatch_ValidCodeWithDescriptionWithRemaininText_OptionalFalse_ReturnsFailure()
        {
            var modelStructure = SetUpModelStructure();
            var element = modelStructure.Elements.FirstOrDefault(e => e.CodeName == "Line Size");
            var codeDescription = element?.Codes.FirstOrDefault(c => c.Code == "020");

            var matchResult = codeDescription?.Match("H02023");

            Assert.IsNotNull(matchResult);
            Assert.IsFalse(matchResult.Success());
            Assert.AreEqual("H02023", matchResult.RemainingText());
        }

        [TestMethod]
        public void CodeDescriptionMatch_ValidCodeWithDescriptionWithRemaininTextNoOption_OptionalTrue_ReturnsSuccess()
        {
            var modelStructure = SetUpModelStructure();
            var element = modelStructure.Elements.FirstOrDefault(e => e.CodeName == "Flange Type and Material");
            var codeDescription = element?.Codes.FirstOrDefault(c => c.Code == "S");

            var matchResult = codeDescription?.Match("02023");

            Assert.IsNotNull(matchResult);
            Assert.IsFalse(matchResult.Success());
            Assert.AreEqual("02023", matchResult.RemainingText());
        }
    }
}