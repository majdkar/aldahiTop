namespace FirstCall.Core.Entities
{
    public class Unit
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int? BaseUnitId { get; set; }
        public virtual Unit BaseUnit { get; set; }

        public decimal? BaseUnitRatio { get; set; }

        public bool IsActive { get; set; }
    }
}
