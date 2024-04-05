namespace UFAR.DM.API.Core.Services.Expression {
    public interface IExpressionServices {
        //Adding new expressionEntity
        public string AddExpression(string exp, int sectionId);

        //Deleting expressionEntity
        public string DeleteExpression(int expressionId);

        //Finding the section of an expression
        public int SectionOfExp(int expressionId);

        //Getting expressions with their ids
        public Dictionary<int, string> GetExpressionsWithId(int sectionId);
    }
}