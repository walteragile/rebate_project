using SW.Core.Types;
using SW.Core.Entities;
using SW.Core.Contracts;

namespace SW.Infrastructure.Data;

public class ProductDataStore : IRepository<Product>
{
    public Product Get(string id)
    {
        // TODO: Access database to retrieve account (code removed for brevity)
        return new Product
        {
            Identifier = id,
            Price = 10m,
            Uom = "uom",
            SupportedIncentives = SupportedIncentiveType.FixedRateRebate | SupportedIncentiveType.AmountPerUom,
        };
    }

    public Product Save(Product product)
    {
        return product;
    }
}
