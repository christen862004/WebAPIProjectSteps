using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace WebAPIProject.Dto
{
    public class LoginUserDto
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
