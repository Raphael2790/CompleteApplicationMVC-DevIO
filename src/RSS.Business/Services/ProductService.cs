using RSS.Business.Interfaces;
using RSS.Business.Models;
using RSS.Business.Models.Validations;
using System;
using System.Threading.Tasks;

namespace RSS.Business.Services
{
    public class ProductService : BaseService, IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository,
                                INotifiable notifiable) : base(notifiable)
        {
            _productRepository = productRepository;
        }

        public async Task AddProduct(Product product)
        {
            if (!ExecuteValidation(new ProductValidation(), product)) return;

            await _productRepository.Add(product);
        }

        public async Task RemoveProduct(Guid id)
        {
            await _productRepository.Remove(id);
        }

        public async Task UpdateProduct(Product product)
        {
            if (!ExecuteValidation(new ProductValidation(), product)) return;

            await _productRepository.Update(product);
        }

        public void Dispose()
        {
            _productRepository?.Dispose();
        }
    }
}
