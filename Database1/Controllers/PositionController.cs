using database.Context;
using database.Models;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;

namespace Database1.Controllers
{
    public class PositionController : Controller
    {
        public ActionResult Index()
        {
            IList<Position> position = GetAllPositions();
            return View(position);
        }
        public IList<Position> GetAllPositions()
        {
            return SimpleContext.GetEntities(x => x.Position);
        }
         public ActionResult Add()
        {
            return View();
        }
    }
}