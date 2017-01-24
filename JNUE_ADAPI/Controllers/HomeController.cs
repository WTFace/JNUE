using System.Web.Mvc;
using JNUE_ADAPI.AD;
using System.Threading.Tasks;
using System.Diagnostics;

namespace JNUE_ADAPI.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //var user = AzureAD.getUser("nkh@hddemo.co.kr");
            //var result = AzureAD.setUsageLocation("test3355@hddemo.co.kr");
            //AzureAD.setAssignLicense("test2@hddemo.co.kr", "e82ae690-a2d5-4d76-8d30-7c6e01e6022e");   //assign할땐 usageLocation

            //AzureAD.delAssignLicense("test2@hddemo.co.kr", "e82ae690-a2d5-4d76-8d30-7c6e01e6022e");
            ViewBag.Title = "JNUE MS Office365 GateWay";
            //Debug.WriteLine("Test: " + result);
            return View();
        }
        
    }
}
