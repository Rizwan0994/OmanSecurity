using omansecurityapp.Shared.Models;
using System.Data.SqlClient;
using System.Data;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;

namespace omansecurityapp.Server.Services
{
    public interface IUser
    {
        Task AddNewUser(AdminUsers usersList);
        AdminUsers GetUserData(int uniqueID);
        void UpdatePassword(PasswordUpdateModelForAdmins user);
    }

    public class UserServices : IUser
    {
        private readonly IConfiguration _configuration;

        public UserServices(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetConnection()
        {
            var connection = _configuration.GetSection("ConnectionStrings").GetSection("DefaultConnection").Value;
            return connection;
        }

        //add signed-up users data to database
        public async Task AddNewUser(AdminUsers user)
        {
            // Get a connection to the database and open it
            var connectionString = this.GetConnection();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Define the SQL query to insert a new report into the AdminUser table
                string query = @"SET IDENTITY_INSERT [OMANSECURITY].[dbo].[AdminUser] ON;
                                 INSERT INTO [OMANSECURITY].[dbo].[AdminUser] (Username, FirstName, LastName, Password, Role, UniqueID)
                                 VALUES (@Username, @FirstName, @LastName, @Password, @Role, @UniqueID);
                                 SET IDENTITY_INSERT [OMANSECURITY].[dbo].[AdminUser] OFF;";

                // Create a new SqlCommand object with the query and connection objects
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    // Set the parameter values based on the new report object
                    cmd.Parameters.AddWithValue("@Username", user.Username);
                    cmd.Parameters.AddWithValue("@FirstName", user.FirstName);
                    cmd.Parameters.AddWithValue("@LastName", user.LastName);
                    cmd.Parameters.AddWithValue("@Password", user.Password);
                    cmd.Parameters.AddWithValue("@Role", user.Role);
                    cmd.Parameters.AddWithValue("@UniqueID", user.UniqueID);

                    // Execute the query and get the number of rows affected
                    await cmd.ExecuteNonQueryAsync();
                }
            }

        }



        //match unique id with db and update password
        public void UpdatePassword(PasswordUpdateModelForAdmins user)
        {
            string connString = GetConnection();
            using (SqlConnection con = new SqlConnection(connString))
            {
                try
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("UPDATE AdminUser SET Password = @Password WHERE UniqueID = @UniqueID", con);
                    cmd.Parameters.AddWithValue("@UniqueID", user.UniqueID);
                    cmd.Parameters.AddWithValue("@Password", user.Password);
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected == 0)
                    {
                        throw new Exception("No records found with the given Unique ID");
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        //Get the details of a user using UniqueID for login purpose
        public AdminUsers GetUserData(int UniqueID)
        {
            try
            {
                AdminUsers user = new AdminUsers();
                string connString = GetConnection();
                using (SqlConnection con = new SqlConnection(connString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("SELECT * FROM AdminUser WHERE UniqueID = @UniqueID", con);
                    cmd.Parameters.AddWithValue("@UniqueID", UniqueID);
                    SqlDataReader rdr = cmd.ExecuteReader();
                    if (rdr.HasRows)
                    {
                        rdr.Read();
                        user.UniqueID = Convert.ToInt32(rdr["UniqueID"]);
                        user.Username = rdr["Username"].ToString();
                        user.Password = rdr["Password"].ToString();
                        user.Role = rdr["Role"].ToString();
                    }
                    else
                    {
                        throw new ArgumentNullException();
                    }
                    rdr.Close();
                }
                return user;
            }
            catch
            {
                throw;
            }
        }
    }
}
