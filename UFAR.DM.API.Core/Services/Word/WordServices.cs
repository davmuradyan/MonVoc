using UFAR.DM.API.Core.Services.ChatGPT;
using UFAR.DM.API.Core.Services.Question;
using UFAR.DM.API.Core.Services.Section;
using UFAR.DM.API.Data.DAO;
using UFAR.DM.API.Data.Entities;

namespace UFAR.DM.API.Core.Services.Word {
    public class WordServices : IWordServices {
        MainDbContext context;
        IGPTservices gpt;
        ISectionServices sectionServices;
        IQuestionServices questionServices;

        public WordServices(MainDbContext _context, IGPTservices _gpt, ISectionServices _sectionServices, IQuestionServices _questionServices) {
            context = _context;
            gpt = _gpt;
            sectionServices = _sectionServices;
            questionServices = _questionServices;
        }

        //Adding a new wordentity
        public string AddWord(string word, int sectionId) {
            if (sectionServices.HasWord(sectionId, word)) {
                return "This word is already added to this section.";
            }


            SectionEntity _section = context.Sections.FirstOrDefault(x => x.Id == sectionId);
            WordEntity newWord = new WordEntity() {
                SectionId = sectionId,
                Section = _section
            };

            string gptAnswer = gpt.CorrectWord(word);
            if (gptAnswer.ToLower() == word.ToLower()) {
                newWord.Word = word.ToLower();
                newWord.Level = gpt.LevelOfWordOrExpression(word);
                context.Words.Add(newWord);
                sectionServices.AddWordToSection(sectionId, newWord);

                questionServices.MakeQuestion(sectionId, word.ToLower());

                context.SaveChanges();
                return "Done!";
            } else {
                return gptAnswer;
            }
        }
        //Delete wordentity using id
        public string DeleteWord(int wordId) {
            if (context.Words.Count() == 0) {
                return "There is no words yet\n";
            }

            WordEntity? deletingEntity = context.Words.FirstOrDefault(e => e.Id == wordId);

            if (deletingEntity != null) {
                int sectionId = deletingEntity.SectionId;

                SectionEntity section = sectionServices.GetSectionWithQuestions(sectionId);
                questionServices.DeleteQuestion("Trouvez le synonyme de cela: " + deletingEntity.Word);
                context.Remove(deletingEntity);
            } else {
                return "There is no word with " + wordId + " Id\n";
            }
            context.SaveChanges();
            return "Done\n";
        }
        //Finding the section of a word
        public int SectionOfWord(int WordId) {
            return context.Words.FirstOrDefault(w => w.Id == WordId).SectionId;
        }
        //Getting words with their ids
        public Dictionary<int, string> GetWordsWithId(int sectionId) {
            Dictionary<int, string> words = new Dictionary<int, string>();
            if (context.Sections.FirstOrDefault(s => s.Id == sectionId) == null || sectionServices.GetWordCount(sectionId) == 0) {
                return words;
            }

            SectionEntity section = sectionServices.GetSectionWithWords(sectionId);

            foreach (var word in section.Words) {
                words.Add(word.Id, word.Word);
            }
            return words;
        }
    }
}