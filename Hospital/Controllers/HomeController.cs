using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using Hospital.Db;
using MySql.Data.MySqlClient;

namespace Hospital.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}