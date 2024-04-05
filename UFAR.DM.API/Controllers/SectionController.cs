using Microsoft.AspNetCore.Mvc;
using UFAR.DM.API.Core.Services.Section;
namespace UFAR.DM.API.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class SectionController : ControllerBase {
        public ISectionServices services;

        public SectionController(ISectionServices _services) {
            services = _services;
        }

        [HttpPost("AddSection")]
        public IActionResult AddSection(string section) {
            return Ok(services.AddSection(section));
        }

        [HttpDelete("DeleteSection")]
        public IActionResult DeleteSection(int sectionId) {
            return Ok(services.DeleteSection(sectionId));
        }

        [HttpGet("GetNumberOfWords")]
        public IActionResult GetNumberOfWords(int sectionId) {
            return Ok(services.GetWordCount(sectionId));
        }

        [HttpGet("GetNumberOfExpressions")]
        public IActionResult GetNumberOfExpressions(int sectionId) {
            return Ok(services.GetExpressionCount(sectionId));
        }
        [HttpGet("GetSectionLevel")]
        public IActionResult GetSectionLevel(int sectionId) {
            return Ok(services.GetLevelOfSection(sectionId));
        }

        [HttpGet("GetSections")]
        public IActionResult GetSections() {
            return Ok(services.GetSections());
        }
        [HttpGet("MakeQuizz")]
        public IActionResult MakeQuizz(int sectionId) {
            return Ok(services.MakeQuizz(sectionId));
        }
    }
}
