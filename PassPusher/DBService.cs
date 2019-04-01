using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PassPusher
{
    public static class DBService
    {
        public static bool Push(string password, string login)
        {
            if (!Validate(password, login))
                return false;


            byte[] salt,
                hashedPass = PasswordHasher.GetHashedPassword(out salt, password);
            MySqlConnection connection;

            if (GetConnection(out connection))
            {
                MySqlCommand sqlCommand = new MySqlCommand("Insert into auth(login, hash, salt) Values(@login, @hash, @salt)", connection);
                sqlCommand.Parameters.Add("@login", MySqlDbType.VarChar).Value = login;
                sqlCommand.Parameters.Add("@hash", MySqlDbType.VarBinary).Value = hashedPass;
                sqlCommand.Parameters.Add("@salt", MySqlDbType.Binary).Value = salt;

                try
                {
                    sqlCommand.ExecuteNonQuery();
                    connection.Close();
                    return true;
                }
                catch (MySqlException)
                {
                    if (connection.State == System.Data.ConnectionState.Open)
                        connection.Close();
                    return false;
                }
            }
            connection.Close();
            return false;
        }
        public static bool Validate(string password, string login)
        {
            Regex regex = new Regex("^(?=.{5,25}$)(?![_.])(?!.*[_.]{2})[a-zA-Z0-9._]+(?<![_.])$");
            int result = 0;
            MySqlConnection connection;

            if(GetConnection(out connection))
            {
                MySqlCommand sqlCommand = new MySqlCommand("Select Count(*) from auth where login = @login", connection);
                sqlCommand.Parameters.Add("@login", MySqlDbType.VarChar).Value = login;

                result = Convert.ToInt32(sqlCommand.ExecuteScalar());
            }

            connection.Close();

            if (regex.IsMatch(login) && result == 0)
                return true;
            return false;
        }
        public static bool TryConnection()
        {
            MySqlConnection connection;

            if (GetConnection(out connection))
            {
                connection.Close();
                return true;
            }
            return false;
        }

        private static bool GetConnection(out MySqlConnection connection)
        {
            connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["cp_dbConnectionString"].ConnectionString);
            try
            {
                connection.Open();
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }
    }
}
