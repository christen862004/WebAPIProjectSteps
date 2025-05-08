using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using WebAPIProject.Models;

namespace WebAPIProject.Controllers
{
    [Route("api/[controller]")] //resourec uniform name 
    [ApiController]//filtter api (binding | validation)
    public class DepartmentController : ControllerBase
    {
        CompanyContext context = new CompanyContext();
        //CRUD ==>Departem
        #region api/Department   GET 
        [HttpGet]
        public IActionResult ShowAll()
        {
            List<Department> departmentList = context.Departments.ToList();
            
            return Ok(departmentList);//reponse statis code 200 ,body 
        }

        //api/Department/1   GET 
        [HttpGet("{did:int}")]//3 sigment
        public IActionResult FindbyID(int did)
        {
            Department dept = context.Departments.FirstOrDefault(d => d.Id == did);
            if (dept != null)
                return Ok(dept);
            else
                return BadRequest("Invalid id");
        }
       
        
        [HttpGet("{name:alpha}")]//api/department/ahmed
        public IActionResult GetByName(string name)
        {
            Department dept = context.Departments.FirstOrDefault(d => d.Name == name);
            if (dept != null)
                return Ok(dept);
            else
                return BadRequest("Invalid name");
        }
        #endregion

        #region api/Department   POST
        [HttpPost]
        public IActionResult Add(Department deptFromReq)
        {
            if (ModelState.IsValid)
            {
                context.Departments.Add(deptFromReq);
                context.SaveChanges();
                //return Created($"http://localhost:52276/api/department/{deptFromReq.Id}",deptFromReq);
                return CreatedAtAction("FindbyID", new { did = deptFromReq.Id }, deptFromReq);
            }
            return BadRequest(ModelState);
        }
        #endregion

        #region api/Department/1   PUT
        [HttpPut("{id:int}")]
        public IActionResult Edit(int id,Department deptFromRequest) {
            //primitiver  (URL ) routeValues | Quersting
            //Complex      RequestBody
            if (ModelState.IsValid)
            {
                Department deptFromDb = context.Departments.FirstOrDefault(d => d.Id == id);
                deptFromDb.Name = deptFromRequest.Name;
                deptFromDb.ManagerName = deptFromRequest.ManagerName;
                context.SaveChanges();
                // return Ok("Update Success");
                return NoContent();
            }
            return BadRequest(ModelState);
        }
        #endregion

        //api/Department/1   Delete
        //[HttpDelete("{id:int}")]
        //public IActionResult Remove(int id)
        //{
        //    //500
        //    Department deptFromDb = context.Departments.FirstOrDefault(d => d.Id == id);
        //    context.Departments.Remove(deptFromDb);
        //    context.SaveChanges();
        //    //return Ok("Delete Success");
        //    return NoContent();
        //}
    }
}
