using LTS.Common.Interfaces;
using LTS.Common.Models;

namespace LTS.Application.Services
{
    public class ProcessLocationService
    {
        private readonly IProcessLocationRepository _repo;

        public ProcessLocationService(
            IProcessLocationRepository repo)
        {
            _repo = repo;
        }

        public List<ProcessLocation> GetLocations()
        {
            return _repo.GetAll();
        }
    }
}