namespace UFAR.DM.API.Data.Entities {
    public record ExpressionEntity : BaseEntity {
        public string Expression { get; set; } = null!;
        required public int SectionId { get; set; }
        required public SectionEntity Section { get; set; }
    }
}