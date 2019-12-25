using System;

namespace CORTNE.Models
{
    public partial class UserTokenCach
    {
        public int UserTokenCacheId { get; set; }
        public string WebUserUniqueId { get; set; }
        public byte[] CacheBits { get; set; }
        public DateTime LastWrite { get; set; }
    }
}
