using LTS.Common.Interfaces;
using LTS.Common.Models;

namespace LTS.Application.Services
{
    public class LogService
    {
        private readonly ILogRepository _logRepository;

        public LogService(
            ILogRepository logRepository)
        {
            _logRepository =
                logRepository;
        }

        // INFO

        public void Info(string message)
        {
            AddLog(
                "INFO",
                message);
        }

        // WARNING

        public void Warn(string message)
        {
            AddLog(
                "WARN",
                message);
        }

        // ERROR

        public void Error(string message)
        {
            AddLog(
                "ERROR",
                message);
        }

        // PRIVATE

        private void AddLog(
            string level,
            string message)
        {
            _logRepository.Add(
                new LogMessage
                {
                    Level = level,

                    Message = message,

                    Timestamp =
                        DateTime.Now
                        .ToString(
                            "yyyy-MM-dd HH:mm:ss")
                });
        }

        // GET ALL

        public List<LogMessage> GetLogs()
        {
            return _logRepository.GetAll();
        }
    }
}