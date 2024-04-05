using UFAR.DM.API.Core.Services.ChatGPT;
using UFAR.DM.API.Core.Services.Question;
using UFAR.DM.API.Core.Services.Section;
using UFAR.DM.API.Data.DAO;
using UFAR.DM.API.Data.Entities;

namespace UFAR.DM.API.Core.Services.Expression {
    public class ExpressionServices : IExpressionServices {
        MainDbContext context;
        IGPTservices gpt;
        ISectionServices sectionServices;
        IQuestionServices questionServices;

        public ExpressionServices(MainDbContext _context, IGPTservices _gpt, ISectionServices _sectionServices, IQuestionServices _questionServices) {
            context = _context;
            gpt = _gpt;
            sectionServices = _sectionServices;
            questionServices = _questionServices;
        }

        public string AddExpression(string exp, int sectionId) {

            if (sectionServices.HasExp(sectionId, exp)) {
                return "This expression is already added to this section.";
            }

            ExpressionEntity newExp = new ExpressionEntity() {
                SectionId = sectionId,
                Section = context.Sections.FirstOrDefault(x => x.Id == sectionId)
            };

            string gptAnswer = gpt.CorrectExp(exp);
            if (gptAnswer.ToLower() == (exp.ToLower() + ".") || (gptAnswer.ToLower() == exp.ToLower()) || (gptAnswer.ToLower() == "\"" + exp.ToLower() + "\"") || (gptAnswer.ToLower() == "\"" + exp.ToLower() + "\"")) {
                newExp.Expression = exp.ToLower();
                newExp.Level = gpt.LevelOfWordOrExpression(exp);
                context.Expressions.Add(newExp);

                questionServices.MakeQuestion(sectionId, exp);

                context.SaveChanges();
                return "Done!";
            } else {
                return gptAnswer;
            }
        }
        //Deleting an expressionEntity
        public string DeleteExpression(int expressionId) {
            if (context.Expressions.Count() == 0) {
                return "There is no expressions yet\n";
            }

            ExpressionEntity? deletingEntity = context.Expressions.FirstOrDefault(e => e.Id == expressionId);

            if (deletingEntity != null) {
                int sectionId = deletingEntity.SectionId;

                SectionEntity section = sectionServices.GetSectionWithQuestions(sectionId);
                questionServices.DeleteQuestion("Trouvez le synonyme de cela: " + deletingEntity.Expression);
                context.Remove(deletingEntity);

            } else {
                return "There is no expression with " + expressionId + " Id\n";
            }
            context.SaveChanges();
            return "Done\n";
        }
        //Finding the section of an expression
        public int SectionOfExp(int expressionId) {
            return context.Expressions.FirstOrDefault(e => e.Id == expressionId).SectionId;
        }
        //Getting expressions with their ids
        public Dictionary<int, string> GetExpressionsWithId(int sectionId) {
            Dictionary<int, string> expressions = new Dictionary<int, string>();
            if (context.Sections.FirstOrDefault(s => s.Id == sectionId) == null || sectionServices.GetExpressionCount(sectionId) == 0) {
                return expressions;
            }

            SectionEntity section = sectionServices.GetSectionWithExpressions(sectionId);

            foreach (var exp in section.Expressions) {
                expressions.Add(exp.Id, exp.Expression);
            }
            return expressions;
        }
    }
}