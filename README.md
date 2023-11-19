```
# QuickShipParser

## Overview
QuickShipParser is an Azure Function-based project designed to validate product model strings based on specific business rules. In this case, it validates models that are available for QuickShip.

## Features
- **Azure Blob Storage Integration**: Uses Azure Blob Storage for storing and managing business rules stored in JSON format.
- **HTTP Triggered Azure Function**: On-demand validation via HTTP requests for easy integration.
- **Scalability and Flexibility**: Leverages Azure Functions for scalability and efficiency.
- **Excel to JSON Conversion**: Converts user-friendly, template-based Excel tables to the data structure required by the algorithm. This allows system administrators to update business rules without interacting with the codebase, and without requiring coding knowledge.

## Prerequisites
Before you begin, ensure you have met the following requirements:
- An Azure account with an active subscription. [Create one for free](https://azure.microsoft.com/en-us/free/).
- Azure Blob Storage account.
- [.NET Core 3.1 SDK](https://dotnet.microsoft.com/download) or later.
- [Azure Functions Core Tools version 3.x](https://docs.microsoft.com/en-us/azure/azure-functions/functions-run-local).

## Setup and Installation
1. **Clone the Repository**:
   ```sh
   git clone https://github.com/your-username/QuickShipParser.git
   cd QuickShipParser
   ```

2. **Configure Azure Storage**:
   - Set up a Blob container in your Azure Storage account.
   - Obtain your storage account connection string and store it as an Azure Function configuration setting named 'quickship-az-fn-config-token'.

3. **Local Application Settings**:
   - Rename the `local.settings.json.example` file to `local.settings.json`.
   - Update the `AzureWebJobsStorage` with your Azure Blob Storage connection string.

4. **Deploy to Azure**:
   - Deploy the function app to Azure using [Visual Studio](https://docs.microsoft.com/en-us/azure/azure-functions/functions-develop-vs), [VS Code](https://docs.microsoft.com/en-us/azure/azure-functions/functions-develop-vs-code), or the [Azure CLI](https://docs.microsoft.com/en-us/azure/azure-functions/functions-create-first-azure-function-azure-cli).

## Usage
After deployment, the Azure Function can be triggered via HTTP requests. The function expects a query parameter `model`.

Example HTTP request (Live right now, go test it!):
```
POST [/api/QuickShipParse?model=8705TSA010SPHW0Q9Q4Q66PD](https://quickshipparser20231118174958.azurewebsites.net/api/QuickShipParse?model=8705459)
Content-Type: application/json

{
  "model": "model-name"
}
```

## Contributing
Contributions to the QuickShipParser project are welcome. Please adhere to the following guidelines:
- Fork the repository and create a new branch for your feature or fix.
- Write clear and descriptive commit messages.
- Ensure code style and quality compliance.
- Create a pull request with a detailed description of changes.

## License
[MIT License](LICENSE) - See the LICENSE file for details.

## Contact
For any questions or comments, please contact [Your Name] at [Your Email].

---

Note: Don't forget to replace placeholders like `[Your Name]`, `[Your Email]`, `your-username`, and any specific instructions or URLs with your actual project information and links.
```
