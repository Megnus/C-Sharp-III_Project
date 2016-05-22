using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapdrawingTest
{
    class SchoolEntityFramework
    {
        public SchoolEntityFramework()
        {
            using (var ctx = new SchoolContext())
            {
               /* Student stud = new Student() { StudentName = "Another Student" };
                Teacher te = new Teacher() { TeacherName = "Abbar" };
                Standard st = new Standard() { StandardName = "New standard xxx" };
                te.Standards = new List<Standard>();
                stud.Teachers = new List<Teacher>();
                te.Standards.Add(st);
                stud.Teachers.Add(te);*/

                //ctx.Students.Attach(stud);


                Standard chst = new Standard() { StandardName = "NEW CHANGED SHIT" };
                Teacher temp = ctx.Teachers.First();
                Console.WriteLine(temp.TeacherName);
                //temp.TeacherName = "Missis koko";
                temp.Standards.Add(chst);
                
                //ctx.Entry(temp).State = EntityState.Modified;
               // ctx.Entry(chst).State = EntityState.Added;

                //ctx.Entry(te).State = EntityState.Modified;
                //ctx.Entry(st).State = EntityState.Modified;
                //ctx.Students.Add(stud);
                ctx.SaveChanges();
            }
        }
    }

    public class Student
    {
        public Student()
        {

        }

        public int StudentID { get; set; }
        public string StudentName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public byte[] Photo { get; set; }
        public decimal Height { get; set; }
        public float Weight { get; set; }

        public Standard Standard { get; set; }
        //public Teacher Teacher { get; set; }
        public virtual List<Teacher> Teachers { get; set; }
    }

    public class Teacher
    {
        public Teacher()
        {

        }
        public int TeacherId { get; set; }
        public string TeacherName { get; set; }
        public virtual List<Standard> Standards { get; set; }
    }
  

    public class Standard
    {
        public Standard()
        {

        }

        public int StandardId { get; set; }
        public string StandardName { get; set; }

       // public IList<Student> Students { get; set; }

    }

    public class SchoolContext : DbContext
    {
        public SchoolContext()
            : base()
        {

        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Standard> Standards { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
    }
}
