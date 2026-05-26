using Microsoft.Data.Sqlite;
using LTS.Common.Interfaces;
using LTS.Common.Models;

namespace LTS.Data.Repositories
{
    public class LotHistoryRepository :
        ILotHistoryRepository
    {
        private readonly string connection =
            "Data Source=lts.db";

        // ADD HISTORY

        public void Add(
            LotHistory history)
        {
            using var conn =
                new SqliteConnection(connection);

            conn.Open();

            var cmd = conn.CreateCommand();

            cmd.CommandText =
            @"
            INSERT INTO LotHistory
            (
                LotId,
                LotCode,
                Action,
                FromStation,
                ToStation,
                Status,
                Timestamp
            )

            VALUES
            (
                $lotId,
                $lotCode,
                $action,
                $from,
                $to,
                $status,
                $time
            )
            ";

            cmd.Parameters.AddWithValue(
                "$lotId",
                history.LotId);

            cmd.Parameters.AddWithValue(
                "$lotCode",
                history.LotCode);

            cmd.Parameters.AddWithValue(
                "$action",
                history.Action);

            cmd.Parameters.AddWithValue(
                "$from",
                history.FromStation);

            cmd.Parameters.AddWithValue(
                "$to",
                history.ToStation);

            cmd.Parameters.AddWithValue(
                "$status",
                history.Status);

            cmd.Parameters.AddWithValue(
                "$time",
                history.Timestamp);

            cmd.ExecuteNonQuery();
        }

        // GET ALL

        public List<LotHistory> GetAll()
        {
            var historyList =
                new List<LotHistory>();

            using var conn =
                new SqliteConnection(connection);

            conn.Open();

            var cmd = conn.CreateCommand();

            cmd.CommandText =
            @"
            SELECT
                HistoryId,
                LotId,
                LotCode,
                Action,
                FromStation,
                ToStation,
                Status,
                Timestamp

            FROM LotHistory

            ORDER BY HistoryId DESC
            ";

            using var reader =
                cmd.ExecuteReader();

            while (reader.Read())
            {
                historyList.Add(
                    new LotHistory
                    {
                        HistoryId =
                            reader.GetInt32(0),

                        LotId =
                            reader.GetInt32(1),

                        LotCode =
                            reader.GetString(2),

                        Action =
                            reader.GetString(3),

                        FromStation =
                            reader.GetInt32(4),

                        ToStation =
                            reader.GetInt32(5),

                        Status =
                            reader.GetString(6),

                        Timestamp =
                            reader.GetString(7)
                    });
            }

            return historyList;
        }

        // GET BY LOT

        public List<LotHistory> GetByLot(
            int lotId)
        {
            var historyList =
                new List<LotHistory>();

            using var conn =
                new SqliteConnection(connection);

            conn.Open();

            var cmd = conn.CreateCommand();

            cmd.CommandText =
            @"
            SELECT
                HistoryId,
                LotId,
                LotCode,
                Action,
                FromStation,
                ToStation,
                Status,
                Timestamp

            FROM LotHistory

            WHERE LotId = $lotId

            ORDER BY HistoryId DESC
            ";

            cmd.Parameters.AddWithValue(
                "$lotId",
                lotId);

            using var reader =
                cmd.ExecuteReader();

            while (reader.Read())
            {
                historyList.Add(
                    new LotHistory
                    {
                        HistoryId =
                            reader.GetInt32(0),

                        LotId =
                            reader.GetInt32(1),

                        LotCode =
                            reader.GetString(2),

                        Action =
                            reader.GetString(3),

                        FromStation =
                            reader.GetInt32(4),

                        ToStation =
                            reader.GetInt32(5),

                        Status =
                            reader.GetString(6),

                        Timestamp =
                            reader.GetString(7)
                    });
            }

            return historyList;
        }
    }
}