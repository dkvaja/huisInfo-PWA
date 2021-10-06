using Portal.JPDS.Domain.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Portal.JPDS.AppCore.ApiModels
{
    public class ProductServiceApiModel
    {
        public ProductServiceApiModel(ProductDienst entity)
        {
            Id = entity.Guid;
            Code = entity.ProductCode;
            Description = entity.Omschrijving;
            SubProductServiceList = entity.ProductDienstSub1s.OrderBy(x => x.Omschrijving).Select(x => new ProductServiceApiModel(x)).ToList();
        }

        public ProductServiceApiModel(ProductDienstSub1 entity)
        {
            Id = entity.Guid;
            Description = entity.Omschrijving;
            SubProductServiceList = entity.ProductDienstSub2s.OrderBy(x => x.Omschrijving).Select(x => new ProductServiceApiModel(x)).ToList();
        }

        public ProductServiceApiModel(ProductDienstSub2 entity)
        {
            Id = entity.Guid;
            Description = entity.Omschrijving;
        }

        public string Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public List<ProductServiceApiModel> SubProductServiceList { get; set; }
    }
}
