using LTS.Common.Models;

namespace LTS.Common.Interfaces
{
    public interface ISupplierRepository
    {
        void Add(Supplier supplier);

        List<Supplier> GetAll();

        void Delete(int supplierId);
    }
}