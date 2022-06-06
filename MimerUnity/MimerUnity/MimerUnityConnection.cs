using Mimer.Data.Client;

namespace MimerUnity
{
    public class MimerUnityConnection
    {
        private MimerConnection connection;

        public string CurrentUser
        {
            get
            {
                return connection.CurrentUser;
            }
        }

        public void Open(string database, string username, string password)
        {
            var connectionString = new MimerConnectionStringBuilder();
            connectionString.Add("Database", database);
            connectionString.Add("User ID", username);
            connectionString.Add("Password", password);

            if (connection != null)
            {
                connection.Close();
                connection = null;
            }

            connection = new MimerConnection(connectionString.ToString());
            connection.Open();
        }

        public void Close()
        {
            connection.Close();
        }
    }
}
