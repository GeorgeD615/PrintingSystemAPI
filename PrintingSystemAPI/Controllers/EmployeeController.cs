using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrintingSystem.Db;
using PrintingSystem.Db.Models;

namespace PrintingSystemAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EmployeeController : Controller
    {
        private readonly PrintingSystemContext context;

        public EmployeeController(PrintingSystemContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees()
        {
            return await context.Employees.ToListAsync();
        }

    }
}
