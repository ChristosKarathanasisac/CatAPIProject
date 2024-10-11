namespace NatechAPI.Models.Entities
{
    public class CatTag
    {
        public Guid CatEntityId { get; set; }  
        public CatEntity CatEntity { get; set; }

        public Guid TagEntityId { get; set; }  
        public TagEntity TagEntity { get; set; }
    }
}