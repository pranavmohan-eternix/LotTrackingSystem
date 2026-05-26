using Microsoft.Data.Sqlite;
using LTS.Common.Interfaces;
using LTS.Common.Models;

namespace LTS.Data.Repositories
{
    public class CarrierRepository :
        ICarrierRepository
    {
        private readonly string connection =
            "Data Source=lts.db";

        // ADD

        public void Add(Carrier carrier)
        {
            using var conn =
                new SqliteConnection(connection);

            conn.Open();

            var cmd = conn.CreateCommand();

            cmd.CommandText =
            @"
            INSERT INTO Carriers
            (
                CarrierCode,
                Status,
                Capacity,
                CurrentLocationId,
                CreatedDate
            )

            VALUES
            (
                $code,
                $status,
                $capacity,
                $location,
                $date
            )
            ";

            cmd.Parameters.AddWithValue(
                "$code",
                carrier.CarrierCode);

            cmd.Parameters.AddWithValue(
                "$status",
                carrier.Status);

            cmd.Parameters.AddWithValue(
                "$capacity",
                carrier.Capacity);

            cmd.Parameters.AddWithValue(
                "$location",
                carrier.CurrentLocationId.HasValue
                    ? carrier.CurrentLocationId.Value
                    : DBNull.Value);

            cmd.Parameters.AddWithValue(
                "$date",
                carrier.CreatedDate);

            cmd.ExecuteNonQuery();
        }

        // GET ALL

        public List<Carrier> GetAll()
        {
            var carriers =
                new List<Carrier>();

            using var conn =
                new SqliteConnection(connection);

            conn.Open();

            var cmd = conn.CreateCommand();

            cmd.CommandText =
                "SELECT * FROM Carriers";

            using var reader =
                cmd.ExecuteReader();

            while (reader.Read())
            {
                carriers.Add(new Carrier
                {
                    CarrierId =
                        reader.GetInt32(0),

                    CarrierCode =
                        reader.GetString(1),

                    Status =
                        reader.GetString(2),

                    Capacity =
                        reader.GetInt32(3),

                    CurrentLocationId =
                        reader.IsDBNull(4)
                        ? null
                        : reader.GetInt32(4),

                    CreatedDate =
                        reader.GetString(5)
                });
            }

            return carriers;
        }

        // OCCUPY CARRIER

        public void OccupyCarrier(
            int carrierId,
            int? stationNumber)
        {
            using var conn =
                new SqliteConnection(connection);

            conn.Open();

            var cmd = conn.CreateCommand();

            cmd.CommandText =
            @"
            UPDATE Carriers

            SET
                Status = 'Occupied',
                CurrentLocationId = $station

            WHERE CarrierId = $id
            ";

            cmd.Parameters.AddWithValue(
                "$station",
                stationNumber.HasValue
                    ? stationNumber.Value
                    : DBNull.Value);

            cmd.Parameters.AddWithValue(
                "$id",
                carrierId);

            cmd.ExecuteNonQuery();
        }

        // RELEASE CARRIER

        public void ReleaseCarrier(
            int carrierId)
        {
            using var conn =
                new SqliteConnection(connection);

            conn.Open();

            var cmd = conn.CreateCommand();

            cmd.CommandText =
            @"
            UPDATE Carriers

            SET
                Status = 'Available',
                CurrentLocationId = NULL

            WHERE CarrierId = $id
            ";

            cmd.Parameters.AddWithValue(
                "$id",
                carrierId);

            cmd.ExecuteNonQuery();
        }

        // DELETE

        public void Delete(int carrierId)
        {
            using var conn =
                new SqliteConnection(connection);

            conn.Open();

            var cmd = conn.CreateCommand();

            cmd.CommandText =
            @"
            DELETE FROM Carriers
            WHERE CarrierId = $id
            ";

            cmd.Parameters.AddWithValue(
                "$id",
                carrierId);

            cmd.ExecuteNonQuery();
        }
    }
}