namespace UFAR.DM.API.Data.Entities {
    public abstract record BaseEntity {
        public int Id { get; set; }
        public string Level { get; set; } = "A0";
    }
}