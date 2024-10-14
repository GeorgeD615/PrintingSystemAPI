using Microsoft.AspNetCore.Mvc;
using PrintingSystem.Db.Interfaces;
using PrintingSystemAPI.Models;

namespace PrintingSystemAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeRepository employeeRepository;

        public EmployeesController(IEmployeeRepository employeeRepository)
        {
            this.employeeRepository = employeeRepository;
        }

        /// <summary>
        /// Получает список всех сотрудников.
        /// </summary>
        /// <returns>Список сотрудников.</returns>
        /// <response code="200">Возвращает список сотрудников.</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<EmployeeDTO>>> GetAllAsync()
        {
            var employees = await employeeRepository.GetAllAsync();
            return Ok(employees.Select(e => new EmployeeDTO() { 
                Id = e.Id, 
                Name = e.Name 
            }));
        }
    }
}
