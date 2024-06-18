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
                MySqlConnection connection = new MySqlConnection(connectionString);
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

        // Get all the data from a generic DB table
        public List<T> GetData<T>(string query) where T : new()
        {
            List<T> results = [];

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                connection.Open();

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
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
                connection.Close();
            }

            return results;
        }
    }
}
