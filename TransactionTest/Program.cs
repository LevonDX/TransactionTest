using Microsoft.Data.SqlClient;

namespace TransactionTest
{
    internal class Program
    {
        private const string ConnectionString = "Data Source=lectures-db-dev.database.windows.net;Initial Catalog=BlocknotDB;Persist Security Info=True;User ID=ad;Password=Ararat73;Encrypt=True;Trust Server Certificate=True";

        static void Main(string[] args)
        {
            // indert numbers from 1 to 100 to Numbers table of the DB

            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                SqlTransaction transaction = connection.BeginTransaction();

                string commandText = "INSERT INTO Numbers (Number) VALUES (@Value)";

                SqlCommand command = new SqlCommand(commandText, connection);

                command.Transaction = transaction;

                SqlParameter parameter = command.CreateParameter();
                parameter.ParameterName = "@Value";
                command.Parameters.Add(parameter);

                try
                {
                    for (int i = 1; i <= 100; i++)
                    {
                        //if (i == 50)
                        //{
                        //    throw new Exception("Something went wrong");
                        //}
                        parameter.Value = i;
                        command.ExecuteNonQuery();
                    }

                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                }
            }
        }
    }
}
