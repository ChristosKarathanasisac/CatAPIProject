using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace NatechAPI.Models.Entities
{
    public class CatEntity
    {
        public  Guid Id { get; set; } 
        public required string CatId { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Width must be a positive number.")]
        public int? Width { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Height must be a positive number.")]
        public int? Height { get; set; }  
        public required string Image { get; set; }  
        public required DateTime Created { get; set; }
        public ICollection<CatTag> CatTags { get; set; } = new List<CatTag>();
    }
}
