using Microsoft.AspNetCore.Mvc;
using StudentApi.Services.Interface;
using StudentApi.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StudentApi.StudentModel;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;

namespace StudentApi.Controllers
{
    [Authorize]
    public class StudentController : Controller
    {
        public readonly IStudentInterface _studentService;

        public StudentController(IStudentInterface studentService)
        {
            _studentService = studentService;

        }




        [HttpGet("GetStudent")]
        public async Task<ActionResult> GetStudent()
        {

            var apiResposne = await _studentService.GetAllStudents();

            return new JsonResult(apiResposne);

        }




        [HttpPost("AddStudent")]
        public void AddStudent([FromBody] AllStudentData Student)
        {
            _studentService.AddStudent(Student);
        }




        //UpdateStudent
        //Pass Studentid in int Studentid Perameter 
        [HttpPost("UpdateStudent")]
        public async Task UpdateStudent([FromBody] AllStudentData Student, int Studentid)
        {
            await _studentService.UpdateStudent(Student, Studentid);
        }




        //Get student By id 
        [HttpGet("GetStudentById")]
        public async Task<ActionResult> GetStudent([FromBody] int Studentid)
        {
            var apiResposne = await _studentService.GetStudent(Studentid);
            return new JsonResult(apiResposne);
        }




        ////Delete Student Record
        //[HttpDelete("DeleteStudent")]
        //public ActionResult DeleteStudent([FromBody] int Studentid)
        //{
        //    _studentService.DeleteData(Studentid);
        //    return new JsonResult("Student Removed");
        //}


        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<ActionResult> LoginUser([FromBody] UserModel login)
        {
            var a = await _studentService.Login(login);
            return new JsonResult(a);
        }
    }
}
