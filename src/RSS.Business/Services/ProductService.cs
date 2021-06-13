using RSS.Business.Interfaces;
using RSS.Business.Models;
using RSS.Business.Models.Validations;
using System;
using System.Threading.Tasks;

namespace RSS.Business.Services
{
    public class ProductService : BaseService, IProductService
    {
        public async Task AddProduct(Product product)
        {
            if (!ExecuteValidation(new ProductValidation(), product)) return;
        }

        public async Task RemoveProduct(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateProdutc(Product product)
        {
            if (!ExecuteValidation(new ProductValidation(), product)) return;
        }
    }
}
