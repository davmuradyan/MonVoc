using OpenAI_API;
using OpenAI_API.Chat;

namespace UFAR.DM.API.Core.Services.ChatGPT {
    public class GPTservices : IGPTservices {
        public string Generate(string request) {
            var prompt = request;

            var openAI = new OpenAIAPI("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");

            var response = openAI.Chat.CreateChatCompletionAsync
                (
                    new ChatRequest {
                        Messages = new List<ChatMessage>() {
                            new ChatMessage() {
                                TextContent = prompt,
                            }
                        },
                        MaxTokens = 1000,
                        Temperature = 0.1,
                        Model = "gpt-3.5-turbo"
                    }
                ).Result;

            foreach (var item in response.Choices) {
                return item.Message.TextContent;
            }
            return "Request failed.";
        }

        public string GetSynonym(string word) {
            string prompt = "Veuillez fournir un synonyme pour le mot français \"" + word + "\"," +
                " ou s'il n'y a pas de synonyme exact, fournissez plutôt une définition. " +
                "Veuillez vous assurer que la réponse ne contient que le synonyme ou la définition demandée, " +
                "sans aucun signe, ponctuation ou mot supplémentaire.";

            string answer = Generate(prompt);
            return answer;  
        }

        public string CorrectWord(string word) {
            string prompt = "Corrigez le mot \"" + word + "\" et envoiez la version corrige. Si le mot est valide, envoiez le meme mot.";

            string answer = Generate(prompt);
            return answer;
        }

        public string CorrectExp(string exp) {
            var prompt = "Corrigez l'expression \"" + exp + "\" et envoiez la version corrige. Si l'expression est valide, envoiez le meme expression.";

            string answer = Generate(prompt);
            return answer;
        }

        public string LevelOfSection(string wordsAndExpressions) {
            var prompt = "Quel est le niveau de ses mots? \'" + wordsAndExpressions + "\'. Ne donnez pas les niveaux pour touts les mots, " +
                "donnez un niveau moyenne pour ses mots. Tu peut choisir seulement l'un de ses reponses " +
                "\'A1\', \'A2\', \'A2+\', \'B1\', \'B1+\', \'B2\', \'B2+\', \'C1\', \'C2\'. " +
                "N'outilise pas les autres mots dans la reponse! " +
                "Et aussi, ne dire pas \"Le niveau moyenne...\". Dire seulement le niveau.";

            string answer = Generate(prompt);
            return answer;
        }

        public string LevelOfWordOrExpression(string wordOrExp) {
            var prompt = "Quel est le niveau de cela? \'" + wordOrExp + "Tu peut choisir seulement l'un de ses reponses " +
                            "\'A1\', \'A2\', \'A2+\', \'B1\', \'B1+\', \'B2\', \'B2+\', \'C1\', \'C2\'. " +
                            "N'outilise pas les autres mots dans la reponse! " +
                            "Et aussi, ne dire pas \"Le niveau est...\". Dire seulement le niveau.";

            string answer = Generate(prompt);
            return answer;
        }

        public string RandomN(string WoE, string r1 = "", string r2 = "") {
            var prompt = "";
            if (r1 == "" && r2 == "") {
                prompt = "Donnez moi un mot ou une definition au hazard, qui n'est pas l'un des synonymes du mot '" + WoE + "'. Envoiez seulement le mot ou la definition.";
            } else if (r1 != "" && r2 == "") {
                prompt = "Donnez moi un mot ou une definition au hazard sauf du mot \'" + r1 + "\', qui n'est pas l'un des synonymes du mot '" + WoE + "'. Envoiez seulement le mot ou la definition.";
            } else if (r1 != "" && r2 != "") {
                prompt = "Donnez moi un mot ou une definition au hazard sauf \'" + r1 + "\' et \'" + r2 + "\', qui n'est pas l'un des synonymes du mot '" + WoE + "'. Envoiez seulement le mot ou la definition.";
            }

            string answer = Generate(prompt);
            return answer;
        }
    }
}
