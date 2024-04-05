using Microsoft.AspNetCore.Mvc;
using UFAR.DM.API.Core.Services.ChatGPT;
using UFAR.DM.API.Core.Services.Expression;
using UFAR.DM.API.Core.Services.Section;

namespace UFAR.DM.API.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class ExpressionController : ControllerBase {
        public IExpressionServices services;
        public IGPTservices gpt;
        public ISectionServices sectionServices;
        public ExpressionController(IExpressionServices _services, IGPTservices _gpt, ISectionServices sectionServices) {
            services = _services;
            gpt = _gpt;
            this.sectionServices = sectionServices;
        }

        [HttpPost("AddExpression")]
        public IActionResult AddExpression(string expression, int sectionId) {
            string rv = services.AddExpression(expression, sectionId);
            sectionServices.UpdateSectionLevel(sectionId);

            return Ok(rv);
        }

        [HttpDelete("DeleteExpression")]
        public IActionResult DeleteExpression(int expressionId) {
            int sectionId = services.SectionOfExp(expressionId);
            string rv = services.DeleteExpression(expressionId);
            sectionServices.UpdateSectionLevel(sectionId);

            return Ok(rv);
        }

        [HttpPost("GetExpressionsWithID")]
        public IActionResult GetExpressions(int sectionId) {
            return Ok(services.GetExpressionsWithId(sectionId));
        }
    }
}
