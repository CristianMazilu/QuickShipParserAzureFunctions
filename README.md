# QuickShipParser

## Overview
QuickShipParser is an Azure Function-based project designed to validate product model strings based on specific business rules. In this case, it validates models that are available for QuickShip.

## Features
- **Azure Blob Storage Integration**: Uses Azure Blob Storage for storing and managing business rules stored in JSON format.
- **HTTP Triggered Azure Function**: On-demand validation via HTTP requests for easy integration.
- **Scalability and Flexibility**: Leverages Azure Functions for scalability and efficiency.
- **Excel to JSON Conversion** (coming soon!): Converts user-friendly, template-based Excel tables to the data structure required by the algorithm. This allows system administrators to update business rules without interacting with the codebase, and without requiring coding knowledge.

## Prerequisites
Before you begin, ensure you have met the following requirements:
- An Azure account with an active subscription. [Create one for free](https://azure.microsoft.com/en-us/free/).
- Azure Blob Storage account.
- [.NET Core 3.1 SDK](https://dotnet.microsoft.com/download) or later.
- [Azure Functions Core Tools version 3.x](https://docs.microsoft.com/en-us/azure/azure-functions/functions-run-local).

## Usage
After deployment, the Azure Function can be triggered via HTTP requests. The function expects a query parameter `model`.

Example HTTP request:
```
POST https://[your-app-name].azurewebsites.net/api/QuickShipParse?model=[your-model-string])
Content-Type: application/json
```
Example HTTP response (valid model string):
```
Response Status: 200 OK
Response Body:
{
  "ModelString": "[your-model-here]",
  "QuickShipValid": true,
  "QuickShipInvalidPart": "",
  "ExceptionMessage": "None"
}
```
Example HTTP response (invalid model string):
```
Response Status: 200 OK
Response Body:
{
  "ModelString": "[your-model-here]",
  "QuickShipValid": false,
  "QuickShipInvalidPart": "[invalid-part-of-model-string]",
  "ExceptionMessage": "None"
}
```

## Implementation
The codebase uses two interfaces (```IPattern``` and ```IMatch```) that work together to provide customizable query-based logic for the business rules, while consuming the business-logic JSON file (see next chapter).
These interfaces allow for objects to nest and create increasingly complex model string patterns, such as below:
```
[...]
public class Element : IPattern
{
    public string CodeName { get; set; }
    public bool Optional { get; set; }
    public List<CodeDescription> Codes { get; set; }

    public IMatch Match(string text)
    {
        if (Optional)
        {
            var pattern = new Many(new Choice(Codes.ToArray()));
            return pattern.Match(text);
        }
        else
        {
            var pattern = new Choice(Codes.ToArray());
            return pattern.Match(text);
        }
    }
}
[...]
```
Right now, the project is accessible live at: [QuickShipParserAPI](https://quickshipparser20231118174958.azurewebsites.net/api/QuickShipParse?model=8705TSA010SPHW0Q4Q9Q66PD). Play around with some model strings: 8705TSA010SPHW0Q9Q4Q66PD (valid), 8705TSA010SPHW0 (valid), 8705STA010SH (invalid).

## Working principle
At the heart of this project lies the business logic JSON file. Let's take Emerson's Rosemount 8705 Flanged Magnetic Flow Meter for example:
```
{
  "modelName": "Mag meters",
  "baseModel": "8705",
  "elements": [
    {
      "codeName": "Base",
      "optional": false,
      "codes": [
        {
          "code": "8705",
          "description": "Magnetic Flowmeter Sensor - Flanged"
        }
      ]
    },
[...]
    {
      "codeName": "Line Size",
      "optional": false,
      "codes": [
        {
          "code": "010",
          "description": "1 inch (25 mm)"
        },
        {
          "code": "015",
          "description": "1.5 inch (40 mm)"
        }
      ]
    },
[...]
    {
     "codeName": "Quality Certifications",
     "optional": true,
     "codes": [
       {
         "code": "Q4",
         "description": "Calibration Certificate per ISO 10474 3.1B/ EN 10204 3.1"
       },
       {
         "code": "Q8",
         "description": "Material Traceability per ISO 10474 3.1B/ EN 10204 3.1"
       },
       {
         "code": "Q9",
         "description": "Material Traceability Electrode Only per ISO 10474 3.1B / EN 10204 3.1"
       },
       {
         "code": "Q66",
         "description": "Welding Procedure Qualification Record Documentation (PQR)"
       },
     ]
   },
  ]
}
```
This (partial) JSON file allows validation of models such as ```8705TSA010SPHW0``` or ```8705TSA010SPHW0Q4Q9Q66PD```, while invalidating any incorrect configurations, or model configurations that are not available for QuickShip.

## Setup and Installation
1. **Clone the Repository**:
   ```sh
   git clone https://github.com/CristianMazilu/QuickShipParser.git
   cd QuickShipParser
   ```

2. **Configure Azure Storage**:
   - Set up a Blob container in your Azure Storage account.
   - Obtain your storage account connection string and store it as an Azure Function configuration setting named ```'quickship-az-fn-config-token'```.

3. **Local Application Settings**:
   - Rename the `local.settings.json.example` file to `local.settings.json`.
   - Update the `AzureWebJobsStorage` with your Azure Blob Storage connection string.

4. **Deploy to Azure**:
   - Deploy the function app to Azure using [Visual Studio](https://docs.microsoft.com/en-us/azure/azure-functions/functions-develop-vs), [VS Code](https://docs.microsoft.com/en-us/azure/azure-functions/functions-develop-vs-code), or the [Azure CLI](https://docs.microsoft.com/en-us/azure/azure-functions/functions-create-first-azure-function-azure-cli).

## Contact
For any questions or comments, please contact Cristian Mazilu at mazilu6@gmail.com.
