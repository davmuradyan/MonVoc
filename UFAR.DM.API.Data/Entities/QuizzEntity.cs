namespace UFAR.DM.API.Data.Entities {
    public class QuizzEntity {
        public string Question { get; set; } = null!;
        public string A { get; set; } = null!;
        public string B { get; set; } = null!;
        public string C { get; set; } = null!;
        public string D { get; set; } = null!;
        public int rightAnswer { get; set; }
    }
}