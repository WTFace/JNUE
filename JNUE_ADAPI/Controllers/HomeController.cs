using JNUE_ADAPI.DAL;
using JNUE_ADAPI.Models;
using System.Web.Mvc;
using System.Linq;
using System;
using JNUE_ADAPI.AD;
using log4net;
using System.Reflection;
using System.Net.Http;
using System.Text;
using System.Diagnostics;

namespace JNUE_ADAPI.Controllers
{
    public class HomeController : Controller
    {
        #region Private Fields
        readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        static StntNumbCheckViewModel _StntNumbModel = new StntNumbCheckViewModel();
        #endregion
        // Get: 학번/교번 체크
        public ActionResult Index()
        {
            //AzureAD.setUsageLocation("111@hddemo.co.kr");
            //AzureAD.setLicense("111@hddemo.co.kr", Properties.StuLicense, ""); //여기서 call하면 할당됨
            return View();
        }
        
        
        [HttpPost]
        public ActionResult Index(StntNumbCheckViewModel model)
        {
            if (ModelState.IsValid)
            {
                //var pass_word = string.Format("J{0:D4}{1:D2}{2:D2}#", model.Year, model.Month, model.Day);
                //logger.Debug(string.Format("pass_word: {0}", pass_word));
                using (var haksaContext = new HaksaContext())
                {
                    try
                    {
                        var haksa = haksaContext.HaksaMembers.Where(m => m.stnt_numb == model.Stnt_Numb).ToList();

                        if (haksa.Count == 1) // 조회결과가 1개이고
                        {
                            if (haksa[0].user_used == "N") // 비활성화된 계정
                            {
                                ModelState.AddModelError("", "입력하신 학번은 현재 사용중이지 않습니다.\n관리자에게 문의하여 주시기 바랍니다.");
                            }
                            else if (LocalAD.ExistAttributeValue("extensionAttribute1", model.Stnt_Numb.ToString()) == true)
                            {   // AD에 있는 학번이면

                                string uid = LocalAD.getSingleAttr("userPrincipalName", model.Stnt_Numb.ToString()); //@hddemo 포함
                                TempData["id"] = uid; //login시 id 넘겨줄 용도

                                //여기서는 안되고 Index() 내에서는 됨
                                AzureAD.setUsageLocation(uid);
                                if (LocalAD.getSingleAttr("employeeType", model.Stnt_Numb.ToString()) == "student")
                                {
                                    var res = AzureAD.setLicense(uid, Properties.StuLicense, "");
                                }
                                else if (LocalAD.getSingleAttr("employeeType", model.Stnt_Numb.ToString()) == "faculty")
                                {
                                    AzureAD.setLicense(uid, Properties.FacLicense, "");
                                }

                                return RedirectToAction("Alert", "Home");
                                //return Redirect(Properties.ADFS_URL); //RedirectToAction("Login","Home")
                            }
                            else
                            {                                    
                                _StntNumbModel = model;
                                // 없으면 회원가입페이지로 리디렉션
                                return RedirectToAction("RegisterJnueO365", "Home");
                            }
                        }
                        else if (haksa.Count == 0)
                        {
                            // 조회 결과가 없으면
                            ModelState.AddModelError("", "입력하신 학번과 생년월일 값이 조회되지 않습니다.\n관리자에게 문의하여 주시기 바랍니다.");
                        }
                        else if (haksa.Count > 1)
                        {
                            ModelState.AddModelError("", "입력하신 학번과 생년월일 값이 2건이상 조회되었습니다.\n관리자에게 문의하여 주시기 바랍니다.");
                        }
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", "학번 및 생년월일 조회에 실패하였습니다.\n관리자에게 문의하여 주시기 바랍니다.");
                        logger.Debug(ex.ToString());
                    }
                }
            }
            // 이 경우 오류가 발생한 것이므로 폼을 다시 표시
            return View(model);
        }
        
        // GET: Home
        public ActionResult RegisterJnueO365()
        {
            return View();
        }

        public ActionResult Alert()
        {
            return View();
        }

        [HttpPost]
        public ActionResult RegisterJnueO365(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                string cua = LocalAD.CreateUserAccount(model.ID, model.Password, _StntNumbModel.Stnt_Numb.ToString());
                
                if (cua != "NONE")
                {
                    return Redirect("https://adfs.hddemo.co.kr/adfs/ls/?lc=1042&client-request-id=4d5bbac1-8655-464f-acdc-862c63f8729b&username=" + LocalAD.getUserId(_StntNumbModel.Stnt_Numb.ToString()) + "&wa=wsignin1.0&wtrealm=urn%3afederation%3aMicrosoftOnline&wctx=estsredirect%3d2%26estsrequest%3drQIIAeNisFLOKCkpKLbS1y_ILypJzNHLT0vLTE7VS87P1csvSs9MAbGKhLgEDu6K9g1t83dbc6dNJ_lH0exVjGo4dernJOalZOal6yUWF1RcYGTsYmIxNDAx2sTE6uvs6-R5gmnCWblbTIL-RemeKeHFbqkpqUWJJZn5eY-YeEOLU4v883IqQ_KzU_N2MauYmaaZmFgYGeomJiUn6ZqYmJnpWlgkp-iaGKeamhmlJiYnJpkcYNkQcoFF4BULjwGzFQcHlwCDBIMCww8WxkWsQIcHTr9_ZHZxv3Pj4e63y8TZ6k-x6od6V3mZGjmnGmVb5Jik-7oX5WubuCa65mcaJke4h3kY-BYkZkQ5WlalZbjamlsZTmATmsDGtIvTljgP25ckFqWnltiqGqWlpKYlluaUgIUB0&popupui="); 
                }
                ModelState.AddModelError("", "사용자를 추가할 수 없습니다.\n관리자에게 문의하여 주시기 바랍니다.");
            }
            
            // 이 경우 오류가 발생한 것이므로 폼을 다시 표시하십시오.
            return View(model);
        }
    }
}