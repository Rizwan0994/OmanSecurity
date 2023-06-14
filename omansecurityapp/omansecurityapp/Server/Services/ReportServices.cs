using omansecurityapp.Shared.Models;
using System.Data.SqlClient;
using System.Text.Json;
using System.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace omansecurityapp.Server.Services
{
    public interface IReports
    {
        Task AddNewReport(AddNewReports newReport);
        Task SetPriorityAndAssignOfficer(PoliceOfficerReports report);
        Task SetStatus(InvestigationOfficerReports report);

        Task<List<AddNewReports>> GetSubmittedReports();
        Task<List<PoliceOfficerReports>> GetReportsFromPoliceOfficer();
        Task<List<InvestigationOfficerReports>> GetReportsFromInvestigationOfficer();
        Task<List<InvestigationOfficerReports>> ReportsForMainpage(); //to dsplay on main page


        public class ReportServices : IReports
        {
            private readonly IConfiguration _configuration;

            public ReportServices(IConfiguration configuration)
            {
                _configuration = configuration;
            }

            public string GetConnection()
            {
                var connection = _configuration.GetSection("ConnectionStrings").GetSection("DefaultConnection").Value;
                return connection;
            }


            //for displaying data on main page
            public async Task<List<InvestigationOfficerReports>> ReportsForMainpage()
            {
                var connectionString = this.GetConnection();
                List<InvestigationOfficerReports> lst = new List<InvestigationOfficerReports>();
                using (var con = new SqlConnection(connectionString))
                {
                    try
                    {
                        await con.OpenAsync();
                        var com = new SqlCommand("SELECT ReportType, Location, [Description], [Status] FROM Reports WHERE Status IS NOT NULL",
                            con)
                        {
                            CommandType = CommandType.Text
                        };
                        var rdr = await com.ExecuteReaderAsync();

                        while (rdr.Read())
                        {
                            lst.Add(new InvestigationOfficerReports
                            {
                                ReportType = rdr["ReportType"].ToString(),
                                Location = rdr["Location"].ToString(),
                                Description = rdr["Description"].ToString(),
                                Status = rdr["Status"].ToString(),
                            });
                        }
                        return lst;
                    }
                    finally { con.Close(); }
                }
            }

            public async Task<List<InvestigationOfficerReports>> GetReportsFromInvestigationOfficer()
            {
                var connectionString = this.GetConnection();
                List<InvestigationOfficerReports> lst = new List<InvestigationOfficerReports>();
                using (var con = new SqlConnection(connectionString))
                {
                    try
                    {
                        await con.OpenAsync();
                        var com = new SqlCommand("SELECT * FROM Reports",
                            con)
                        {
                            CommandType = CommandType.Text
                        };
                        var rdr = await com.ExecuteReaderAsync();

                        while (rdr.Read())
                        {
                            lst.Add(new InvestigationOfficerReports
                            {
                                ReportType = rdr["ReportType"].ToString(),
                                Location = rdr["Location"].ToString(),
                                Description = rdr["Description"].ToString(),
                                Priority = rdr["Priority"].ToString(),
                                AssignedOfficer = rdr["AssignedOfficer"].ToString(),
                                Status = rdr["Status"].ToString(),
                                Media = rdr.IsDBNull(rdr.GetOrdinal("Media")) ? null : (byte[])rdr["Media"],
                                MediaType = rdr["MediaType"].ToString()
                            });
                        }
                        return lst;
                    }
                    finally { con.Close(); }
                }
            }

            public async Task<List<PoliceOfficerReports>> GetReportsFromPoliceOfficer()
            {
                var connectionString = this.GetConnection();
                List<PoliceOfficerReports> lst = new List<PoliceOfficerReports>();
                using (var con = new SqlConnection(connectionString))
                {
                    try
                    {
                        await con.OpenAsync();
                        var com = new SqlCommand("SELECT ReportType, Location, [Description], [Priority], [AssignedOfficer], Media, MediaType FROM Reports WHERE Status IS NULL AND AssignedOfficer IS NOT NULL AND Priority IS NOT NULL",
                            con)
                        {
                            CommandType = CommandType.Text
                        };
                        var rdr = await com.ExecuteReaderAsync();

                        while (rdr.Read())
                        {
                            lst.Add(new PoliceOfficerReports
                            {
                                ReportType = rdr["ReportType"].ToString(),
                                Location = rdr["Location"].ToString(),
                                Description = rdr["Description"].ToString(),
                                Priority = rdr["Priority"].ToString(),
                                AssignedOfficer = rdr["AssignedOfficer"].ToString(),
                                Media = rdr.IsDBNull(rdr.GetOrdinal("Media")) ? null : (byte[])rdr["Media"],
                                MediaType = rdr["MediaType"].ToString()
                            });
                        }
                        return lst;
                    }
                    finally { con.Close(); }
                }
            }

            public async Task<List<AddNewReports>> GetSubmittedReports()
            {
                var connectionString = this.GetConnection();
                List<AddNewReports> lst = new List<AddNewReports>();
                using (var con = new SqlConnection(connectionString))
                {
                    try
                    {
                        await con.OpenAsync();
                        var com = new SqlCommand("SELECT ReportType, Location, [Description], Media, MediaType FROM Reports WHERE Priority IS NULL AND AssignedOfficer IS NULL",
                            con)
                        {
                            CommandType = CommandType.Text
                        };
                        var rdr = await com.ExecuteReaderAsync();

                        while (rdr.Read())
                        {
                            lst.Add(new AddNewReports
                            {
                                ReportType = rdr["ReportType"].ToString(),
                                Location = rdr["Location"].ToString(),
                                Description = rdr["Description"].ToString(),
                                Media = rdr.IsDBNull(rdr.GetOrdinal("Media")) ? null : (byte[])rdr["Media"],
                                MediaType = rdr["MediaType"].ToString()
                            });
                        }
                        return lst;
                    }
                    finally { con.Close(); }
                }
            }

            public async Task SetPriorityAndAssignOfficer(PoliceOfficerReports report)
            {
                var connectionString = this.GetConnection();
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                   
                    string query = "UPDATE Reports SET [AssignedOfficer] = @AssignedOfficer, [Priority] = @Priority WHERE ReportType = @ReportType AND Location = @Location AND Description = @Description AND Media = @Media AND MediaType = @MediaType";

                    using (SqlCommand com = new SqlCommand(query, connection))
                    {
                        com.Parameters.AddWithValue("@ReportType", report.ReportType);
                        com.Parameters.AddWithValue("@Location", report.Location);
                        com.Parameters.AddWithValue("@Description", report.Description);
                        com.Parameters.AddWithValue("@Priority", report.Priority);
                        com.Parameters.AddWithValue("@AssignedOfficer", report.AssignedOfficer);
                        com.Parameters.AddWithValue("@Media", report.Media);
                        com.Parameters.AddWithValue("@MediaType", report.MediaType);
                        await com.ExecuteNonQueryAsync();
                    }
                }
            }

            public async Task SetStatus(InvestigationOfficerReports report)
            {
                var connectionString = this.GetConnection();
                using (var con = new SqlConnection(connectionString))
                {
                    await con.OpenAsync();
                    var com = new SqlCommand("UPDATE Reports SET [Status] = @Status WHERE ReportType = @ReportType AND Location = @Location AND Description = @Description AND Priority = @Priority AND AssignedOfficer = @AssignedOfficer AND Media = @Media AND MediaType = @MediaType", con);
                    com.Parameters.AddWithValue("@Description", report.Description);
                    com.Parameters.AddWithValue("@Priority", report.Priority);
                    com.Parameters.AddWithValue("@AssignedOfficer", report.AssignedOfficer);
                    com.Parameters.AddWithValue("@Status", report.Status);
                    com.Parameters.AddWithValue("@ReportType", report.ReportType);
                    com.Parameters.AddWithValue("@Location", report.Location);
                    com.Parameters.AddWithValue("@Media", report.Media);
                    com.Parameters.AddWithValue("@MediaType", report.MediaType);
                    await com.ExecuteNonQueryAsync();
                }
            }

            
                public async Task AddNewReport(AddNewReports newReport)
            {
                // Get a connection to the database and open it
                var connectionString = this.GetConnection();
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Define the SQL query to insert a new report into the Reports table
                    string query = "INSERT INTO Reports (ReportType, Location, Description, Media, MediaType) " +
                                   "VALUES (@ReportType, @Location, @Description, @Media, @MediaType)";

                    // Create a new SqlCommand object with the query and connection objects
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Set the parameter values based on the new report object
                        command.Parameters.AddWithValue("@ReportType", newReport.ReportType);
                        command.Parameters.AddWithValue("@Location", newReport.Location);
                        command.Parameters.AddWithValue("@Description", newReport.Description);
                        command.Parameters.AddWithValue("@Media", newReport.Media ?? (object)DBNull.Value);  // set Media to null if it is not provided
                        command.Parameters.AddWithValue("@MediaType", newReport.MediaType);

                        // Execute the query and get the number of rows affected
                        await command.ExecuteNonQueryAsync();
                    }
                }

            }


        }
    }
}