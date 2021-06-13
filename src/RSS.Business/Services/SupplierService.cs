using RSS.Business.Interfaces;
using RSS.Business.Models;
using RSS.Business.Models.Validations;
using System.Threading.Tasks;

namespace RSS.Business.Services
{
    public class SupplierService : BaseService, ISupplierService
    {
        public async Task AddSupplier(Supplier supplier)
        {
            if (!ExecuteValidation(new SupplierValidation(), supplier)
                && !ExecuteValidation(new AddressValidation(), supplier.Adress)) return;
        }

        public async Task RemoveSupplier(Supplier supplier)
        {
           
        }

        public async Task UpdateSupplier(Supplier supplier)
        {
            if (!ExecuteValidation(new SupplierValidation(), supplier)) return;
        }

        public async Task UpdateSupplierAddress(Address address)
        {
            if (!ExecuteValidation(new AddressValidation(), address)) return;
        }
    }
}
