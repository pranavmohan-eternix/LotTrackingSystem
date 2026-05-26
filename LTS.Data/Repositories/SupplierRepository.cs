using Microsoft.Data.Sqlite;
using LTS.Common.Interfaces;
using LTS.Common.Models;

namespace LTS.Data.Repositories
{
    public class SupplierRepository : ISupplierRepository
    {
        private readonly string connectionString =
            "Data Source=lts.db";

        public void Add(Supplier supplier)
        {
            using var connection =
                new SqliteConnection(connectionString);

            connection.Open();

            var cmd = connection.CreateCommand();

            cmd.CommandText =
            @"
            INSERT INTO Suppliers
            (SupplierName, ContactPerson, Email, AddedDate)

            VALUES
            ($name, $contact, $email, $date)
            ";

            cmd.Parameters.AddWithValue(
                "$name",
                supplier.SupplierName);

            cmd.Parameters.AddWithValue(
                "$contact",
                supplier.ContactPerson);

            cmd.Parameters.AddWithValue(
                "$email",
                supplier.Email);

            cmd.Parameters.AddWithValue(
                "$date",
                supplier.AddedDate);

            cmd.ExecuteNonQuery();
        }

        public List<Supplier> GetAll()
        {
            var suppliers = new List<Supplier>();

            using var connection =
                new SqliteConnection(connectionString);

            connection.Open();

            var cmd = connection.CreateCommand();

            cmd.CommandText =
                "SELECT * FROM Suppliers";

            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                suppliers.Add(new Supplier
                {
                    SupplierId = reader.GetInt32(0),
                    SupplierName = reader.GetString(1),
                    ContactPerson = reader.GetString(2),
                    Email = reader.GetString(3),
                    AddedDate = reader.GetString(4)
                });
            }

            return suppliers;
        }

        public void Delete(int supplierId)
        {
            using var connection =
                new SqliteConnection(connectionString);

            connection.Open();

            var cmd = connection.CreateCommand();

            cmd.CommandText =
            @"
            DELETE FROM Suppliers
            WHERE SupplierId = $id
            ";

            cmd.Parameters.AddWithValue(
                "$id",
                supplierId);

            cmd.ExecuteNonQuery();
        }
    }
}