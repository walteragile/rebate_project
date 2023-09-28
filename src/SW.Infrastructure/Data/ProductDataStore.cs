using SW.Core.Types;
using SW.Core.Entities;

namespace SW.Infrastructure.Data;

public class ProductDataStore
{
    public Product GetProduct(string productIdentifier)
    {
        // TODO: Access database to retrieve account (code removed for brevity)
        return new Product
        {
            Identifier = productIdentifier,
            Price = 10m,
            Uom = "uom",
            SupportedIncentives = SupportedIncentiveType.FixedRateRebate,
        };
    }
}
