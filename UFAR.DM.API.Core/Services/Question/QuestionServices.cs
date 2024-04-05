using UFAR.DM.API.Core.Services.ChatGPT;
using UFAR.DM.API.Core.Services.Section;
using UFAR.DM.API.Data.DAO;
using UFAR.DM.API.Data.Entities;

namespace UFAR.DM.API.Core.Services.Question {
    public class QuestionServices : IQuestionServices {
        MainDbContext context;
        IGPTservices gpt;

        public QuestionServices(MainDbContext _context, IGPTservices _gpt) {
            context = _context;
            gpt = _gpt;
        }

        public QuestionEntity MakeQuestion(int SectionId, string WoE) {
            string synonym = gpt.GetSynonym(WoE);
            string r1 = gpt.RandomN(WoE);
            string r2 = gpt.RandomN(WoE, r1);
            string r3 = gpt.RandomN(WoE, r1, r2);

            QuestionEntity question = new QuestionEntity() {
                Question = "Trouvez le synonyme de cela: " + WoE,
                Synonym = synonym,
                Random1 = r1,
                Random2 = r2,
                Random3 = r3,
                SectionId = SectionId,
                SectionEntity = context.Sections.FirstOrDefault(s => s.Id == SectionId)
            };

            context.Add(question);
            return question;
        }
        public void DeleteQuestion(string question) {
            QuestionEntity qst = context.Questions.FirstOrDefault(q => q.Question == question);
            context.Questions.Remove(qst);
            context.SaveChanges();
        }
    }
}