using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace NatechAPI.Models.Entities
{
    public class TagEntity
    {
        public Guid Id { get; set; } 
        public required string Name { get; set; }
        public required DateTime Created { get; set; }
        public ICollection<CatTag> CatTags { get; set; } = new List<CatTag>();
    }
}
