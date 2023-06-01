using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using EdPro.Models;

namespace EdPro.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "admin,user,worker")]
    public class ChartController : ControllerBase
    {
        private readonly EdProContext _context;

        public ChartController(EdProContext context)
        {
            _context = context;
        }

        [HttpGet("JsonDataUniversity")]
        public JsonResult JsonDataUniversity()
        {
            var universities = _context.Universities.ToList();
            List<object> catOrder = new List<object>();
            catOrder.Add(new[] { "Університет", "Кількість Факультетів" });
            foreach (var s in universities)
                catOrder.Add(new object[] { s.Name, _context.Faculties.Where(b => b.UniversityId == s.Id).Count() });
            return new JsonResult(catOrder);
        }

        [HttpGet("JsonDataFaculty")]
        public JsonResult JsonDataFaculty()
        {
            var faculties = _context.Faculties.ToList();
            List<object> catOrder = new List<object>();
            catOrder.Add(new[] { "Факультет", "Кількість освітніх програм" });
            foreach (var s in faculties)
                catOrder.Add(new object[] { s.Name, _context.EducationPrograms.Where(b => b.FacultyId == s.Id).Count() });
            return new JsonResult(catOrder);
        }

        [HttpGet("JsonDataEdProgramTypes")]
        public JsonResult JsonDataEdProgramTypes()
        {
            var faculties = _context.EdProgramTypes.ToList();
            List<object> catOrder = new List<object>();
            catOrder.Add(new[] { "Тип освітньої програми", "Кількість освітніх програм" });
            foreach (var s in faculties)
                catOrder.Add(new object[] { s.TypeName, _context.EducationPrograms.Where(b => b.EdPrTypeId == s.Id).Count() });
            return new JsonResult(catOrder);
        }

    }
}
