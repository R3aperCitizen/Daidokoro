using MySqlConnector;
using System.Data;
using System.Reflection;

namespace Daidokoro.Model
{
    public class DBService
    {
        public DBCredentials dbCredentials = new();
        private string connectionString = string.Empty;

        public bool TryConnection(DBCredentials dbc)
        {
            connectionString = "Server=" + dbc.Server 
                            + ";Database=" + dbc.Database 
                            + ";Uid=" + dbc.User 
                            + ";Pwd=" + dbc.Password;

            try
            {
                MySqlConnection connection = new(connectionString);
                connection.Open();

                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                    dbCredentials = dbc;
                    return true;
                }

                connectionString = string.Empty;
                connection.Close();
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
                return false;
            }
        }

        // Checks if a given DB table give at least 1 row in the query with the given condition
        public async Task<bool> ExistInTable(string query)
        {
            bool exist = false;
            using (MySqlConnection connection = new(connectionString))
            {
                await connection.OpenAsync();
                using (MySqlCommand command = new(query, connection))
                {
                    exist = Convert.ToInt32(await command.ExecuteScalarAsync()) > 0;
                    await connection.CloseAsync();
                }
            }

            return exist;
        }

        // Get all the data from a generic DB table
        public async Task<List<T>> GetData<T>(string query) where T : new()
        {
            List<T> results = [];

            using (MySqlConnection connection = new(connectionString))
            {
                await connection.OpenAsync();

                using (MySqlCommand command = new(query, connection))
                {
                    using (MySqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            // Create an instance of the type T
                            T item = new();

                            // Map each column value to the corresponding property of T
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                PropertyInfo property = item.GetType().GetProperty(reader.GetName(i))!;
                                property?.SetValue(item, reader.GetValue(i));
                            }

                            results.Add(item);
                        }
                    }
                    await connection.CloseAsync();
                }
            }

            return results;
        }

        // Insert a given element inside the db using the given query
        public async Task InsertElement(List<Tuple<string, object>> values, string query)
        {
            using (MySqlConnection connection = new(connectionString))
            {
                await connection.OpenAsync();

                using (MySqlCommand command = new(query, connection))
                {
                    foreach (Tuple<string, object> t in values)
                    {
                        command.Parameters.AddWithValue($"@{t.Item1}", t.Item2);
                    }

                    await command.ExecuteNonQueryAsync();
                }

                await connection.CloseAsync();
            }
        }

        // Remove an element from a db table using the given query
        public async Task RemoveOrUpdateElement(string query)
        {
            using (MySqlConnection connection = new(connectionString))
            {
                await connection.OpenAsync();

                using (MySqlCommand command = new(query, connection))
                {
                    await command.ExecuteNonQueryAsync();
                }

                await connection.CloseAsync();
            }
        }
    }
}
