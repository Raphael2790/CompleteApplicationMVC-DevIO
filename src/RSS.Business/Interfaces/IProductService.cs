using RSS.Business.Models;
using System;
using System.Threading.Tasks;

namespace RSS.Business.Interfaces
{
    public interface IProductService
    {
        Task AddProduct(Product product);
        Task UpdateProdutc(Product product);
        Task RemoveProduct(Guid id);
    }
}
