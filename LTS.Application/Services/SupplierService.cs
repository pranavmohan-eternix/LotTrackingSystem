using LTS.Common.Interfaces;
using LTS.Common.Models;
using System.Linq;
using System.Collections.Generic;

namespace LTS.Application.Services
{
    public class SupplierService
    {
        private readonly ISupplierRepository _repo;

        public SupplierService(ISupplierRepository repo)
        {
            _repo = repo;
        }

        public void AddSupplier(Supplier supplier)
        {
            if (string.IsNullOrWhiteSpace(supplier.SupplierName))
                throw new Exception("Supplier name required");

            if (string.IsNullOrWhiteSpace(supplier.ContactPerson))
                throw new Exception("Contact Person required");

            if (string.IsNullOrWhiteSpace(supplier.Email))
                throw new Exception("Email required");

            var suppliers =
                _repo.GetAll() ?? new List<Supplier>();

            bool exists =
                suppliers.Any(x =>
                    x.SupplierName.ToLower()
                    == supplier.SupplierName.ToLower());

            if (exists)
                throw new Exception("Supplier already exists");

            supplier.AddedDate =
                DateTime.Now.ToString("dd-MM-yyyy");

            _repo.Add(supplier);
        }

        public List<Supplier> GetSuppliers()
        {
            return _repo.GetAll();
        }

        public void DeleteSupplier(int supplierId)
        {
            _repo.Delete(supplierId);
        }
    }
}