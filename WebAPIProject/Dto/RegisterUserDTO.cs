﻿using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace WebAPIProject.Dto
{
    public class RegisterUserDTO
    {
        public string UserName { get; set; }
        
        public string Password { get; set; }
        
        public string? Email { get; set; }
    }
}
