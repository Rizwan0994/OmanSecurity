using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace omansecurityapp.Shared.Models
{
    public class AdminUsers
    {
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }

        public int UniqueID { get; set; }
        public string Role { get; set; }
    }
}
