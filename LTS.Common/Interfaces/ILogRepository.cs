using LTS.Common.Models;

namespace LTS.Common.Interfaces
{
    public interface ILogRepository
    {
        void Add(LogMessage log);

        List<LogMessage> GetAll();
    }
}