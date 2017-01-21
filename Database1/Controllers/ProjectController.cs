using database.Context;
using database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;

namespace Database1.Controllers
{
    public class ProjectController : Controller
    {
        public ActionResult Index()
        {
            IList<Projects> projects = GetAllProjects();
            return View(projects);
        }

        public IList<Projects> GetAllProjects()
        {
            return SimpleContext.GetEntities(x => x.Projects.Include(s => s.Departments));
        }

        [HttpGet]
        public ActionResult AddProject(string name, DateTime? begin, DateTime? end, DateTime? realEnd, int? c)
        {
            var project = new Projects
            {
                ProjectName = name,
                beginDate = begin,
                endDate = end,
                realEndDate = realEnd,
                cost = c
            };
            var newProject = SimpleContext.AddProjectWithProcedure(project);
            return View("Index", GetAllProjects());
        }
        [HttpGet]
        public ActionResult Add()
        {
            return View("Add");
        }

        public ActionResult Delete(int id)
        {
            var partisipation = SimpleContext.GetEntities(x => x.Participation, set => set.Where(x => x.projectsId == id));
            foreach (var s in partisipation)
            {
                SimpleContext.DeleteFromTable(x => x.Participation, set => set.Where(x => x.projectsId == s.projectsId));
            }

            var level = SimpleContext.GetEntities(x => x.Levels, set => set.Where(x => x.projectId == id));
            foreach(var s in level)
            {
                SimpleContext.DeleteFromTable(x => x.Levels, set => set.Where(x => x.projectId == s.projectId));
            }

            SimpleContext.DeleteFromTable(x => x.Projects, set => set.Where(s => s.id == id));
            var project = GetAllProjects();
            return View("Index", project);
        }

        [HttpGet]
        public ActionResult ChangeProject(int id, string projName, DateTime? bgn, DateTime? end, DateTime? rend, int? cost)
        {
            var proj = SimpleContext.GetEntities(x => x.Projects, s => s.Where(x => x.id == id))[0];
            proj.ProjectName = projName;
            proj.beginDate = bgn;
            proj.endDate = end;
            proj.realEndDate = rend;
            proj.cost = cost;
            SimpleContext.ChangeTable(x => x.Projects, proj);
            var project = GetAllProjects();
            return View("Index", project);
        }

        [HttpGet]
        public ActionResult Change(int id)
        {
            var project = SimpleContext.GetEntities(x => x.Projects, s => s.Where(x => x.id == id));
            return View("Change", project[0]);
        }

    }
}