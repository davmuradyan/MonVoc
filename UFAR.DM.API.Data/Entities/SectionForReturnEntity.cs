namespace UFAR.DM.API.Data.Entities {
    public record SectionForReturnEntity {
        public int Id { get; set; }
        public string? name { get; set; }
        public int words {get; set; }
        public int expressions { get; set; }
        public string? level { get; set; }
    }
}