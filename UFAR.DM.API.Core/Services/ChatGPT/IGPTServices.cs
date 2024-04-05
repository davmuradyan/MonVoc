namespace UFAR.DM.API.Core.Services.ChatGPT {
    public interface IGPTservices {
        public string GetSynonym(string word);
        public string RandomN(string WoE, string r1 = "", string r2 = "");
        public string CorrectWord(string word);
        public string CorrectExp(string Exp);
        public string LevelOfSection(string wordsAndExpressions);
        public string LevelOfWordOrExpression(string wordOrExp);
    }
}