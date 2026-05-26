using LTS.Common.Interfaces;
using LTS.Common.Models;

namespace LTS.Application.Services
{
    public class CarrierService
    {
        private readonly ICarrierRepository _repo;

        public CarrierService(
            ICarrierRepository repo)
        {
            _repo = repo;
        }

        public void AddCarrier(Carrier carrier)
        {
            if (string.IsNullOrWhiteSpace(
                carrier.CarrierCode))
            {
                throw new Exception(
                    "Carrier Code is required");
            }

            if (carrier.Capacity <= 0)
            {
                throw new Exception(
                    "Capacity must be greater than 0");
            }

            carrier.Status = "Available";

            carrier.CreatedDate =
                DateTime.Now.ToString("dd-MM-yyyy");

            _repo.Add(carrier);
        }

        public List<Carrier> GetCarriers()
        {
            return _repo.GetAll();
        }

        public void DeleteCarrier(int carrierId)
        {
            _repo.Delete(carrierId);
        }
    }
}