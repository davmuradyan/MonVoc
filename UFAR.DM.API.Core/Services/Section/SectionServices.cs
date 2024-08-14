using Microsoft.EntityFrameworkCore;
using UFAR.DM.API.Core.Services.ChatGPT;
using UFAR.DM.API.Core.Services.Question;
using UFAR.DM.API.Data.DAO;
using UFAR.DM.API.Data.Entities;

namespace UFAR.DM.API.Core.Services.Section {
    public class SectionServices : ISectionServices {
        //Injections
        MainDbContext context;
        IGPTservices gpt;
        IQuestionServices question;
        public SectionServices(MainDbContext _context, IGPTservices _gpt, IQuestionServices _question) {
            context = _context;
            gpt = _gpt;
            question = _question;
        }

        public SectionEntity? GetSectionWithWords(int sectionId) {
            return context.Sections
                .Include(s => s.Words) // Eager loading Words collection
                .FirstOrDefault(s => s.Id == sectionId);
        }
        public SectionEntity? GetSectionWithExpressions(int sectionId) {
            return context.Sections
                .Include(s => s.Expressions)
                .FirstOrDefault(s => s.Id == sectionId);
        }
        public SectionEntity? GetSectionWithQuestions(int sectionId) {
            return context.Sections
                    .Include(s => s.Questions)
                    .FirstOrDefault(s => s.Id == sectionId);
        }
        public string AddSection(string section) {
            if (section.Length > 30) {
                return "The length of the section's name can't exceed 30 symbols!";
            }
            string nameWithoutSpaces = "";

            foreach (var ch in section) {
                nameWithoutSpaces += ch;
            }

            if (nameWithoutSpaces == "") {
                return "Name can't consist only from spaces";
            }

            int i = 0;
            foreach (var item in context.Sections) {
                if (item.Name == section) {
                    i++;
                    break;
                }
            }

            if (i == 1) {
                return "The section with " + section + " exists.";
            }
            SectionEntity sec = new SectionEntity() {
                Name = section,
                Words = new List<WordEntity>(),
                Expressions = new List<ExpressionEntity>(),
                Questions = new List<QuestionEntity>()
            };
            context.Add(sec);
            context.SaveChanges();
            return "Done!";
        }
        public string DeleteSection(int sectionId) {
            if (context.Sections.Count() == 0) {
                return "There is no sections yet\n";
            }

            SectionEntity? deletingEntity = context.Sections.FirstOrDefault(e => e.Id == sectionId);

            if (deletingEntity != null) {
                context.Remove(deletingEntity);
            } else {
                return "There is no section with " + sectionId + " Id\n";
            }
            context.SaveChanges();
            return "Done\n";
        }
        public int GetWordCount(int sectionId) {
            // Include the Words collection when querying for the section
            var sectionWithWords = GetSectionWithWords(sectionId);

            // Check if the section exists
            if (sectionWithWords != null) {
                // Return the count of words in the section
                return sectionWithWords.Words.Count;
            } else {
                // Handle the case when the section doesn't exist
                return 0;
            }
        }
        public int GetExpressionCount(int sectionId) {
            // Include the Words collection when querying for the section
            var sectionWithExpressions = GetSectionWithExpressions(sectionId);

            // Check if the section exists
            if (sectionWithExpressions != null) {
                // Return the count of words in the section
                return sectionWithExpressions.Expressions.Count;
            } else {
                // Handle the case when the section doesn't exist
                return 0;
            }
        }
        public string GetLevelOfSection(int sectionId) {
            SectionEntity? sectionEntity = context.Sections.FirstOrDefault(x => x.Id == sectionId);
            if (sectionEntity == null) {
                return "The section with " + sectionId + " ID couldn't be found";
            }
            return sectionEntity.Level;
        }
        public void UpdateSectionLevel(int sectionId) {
            SectionEntity? section = context.Sections.FirstOrDefault(x => x.Id == sectionId);
            if (section == null) {
                return;
            }

            if ((GetExpressionCount(sectionId) == 0) && (GetWordCount(sectionId) == 0)) {
                section.Level = "A0";
            } else {
                string wordsAndExpressions = "";

                SectionEntity? sec = GetSectionWithWords(sectionId);

                if (sec == null) {
                    return;
                }

                if (sec.Words != null) {
                    foreach (var word in sec.Words) {
                        wordsAndExpressions += word + ", ";
                    }
                }
                sec = GetSectionWithExpressions(sectionId);

                if (sec == null) {
                    return;
                }

                if (sec.Expressions != null) {
                    foreach (var exp in sec.Expressions) {
                        wordsAndExpressions += exp + ", ";
                    }
                }

                string sectionLevel = gpt.LevelOfSection(wordsAndExpressions);
                section.Level = sectionLevel;

                context.Sections.FirstOrDefault(s => s.Id == sectionId).Level = section.Level;
            }
            context.SaveChanges();
        }
        public void AddWordToSection(int sectionId, WordEntity word) {
            context.Sections.FirstOrDefault(s => s.Id == sectionId).Words.Add(word);
            context.SaveChanges();
        }
        public bool HasWord(int sectionId, string word) {
            SectionEntity? sectionWithWords = GetSectionWithWords(sectionId);
            word = word.ToLower();
            if (sectionWithWords == null)
            {
                return false;
            }
            foreach (var w in sectionWithWords.Words) {
                if (w.Word == word) {
                    return true;
                }
            }
            return false;
        }
        public bool HasExp(int sectionId, string exp) {
            SectionEntity sectionWithWords = GetSectionWithExpressions(sectionId);
            exp = exp.ToLower();
            foreach (var w in sectionWithWords.Expressions) {
                if (w.Expression == exp) {
                    return true;
                }
            }
            return false;
        }
        public ICollection<SectionForReturnEntity> GetSections() {
            ICollection<SectionForReturnEntity> returnSections = new HashSet<SectionForReturnEntity>();
            ICollection<SectionEntity> sections = new HashSet<SectionEntity>();
            foreach(var s in context.Sections) {
                sections.Add(s);
            }
            

            foreach (var section in sections) {
                int wordCount = GetWordCount(section.Id);
                int expressionCount = GetExpressionCount(section.Id);
                SectionForReturnEntity sectionForReturn = new SectionForReturnEntity {
                    Id = section.Id,
                    name = section.Name,
                    words = wordCount,
                    expressions = expressionCount,
                    level = section.Level
                };
                returnSections.Add(sectionForReturn);
            }
            return returnSections;
        }
        public ICollection<QuizzEntity> MakeQuizz(int sectionId) {
            SectionEntity section = GetSectionWithQuestions(sectionId);
            ICollection<QuizzEntity> quizz = new HashSet<QuizzEntity>();
            Random rnd = new Random();
            int right;
            int rAnswer = -1;
            string a = "", b = "", c = "", d = "";
            string question;

            foreach (var q in section.Questions) {
                question = q.Question;
                right = rnd.Next(1, 5);
                switch (right) {
                    case 1:
                        rAnswer = 0;
                        a = q.Synonym;
                        b = q.Random1;
                        c = q.Random2;
                        d = q.Random3;
                        break;
                    case 2:
                        rAnswer = 1;
                        b = q.Synonym;
                        a = q.Random1;
                        c = q.Random2;
                        d = q.Random3;
                        break;
                    case 3:
                        rAnswer = 2;
                        c = q.Synonym;
                        a = q.Random1;
                        b = q.Random2;
                        d = q.Random3;
                        break;
                    case 4:
                        rAnswer = 3;
                        d = q.Synonym;
                        a = q.Random1;
                        b = q.Random2;
                        c = q.Random3;
                        break;
                }

                QuizzEntity quizzQuestion = new QuizzEntity() {
                    Question = question,
                    A = a,
                    B = b,
                    C = c,
                    D = d,
                    rightAnswer = rAnswer
                };

                quizz.Add(quizzQuestion);
            }

            return quizz;
        }
        public int GetSectionNumber() {
            int num = 0;
            foreach (var sec in context.Sections)
            {
                num++;
            }
            return num;
        }
    }
}