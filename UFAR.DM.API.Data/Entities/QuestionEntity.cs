namespace UFAR.DM.API.Data.Entities {
    public record QuestionEntity {
        public int Id { get; set; }

        public required string Question { get; set; }

        public required string Synonym { get; set; }

        public required string Random1 { get; set; }

        public required string Random2 { get; set; }

        public required string Random3 { get; set; }

        public required int SectionId { get; set; }

        public required SectionEntity SectionEntity { get; set; }
    }
}