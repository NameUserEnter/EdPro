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

        [HttpGet("JsonData")]
        public JsonResult JsonData()
        {
            var services = _context.Universities.ToList();
            List<object> catOrder = new List<object>();
            catOrder.Add(new[] { "Університет", "Кількість Факультетів" });
            foreach (var s in services)
                catOrder.Add(new object[] { s.Name, _context.Faculties.Where(b => b.UniversityId == s.Id).Count() });
            return new JsonResult(catOrder);
        }

        //[HttpGet("JsonD")]
        //public JsonResult JsonD()
        //{
        //    var employees = _context.Employees.ToList();
        //    List<object> catOrder = new List<object>();
        //    catOrder.Add(new[] { "Перукар", "Кількість замовлення" });
        //    foreach (var e in employees)
        //    {
        //        var orders = _context.Orders.Where(o => o.EmployeeId == e.EmployeeId).ToList();
        //        int count_o = 0;
        //        foreach (var order in orders)
        //        {
        //            count_o += _context.OrdersItems.Where(b => b.OrderId == order.OrderId).Count();
        //        }
        //        catOrder.Add(new object[] { e.LastName, count_o });
        //    }
        //    return new JsonResult(catOrder);
        //}

    }
}
