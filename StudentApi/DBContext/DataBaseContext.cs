using Microsoft.EntityFrameworkCore;
using StudentApi.StudentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentApi.DBContext
{
    public class DataBaseContext : DbContext
    {
        public DataBaseContext(DbContextOptions<DataBaseContext> options)
      : base(options)
        { }

        public DbSet<AllStudentData> Student { get; set; }
    }
}
