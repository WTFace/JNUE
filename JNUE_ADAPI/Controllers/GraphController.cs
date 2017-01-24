using JNUE_ADAPI.AD;
using JNUE_ADAPI.Helper;
using JNUE_ADAPI.Models;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Mvc.Filters;


namespace JNUE_JNUE_ADAPI.Controllers
{
    [Authorize]
    public class GraphController : ApiController
    {
        public GraphController()
        {
        }
        /// <returns></returns>
        [HttpGet]
        [Route("api/getDN")]

        public IEnumerable<Messages> getDistinguishName(string secure_key, string userid)
        {
            string res = "NONE";
            string res_val = "";

            if (LocalAD.SecureKey(secure_key) == true)
            {
                string distinguishName = LocalAD.AttributeValuesSingleString("distinguishedName", userid);

                if (distinguishName.Equals("NONE") == false)
                {
                    res = "_AD_SUCCESS_";
                    res_val = distinguishName;
                }
                else
                {
                    res = "_AD_FAIL_";
                    res_val = "";
                }
            }
            else
            {
                res = "_AD_FAIL_";
                res_val = "Site Secure Key Invalid";

            }

            Messages[] msg = new Messages[]
            {
                    new Messages
                    {
                        res_code = res,
                        res_mesg = res_val
                    }
            };
            return msg;
        }
        
        [HttpGet]
        [Route("api/getUserInfo")]
        public IEnumerable<User> getUserInfoById(string secure_key, string userid)
        {
            if (LocalAD.SecureKey(secure_key) == true)
            {
                return LocalAD.getLocalUserById(userid);
            }
            else
            {
                return null;
            }
        }

        
        //[HttpGet]
        //[Route("api/createAccount")]
        //public IEnumerable<Messages> setUser(string secure_key, string userid, string passwd, string displayname)
        //{
        //    string res = "NONE";
        //    string res_val = "";

        //    if (LocalAD.SecureKey(secure_key) == true)
        //    {
        //        string cua = LocalAD.CreateUserAccount(userid, passwd, displayname, );

        //        if (cua.Equals("NONE") == false)
        //        {
        //            res = "_AD_SUCCESS_";
        //            res_val = cua;
        //        }
        //        else
        //        {
        //            res = "_AD_FAIL_";
        //            res_val = "";
        //        }

        //    }
        //    else
        //    {
        //        res = "_AD_FAIL_";
        //        res_val = "Site Secure Key Invalid";
        //    }

        //    Messages[] msg = new Messages[]
        //    {
        //        new Messages
        //        {
        //            res_code = res,
        //            res_mesg = res_val
        //        }
        //    };

        //    return msg;
        //}
        
        [HttpGet]
        [Route("api/setPassword")]
        public IEnumerable<Messages> setPassword(string secure_key, string userid, string passwd)
        {

            string res = "NONE";
            string res_val = "";

            if (LocalAD.SecureKey(secure_key) == true)
            {
                string spw = LocalAD.setPassword(userid, passwd);

                if (spw.Equals("NONE") == false)
                {
                    res = "_AD_SUCCESS_";
                    res_val = spw;
                }
                else
                {
                    res = "_AD_FAIL_";
                    res_val = "";
                }
            }
            else
            {
                res = "_AD_FAIL_";
                res_val = "Site Secure Key Invalid";
            }


            Messages[] msg = new Messages[]
            {
                new Messages
                {
                    res_code = res,
                    res_mesg = res_val
                }
            };

            return msg;
        }

        
        [HttpGet]
        [Route("api/setGroup")]
        public IEnumerable<Messages> setGroup(string secure_key, string userid, string groupname)
        {

            string res = "NONE";
            string res_val = "";
            if (LocalAD.SecureKey(secure_key) == true)
            {
                string spw = LocalAD.AddToGroup(userid, groupname);
                if (spw.Equals("NONE") == false)
                {
                    res = "_AD_SUCCESS_";
                    res_val = spw;
                }
                else
                {
                    res = "_AD_FAIL_";
                    res_val = "";
                }
            }
            else
            {
                res = "_AD_FAIL_";
                res_val = "Site Secure Key Invalid";
            }

            Messages[] msg = new Messages[]
            {
                new Messages
                {
                    res_code = res,
                    res_mesg = res_val
                }
            };

            return msg;
        }

