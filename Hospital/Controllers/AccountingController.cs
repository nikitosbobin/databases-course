using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Hospital.Db;
using Hospital.Models;

namespace Hospital.Controllers
{
    public class AccountingController : Controller
    {
        public ActionResult Index()
        {
            var allPatientAccounts = GetAllPatientAccounts().OrderBy(x => x.Id).ToList();
            return View(allPatientAccounts);
        }

        [HttpGet]
        public ActionResult AddNew(int patientId)
        {
            if (HospitalDb.Count(x => x.Patients, x => x.Where(p => p.Id == patientId)) == 0)
            {
                return RedirectToAction("NotExist", "Patients");
            }
            var departments = HospitalDb.GetEntities(x => x.Departments);
            var complexities = HospitalDb.GetEntities(x => x.Complexities);
            var diseases = HospitalDb.GetEntities(x => x.Diseases);
            return View(Tuple.Create(patientId, departments, complexities, diseases));
        }

        [HttpGet]
        public ActionResult AddNewAccount(
            int patientId,
            int departmentId,
            int complexityId,
            int diseaseId)
        {
            var result = new PatientAccount
            {
                ComplexityId = complexityId,
                DateStart = DateTime.Now,
                PatientId = patientId,
                DepartmentId = departmentId,
                DiseaseId = diseaseId
            };
            HospitalDb.AddToDataBase(x => x.PatientAccounts, result);
            return RedirectToAction("GetPatientInfo", "Patients", new { id = patientId });
        }

        [HttpGet]
        public ActionResult Discharge(int accountId)
        {
            var patientAccount = HospitalDb.GetEntities(x => x.PatientAccounts, x => x.Where(a => a.Id == accountId))
                .First();
            patientAccount.DateEnd = DateTime.Now;
            HospitalDb.AddOrUpdate(x => x.PatientAccounts, patientAccount);
            return RedirectToAction("GetPatientInfo", "Patients", new { id = patientAccount.PatientId });
        }

        private IList<PatientAccount> GetAllPatientAccounts()
        {
            return HospitalDb.GetEntities(x => x.PatientAccounts
                .Include(account => account.Disease)
                .Include(account => account.Complexity)
                .Include(account => account.Department)
                .Include(account => account.Patient));
        }

        public ActionResult IndexSort(string orderBy, string filter)
        {
            var allPatientAccounts = GetAllPatientAccounts();
            if (filter != null)
            {
                ViewBag.Filter = filter;
                filter = filter.ToLower();
                allPatientAccounts = allPatientAccounts.Where(x => x.HasFiltered(filter)).ToList();
            }
            if (orderBy != null)
            {
                switch (orderBy)
                {
                    case "Id":
                        allPatientAccounts = allPatientAccounts.OrderBy(x => x.Id).ToList();
                        break;
                    case "Patient id":
                        allPatientAccounts = allPatientAccounts.OrderBy(x => x.PatientId).ToList();
                        break;
                    case "Depatment":
                        allPatientAccounts = allPatientAccounts.OrderBy(x => x.DepartmentId).ToList();
                        break;
                    case "Complexity":
                        allPatientAccounts = allPatientAccounts.OrderBy(x => x.ComplexityId).ToList();
                        break;
                    case "Disease":
                        allPatientAccounts = allPatientAccounts.OrderBy(x => x.DiseaseId).ToList();
                        break;
                    case "DateStart":
                        allPatientAccounts = allPatientAccounts.OrderBy(x => x.DateStart).ToList();
                        break;
                    case "DateEnd":
                        allPatientAccounts = allPatientAccounts.OrderBy(x => x.DateEnd).ToList();
                        break;
                }
            }
            return View("Index", allPatientAccounts);
        }
    }
}