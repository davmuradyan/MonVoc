using UFAR.DM.API.Data.Entities;

namespace UFAR.DM.API.Core.Services.Question {
    public interface IQuestionServices {
        public QuestionEntity MakeQuestion(int SectionId, string WoE);
        public void DeleteQuestion(string question);
    }
}