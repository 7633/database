using database.Context;
using database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;

namespace Database1.Controllers
{
    public class StudentsController : Controller
    {
        public ActionResult Index()
        {
             //var student = SimpleContext.GetEntities(x => x.Student.Include(stud => stud.Participation));
            //var project = students.Select(x => SimpleContext.GetEntities(a => a.Projects.Where(e => e.id == x.projectId).FirstOrDefault());
            var students = GetAllStudents();
            return View(students);   
        }
        public IList<Student> GetAllStudents()
        {
            return SimpleContext.GetEntities(x => x.Student);
        }

        [HttpGet]
        public ActionResult AddStudent(string names,string surnames, int deptId, int posId)
        {
           var student = new Student
            {
               name = names,
               surname = surnames,
               departmentsId = deptId,
               positionId = posId
              
            };
            var newProject = SimpleContext.AddStudentWithProcedure(student);
            return View("Index", GetAllStudents());
        }
        [HttpGet]
        public ActionResult Add()
        {
            IList<Position> pos = SimpleContext.GetEntities(x => x.Position);
            IList<Departments> dept = SimpleContext.GetEntities(x => x.Departments);
            var student = SimpleContext.GetEntities(x => x.Student);
            return View("Add", Tuple.Create(student, dept, pos));
        }

        [HttpGet]
        public ActionResult ChangeStudent(int id, string names, string surnames, int deptId, int posId)
        {
            var student = SimpleContext.GetEntities(x => x.Student, s => s.Where(x => x.id == id))[0];
            student.name = names;
            student.surname = surnames;
            student.departmentsId = deptId;
            student.positionId = posId;
            SimpleContext.ChangeTable(x=> x.Student, student);
            var stud = GetAllStudents();
            return View("Index", stud);
        }

        [HttpGet]
        public ActionResult Change(int id)
        {
            IList<Position> pos = SimpleContext.GetEntities(x => x.Position);
            IList<Departments> dept = SimpleContext.GetEntities(x => x.Departments);
            var student = SimpleContext.GetEntities(x => x.Student, s => s.Where(x => x.id == id))[0];
           
            return View("Change", Tuple.Create(student, dept, pos));
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var stud = SimpleContext.GetEntities(x => x.Participation, e => e.Where(x => x.studentsId == id));
            foreach(var e in stud)
            {
                SimpleContext.DeleteFromTable(x => x.Participation, set => set.Where(x => x.studentsId == e.studentsId));
            }
            SimpleContext.DeleteFromTable(x => x.Student, set => set.Where(s => s.id == id));
            var student = GetAllStudents();
            return View("Index", student);
        }
    }
}