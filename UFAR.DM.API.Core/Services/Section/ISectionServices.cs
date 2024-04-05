using UFAR.DM.API.Data.Entities;

namespace UFAR.DM.API.Core.Services.Section {
    public interface ISectionServices {
        //Return a section with ICollection<WordEntity>
        public SectionEntity? GetSectionWithWords(int sectionId);

        //Return a section with ICollection<ExpressionEntity>
        public SectionEntity? GetSectionWithExpressions(int sectionId);

        //Return a section with ICollection<QuestionEntity>
        public SectionEntity? GetSectionWithQuestions(int sectionId);

        //Add a new SectionEntity
        public string AddSection(string section);

        //Delete SectionEntity
        public string DeleteSection(int sectionId);

        //Get the number of words of a section
        public int GetWordCount(int sectionId);

        //Get the number of expressions of a section
        public int GetExpressionCount(int sectionId);

        //Get the level of a section
        public string GetLevelOfSection(int sectionId);

        //Update section level after adding or deleting word or expression
        public void UpdateSectionLevel(int sectionId);

        //Add a WordEntity to a section
        public void AddWordToSection(int sectionId, WordEntity word);

        //Checks if the section has words
        public bool HasWord(int sectionId, string word);

        //Checks if the section has expressions
        public bool HasExp(int sectionId, string exp);

        //Returns sections
        public ICollection<SectionEntity> GetSections();

        //Creates a quizz based on questions that have been already created
        public ICollection<QuizzEntity> MakeQuizz(int sectionId);
    }
}