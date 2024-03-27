using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace CacheAPI.Models
{
    public class CacheEntryModel
    {
        [Required(ErrorMessage = "Key cannont be null or empty.")]
        public string Key { get; set; }

        [Required(ErrorMessage = "Value cannot be null.")]
        public object Value { get; set; }
    }
}
