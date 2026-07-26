using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ApiTienda.Database.Entities
{
    public class UserEntity : IdentityUser
    {
        [StringLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [StringLength(100)]
        public string LastName { get; set; } = string.Empty;
    }
} 