using JNUE_ADAPI.DAL;
using JNUE_ADAPI.Models;
using System.Web.Mvc;
using System.Linq;
using System;
using JNUE_ADAPI.AD;
using log4net;
using System.Reflection;

namespace JNUE_ADAPI.Controllers
{
    public class HomeController : Controller
    {
        #region Private Fields
        readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        static StntNumbCheckViewModel _StntNumbModel = new StntNumbCheckViewModel();
        #endregion
        
        [HttpGet]
        public ActionResult Index()
        {
            AzureAD.setUsageLocation("112@hddemo.co.kr");
            AzureAD.setLicense("112@hddemo.co.kr", Properties.StuLicense,"", "");
            return View();}

        [HttpPost]
        public ActionResult Index(StntNumbCheckViewModel model)
        {
            if (ModelState.IsValid)
            {
                using (var haksaContext = new HaksaContext())
                {
                    try{
                        var haksa = haksaContext.HaksaMembers.Where(m => m.stnt_numb == model.Stnt_Numb).ToList();

                        if (haksa.Count == 1) // 조회결과가 1개이고
                        {
                            if (haksa[0].user_used == "N") // 비활성화된 계정
                            {ModelState.AddModelError("", "입력하신 학번은 현재 사용중이지 않습니다.\n관리자에게 문의하여 주시기 바랍니다.");}
                            else if (LocalAD.ExistAttributeValue("extensionAttribute1", model.Stnt_Numb.ToString()) == true)
                            {
                                string upn = LocalAD.getSingleAttr("userPrincipalName", model.Stnt_Numb.ToString()); //@hddemo 포함
                                TempData["upn"] = upn; //login시 id 넘겨줄 용도

                                if (haksa[0].status.ToString() != LocalAD.getSingleAttr("description", model.Stnt_Numb.ToString())) //학적변동
                                {
                                    LocalAD.UpdateStatus(model.Stnt_Numb.ToString(), haksa[0].status.ToString());
                                    TempData["status"] = "학적 상태가 변동되었습니다.";
                                    //License();
                                }

                                AzureAD.setUsageLocation(upn);
                                if (LocalAD.getSingleAttr("employeeType", model.Stnt_Numb.ToString()) == "student")
                                {
                                    if (haksa[0].status==1){ //재
                                        AzureAD.setLicense(upn, "", Properties.PlusLicense, "\"a23b959c-7ce8-4e57-9140-b90eb88a9e97\",\"882e1d05-acd1-4ccb-8708-6ee03664b117\",\"2078e8df-cff6-4290-98cb-5408261a760a\"");
                                    }
                                    else if (haksa[0].status == 2){ //휴
                                        var res = AzureAD.setLicense(upn, Properties.StuLicense, Properties.PlusLicense, "");
                                    }
                                    else{ //졸
                                    }
                                }
                                else if (LocalAD.getSingleAttr("employeeType", model.Stnt_Numb.ToString()) == "faculty")
                                {
                                    if (haksa[0].status == 0){ //퇴직
                                        AzureAD.setLicense(upn, "", Properties.FacLicense, "43de0ff5-c92c-492b-9116-175376d08c38");
                                    }
                                    else{
                                        var res = AzureAD.setLicense(upn, Properties.FacLicense,"", ""); //재직
                                    }
                                }
                                return RedirectToAction("Alert", "Home");
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
                            ModelState.AddModelError("", "입력하신 학번이 조회되지 않습니다.\n관리자에게 문의하여 주시기 바랍니다.");
                        }
                        else if (haksa.Count > 1)
                        {
                            ModelState.AddModelError("", "입력하신 학번이 2건이상 조회되었습니다.\n관리자에게 문의하여 주시기 바랍니다.");
                        }
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", "학번 조회에 실패하였습니다.\n관리자에게 문의하여 주시기 바랍니다.");
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
            return View(model);
        }
    }
}