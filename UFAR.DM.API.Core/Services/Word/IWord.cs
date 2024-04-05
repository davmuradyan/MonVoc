using UFAR.DM.API.Core.Services.ChatGPT;
using UFAR.DM.API.Data.Entities;

namespace UFAR.DM.API.Core.Services.Word {
    public interface IWordServices {
        //Add new WordEntity
        public string AddWord(string word, int sectionId);

        //GetWordsWithTheirID
        public Dictionary<int, string> GetWordsWithId(int sectionId);

        //Delete a WordEntity
        public string DeleteWord(int wordId);

        //Find the section of the word
        public int SectionOfWord(int expressionId);
    }
}