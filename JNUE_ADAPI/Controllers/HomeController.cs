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
            AzureAD.getToken();
            return View();}

        [AcceptVerbs( HttpVerbs.Post | HttpVerbs.Patch)] //이안에서는 token 안받아짐
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
                                
                                if (AzureAD.getUser(upn).Result.Equals("False"))
                                {
                                    TempData["false"] = "가입은 되었으나 아직 계정이 생성되지 않았습니다.\n계정이 생성되면 로그인 화면으로 갈 수 있습니다.";
                                    return RedirectToAction("Index", "Home");}

                                if (haksa[0].status.ToString() != LocalAD.getSingleAttr("description", model.Stnt_Numb.ToString())) //학적변동
                                {
                                    LocalAD.UpdateStatus(model.Stnt_Numb.ToString(), haksa[0].status.ToString());
                                    if (haksa[0].status == 2){
                                        TempData["status"] = "학적 상태가 '휴학'으로 변경되었습니다.";
                                    }
                                    else if (haksa[0].status == 1){
                                        TempData["status"] = "학적 상태가 '재학'으로 변경되었습니다.";
                                    }
                                    else { TempData["status"] = "학적 상태가 '졸업/퇴직'으로 변경되었습니다."; }
                                    
                                    //License();
                                    AzureAD.setUsageLocation(upn);
                                    if (LocalAD.getSingleAttr("employeeType", model.Stnt_Numb.ToString()) == "student")
                                    {
                                        if (haksa[0].status == 1){ //재
                                            var res = AzureAD.setLicense(upn, Properties.StuLicense, Properties.PlusLicense, Properties.disables);
                                        }
                                        else if (haksa[0].status == 2){ //휴
                                            var res = AzureAD.setLicense(upn, Properties.StuLicense, "", "");
                                            AzureAD.removeLicense(upn, Properties.PlusLicense);
                                        }
                                        else{ //졸
                                            AzureAD.removeLicense(upn, "\""+Properties.StuLicense+"\""+","+"\""+Properties.PlusLicense+"\"");
                                        }
                                    }
                                    else if (LocalAD.getSingleAttr("employeeType", model.Stnt_Numb.ToString()) == "faculty")
                                    {
                                        if (haksa[0].status == 0){ //퇴직
                                            AzureAD.removeLicense(upn, "\""+Properties.FacLicense+"\"");
                                        }
                                        else{
                                            var res = AzureAD.setLicense(upn, Properties.FacLicense, "", ""); //재직
                                        }
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
                    TempData["false"] = "가입은 되었으나 아직 계정이 생성되지 않았습니다.\n계정이 생성되면 로그인 화면으로 갈 수 있습니다.";
                    return RedirectToAction("Index", "Home");
                }    
                ModelState.AddModelError("", "사용자를 추가할 수 없습니다.\n관리자에게 문의하여 주시기 바랍니다.");
            }
            return View(model);
        }
    }
}
