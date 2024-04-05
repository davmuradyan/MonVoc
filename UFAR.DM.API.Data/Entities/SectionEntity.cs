namespace UFAR.DM.API.Data.Entities {
    public record SectionEntity : BaseEntity {
        public string Name { get; set; } = null!;

        private ICollection<WordEntity> _words = new List<WordEntity>();
        public ICollection<WordEntity> Words {
            get { return _words; }
            set { _words = value; }
        }

        private ICollection<ExpressionEntity> _exp = new List<ExpressionEntity>();
        public ICollection<ExpressionEntity> Expressions {
            get { return _exp; }
            set { _exp = value; }
        }

        private ICollection<QuestionEntity> _questions = new List<QuestionEntity>();
        public ICollection<QuestionEntity> Questions {
            get { return _questions; }
            set { _questions = value; }
        }
    }
}