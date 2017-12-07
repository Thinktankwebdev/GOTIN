using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using ThinkTankJobApp.Models;
using ThinkTankJobApp.Models.ServiceProviders;

namespace ThinkTankJobApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            return View();
        }

        #region Register
        public ActionResult Register()
        {
            RegisteredUser model = new RegisteredUser();
            model.Dob = DateTime.Now;
            return View(model);
        }
        public Response FuncRegister(RegisteredUser User)
        {
             Response res = new Response();
            if(User !=null)
            {
            var user_status = RegistrationProvider.CheckUserAvailability(User.Username);
           
            var flag = false;
            
            if(user_status=="S")
            {
                flag = RegistrationProvider.RegisterUser(User);
                if(flag)
                        {

                            res.IsValid = true;
                            res.ErrorCode = ((int)ErrorProvider.ErrorCode.RegistrationSucces).ToString();
                            res.ErrorMessage = "Registration has been done Successfully";
                            return res;
                        }
                        else
                        {
                            res.IsValid = false;
                            res.ErrorCode = ((int)ErrorProvider.ErrorCode.RegistrationFailed).ToString();
                            res.ErrorMessage = "Registration has been failed";
                            return res;
                        }
                    }
                    else if (user_status == "NA")
                    {

                        res.IsValid = false;
                        res.ErrorCode = ((int)ErrorProvider.ErrorCode.UserInActive).ToString();
                        res.ErrorMessage = "User is Not Active";
                        return res;
                    }
                    else
                    {

                        res.IsValid = false;
                        res.ErrorCode = ((int)ErrorProvider.ErrorCode.UserExists).ToString();
                        res.ErrorMessage = "User Already Exists";
                        return res;
                    }
                }
            res.IsValid = false;
            res.ErrorCode = ((int)ErrorProvider.ErrorCode.UserExists).ToString();
            res.ErrorMessage = "User Already Exists";
            return res;
        }
               
            
        public ActionResult Success()
        {
            return View();
        }
        public ActionResult Registration(RegisteredUser model)
        {
            if (model != null)
            {
                model.Country = "United States";
                model.Mbile_country_code = "+1";
                model.Province = "Massachussets";
                model.Email_address = model.Username;
                model.Registration_ip = ClsFunction.GetVisitorIPAddress();
                var res = FuncRegister(model);
                if (res.ErrorCode == ((int)ErrorProvider.ErrorCode.RegistrationSucces).ToString())
                {
                    TempData["Success"] = res.ErrorMessage;
                    return Json(new { success = true, message = "S|" + res.ErrorMessage }, JsonRequestBehavior.DenyGet);
                }
                else if (res.ErrorCode == ((int)ErrorProvider.ErrorCode.UserInActive).ToString())
                {
                    return Json(new { success = false, message = "U|" + res.ErrorMessage }, JsonRequestBehavior.DenyGet);
                }
                else if (res.ErrorCode == ((int)ErrorProvider.ErrorCode.UserExists).ToString())
                {
                    return Json(new { success = false, message = "UE|" + res.ErrorMessage }, JsonRequestBehavior.DenyGet);
                }
                else
                {
                    return Json(new { success = false, message = "F|" + res.ErrorMessage }, JsonRequestBehavior.DenyGet);
                }
            }
            return Json(new { success = false, message = "F|" + "Something went wrong" }, JsonRequestBehavior.DenyGet);

        }
        #endregion

        #region Confirmation
        public ActionResult Error()
        {
            return View();
        }
        [AllowAnonymous]
        public async Task<ActionResult> Confirmation([Bind(Include = "id")]string id)
        {
            
            string usedon_ip = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            string UsedOnIp = ClsFunction.GetVisitorIPAddress();
            string Token = id;
            if (!string.IsNullOrEmpty(UsedOnIp) && !string.IsNullOrEmpty(Token))
            {
                var result = RegistrationProvider.ActivateRegistration(Token, UsedOnIp);

                if (result == "S")
                {
                    TempData["Success"] = "Registration Activated";
                    return RedirectToAction("Success", "Home");
                }
                else if(result == "LE")
                {
                    TempData["Error"] = "Link is Expired";
                    return RedirectToAction("Error", "Home");
                }
                else
                {
                    TempData["Error"] = "Failed";
                    return RedirectToAction("Error", "Home");
                }
                
            }
            TempData["Error"] = "Sonething went Wrong Please try again!";
            return RedirectToAction("Error", "Home");
        }
        #endregion
        #region Login
        public ActionResult Login()
        {
            if (Request.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                LoginModel model = new LoginModel();
                if (Session["LoginAttempts"] != null)
                {
                    model.FailedCount = (int)Session["LoginAttempts"];
                }
                else
                {
                    model.FailedCount = 0;
                }
                return View(model);

            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login([Bind(Include = "user,word,rememberme")] string user, string word, string rememberme)
        {
            try
            {
                HttpContext.User = null;
                FormsAuthentication.SignOut();
                LoginModel model = new LoginModel();
                model.Username = user;
                model.Password = word;
                model.IpAddress = ClsFunction.GetVisitorIPAddress();
                model.Type = "WEB";
                bool RememberMe = false;
                if (rememberme != null)
                {
                    RememberMe = true;
                }
                else
                {
                    RememberMe = false;
                }
                Response res = new Response();
                res = RegistrationProvider.LoginFunc(model);
                if (res.ErrorCode == ((int)ErrorProvider.ErrorCode.LoginSuccess).ToString())
                {
                    FormsAuthentication.SetAuthCookie(user, RememberMe);
                    Session["LoginAttempts"] = 0;
                    return Json(new { title = "Login", success = true, message = res.ErrorMessage, failcount = Session["LoginAttempts"].ToString() }, JsonRequestBehavior.DenyGet);
                }
                else if (res.ErrorCode == ((int)ErrorProvider.ErrorCode.FirstLogin).ToString())
                {
                    FormsAuthentication.SetAuthCookie(user, RememberMe);
                    Session["LoginAttempts"] = 0;
                    return Json(new { title = "Login", success = true, message = res.ErrorMessage, failcount = Session["LoginAttempts"].ToString() }, JsonRequestBehavior.DenyGet);
                }
                else if (res.ErrorCode == ((int)ErrorProvider.ErrorCode.LoginFailure).ToString())
                {
                    if (Session["LoginAttempts"] == null)
                        Session["LoginAttempts"] = 1;
                    else
                        Session["LoginAttempts"] = 1 + (int)Session["LoginAttempts"];
                    return Json(new { title = "Login", success = false, message = res.ErrorMessage, failcount = Session["LoginAttempts"].ToString() }, JsonRequestBehavior.DenyGet);
                }
                else if (res.ErrorCode == ((int)ErrorProvider.ErrorCode.InvalidUserNamePass).ToString())
                {
                    if (Session["LoginAttempts"] == null)
                        Session["LoginAttempts"] = 1;
                    else
                        Session["LoginAttempts"] = 1 + (int)Session["LoginAttempts"];
                    return Json(new { title = "Login", success = false, message = res.ErrorMessage, failcount = Session["LoginAttempts"].ToString() }, JsonRequestBehavior.DenyGet);
                }
                else if (res.ErrorCode == ((int)ErrorProvider.ErrorCode.UserNotExist).ToString())
                {
                    if (Session["LoginAttempts"] == null)
                        Session["LoginAttempts"] = 1;
                    else
                        Session["LoginAttempts"] = 1 + (int)Session["LoginAttempts"];
                    return Json(new { title = "Login", success = false, message = res.ErrorMessage, failcount = Session["LoginAttempts"].ToString() }, JsonRequestBehavior.DenyGet);
                }
                else
                {
                    if (Session["LoginAttempts"] == null)
                        Session["LoginAttempts"] = 1;
                    else
                        Session["LoginAttempts"] = 1 + (int)Session["LoginAttempts"];
                    return Json(new { title = "Login", success = false, message = "Something Went Wrong Please Try again", failcount = Session["LoginAttempts"].ToString() }, JsonRequestBehavior.DenyGet);
                }
            }
            catch (Exception ex)
            {
                if (Session["LoginAttempts"] == null)
                    Session["LoginAttempts"] = 1;
                else
                    Session["LoginAttempts"] = 1 + (int)Session["LoginAttempts"];
                return Json(new { title = "Login", success = true, message = "Something went wrong Please try again later", failcount = Session["LoginAttempts"].ToString() }, JsonRequestBehavior.DenyGet);
            }
        }

        #endregion
        #region ForgotPassword
        public ActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ForgotPassword(ForgotPasswordModel model)
        {
            try
            {

                model.deviceId = ClsFunction.GetVisitorIPAddress();
                var res = RegistrationProvider.GetUsersNamecheck(model.username,model.deviceId);
                if (res)
                {
                    return Json(new { title = "ForgotPassword", success = true, message = "Email has been sent to you with the Link"}, JsonRequestBehavior.DenyGet);
                }
                else
                {
                    return Json(new { title = "ForgotPassword", success = false, message = "User Does not Exist"}, JsonRequestBehavior.DenyGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { title = "ForgotPassword", success = false, message = "Something went Wrong Please try again" }, JsonRequestBehavior.DenyGet);
            }

        }


        #endregion

        #region ResetPassword

        [AllowAnonymous]
        public async Task<ActionResult> ResetPassword([Bind(Include = "id")]string id)
        {
            var ID = RegistrationProvider.GetUserId(id);
            ResetPasswordModel model = new ResetPasswordModel();
            string str = "";
            model.token = id;
            if (model.token != null)
            {
                str = RegistrationProvider.CheckpasswordToken(model.token);
                if (str == "S")
                {
                    TempData["ResetPassword"] = "Successfully done Please Login to try again.";
                    return View(model);
                }
                else if (str == "LE")
                {
                    TempData["Error"] = "Link has been expired";
                    return RedirectToAction("Error", "Home");
                }
                else
                {
                    TempData["Error"] = "Something went wrong Please try again later";
                    return RedirectToAction("Error", "Home");
                }
            }
            TempData["Error"] = "Something went wrong Please try again later";
            return RedirectToAction("Error", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UpdatePassword([Bind(Include = "password,confirm,token")] string password, string confirm, string token)
        {
            
                ResetPasswordModel model = new ResetPasswordModel();
                model.password = password;
                model.used_on_ip = ClsFunction.GetVisitorIPAddress();
                model.token = token;
                var res = RegistrationProvider.ResetPassword(model.password,model.token,model.used_on_ip);
                var MODEL = new ResetPasswordModel();
                if (res == "S")
                {
                    return Json(new { success = true, message ="Successfully done!", data = MODEL }, JsonRequestBehavior.DenyGet);


                }
                else if (res == "LE")
                {
                    return Json(new { success = false, message = "Link has been expired" }, JsonRequestBehavior.DenyGet);
                }
                else
                {
                    return Json(new { success = false, message = "Resetting Password Failed" }, JsonRequestBehavior.DenyGet);
                }

                return Json(new { success = false, message = "Resetting Password Failed" }, JsonRequestBehavior.DenyGet);

        }
        #endregion

        #region Logout
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Home");
        }
        #endregion
        public ActionResult AboutUs()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult SearchJobs()
        {
            long userId = 0;
            if (User.Identity.IsAuthenticated)
            {
                userId = RegistrationProvider.GetUseridfromUsername(User.Identity.Name);
            }
            JobList model = new JobList();
            
            //DataTable dt = loc;
            

            if (Session["ListOfJobs"] != null)
            {
                model = (JobList)Session["ListOfJobs"];
                
            }
            else
            {
                model = JobsProvider.GetAllJobs(userId);

                Session["ListOfJobs"] = model;
            }

            return PartialView("_SearchJobs", model.Jobs);
        }
    }
}
