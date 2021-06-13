using RSS.Business.Models;
using System.Threading.Tasks;

namespace RSS.Business.Interfaces
{
    public interface ISupplierService
    {
        Task AddSupplier(Supplier supplier);
        Task UpdateSupplier(Supplier supplier);
        Task RemoveSupplier(Supplier supplier);
        Task UpdateSupplierAddress(Address address);
    }
}
