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
    public class MemberJoinController : Controller
    {
        #region Private Fields
        readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        static StntNumbCheckViewModel _StntNumbModel = new StntNumbCheckViewModel();
        #endregion
        // Get: 학번/교번 체크
        public ActionResult CheckStntNumb()
        {
            return View();
        }

        // POST: /MemberJoin/CheckStntNumb
        [HttpPost]
        public ActionResult CheckStntNumb(StntNumbCheckViewModel model)
        {
            if (ModelState.IsValid)
            {
                var pass_word = string.Format("J{0:D4}{1:D2}{2:D2}#", model.Year, model.Month, model.Day);
                logger.Debug(string.Format("pass_word: {0}", pass_word));

                using (var haksaContext = new HaksaContext())
                {
                    try
                    {
                        var haksa = haksaContext.HaksaMembers.Where(m => m.stnt_numb == model.Stnt_Numb &&  m.stnt_knam== model.realName && m.pass_word == pass_word).ToList();

                        if (haksa.Count == 1) // 조회결과가 1개이고
                        {
                            if (haksa[0].user_used == "N") // 비활성화된 계정
                            {
                                ModelState.AddModelError("", "입력하신 학번은 현재 사용중이지 않습니다.\n관리자에게 문의하여 주시기 바랍니다.");
                            }
                            else if (LocalAD.ExistAttributeValue("extensionAttribute1", model.Stnt_Numb.ToString()) == true)
                            {
                                // AD에 있는 학번이면 로그인 화면으로 리디렉션

                                TempData["id"] = LocalAD.getUserId(model.Stnt_Numb.ToString());
                                return RedirectToAction("Alert","MemberJoin");
                                //return Redirect(Properties.ADFS_URL); //RedirectToAction("Login","MemberJoin")
                            }
                            else
                            {                                    
                                _StntNumbModel = model;
                                // 없으면 회원가입페이지로 리디렉션
                                return RedirectToAction("RegisterJnueO365", "MemberJoin");
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

        
        // GET: MemberJoin
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
                
                string cua = LocalAD.CreateUserAccount(model.ID, model.Password, _StntNumbModel.realName, _StntNumbModel.Stnt_Numb.ToString());
                
                if (cua != "NONE")
                {
                    return Redirect(Properties.ADFS_URL);
                }
                ModelState.AddModelError("", "사용자를 추가할 수 없습니다.\n관리자에게 문의하여 주시기 바랍니다.");
            }

            //AzureAD.setAssignLicense(model.ID+"@"+Properties.AzDomainUrl, "e82ae690-a2d5-4d76-8d30-7c6e01e6022e", "NONE");
            // 이 경우 오류가 발생한 것이므로 폼을 다시 표시하십시오.
            return View(model);
        }
    }
}