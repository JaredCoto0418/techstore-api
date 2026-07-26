using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ApiTienda.Database.Entities
{
    public class RoleEntity : IdentityRole
    {
        [StringLength(250)]
        public string Description { get; set; } = string.Empty;
    }
} 