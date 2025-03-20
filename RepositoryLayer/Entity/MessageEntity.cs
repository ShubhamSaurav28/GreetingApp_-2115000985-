using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Entity
{
    public class MessageEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Message { get; set; }

        // Foreign Key

        [Required]
        [ForeignKey("UserId")]
        public int UserId { get; set; }

        // Navigation Property (nullable to avoid circular reference issues)
        public UserEntity? User { get; set; }

        // Timestamps
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}
