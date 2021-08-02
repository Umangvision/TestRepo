using StudentApi.DBContext;
using StudentApi.StudentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentApi.Services.Interface
{
    public interface IStudentInterface
    {
        public Task<List<AllStudentData>> GetAllStudents();
        public void AddStudent(AllStudentData Student);
        public Task UpdateStudent(AllStudentData Student, int StudentId);
        public Task<AllStudentData> GetStudent(int StudentId);
        public void DeleteData(int StudentId);

        public Task<JsonResponse> Login(UserModel model);
    }
}
