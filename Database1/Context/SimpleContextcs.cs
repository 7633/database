using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Data.Entity;
using System.Linq;
using Antlr.Runtime.Misc;
using database.Models;
using MySql.Data.MySqlClient;
using System.Data.Entity.Migrations;

namespace database.Context
{
    public class SimpleContext : DbContext
    {
        public SimpleContext() : base("mydb")
        {
            Database.SetInitializer(
                new DropCreateDatabaseIfModelChanges<SimpleContext>());
        }
        public SimpleContext(DbConnection connection) : base(connection, false)
        {
            Configuration.ValidateOnSaveEnabled = false;
        }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Projects> Projects { get; set; }
        public DbSet<Departments> Departments { get; set; }
        public DbSet<Levels> Levels { get; set; }
        public DbSet<Participation> Participation { get; set; }
        public DbSet<Position> Position { get; set; }
        public DbSet<Student> Student { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Projects>()
                .HasMany(x => x.Customers)
                .WithMany(x => x.Projects)
                .Map(x =>
                {
                    x.MapRightKey("customers_id");
                    x.MapLeftKey("projects_id");
                    x.ToTable("customers_has_projects");
                });

             modelBuilder.Entity<Projects>()
             .HasMany(x => x.Departments)
             .WithMany(x => x.Projects)
             .Map(x =>
             {
                 x.MapRightKey("departments_id");
                 x.MapLeftKey("projects_id");
                 x.ToTable("projects_has_departments");
             });  
        }



        public static List<TEntity> GetEntities<TEntity>(
            Func<SimpleContext, IQueryable<TEntity>> entities)
            where TEntity : class
        {
            using (var sqlConnection = new MySqlConnection())
            {
                sqlConnection.ConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                sqlConnection.Open();
                using (var context = new SimpleContext(sqlConnection))
                {
                    return entities(context).ToList();
                }
            }
        }

        public static List<TModel> GetEntities<TModel>(
            Func<SimpleContext, DbSet<TModel>> entities,
            Func<DbSet<TModel>, IQueryable<TModel>> query)
            where TModel : class
        {
            using (var sqlConnection = new MySqlConnection())
            {
                sqlConnection.ConnectionString =
                    ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                sqlConnection.Open();
                using (var context = new SimpleContext(sqlConnection))
                {
                    return query(entities(context)).ToList();
                }
            }
        }

        public static void ChangeRelated (int projId, int custId)
        {
            using (var sqlConnection = new MySqlConnection())
            {
                sqlConnection.ConnectionString =
                ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                sqlConnection.Open();
                using (var delete = new MySqlCommand($"delete from `customers_has_projects` where customers_id={custId}",
                    sqlConnection))
                {
                    delete.ExecuteNonQuery();
                }
                using (var insert = new MySqlCommand($"insert into `customers_has_projects` (projects_id, customers_id) values ({projId},{custId})",
                    sqlConnection))
                {
                    insert.ExecuteNonQuery();
                }
            }

        }


        public static Customer AddCustomerWithProcedure(Customer customer, int ids)
        {
            using (var sqlConnection = new MySqlConnection())
            {
                sqlConnection.ConnectionString =
                ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                sqlConnection.Open();
                var command = new MySqlCommand($"call addCustomer ('{customer.customerName}',"+
                    $"'{ids}'," +
                    $"'{customer.customerSurname}')", sqlConnection);
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        var custId = reader.GetInt32(0);
                        customer.id = custId;
                        return customer;
                    }
                    return null;
                }
            }
        }

        public static Projects AddProjectWithProcedure(Projects project)
        {
            using (var sqlConnection = new MySqlConnection())
            {
                sqlConnection.ConnectionString =
                ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                sqlConnection.Open();
                var command = new MySqlCommand($"call addProject ('{project.ProjectName}',"+
                    (project.beginDate.HasValue ? $"'{project.beginDate.Value.ToString("s")}'," : "null,") +
                    (project.endDate.HasValue ? $"'{project.endDate.Value.ToString("s")}'," : "null,") +
                    (project.realEndDate.HasValue ? $"'{project.realEndDate.Value.ToString("s")}'," : "null,") +
                    (project.cost.HasValue ? $"'{project.cost.Value}')" : "null)" ),
                    sqlConnection);
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        var projId = reader.GetInt32(0);
                        project.id = projId;
                        return project;
                    }
                    return null;
                }
            }
        }

        public static Student AddStudentWithProcedure(Student student)
        {
            using (var sqlConnection = new MySqlConnection())
            {
                sqlConnection.ConnectionString =
                ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                sqlConnection.Open();
                var command = new MySqlCommand($"call addStudent ('{student.name}'," +
                    $"'{student.surname}'," +
                    $"'{student.departmentsId}',"+ 
                    $"'{student.positionId}')", sqlConnection);
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        var studId = reader.GetInt32(0);
                        student.id = studId;
                        return student;
                    }
                    return null;
                }
            }
        }

        public static IEnumerable<TModel> DeleteFromTable<TModel>(
           Func<SimpleContext, DbSet<TModel>> entity,
           Func<DbSet<TModel>, IQueryable<TModel>> query)
           where TModel : class
        {
            using (var sqlConnection = new MySqlConnection())
            {
                sqlConnection.ConnectionString =
                    ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                sqlConnection.Open();
                using (var context = new SimpleContext(sqlConnection))
                {
                    var entities = GetEntities(entity, query);
                    foreach (var e in entities)
                    {
                        var set = entity(context);
                        set.Attach(e);
                        context.Entry(e).State = EntityState.Deleted;
                    }
                    context.SaveChanges();
                    return entities;
                }
            }
        }

        public static void ChangeTable<TModel>(
           Func<SimpleContext, DbSet<TModel>> entities,
           params TModel[] entity)
           where TModel : class
        {
            using (var sqlConnection = new MySqlConnection())
            {
                sqlConnection.ConnectionString =
                    ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                sqlConnection.Open();
                using (var context = new SimpleContext(sqlConnection))
                {
                    entities(context).AddOrUpdate(entity);
                    context.SaveChanges();
                }
            }
        }

    }

}