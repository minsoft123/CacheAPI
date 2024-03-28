using System.ComponentModel.DataAnnotations;

namespace CacheAPI.Models
{
    public class CacheSettings
    {
        //ToDo: Add required options pattern
        public int CacheSizeLimit { get; set; }
    }
}
