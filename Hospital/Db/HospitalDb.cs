using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using Antlr.Runtime.Misc;
using Hospital.Models;
using MySql.Data.MySqlClient;

namespace Hospital.Db
{
    public class HospitalDb : DbContext
    {
        public HospitalDb(DbConnection connection) : base(connection, false)
        {
            Configuration.ValidateOnSaveEnabled = false;
        }

        public DbSet<Complexity> Complexities { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Disease> Diseases { get; set; }
        public DbSet<DiseaseCategory> DiseaseCategories { get; set; }
        public DbSet<District> Districts { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<PatientAccount> PatientAccounts { get; set; }

        public static IEnumerable<TModel> DeleteEntities<TModel>(
            Func<HospitalDb, DbSet<TModel>> entitiesGetter,
            Func<DbSet<TModel>, IQueryable<TModel>> query)
            where TModel : class
        {
            using (var sqlConnection = new MySqlConnection())
            {
                sqlConnection.ConnectionString =
                    ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                sqlConnection.Open();
                using (var context = new HospitalDb(sqlConnection))
                {
                    var targetEntities = GetEntities(entitiesGetter, query);
                    foreach (var entity in targetEntities)
                    {
                        var set = entitiesGetter(context);
                        set.Attach(entity);
                        context.Entry(entity).State = EntityState.Deleted;
                    }
                    context.SaveChanges();
                    return targetEntities;
                }
            }
        }

        public static IEnumerable<TModel> DeleteEntities<TModel>(
            Func<HospitalDb, DbSet<TModel>> entitiesGetter)
            where TModel : class
        {
            return DeleteEntities(entitiesGetter, set => set);
        }

        public static TModel GetExistsEntityOrAddNew<TModel>(
            Func<HospitalDb, DbSet<TModel>> entitiesGetter,
            Func<DbSet<TModel>, IQueryable<TModel>> query,
            TModel newEntity)
            where TModel : class
        {
            var possibleEntities = GetEntities(entitiesGetter, query);
            if (possibleEntities.Count == 0)
                return AddToDataBase(entitiesGetter, newEntity).First();
            return possibleEntities.First();
        }

        public static List<TModel> GetEntities<TModel>(
            Func<HospitalDb, DbSet<TModel>> entitiesGetter,
            Func<DbSet<TModel>, IQueryable<TModel>> query)
            where TModel : class
        {
            using (var sqlConnection = new MySqlConnection())
            {
                sqlConnection.ConnectionString =
                    ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                sqlConnection.Open();
                using (var context = new HospitalDb(sqlConnection))
                {
                    return query(entitiesGetter(context)).ToList();
                }
            }
        }

        public static List<TModel> GetEntities<TModel>(
            Func<HospitalDb, IQueryable<TModel>> query)
            where TModel : class
        {
            using (var sqlConnection = new MySqlConnection())
            {
                sqlConnection.ConnectionString =
                    ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                sqlConnection.Open();
                using (var context = new HospitalDb(sqlConnection))
                {
                    return query(context).ToList();
                }
            }
        }

        public static List<TModel> GetEntities<TModel>(
            Func<HospitalDb, DbSet<TModel>> entitiesGetter)
            where TModel : class
        {
            return GetEntities(entitiesGetter, set => set);
        }

        public static Patient AddPatientWithProcedure(Patient patient)
        {
            using (var sqlConnection = new MySqlConnection())
            {
                sqlConnection.ConnectionString =
                    ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                sqlConnection.Open();
                var command = new MySqlCommand($"call addPatient ('{patient.Name}'," +
                                               $"'{patient.Surname}'," +
                                               $"'{patient.SecondName}'," +
                                               $"'{patient.Adress}'," +
                                               $"'{patient.Birth.Value:s}'," +
                                               $"'{patient.DistrictId}')", sqlConnection);
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        var id = reader.GetInt32(0);
                        patient.Id = id;
                        return patient;
                    }
                    return null;
                }
            }
        }

        public static IEnumerable<TModel> AddToDataBase<TModel>(
            Func<HospitalDb, DbSet<TModel>> entitiesGetter,
            params TModel[] entities)
            where TModel : class
        {
            using (var sqlConnection = new MySqlConnection())
            {
                sqlConnection.ConnectionString =
                    ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                sqlConnection.Open();
                using (var context = new HospitalDb(sqlConnection))
                {
                    var result = entitiesGetter(context).AddRange(entities);
                    context.SaveChanges();
                    return result;
                }
            }
        }

        public static void AddOrUpdate<TModel>(
            Func<HospitalDb, DbSet<TModel>> entitiesGetter,
            params TModel[] entities)
            where TModel : class
        {
            using (var sqlConnection = new MySqlConnection())
            {
                sqlConnection.ConnectionString =
                    ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                sqlConnection.Open();
                using (var context = new HospitalDb(sqlConnection))
                {
                    entitiesGetter(context).AddOrUpdate(entities);
                    context.SaveChanges();
                }
            }
        }

        public static int Count<TModel>(
            Func<HospitalDb, DbSet<TModel>> entitiesGetter,
            Func<DbSet<TModel>, IQueryable<TModel>> query)
            where TModel : class
        {
            using (var sqlConnection = new MySqlConnection())
            {
                sqlConnection.ConnectionString =
                    ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                sqlConnection.Open();
                using (var context = new HospitalDb(sqlConnection))
                {
                    return query(entitiesGetter(context)).Count();
                }
            }
        }

        public static int Count<TModel>(
            Func<HospitalDb, DbSet<TModel>> entitiesGetter)
            where TModel : class
        {
            return Count(entitiesGetter, set => set);
        }
    }
}