        /// <summary>
        /// AD에 그룹에 사용자 삭제 _AD_SUCCESS_ 실패시 _AD_FAIL_
        /// </summary>
        /// <param name="secure_key"></param>
        /// <param name="userid"></param>
        /// <param name="groupname"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/removeGroup")]
        public IEnumerable<Messages> removeGroup(string secure_key, string userid, string groupname)
        {

            string res = "NONE";
            string res_val = "";
            if (LocalAD.SecureKey(secure_key) == true)
            {
                string spw = LocalAD.RemoveUserFromGroup(userid, groupname);

                if (spw.Equals("NONE") == false)
                {
                    res = "_AD_SUCCESS_";
                    res_val = spw;
                }
                else
                {
                    res = "_AD_FAIL_";
                    res_val = "";
                }
            }
            else
            {
                res = "_AD_FAIL_";
                res_val = "Site Secure Key Invalid";
            }

            Messages[] msg = new Messages[]
            {
                new Messages
                {
                    res_code = res,
                    res_mesg = res_val
                }
            };

            return msg;
        }

        /// <summary>
        /// AD에 사용자 삭제 _AD_SUCCESS_ 실패시 _AD_FAIL_
        /// </summary>
        /// <param name="secure_key"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/removeUser")]
        public IEnumerable<Messages> removeUser(string secure_key, string userid)
        {

            string res = "NONE";
            string res_val = "";
            if (LocalAD.SecureKey(secure_key) == true)
            {
                string spw = LocalAD.DeleteUserAccount(userid);

                if (spw.Equals("NONE") == false)
                {
                    res = "_AD_SUCCESS_";
                    res_val = spw;
                }
                else
                {
                    res = "_AD_FAIL_";
                    res_val = "";
                }
            }
            else
            {
                res = "_AD_FAIL_";
                res_val = "Site Secure Key Invalid";
            }

            Messages[] msg = new Messages[]
            {
                new Messages
                {
                    res_code = res,
                    res_mesg = res_val
                }
            };

            return msg;
        }


        /// <summary>
        /// AD에 EXCHANGE 메일 숨기기 _AD_SUCCESS_ 실패시 _AD_FAIL_
        /// </summary>
        /// <param name="secure_key"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/hideExchageUser")]
        public IEnumerable<Messages> hideExchageUser(string secure_key, string userid)
        {

            string res = "NONE";
            string res_val = "";
            if (LocalAD.SecureKey(secure_key) == true)
            {
                string spw = LocalAD.UpdateUserExchage(userid);

                if (spw.Equals("NONE") == false)
                {
                    res = "_AD_SUCCESS_";
                    res_val = spw;
                }
                else
                {
                    res = "_AD_FAIL_";
                    res_val = "";
                }
            }
            else
            {
                res = "_AD_FAIL_";
                res_val = "Site Secure Key Invalid";
            }

            Messages[] msg = new Messages[]
            {
                new Messages
                {
                    res_code = res,
                    res_mesg = res_val
                }
            };

            return msg;
        }


        /// <summary>
        /// AD에 사용자 정보수정 _AD_SUCCESS_ 실패시 _AD_FAIL_
        /// </summary>
        /// <param name="secure_key"></param>
        /// <param name="userid"></param>
        /// <param name="displayname"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/updateUser")]
        public IEnumerable<Messages> updateUser(string secure_key, string userid, string displayname)
        {

            string res = "NONE";
            string res_val = "";


            if (LocalAD.SecureKey(secure_key) == true)
            {
                string spw = LocalAD.UpdateUserAccount(userid, displayname);

                if (spw.Equals("NONE") == false)
                {
                    res = "_AD_SUCCESS_";
                    res_val = spw;
                }
                else
                {
                    res = "_AD_FAIL_";
                    res_val = "";
                }
            }
            else
            {
                res = "_AD_FAIL_";
                res_val = "Site Secure Key Invalid";
            }

            Messages[] msg = new Messages[]
            {
                new Messages
                {
                    res_code = res,
                    res_mesg = res_val
                }
            };

            return msg;
        }

