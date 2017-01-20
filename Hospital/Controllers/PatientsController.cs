using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Hospital.Db;
using Hospital.Models;

namespace Hospital.Controllers
{
    public class PatientsController : Controller
    {
        public ActionResult Index()
        {
            var patients = GetAllPatients();
            return View(patients);
        }

        public ActionResult IndexSort(string orderBy, string filter)
        {
            var patients = GetAllPatients();
            if (filter != null)
            {
                ViewBag.Filter = filter;
                filter = filter.ToLower();
                patients = patients.Where(x => x.HasFiltered(filter)).ToList();
            }
            if (orderBy != null)
                switch (orderBy)
                {
                    case "Surname":
                        patients = patients.OrderBy(x => x.Surname).ToList();
                        break;
                    case "Name":
                        patients = patients.OrderBy(x => x.Name).ToList();
                        break;
                    case "Second name":
                        patients = patients.OrderBy(x => x.SecondName).ToList();
                        break;
                    case "Birth":
                        patients = patients.OrderBy(x => x.Birth).ToList();
                        break;
                    case "District":
                        patients = patients.OrderBy(x => x.District.Title).ToList();
                        break;
                    case "Id":
                        patients = patients.OrderBy(x => x.Id).ToList();
                        break;
                }
            return View("Index", patients);
        }

        [HttpGet]
        public ActionResult Add()
        {
            return View(GetAllDistricts());
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            HospitalDb.DeleteEntities(x => x.Patients, set => set.Where(s => s.Id == id));
            var patients = GetAllPatients();
            return View("index", patients);
        }

        [HttpGet]
        public ActionResult GetPatientInfo(int id)
        {
            var targetPatient =
                HospitalDb.GetEntities(x => x.Patients, set => set
                    .Where(p => p.Id == id)
                    .Include(x => x.District)).FirstOrDefault();
            if (targetPatient != null)
            {
                var accounts = HospitalDb.GetEntities(x => x.PatientAccounts, x => x
                    .Where(a => a.PatientId == targetPatient.Id)
                    .Include(a => a.Complexity)
                    .Include(a => a.Disease)
                    .Include(a => a.Department));
                targetPatient.PatientAccount = accounts;
            }
            return targetPatient == null ? NotExist() : View("PatientInfo", targetPatient);
        }

        [HttpGet]
        public ActionResult NotExist()
        {
            return View("NotExistPatient");
        }

        [HttpGet]
        public ActionResult AddNewPatient(
            string name,
            string surname,
            string secondName,
            string adress,
            DateTime? birth,
            int districtId)
        {
            var patient = new Patient
            {
                Name = name,
                Surname = surname,
                SecondName = secondName,
                Birth = birth,
                Adress = adress,
                DistrictId = districtId
            };
            var addedPatient = HospitalDb.AddPatientWithProcedure(patient);
            addedPatient.District =
                HospitalDb.GetEntities(x => x.Districts, x => x.Where(d => d.Id == addedPatient.DistrictId)).First();
            return View("PatientInfo", addedPatient);
        }

        private IList<Patient> GetAllPatients()
        {
            return HospitalDb.GetEntities(x => x.Patients
                .Include(patient => patient.District)
                .Include(patient => patient.PatientAccount));
        }

        private IList<District> GetAllDistricts()
        {
            return HospitalDb.GetEntities(x => x.Districts);
        }
    }
}