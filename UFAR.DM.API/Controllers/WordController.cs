using Microsoft.AspNetCore.Mvc;
using UFAR.DM.API.Core.Services.Word;
using UFAR.DM.API.Core.Services.Section;

namespace UFAR.DM.API.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class WordController : ControllerBase {
        public IWordServices services;
        public ISectionServices sectionServices;

        public WordController(IWordServices _services, ISectionServices _sectionServices) {
            services = _services;
            sectionServices = _sectionServices;
        }

        [HttpPost("AddWord")]
        public IActionResult AddWord(string word, int sectionId) {
            string rv = services.AddWord(word, sectionId);
            sectionServices.UpdateSectionLevel(sectionId);

            return Ok(rv);
        }

        [HttpDelete("DeleteWord")]
        public IActionResult DeleteWord(int wordId) {
            int sectionId = services.SectionOfWord(wordId);
            string rv = services.DeleteWord(wordId);
            sectionServices.UpdateSectionLevel(sectionId);

            return Ok(rv);
        }

        [HttpGet("GetWordsWithID")]
        public IActionResult GetWords(int sectionId) {
            return Ok(services.GetWordsWithId(sectionId));
        }
    }
}