        /// <summary>
        /// AZURE에 사용자 정보 성공시 JSON 실패시 실패시 null 
        /// </summary>
        /// <param name="secure_key"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("graph/user")]
        public async Task<string> getUser(string secure_key, string userid)
        {
            if (LocalAD.SecureKey(secure_key) == true)
            {
                return await AzureAD.getUser(userid);
            }
            else
            {
                return null;
            }
        }

        
        [HttpGet]
        [Route("graph/setAssignLicense")]
        public async Task<string> setAssignLicense(string secure_key, string userid, string skuid, string disableplans)
        {
            if (LocalAD.SecureKey(secure_key) == true)
            {
                return await AzureAD.setAssignLicense(userid, skuid, disableplans);
            }
            else
            {
                return null;
            }
        }

        
        [HttpGet]
        [Route("graph/delAssignLicense")]
        public async Task<string> delAssignLicense(string secure_key, string userid, string skuid)
        {
            if (LocalAD.SecureKey(secure_key) == true)
            {
                return await AzureAD.delAssignLicense(userid, skuid);
            }
            else
            {
                return null;
            }
        }


        /// <summary>
        /// AZURE에 Office365 토큰 가져오기
        /// </summary>
        /// <param name="secure_key"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("graph/getToken")]
        public async Task<string> getOfficeToken(string secure_key)
        {
            if (LocalAD.SecureKey(secure_key) == true)
            {
                return await oAuth.Authorize();
            }
            else
            {
                return null;
            }


        }

        /// <summary>
        /// AZURE에 사용자 Location 추가
        /// </summary>
        /// <param name="secure_key"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("graph/setLocation")]
        public async Task<string> setLocation(string secure_key, string userid)
        {
            if (LocalAD.SecureKey(secure_key) == true)
            {
                return await AzureAD.setUsageLocation(userid);
            }
            else
            {
                return null;
            }
        }


        //        [HttpGet]
        //        [Route("office/sendMail")]
        //        public async Task<string> sendMail(string secure_key, string userid,  string dearMail,  string subject, string context, string textype)
        //        {
        //            if (LocalAD.SecureKey(secure_key) == true)
        //            {
        //                return await Office.sendMail(userid, dearMail, subject, context, textype);
        //            }
        //            else
        //            {
        //                return null;
        //            }
        //        }



        //        [HttpGet]
        //        [Route("office/getMailBox")]
        //        public async Task<string> getMailBox(string secure_key, string userid, string mailbox, string mailcount)
        //        {
        //            var res = "";
        //            if (LocalAD.SecureKey(secure_key) == true)
        //            {
        //                if(mailcount.Equals("Y"))
        //                {
        //                    res = await Office.MailBox(userid, mailbox, mailcount);
        //                    res = res.Replace("\"", "");
        //                } else
        //                {
        //                    res = await Office.MailBox(userid, mailbox, mailcount);
        //                }

        //                return res;
        //            }
        //            else
        //            {
        //                return null;
        //            }
        //        }

        //        [HttpGet]
        //        [Route("office/getMailBoxJSONP")]
        //        public async Task<HttpResponseMessage> getMailBoxJSONP(string secure_key, string userid, string mailbox, string callback)
        //        {

        //            var mailctx = "";
        //            if (LocalAD.SecureKey(secure_key) == true)
        //            {

        //                mailctx = await Office.MailBox(userid, mailbox, "Y");
        //                mailctx = callback + "({\"mailcount\":\"" + mailctx + "\"})";

        //                var response = new HttpResponseMessage(HttpStatusCode.OK);
        //                response.Content = new StringContent(mailctx);
        //                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

        //                return response;
        //            }
        //            else
        //            {
        //                return null;
        //            }
        //        }


        //        [HttpGet]
        //        [Route("office/getContacts")]
        //        public async Task<string> getContacts(string secure_key, string userid)
        //        {
        //            if (LocalAD.SecureKey(secure_key) == true)
        //            {

        //                return await Office.Contacts(userid);
        //            }
        //            else
        //            {
        //                return null;
        //            }
        //        }
        //    }
    }
}