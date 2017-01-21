using database.Context;
using database.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;
using System;

namespace Database1.Controllers
{
    public class CustomerController : Controller
    {
        public ActionResult Index()
        {
            IList<Customer> customers = GetAllCustomers();
            return View(customers);
        }
        public IList<Customer> GetAllCustomers()
        {
            return SimpleContext.GetEntities(x => x.Customers.Include(l => l.Projects));
        }

        [HttpGet]
        public ActionResult AddCustomer(string name, string surname, int projId)
        {
            var customer = new Customer
            {
                customerName = name,
                customerSurname = surname
            };
            var newCustomer = SimpleContext.AddCustomerWithProcedure(customer, projId);
            return View("Index", GetAllCustomers());
        }
        [HttpGet]
        public ActionResult Add()
        {
            var proj = SimpleContext.GetEntities(x => x.Projects);
            return View("Add", proj);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            SimpleContext.DeleteFromTable(x => x.Customers, set => set.Where(s => s.id == id));
            var custom = GetAllCustomers();
            return View("Index", custom);
        }
        [HttpGet]
        public ActionResult ChangeCustomer(int id, string name, string surname, int projId)
        {
            var customers = SimpleContext.GetEntities(x => x.Customers, s => s.Where(x => x.id == id))[0];
            customers.customerName = name;
            customers.customerSurname = surname;
            SimpleContext.ChangeRelated(projId, id);
            SimpleContext.ChangeTable(x => x.Customers, customers);
            var custom = GetAllCustomers();
            return View("Index", custom);
        }
        [HttpGet]
        public ActionResult Change(int id)
        {
            var customers = SimpleContext.GetEntities(x => x.Customers, set => set.Where(x => x.id == id))[0];
            var proj = SimpleContext.GetEntities(x => x.Projects);
            return View("Change", Tuple.Create(customers, proj));
        }
        
    }
}