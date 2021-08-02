using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using StudentApi.DBContext;
using StudentApi.Services.Interface;
using StudentApi.StudentModel;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;


namespace StudentApi.Services.Authentication
{
    public static class UserRoles
    {
        public const string Admin = "Admin";
        public const string User = "User";
    }
    public class StudentInterface : IStudentInterface
    {
        private IConfiguration _config;
        public readonly DataBaseContext _studentDbContext;


        public StudentInterface(DataBaseContext studentDbContext, IConfiguration config)
        {
            _studentDbContext = studentDbContext;
            _config = config;
        }

        public async Task<List<AllStudentData>> GetAllStudents()
        {
            var Student = await _studentDbContext.Student.ToListAsync();
            return Student;
        }

        public void AddStudent(AllStudentData Student)
        {
            var AddStudent = new AllStudentData
            {
                Standerd = Student.Standerd,
                Phone_Number = Convert.ToInt64(Student.Phone_Number),
                Name = Student.Name,
                Age = Student.Age,
                Email = Student.Email
            };

            _studentDbContext.Student.AddAsync(AddStudent);
            _studentDbContext.SaveChangesAsync();
        }

        public async Task UpdateStudent(AllStudentData studentData, int StudentId)
        {
            try
            {
                var Student = await _studentDbContext.Student.FirstOrDefaultAsync(x => x.StudentId == studentData.StudentId);
                Student.Name = studentData.Name;
                Student.Phone_Number = studentData.Phone_Number;
                Student.Age = studentData.Age;
                Student.Email = studentData.Email;
                Student.Standerd = studentData.Standerd;
                await _studentDbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }

        public async Task<AllStudentData> GetStudent(int Studentid)
        {
            var Student = await _studentDbContext.Student.Where(x => x.StudentId == Studentid).FirstOrDefaultAsync();
            return Student;
        }

        public void DeleteData(int StudentID)
        {
            var Student = _studentDbContext.Student.Where(x => x.StudentId == StudentID).FirstOrDefault();
            _studentDbContext.Student.Remove(Student);
            _studentDbContext.SaveChangesAsync();
        }

        public async Task<JsonResponse> Login(UserModel login)
        {
            JsonResponse Response = new JsonResponse();
            var user = await AuthenticateUser(login);

            if (user != null)
            {
                Response.IsSuccess = true;
                var tokenString = GenerateJSONWebToken(user.Data);
                Response.Data = tokenString;
            }
            else
            {
                Response.IsSuccess = false;
                Response.Message = "User not Found";
            }

            return Response;
        }

        private async Task<JsonResponse> AuthenticateUser(UserModel login)
        {
            UserModel user = null;
            JsonResponse Response = new JsonResponse();
            var Student = await _studentDbContext.Student.Where(x => x.Email == login.EmailAddress).FirstOrDefaultAsync();
            if (Student != null)
            {
                if (Student.Password == login.Password)
                {
                    Response.IsSuccess = true;
                    user = new UserModel { EmailAddress = Student.Email , Role = Student.Role };
                    Response.Data = user;
                }
                else
                {
                    Response.IsSuccess = false;
                    Response.Message = "Wrong Password!";
                }
            }
            else
            {
                Response.IsSuccess = false;
                Response.Message = "User Not Found!";
            }
            return Response;
        }

        private string GenerateJSONWebToken(UserModel userInfo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[] {
              new Claim(JwtRegisteredClaimNames.Email, userInfo.EmailAddress),
              new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
              new Claim(ClaimTypes.Role, userInfo.Role)
            };

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Issuer"],
                claims,
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
