using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiTienda.Database.Entities.Common
{
    /// <summary>
    /// Clase base para entidades de base de datos con campos de auditoría.
    /// </summary>
    public class EntidadBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        [Required]
        [StringLength(100)]
        public string CreadoPor { get; set; } = string.Empty;

        public DateTime? FechaActualizacion { get; set; }

        [StringLength(100)]
        public string? ActualizadoPor { get; set; }
    }
} 