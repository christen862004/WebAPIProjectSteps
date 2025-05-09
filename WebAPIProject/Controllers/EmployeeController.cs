using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPIProject.Dto;
using WebAPIProject.Models;

namespace WebAPIProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        CompanyContext context=new CompanyContext();
        [HttpGet("{id:int}")]
        public IActionResult Details(int id)
        {
            Employee emp= context.Employees.Include(e=>e.Department)
                .FirstOrDefault(e => e.Id == id);
            //create DTO
            EmployeeWithDepartmentDTO empDTO=new EmployeeWithDepartmentDTO();
            //mapping
            empDTO.EmpId = emp.Id;
            empDTO.EmpName = emp.Name;
            empDTO.DeptName = emp.Department.Name;//null ref exception

            //retsurn to repsosnse
            return Ok(empDTO);
        }
    }
}
