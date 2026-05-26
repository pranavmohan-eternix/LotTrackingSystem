using Microsoft.Data.Sqlite;

namespace LTS.Data.Database
{
    public static class DatabaseInitializer
    {
        private static string connection =
            "Data Source=lts.db";

        public static void Initialize()
        {
            using var conn =
                new SqliteConnection(connection);

            conn.Open();

            var cmd = conn.CreateCommand();

            cmd.CommandText =
            @"
            CREATE TABLE IF NOT EXISTS Users (
                UserId INTEGER PRIMARY KEY AUTOINCREMENT,
                Username TEXT NOT NULL UNIQUE,
                Password TEXT NOT NULL,
                Role TEXT,
                IsActive INTEGER
            );

            CREATE TABLE IF NOT EXISTS Suppliers (
                SupplierId INTEGER PRIMARY KEY AUTOINCREMENT,
                SupplierName TEXT NOT NULL,
                ContactPerson TEXT NOT NULL,
                Email TEXT NOT NULL,
                AddedDate TEXT
            );

            CREATE TABLE IF NOT EXISTS ProcessLocations (
                ProcessLocationId INTEGER PRIMARY KEY AUTOINCREMENT,
                StationName TEXT NOT NULL,
                SequenceNo INTEGER,
                Status TEXT,
                CurrentLot TEXT,
                WaferCount INTEGER
            );

            CREATE TABLE IF NOT EXISTS Carriers (
                CarrierId INTEGER PRIMARY KEY AUTOINCREMENT,
                CarrierCode TEXT NOT NULL UNIQUE,
                Status TEXT,
                Capacity INTEGER,
                CurrentLocationId INTEGER,
                CreatedDate TEXT,

                FOREIGN KEY(CurrentLocationId)
                REFERENCES ProcessLocations(ProcessLocationId)
            );

            CREATE TABLE IF NOT EXISTS Lots (
                LotId INTEGER PRIMARY KEY AUTOINCREMENT,

                LotCode TEXT NOT NULL UNIQUE,

                CarrierId INTEGER NOT NULL,

                WaferCount INTEGER,

                CurrentStation INTEGER DEFAULT 0,
                

                Status TEXT,
                RouteStations TEXT,

                FOREIGN KEY(CarrierId)
                REFERENCES Carriers(CarrierId)
            );

            CREATE TABLE IF NOT EXISTS Wafers (
                WaferId INTEGER PRIMARY KEY AUTOINCREMENT,

                WaferSerialNo TEXT NOT NULL UNIQUE,

                SupplierId INTEGER NOT NULL,

                LotId INTEGER NULL,

                WaferStatus TEXT,

                CreatedOn TEXT,

                FOREIGN KEY(SupplierId)
                REFERENCES Suppliers(SupplierId),

                FOREIGN KEY(LotId)
                REFERENCES Lots(LotId)
            );

            CREATE TABLE IF NOT EXISTS LotHistory (
                HistoryId INTEGER PRIMARY KEY AUTOINCREMENT,

                LotId INTEGER NOT NULL,

                LotCode TEXT NOT NULL,

                Action TEXT NOT NULL,

                FromStation INTEGER,

                ToStation INTEGER,

                Status TEXT NOT NULL,

                Timestamp TEXT NOT NULL
            );

            CREATE TABLE IF NOT EXISTS ApplicationLogs (

            LogId INTEGER PRIMARY KEY AUTOINCREMENT,

            Level TEXT NOT NULL,

            Message TEXT NOT NULL,

            Timestamp TEXT NOT NULL
            );
            ";

            cmd.ExecuteNonQuery();

            SeedProcessLocations(conn);
        }

        private static void SeedProcessLocations(
            SqliteConnection conn)
        {
            var checkCmd = conn.CreateCommand();

            checkCmd.CommandText =
                "SELECT COUNT(*) FROM ProcessLocations";

            long count =
                (long)checkCmd.ExecuteScalar();

            if (count > 0)
                return;

            for (int i = 1; i <= 10; i++)
            {
                var cmd = conn.CreateCommand();

                cmd.CommandText =
                @"
                INSERT INTO ProcessLocations
                (StationName, SequenceNo, Status, CurrentLot, WaferCount)

                VALUES
                ($name, $seq, $status, $lot, $wafer)
                ";

                cmd.Parameters.AddWithValue(
                    "$name",
                    $"S-{i:00}");

                cmd.Parameters.AddWithValue(
                    "$seq",
                    i);

                cmd.Parameters.AddWithValue(
                    "$status",
                    "Available");

                cmd.Parameters.AddWithValue(
                    "$lot",
                    "");

                cmd.Parameters.AddWithValue(
                    "$wafer",
                    0);

                cmd.ExecuteNonQuery();
            }
        }
    }
}