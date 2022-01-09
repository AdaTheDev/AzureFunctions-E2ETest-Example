This sample project provides an example of how you can create E2E Azure Function tests with xUnit, 
which can be run against a local build of the Azure Functions running via the Azure Functions Core Tools, whether that be on your local dev machine or
in a CI/CD pipeline (i.e. not having to deploy them to Azure first before running them).

A full write up can be found on my blog here: [E2E Testing Azure Functions](https://www.adathedev.co.uk/2022/01/e2e-testing-azure-functions.html)

### You will need:
* Azure Functions Core Tools installed
* Azure Storage Emulator/Azurite installed + running
* set the variables at the top of the AzureFunctionsFixture class in the AdaTheDev.AzureFunctionsE2ETests project, to the appropriate values for your local environment
