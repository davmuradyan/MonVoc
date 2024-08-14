using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using UFAR.DM.API.Core.Services.Section;
using UFAR.DM.API.Data.Entities;
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

        [HttpGet("GetSectionLevel")]
        public IActionResult GetSectionLevel(int sectionId) {
            return Ok(services.GetLevelOfSection(sectionId));
        }

        [HttpGet("GetSections")]
        public IActionResult GetSections() {
            ICollection <SectionForReturnEntity> sct = services.GetSections();
            return Ok(sct);
        }
        [HttpGet("MakeQuizz")]
        public IActionResult MakeQuizz(int sectionId) {
            return Ok(services.MakeQuizz(sectionId));
        }

        [HttpGet("GetSectionNumber")]
        public IActionResult GetSectionNumber() {
            return Ok(services.GetSectionNumber());
        }
    }
}
