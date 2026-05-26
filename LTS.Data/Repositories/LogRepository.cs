using Microsoft.Data.Sqlite;
using LTS.Common.Interfaces;
using LTS.Common.Models;

namespace LTS.Data.Repositories
{
    public class LogRepository :
        ILogRepository
    {
        private readonly string connection =
            "Data Source=lts.db";

        // ADD LOG

        public void Add(LogMessage log)
        {
            using var conn =
                new SqliteConnection(connection);

            conn.Open();

            var cmd = conn.CreateCommand();

            cmd.CommandText =
            @"
            INSERT INTO ApplicationLogs
            (
                Level,
                Message,
                Timestamp
            )

            VALUES
            (
                $level,
                $message,
                $time
            )
            ";

            cmd.Parameters.AddWithValue(
                "$level",
                log.Level);

            cmd.Parameters.AddWithValue(
                "$message",
                log.Message);

            cmd.Parameters.AddWithValue(
                "$time",
                log.Timestamp);

            cmd.ExecuteNonQuery();
        }



        // GET ALL LOGS

        public List<LogMessage> GetAll()
        {
            var logs =
                new List<LogMessage>();

            using var conn =
                new SqliteConnection(connection);

            conn.Open();

            var cmd = conn.CreateCommand();

            cmd.CommandText =
            @"
            SELECT *
            FROM ApplicationLogs
            ORDER BY LogId DESC
            ";

            using var reader =
                cmd.ExecuteReader();

            while (reader.Read())
            {
                logs.Add(new LogMessage
                {
                    LogId =
                        reader.GetInt32(0),

                    Level =
                        reader.GetString(1),

                    Message =
                        reader.GetString(2),

                    Timestamp =
                        reader.GetString(3)
                });
            }

            return logs;
        }
    }
}