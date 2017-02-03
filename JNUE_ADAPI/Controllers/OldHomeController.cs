//using System.Web.Mvc;
//using JNUE_ADAPI.AD;
//using System.Threading.Tasks;
//using System.Diagnostics;

//namespace JNUE_ADAPI.Controllers
//{
//    public class Home-Controller : Controller
//    {
//        public ActionResult Index()
//    {
//        var res = AzureAD.setUsageLocation("honghong@hddemo.co.kr"); //한명만 위치할당해도 나머지 라이선스 할당됨
//        AzureAD.setLicense("nkh@hddemo.co.kr", Models.Properties.facLicense, "");
//        AzureAD.setLicense("honghong@hddemo.co.kr", Models.Properties.stuLicense, "");

//        //AzureAD.removeLicense("test2@hddemo.co.kr", "e82ae690-a2d5-4d76-8d30-7c6e01e6022e");
//        ViewBag.Title = "JNUE MS Office365 GateWay";
//        //Debug.WriteLine("Test: " + result);
//        return View();
//    }
//}
//}
