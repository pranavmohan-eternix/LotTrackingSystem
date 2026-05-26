using Microsoft.Data.Sqlite;
using LTS.Common.Interfaces;
using LTS.Common.Models;

namespace LTS.Data.Repositories
{
    public class ProcessLocationRepository :
        IProcessLocationRepository
    {
        private readonly string connection =
            "Data Source=lts.db";

        // GET ALL

        public List<ProcessLocation> GetAll()
        {
            var locations =
                new List<ProcessLocation>();

            using var conn =
                new SqliteConnection(connection);

            conn.Open();

            var cmd = conn.CreateCommand();

            cmd.CommandText =
                "SELECT * FROM ProcessLocations ORDER BY SequenceNo";

            using var reader =
                cmd.ExecuteReader();

            while (reader.Read())
            {
                locations.Add(new ProcessLocation
                {
                    ProcessLocationId =
                        reader.GetInt32(0),

                    StationName =
                        reader.GetString(1),

                    SequenceNo =
                        reader.GetInt32(2),

                    Status =
                        reader.GetString(3),

                    CurrentLot =
                        reader.GetString(4),

                    WaferCount =
                        reader.GetInt32(5)
                });
            }

            return locations;
        }

        // OCCUPY STATION

        public void OccupyStation(
            int stationNumber,
            string lotCode,
            int waferCount)
        {
            using var conn =
                new SqliteConnection(connection);

            conn.Open();

            var cmd = conn.CreateCommand();

            cmd.CommandText =
            @"
            UPDATE ProcessLocations

            SET
                Status = 'Occupied',
                CurrentLot = $lot,
                WaferCount = $count

            WHERE SequenceNo = $station
            ";

            cmd.Parameters.AddWithValue(
                "$lot",
                lotCode);

            cmd.Parameters.AddWithValue(
                "$count",
                waferCount);

            cmd.Parameters.AddWithValue(
                "$station",
                stationNumber);

            cmd.ExecuteNonQuery();
        }

        // RELEASE STATION

        public void ReleaseStation(
            int stationNumber)
        {
            using var conn =
                new SqliteConnection(connection);

            conn.Open();

            var cmd = conn.CreateCommand();

            cmd.CommandText =
            @"
            UPDATE ProcessLocations

            SET
                Status = 'Available',
                CurrentLot = '',
                WaferCount = 0

            WHERE SequenceNo = $station
            ";

            cmd.Parameters.AddWithValue(
                "$station",
                stationNumber);

            cmd.ExecuteNonQuery();
        }

        // CHECK AVAILABILITY

        public bool IsStationAvailable(
            int stationNumber)
        {
            using var conn =
                new SqliteConnection(connection);

            conn.Open();

            var cmd = conn.CreateCommand();

            cmd.CommandText =
            @"
            SELECT Status

            FROM ProcessLocations

            WHERE SequenceNo = $station
            ";

            cmd.Parameters.AddWithValue(
                "$station",
                stationNumber);

            var status =
                cmd.ExecuteScalar()?.ToString();

            return status == "Available";
        }

        // GET BY SEQUENCE

        public ProcessLocation? GetBySequence(
            int sequenceNo)
        {
            using var conn =
                new SqliteConnection(connection);

            conn.Open();

            var cmd = conn.CreateCommand();

            cmd.CommandText =
            @"
            SELECT *
            FROM ProcessLocations
            WHERE SequenceNo = $sequence
            ";

            cmd.Parameters.AddWithValue(
                "$sequence",
                sequenceNo);

            using var reader =
                cmd.ExecuteReader();

            if (reader.Read())
            {
                return new ProcessLocation
                {
                    ProcessLocationId =
                        reader.GetInt32(0),

                    StationName =
                        reader.GetString(1),

                    SequenceNo =
                        reader.GetInt32(2),

                    Status =
                        reader.GetString(3),

                    CurrentLot =
                        reader.GetString(4),

                    WaferCount =
                        reader.GetInt32(5)
                };
            }

            return null;
        }
    }
}