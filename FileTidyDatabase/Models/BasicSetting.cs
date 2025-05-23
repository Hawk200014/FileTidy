using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileTidyDatabase.Models
{
    /// <summary>
    /// Represents a generic key-value setting for application configuration.
    /// </summary>
    public class BasicSetting
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(128)]
        public string Key { get; set; }

        [MaxLength(1024)]
        public string Value { get; set; }
    }
}
