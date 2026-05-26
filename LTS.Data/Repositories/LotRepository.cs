using Microsoft.Data.Sqlite;
using LTS.Common.Interfaces;
using LTS.Common.Models;

namespace LTS.Data.Repositories
{
    public class LotRepository :
        ILotRepository
    {
        private readonly string connection =
            "Data Source=lts.db";

        // ADD

        public int Add(Lot lot)
        {
            using var conn =
                new SqliteConnection(connection);

            conn.Open();

            var cmd = conn.CreateCommand();

            cmd.CommandText =
            @"
            INSERT INTO Lots
            (
                LotCode,
                CarrierId,
                WaferCount,
                CurrentStation,
                Status,
                RouteStations
            )

            VALUES
            (
                $code,
                $carrier,
                $wafer,
                $station,
                $status,
                $RouteStations
            )
            ";

            cmd.Parameters.AddWithValue(
                "$code",
                lot.LotCode);

            cmd.Parameters.AddWithValue(
                "$carrier",
                lot.CarrierId);

            cmd.Parameters.AddWithValue(
                "$wafer",
                lot.WaferCount);

            cmd.Parameters.AddWithValue(
                "$station",
                lot.CurrentStation);

            cmd.Parameters.AddWithValue(
                "$status",
                lot.Status);
            cmd.Parameters.AddWithValue(
                "$RouteStations",
                lot.RouteStations ?? "");

            // INSERT

            cmd.ExecuteNonQuery();

            // GET CREATED LOT ID

            cmd.CommandText =
                "SELECT last_insert_rowid();";

            long lotId =
                (long)cmd.ExecuteScalar();

            return (int)lotId;
        }

        // GET ALL

        public List<Lot> GetAll()
        {
            var lots =
                new List<Lot>();

            using var conn =
                new SqliteConnection(connection);

            conn.Open();

            var cmd = conn.CreateCommand();

            cmd.CommandText =
            @"
            SELECT
                l.LotId,
                l.LotCode,
                l.CarrierId,
                l.WaferCount,
                l.CurrentStation,
                l.Status,
                l.RouteStations,

                c.CarrierCode

            FROM Lots l

            INNER JOIN Carriers c
            ON l.CarrierId = c.CarrierId
            ";

            using var reader =
                cmd.ExecuteReader();

            while (reader.Read())
            {
                lots.Add(new Lot
                {
                    LotId =
                        reader.GetInt32(0),

                    LotCode =
                        reader.GetString(1),

                    CarrierId =
                        reader.GetInt32(2),

                    WaferCount =
                        reader.GetInt32(3),

                    CurrentStation =
                        reader.GetInt32(4),

                    Status =
                        reader.GetString(5),
                   
                    RouteStations =
                         reader.GetString(6),


                    CarrierCode =
                        reader.GetString(7),
                    
                });
            }

            return lots;
        }

        // START LOT

        public void StartLot(int lotId)
        {
            using var conn =
                new SqliteConnection(connection);

            conn.Open();

            var cmd = conn.CreateCommand();

            cmd.CommandText =
            @"
            UPDATE Lots

            SET
                CurrentStation = 1,
                Status = 'InProgress'

            WHERE LotId = $lotId
            ";

            cmd.Parameters.AddWithValue(
                "$lotId",
                lotId);

            cmd.ExecuteNonQuery();
        }

        // MOVE NEXT

        public void MoveNext(
            int lotId,
            int nextStation,
            string status)
        {
            using var conn =
                new SqliteConnection(connection);

            conn.Open();

            var cmd = conn.CreateCommand();

            cmd.CommandText =
            @"
            UPDATE Lots

            SET
                CurrentStation = $station,
                Status = $status

            WHERE LotId = $lotId
            ";

            cmd.Parameters.AddWithValue(
                "$station",
                nextStation);

            cmd.Parameters.AddWithValue(
                "$status",
                status);

            cmd.Parameters.AddWithValue(
                "$lotId",
                lotId);

            cmd.ExecuteNonQuery();
        }

        public void Delete(int lotId)
        {
            using var conn =
                new SqliteConnection(connection);

            conn.Open();

            var cmd =
                conn.CreateCommand();

            cmd.CommandText =
            @"
            DELETE FROM Lots
            WHERE LotId = @LotId
            ";

            cmd.Parameters.AddWithValue(
                "@LotId",
                lotId);

            cmd.ExecuteNonQuery();
        }
        public void UpdateStatus(
                    int lotId,
                    string status)
        {
            using var conn =
                new SqliteConnection(connection);

            conn.Open();

            var cmd =
                conn.CreateCommand();

            cmd.CommandText =
            @"
            UPDATE Lots
            SET Status = @status
            WHERE LotId = @lotId
            ";

            cmd.Parameters.AddWithValue(
                "@status",
                status);

            cmd.Parameters.AddWithValue(
                "@lotId",
                lotId);

            cmd.ExecuteNonQuery();
        }
        public void UpdateRouteStations(
                int lotId,
                string routeStations)
        {
            using var conn =
                new SqliteConnection(connection);

            conn.Open();

            var cmd =
                conn.CreateCommand();

            cmd.CommandText =
            @"
            UPDATE Lots

            SET RouteStations = @route

            WHERE LotId = @lotId
            ";

            cmd.Parameters.AddWithValue(
                "@route",
                routeStations);

            cmd.Parameters.AddWithValue(
                "@lotId",
                lotId);

            cmd.ExecuteNonQuery();
        }

    }
}