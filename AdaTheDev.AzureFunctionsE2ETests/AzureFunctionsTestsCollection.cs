using Xunit;

namespace AdaTheDev.AzureFunctionsE2ETests
{
    [CollectionDefinition(nameof(AzureFunctionsTestsCollection))]
    public class AzureFunctionsTestsCollection : ICollectionFixture<AzureFunctionsFixture> { }
}
