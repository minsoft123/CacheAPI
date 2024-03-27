using CacheAPI.Attributes;
using System.ComponentModel.DataAnnotations;

namespace CacheAPI.Models
{
    public class CacheEntryWithExpirationOptionsModel : CacheEntryModel
    {
        [MinExpiration]
        public int ExpirationInMinutes { get; set; }
        public bool IsSlidingExpiration { get; set; }
    }
}
