using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPIProject.Models;

namespace WebAPIProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BindController : ControllerBase
    {
        [HttpGet("{age:int}/{name}")]//routeValue  /api/bind/12/ahmed
                                     //queryString /api/bind?name=ahmed&age=12
        public IActionResult testPrimitve(int age, string name)
        {
            return Ok();
        }

        [HttpPut("{id:int}")]//api/bind/1
        public IActionResult Edit(int id, Department dept)
        {
            return Ok();
        }

        //customize binding body ==> URL  (Change DEfault)
        [HttpGet]//api/bind?Latit=24143&Long=335225
        //api/bind/2424/232
        public IActionResult GetLocation([FromQuery]Location loc)//body
        {
            return Ok("Place");
        }

        [HttpPost]
        public IActionResult Add(int id,[FromBody]string name)//primitve body
        {
            return Ok();
        }

    }
}
