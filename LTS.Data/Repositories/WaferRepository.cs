using Microsoft.Data.Sqlite;
using LTS.Common.Interfaces;
using LTS.Common.Models;

namespace LTS.Data.Repositories
{
    public class WaferRepository :
        IWaferRepository
    {
        private readonly string connection =
            "Data Source=lts.db";

        // ADD

        public void Add(Wafer wafer)
        {
            using var conn =
                new SqliteConnection(connection);

            conn.Open();

            var cmd = conn.CreateCommand();

            cmd.CommandText =
            @"
            INSERT INTO Wafers
            (
                WaferSerialNo,
                SupplierId,
                LotId,
                WaferStatus,
                CreatedOn
            )

            VALUES
            (
                $serial,
                $supplier,
                $lot,
                $status,
                $created
            )
            ";

            cmd.Parameters.AddWithValue(
                "$serial",
                wafer.WaferSerialNo);

            cmd.Parameters.AddWithValue(
                "$supplier",
                wafer.SupplierId);

            cmd.Parameters.AddWithValue(
                "$lot",
                wafer.LotId.HasValue
                    ? wafer.LotId.Value
                    : DBNull.Value);

            cmd.Parameters.AddWithValue(
                "$status",
                wafer.WaferStatus);

            cmd.Parameters.AddWithValue(
                "$created",
                wafer.CreatedOn);

            cmd.ExecuteNonQuery();
        }

        // GET ALL

        public List<Wafer> GetAll()
        {
            var wafers =
                new List<Wafer>();

            using var conn =
                new SqliteConnection(connection);

            conn.Open();

            var cmd = conn.CreateCommand();

            cmd.CommandText =
            @"
            SELECT
                w.WaferId,
                w.WaferSerialNo,
                w.SupplierId,
                w.LotId,
                w.WaferStatus,
                w.CreatedOn,

                s.SupplierName

            FROM Wafers w

            INNER JOIN Suppliers s
            ON w.SupplierId = s.SupplierId
            ";

            using var reader =
                cmd.ExecuteReader();

            while (reader.Read())
            {
                wafers.Add(new Wafer
                {
                    WaferId =
                        reader.GetInt32(0),

                    WaferSerialNo =
                        reader.GetString(1),

                    SupplierId =
                        reader.GetInt32(2),

                    LotId =
                        reader.IsDBNull(3)
                        ? null
                        : reader.GetInt32(3),

                    WaferStatus =
                        reader.GetString(4),

                    CreatedOn =
                        reader.GetString(5),

                    SupplierName =
                        reader.GetString(6)
                });
            }

            return wafers;
        }
        // CHECK DUPLICATE SERIAL

        public bool ExistsBySerial(
            string serialNo)
        {
            using var conn =
                new SqliteConnection(connection);

            conn.Open();

            var cmd =
                conn.CreateCommand();

            cmd.CommandText =
            @"
    SELECT COUNT(*)

    FROM Wafers

    WHERE WaferSerialNo = $serial
    ";

            cmd.Parameters.AddWithValue(
                "$serial",
                serialNo);

            long count =
                (long)cmd.ExecuteScalar();

            return count > 0;
        }

        // DELETE

        public void Delete(int waferId)
        {
            using var conn =
                new SqliteConnection(connection);

            conn.Open();

            var cmd = conn.CreateCommand();

            cmd.CommandText =
            @"
            DELETE FROM Wafers
            WHERE WaferId = $id
            ";

            cmd.Parameters.AddWithValue(
                "$id",
                waferId);

            cmd.ExecuteNonQuery();
        }

        // ASSIGN LOT — sets LotId + status Allocated

        public void AssignLot(
            int waferId,
            int lotId)
        {
            using var conn =
                new SqliteConnection(connection);

            conn.Open();

            var cmd = conn.CreateCommand();

            cmd.CommandText =
            @"
            UPDATE Wafers

            SET
                LotId      = $lotId,
                WaferStatus = 'Allocated'

            WHERE WaferId = $waferId
            ";

            cmd.Parameters.AddWithValue(
                "$lotId",
                lotId);

            cmd.Parameters.AddWithValue(
                "$waferId",
                waferId);

            cmd.ExecuteNonQuery();
        }

        // UPDATE STATUS — only touches WaferStatus column

        public void UpdateStatus(
            int waferId,
            string status)
        {
            using var conn =
                new SqliteConnection(connection);

            conn.Open();

            var cmd = conn.CreateCommand();

            cmd.CommandText =
            @"
            UPDATE Wafers

            SET WaferStatus = $status

            WHERE WaferId = $waferId
            ";

            cmd.Parameters.AddWithValue(
                "$status",
                status);

            cmd.Parameters.AddWithValue(
                "$waferId",
                waferId);

            cmd.ExecuteNonQuery();
        }

        // UNASSIGN LOT — clears LotId, resets to Unallocated

        public void UnassignLot(int waferId)
        {
            using var conn =
                new SqliteConnection(connection);

            conn.Open();

            var cmd = conn.CreateCommand();

            cmd.CommandText =
            @"
            UPDATE Wafers

            SET
                LotId       = NULL,
                WaferStatus = 'Unallocated'

            WHERE WaferId = $waferId
            ";

            cmd.Parameters.AddWithValue(
                "$waferId",
                waferId);

            cmd.ExecuteNonQuery();
        }
    }
}
