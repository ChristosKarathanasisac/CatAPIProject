using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace NatechAPI.Models.Entities
{
    public class CatEntity
    {
        public  Guid Id { get; set; } 
        public required string CatId { get; set; }  
        public int? Width { get; set; }  
        public int? Height { get; set; }  
        public required string Image { get; set; }  
        public required DateTime Created { get; set; } = DateTime.UtcNow;
        public ICollection<CatTag> CatTags { get; set; } = new List<CatTag>();
    }
}
