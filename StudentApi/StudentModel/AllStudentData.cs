using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StudentApi.StudentModel
{
    public class AllStudentData
    {
        [Key]
        public int StudentId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Standerd { get; set; }
        public int Age { get; set; }
        public Int64 Phone_Number { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }

    public class UserModel
    {
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }

    public class JsonResponse
    {
        public bool IsSuccess { get; set; } = false;
        public string Message { get; set; }
        public dynamic Data { get; set; }
    }

    public static class Role
    {
        public const string Admin = "Admin";
        public const string User = "User";
    }
}
