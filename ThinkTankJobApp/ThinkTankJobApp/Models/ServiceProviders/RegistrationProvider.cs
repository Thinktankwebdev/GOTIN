using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;
using ThinkTankJobApp.Models.ServiceContext;

namespace ThinkTankJobApp.Models.ServiceProviders
{
    public class RegistrationProvider
    {
        #region SendRegistrationMail
        public static async Task SendMail(RegisteredUser regUser)
        {
            //OTP otp = new OTP();
            //OTP otp;
            // OTP O = new OTP(x,Guid.NewGuid().ToByteArray());
            long Datetimenow = DateTime.UtcNow.Ticks;
            //string token = O.GetCurrentOTP();//OTPGeneration.GenerateOTP();
            // string t = otp.GetNextOTP();
            string t = regUser.RegistereduserId.ToString() + Guid.NewGuid().ToString();
            string token = ClsFunction.GenerateResetToken(t + ":" + Datetimenow);
            string new_token = Regex.Replace(token, "[^0-9a-zA-Z]+", "");
            string link = ConfigurationManager.AppSettings["URL_BASE_Register"];
            string regLink = string.Format(link, HttpUtility.UrlEncode(new_token));
            var resetToken = "<a href='" + regLink + "'>Activation Link</a>";

            RegistrationContext.RegisterToken(new_token, regUser.Registration_ip, regUser.RegistereduserId);

            var content = MailerContext.SelectEmailContent("ss");
            if (content == null)
                content = new EmailTemplate()
                {
                    content = @"Hi [NAME], Your account has been created and your username is [USERNAME]
                        <br/>Please use the below link to activate your account.<br/>Your LINK is [LINK]<br/>Please note this link will expire in 24 hours",
                    subject = "Your account has been created"
                };

            else if (string.IsNullOrEmpty(content.content))
            {
                content.content = @"Hi [NAME], Your account has been created and your username is [USERNAME]
                        <br/>Please use the below link to activate your account.<br/>Your LINK is [LINK]<br/>Please note this link will expire in 24 hours";
                content.subject = content.subject ?? "Your account has been created";
            }
            //var resetToken = "<a href='" + resetTokenAction + (DateTime.UtcNow.ToFileTimeUtc() + ":" + regUser.username).GenerateResetToken() + "'>Activation Link</a>";

            await Mailer.SendMailAsync(new MailContent()
            {
                ccAddress = "",
                Content = HttpUtility.UrlDecode(HttpUtility.HtmlDecode(content.content))
                .Replace("[NAME]", regUser.First_name + regUser.Middle_name + regUser.Last_name)
                .Replace("[USERNAME]", regUser.Username)
                .Replace("[LINK]", resetToken),
                Email = regUser.Email_address,
                Subject = content.subject,
                TypeOfMail = 1
            }, regUser.Username, regUser.RegistereduserId);
        }

        public static async Task ReSendMail(RegisteredUser regUser)
        {
            //OTP p = new OTP();
            long Datetimenow = DateTime.UtcNow.ToFileTimeUtc();
            string t = regUser.RegistereduserId.ToString() + Guid.NewGuid().ToString();
            string token = ClsFunction.GenerateResetToken(t + ":" + Datetimenow);
            string new_token = Regex.Replace(token, "[^0-9a-zA-Z]+", "");
            string link = ConfigurationManager.AppSettings["URL_BASE_Register"];
            string regLink = string.Format(link, HttpUtility.UrlEncode(new_token));
            var resetToken = "<a href='" + regLink + "'>Activation Link</a>";

            RegistrationContext.RegisterToken(token, regUser.Registration_ip, regUser.RegistereduserId);

            var content = MailerContext.SelectEmailContent("ss");
            if (content == null)
                content = new EmailTemplate()
                {
                    content = @"Hi [NAME], Your account has been created and your username is [USERNAME]
                        <br/>Please use the below link to activate your account.<br/>Your LINK is [LINK]<br/>Please note this OTP will expire in 24 hours",
                    subject = "Your account has been created"
                };

            else if (string.IsNullOrEmpty(content.content))
            {
                content.content = @"Hi [NAME], Your account has been created and your username is [USERNAME]
                        <br/>Please use the below link to activate your account.<br/>Your LINK is [LINK]<br/>Please note this OTP will expire in 24 hours";
                content.subject = content.subject ?? "Your account has been created";
            }
            //var resetToken = "<a href='" + resetTokenAction + (DateTime.UtcNow.ToFileTimeUtc() + ":" + regUser.username).GenerateResetToken() + "'>Activation Link</a>";

            await Mailer.SendMailAsync(new MailContent()
            {
                ccAddress = "",
                Content = HttpUtility.UrlDecode(HttpUtility.HtmlDecode(content.content))
                .Replace("[NAME]", regUser.First_name + regUser.Middle_name + regUser.Last_name)
                .Replace("[USERNAME]", regUser.Username)
                .Replace("[LINK]", resetToken),
                Email = regUser.Email_address,
                Subject = content.subject,
                TypeOfMail = 1
            }, regUser.Username, regUser.RegistereduserId);
        
        }
        #endregion

        #region SendActivationMail
        public static async Task SendActivationMail(RegisteredUser regUser)
        {
            var content = MailerContext.SelectEmailContent("ss");
            if (content == null)
                content = new EmailTemplate()
                {
                    content = @"Hi [NAME], Your account has been Activated and your username is [USERNAME]. ",
                    subject = "Account Activation Confirmation"
                };

            else if (string.IsNullOrEmpty(content.content))
            {
                content.content = @"Hi [NAME], Your account has been Activated and your username is [USERNAME].";
                content.subject = content.subject ?? "Account Activation Confirmation";
            }
            //var resetToken = "<a href='" + resetTokenAction + (DateTime.UtcNow.ToFileTimeUtc() + ":" + regUser.username).GenerateResetToken() + "'>Activation Link</a>";

            await Mailer.SendMailAsync(new MailContent()
            {
                ccAddress = "",
                Content = HttpUtility.UrlDecode(HttpUtility.HtmlDecode(content.content))
                .Replace("[NAME]", regUser.First_name + regUser.Middle_name + regUser.Last_name)
                .Replace("[USERNAME]", regUser.Username)
                .Replace("[LINK]", ""),
                Email = regUser.Email_address,
                Subject = content.subject,
                TypeOfMail = 1
            }, regUser.Username, regUser.RegistereduserId);
        }
        #endregion

        #region RegisterUser
        public static bool RegisterUser(RegisteredUser user)
        {
            var reg = RegistrationContext.RegisterUser(new tb_registered_users()
            {

                registered_user_id = user.RegistereduserId,
                first_name = user.First_name,
                middle_name = user.Middle_name,
                last_name = user.Last_name,
                dob = Convert.ToDateTime(user.Dob),
                gender = user.Gender,
                registration_ip = user.Registration_ip,//deviceId for Mobile users
                registred_on = Convert.ToDateTime(user.Registred_on),
                Country = user.Country,
                Province = user.Province,
                mobile_country_code = user.Mbile_country_code,
                mobile_number = user.Mobile_number,
                City = user.City,
                username = user.Username,
                email_address = user.Email_address,
                password = MDD5.CalculateMD5Hash(user.Password),
                Address = user.Address,
                is_active=user.Is_active,
                title = user.Title,
                Organization = user.Organization,
                isEmployer = user.isEmployer,
            });
            if (reg > 0)
            {
                user.RegistereduserId = reg;
                SendMail(user);
                return true;
            }
            return false;
        }

        public static string ActivateRegistration(string token, string used_on_ip)
        {
            var Reg = RegistrationContext.ActivateRegistration(token, used_on_ip);
            if (Reg == "S")
            {
                var user = RegistrationContext.GetUserDetailsFromToken(token);
                if (user != null)
                {
                    SendActivationMail(user);
                }
                return "S";

            }
            else if (Reg == "LE")
            {
                return "LE";
            }
            return "F";
        }



        #endregion

        #region CheckUserName
        public static string CheckUserAvailability(string username)
        {
            return RegistrationContext.CheckUserAvailability(username);
        }
        public static bool CheckUser(string p)
        {
            return RegistrationContext.GetUsersNameCheck(p);
        }
        public static bool GetUsersNamecheck(string p, string deviceId)
        {
            var chk = RegistrationContext.GetUsersNameCheck(p);
            if (chk)
            {
                var user = new RegisteredUser();
                user = RegistrationContext.GetUserInfo(p);
                SendResetMail(user, deviceId);
                return true;
            }
            return false;
        }

        public static bool GetUsersNamecheckForResendOtp(string p, string deviceId)
        {
            var chk = RegistrationContext.GetUsersNameCheck(p);
            if (chk)
            {
                var user = new RegisteredUser();
                user = RegistrationContext.GetUserInfo(p);
                ReSendMail(user);
                return true;
            }
            return false;
        }
        #endregion

        #region GetUserIdFromToken
        public static long GetUserId(string token)
        {
            return RegistrationContext.GetRegTokenUserId(token);
        }
        public static long GetResetUserId(string token)
        {
            return RegistrationContext.GetResetTokenUserId(token);
        }
        #endregion

        #region Login
        public static Response LoginFunc(LoginModel model)
        {
            // long id = 0;
            Response res = new Response();
            bool isFirst = false;
            try
            {
                if (model != null)
                {
                    if (RegistrationProvider.CheckUser(model.Username))
                    {
                        var membershipProvider1 = Membership.Providers["WebMembershipProvider"];
                        //var membershipProvider2 = Membership.Providers["WebMembershipProvider1"];
                        res.IsValid = membershipProvider1.ValidateUser(model.Username, model.Password);
                        //res.IsValid = Membership.ValidateUser(model.Username, model.Password);

                        if (res.IsValid)
                        {
                            isFirst = RegistrationProvider.IsFirstLogin(model.Username);

                            var LogInCheckPoint = RegistrationProvider.LoginCheckPoint(model.Username, model.IpAddress, isFirst, model.Type);
                            string MsgResponse = LogInCheckPoint.ErrorMessage;
                            string DateUTC = Convert.ToString(DateTime.Now);
                            string[] strArr = null;
                            char[] splitchar = { '|' };
                            strArr = MsgResponse.Split(splitchar);
                            string MessageNo = strArr[0];
                            string MessageText = strArr[1];
                            if (MessageNo == "S")
                            {
                                if (isFirst)
                                {
                                    res.IsValid = true;
                                    res.ErrorCode = ((int)ErrorProvider.ErrorCode.FirstLogin).ToString();
                                    res.ErrorMessage = "Logged In Successfully.";
                                    return res;
                                }
                                else
                                {
                                    res.IsValid = true;
                                    res.ErrorCode = ((int)ErrorProvider.ErrorCode.LoginSuccess).ToString();
                                    res.ErrorMessage = "Logged In Successfully.";
                                    return res;
                                }
                            }

                            else
                            {

                                res.IsValid = false;
                                res.ErrorCode = ((int)ErrorProvider.ErrorCode.LoginFailure).ToString();
                                res.ErrorMessage = "LogIn Failed";
                                return res;
                            }
                        }

                    }
                    else
                    {

                        res.IsValid = false;
                        res.ErrorCode = ((int)ErrorProvider.ErrorCode.UserNotExist).ToString();
                        res.ErrorMessage = "User Does not Exist";
                        return res;
                    }
                }
                else
                {

                    res.IsValid = false;
                    res.ErrorCode = ((int)ErrorProvider.ErrorCode.Failure).ToString();
                    res.ErrorMessage = "SignIn Failed";
                    return res;
                }
                return res;
            }
            catch (Exception ex)
            {
                string[] strResponse = null;
                char[] splitmsg = { '|' };
                strResponse = ex.ToString().Split(splitmsg);
                string error_message = strResponse[2];

                if (strResponse[1] == "R")
                {

                    var LoginFailed = RegistrationProvider.LoginFailedCheck(model.Username, model.IpAddress);
                    string MsgResponse = LoginFailed.ErrorMessage;

                    string[] strArr = null;
                    char[] splitchar = { '|' };

                    strArr = MsgResponse.Split(splitchar);
                    if (strArr.Count() == 2)
                    {
                        string MessageNo = strArr[0];
                        string MessageText = strArr[1];


                        if (MessageNo == "OB")
                        {
                            res.IsValid = false;
                            res.ErrorCode = ((int)ErrorProvider.ErrorCode.LoginFailure).ToString();
                            res.ErrorMessage = MessageText;
                            //res.FailedCount = context.Session["LoginAttempts"].ToString();
                            return res;
                            //send mail
                        }
                        else if (MessageNo == "B")
                        {
                            res.IsValid = false;
                            res.ErrorCode = ((int)ErrorProvider.ErrorCode.LoginFailure).ToString();
                            res.ErrorMessage = MessageText;
                            //res.FailedCount = context.Session["LoginAttempts"].ToString();
                            return res;
                            //send mail
                        }
                        else
                        {
                            res.IsValid = false;
                            res.ErrorCode = ((int)ErrorProvider.ErrorCode.LoginFailure).ToString();
                            res.ErrorMessage = MessageText;
                            //res.FailedCount = context.Session["LoginAttempts"].ToString();
                            return res;
                        }
                    }
                    else
                    {

                        res.IsValid = false;
                        res.ErrorCode = ((int)ErrorProvider.ErrorCode.InvalidUserNamePass).ToString();
                        res.ErrorMessage = "Invalid Username or Password";
                        //res.FailedCount = context.Session["LoginAttempts"].ToString();

                    }
                }
                else
                {

                    res.IsValid = false;
                    res.ErrorCode = ((int)ErrorProvider.ErrorCode.LoginFailure).ToString();
                    res.ErrorMessage = error_message;
                    //res.FailedCount = loginitem.ToString();
                    return res;
                }
                
                return res;
            }
        }
        public static bool ValidateUser(string username, string password)
        {
            LoginModel model = new LoginModel();
            model.Username = username;
            model.Password = password;
            var response = Login(model);

            //var response = Account.Login(new LoginModel() { Username = username, Password = password });

            if (response.IsValid)
                return true;
            throw new Exception(response.ErrorMessage);
        }
        public static Response Login(LoginModel data)
        {
            try
            {
                data.Password = MDD5.CalculateMD5Hash(data.Password);
                var response = RegistrationContext.Login(data);
                return new Response() { IsValid = response.IsValid, ErrorCode = ((int)ErrorProvider.ErrorCode.LoginSuccess).ToString(), ErrorMessage = response.ErrorMessage };
            }
            catch (SqlException sqex)
            {

                return new Response() { IsValid = false, ErrorCode = ((int)ErrorProvider.ErrorCode.LoginFailure).ToString(), ErrorMessage = "Login Failed to Connect" };
            }
            catch (Exception ex)
            {

                return new Response() { IsValid = false, ErrorCode = ErrorProvider.ErrorCode.LoginFailure.ToString(), ErrorMessage = "Login Failed"};
            }
        }
        #endregion
        #region LoginCheckPoint
        //public static string token;
        public static LoginCheckPointModel LoginCheckPoint(string user_name, string ip_address, bool isFirst, string type)
        {

            var refId = DateTime.UtcNow.Ticks.ToString();
            var logId = SmtpProvider.Info(refId, "LoginCheckPoint", "Update on login success", "LoginCheckPoint", "LoginCheckPoint", "");
            //var Country = ClsFunction.GetLocationFromIP(ip_address);
            string ip_country = type;
            string r_token = "";
            try
            {
                var MsgReponse = RegistrationContext.LoginCheckPoint(user_name, ip_address, ip_country);

                if (MsgReponse.Split('|')[0] == "IP")
                {
                    if (isFirst)
                    {
                        long Datetimenow = DateTime.UtcNow.ToFileTimeUtc();
                        RegisteredUser regUser = new RegisteredUser();
                        regUser = RegistrationContext.GetUserDetails(user_name);
                        string t = regUser.RegistereduserId.ToString() + Guid.NewGuid().ToString();
                        r_token = ClsFunction.GenerateResetToken(t + ":" + Datetimenow);
                        SendLogincheckpointMail(regUser, ip_address, r_token);
                    }
                    MsgReponse = "S|Success";
                }

                return new LoginCheckPointModel() { IsValid = true, ErrorCode = ErrorProvider.ErrorCode.Success.ToString(), ErrorMessage = MsgReponse, token = r_token };
            }
            catch (SqlException sqex)
            {
                RegisteredUser user = new RegisteredUser()
                {
                    Username = user_name,
                    Registration_ip = ip_address
                };
                SmtpProvider.Error<RegisteredUser, string>(sqex, "", refId, "LoginCheckPoint", "Update on login success", "LoginCheckPoint", sqex.Message, user, "", "999", logId);

                return new LoginCheckPointModel() { IsValid = false, ErrorCode = ErrorProvider.ErrorCode.Failure.ToString(), ErrorMessage = "F|Failed", token = "" };
            }
            catch (Exception ex)
            {
                RegisteredUser user = new RegisteredUser()
                {
                    Username = user_name,
                    Registration_ip = ip_address
                };
                SmtpProvider.Error<RegisteredUser, string>(ex, "", refId, "LoginCheckPoint", "Update on login success", "LoginCheckPoint", ex.Message, user, "", "999", logId);

                return new LoginCheckPointModel() { IsValid = false, ErrorCode = ErrorProvider.ErrorCode.Failure.ToString(), ErrorMessage = "F|Failed", token = "" };
            }
        }

        public static async Task SendLogincheckpointMail(RegisteredUser regUser, string originIp, string token)
        {
            long Datetimenow = DateTime.UtcNow.Ticks;
            string t = regUser.RegistereduserId.ToString() + Guid.NewGuid().ToString();
            string tokens = ClsFunction.GenerateResetToken(t + ":" + Datetimenow);
            string new_token = Regex.Replace(tokens, "[^0-9a-zA-Z]+", "");
            string link = ConfigurationManager.AppSettings["URL_BASE_ResetPassword"];
            string regLink = string.Format(link, HttpUtility.UrlEncode(new_token));
            var resetToken = "<a href='" + regLink + "'>Activation Link</a>";


            //UIContext.RegisterToken(token, "originip", regUser.user_identifier);
            RegistrationContext.ResetPasswordToken(regUser.RegistereduserId, new_token, originIp);

            var content = MailerContext.SelectEmailContent("ss");
            if (content == null)
                content = new EmailTemplate()
                {
                    content = @"Hi [NAME], We noticed a login from different ip location.
                                <br/>If it is not you, Please use the below link to reset your account.<br/> [LINK] <br/>Please note this link will expire in 24 hours",
                    subject = "We Noticed Your IP Changed"
                };

            else if (string.IsNullOrEmpty(content.content))
            {
                content.content = @"Hi [USERNAME],  We noticed a login from different ip location.
                                <br/>If it is not you, Please use the below link to reset your account.<br/> [LINK] <br/>Please note this link will expire in 24 hours";
                content.subject = content.subject == "" || content.subject == null ? "We Noticed Your Ip Changed" : content.subject;
            }
            //var resetToken = "<a href='" + resetTokenAction + (DateTime.UtcNow.ToFileTimeUtc() + ":" + regUser.username).GenerateResetToken() + "'>Activation Link</a>";

            await Mailer.SendMailAsync(new MailContent()
            {
                ccAddress = "",
                Content = HttpUtility.UrlDecode(HttpUtility.HtmlDecode(content.content))
                .Replace("[NAME]", regUser.First_name + regUser.Middle_name + regUser.Last_name)
                .Replace("[USERNAME]", regUser.Username)
                .Replace("[LINK]", ""),
                Email = regUser.Email_address,
                Subject = content.subject,
                TypeOfMail = 1
            }, regUser.Username, regUser.RegistereduserId);
        }
        #endregion

        #region FirstLogin
        public static bool IsFirstLogin(string username)
        {
            return RegistrationContext.IsFirstLogin(username);
        }
        #endregion

        #region UserDetails
        public static RegisteredUser GetUserDetails(string username)
        {
            return RegistrationContext.GetUserDetails(username);
        }
        #endregion

        #region LoginFailCheck
        public static async Task SendInvalidLoginMail(RegisteredUser regUser, string originIp)
        {
            //OTP p = new OTP();
            //long Datetimenow = DateTime.UtcNow.ToFileTimeUtc();
            //OTP O = new OTP(x, Guid.NewGuid().ToByteArray());
            //long Datetimenow = DateTime.UtcNow.ToFileTimeUtc();
            // string token = O.GetCurrentOTP();
            long Datetimenow = DateTime.UtcNow.Ticks;
            string t = regUser.RegistereduserId.ToString() + Guid.NewGuid().ToString();
            string tokens = ClsFunction.GenerateResetToken(t + ":" + Datetimenow);
            string new_token = Regex.Replace(tokens, "[^0-9a-zA-Z]+", "");
            string link = ConfigurationManager.AppSettings["URL_BASE_ResetPassword"];
            string regLink = string.Format(link, HttpUtility.UrlEncode(new_token));
            var resetToken = "<a href='" + regLink + "'>Activation Link</a>";

            //UIContext.RegisterToken(token, "originip", regUser.user_identifier);
            RegistrationContext.ResetPasswordToken(regUser.RegistereduserId, new_token, originIp);

            var content = MailerContext.SelectEmailContent("ss");
            if (content == null)
                content = new EmailTemplate()
                {
                    content = @"Hi [NAME], We noticed an invalid login attempts into your account from a different location or device.
                                <br/>If it is not you, Please use the below link to reset your account.<br/>[LINK]<br/>Please note this link will expire in 24 hours",
                    subject = "Invalid Login Attempt"
                };

            else if (string.IsNullOrEmpty(content.content))
            {
                content.content = @"Hi [USERNAME],  We noticed an invalid login attempts into your account from a different location or device.
                                <br/>If it is not you, Please use the below link to reset your account.<br/>[LINK]<br/>Please note this link will expire in 24 hours";
                content.subject = content.subject == "" || content.subject == null ? "Invalid Logn Attempt" : content.subject;
            }
            //var resetToken = "<a href='" + resetTokenAction + (DateTime.UtcNow.ToFileTimeUtc() + ":" + regUser.username).GenerateResetToken() + "'>Activation Link</a>";

            await Mailer.SendMailAsync(new MailContent()
            {
                ccAddress = "",
                Content = HttpUtility.UrlDecode(HttpUtility.HtmlDecode(content.content))
                .Replace("[NAME]", regUser.First_name + regUser.Middle_name + regUser.Last_name)
                .Replace("[USERNAME]", regUser.Username)
                .Replace("[LINK]", ""),
                Email = regUser.Email_address,
                Subject = content.subject,
                TypeOfMail = 1
            }, regUser.Username, regUser.RegistereduserId);
        }
        public static async Task SendBlockedMail(RegisteredUser regUser, string originIp)
        {
            // OTP p = new OTP();
            // long Datetimenow = DateTime.UtcNow.ToFileTimeUtc();
            //OTP O = new OTP(x, Guid.NewGuid().ToByteArray());
            //long Datetimenow = DateTime.UtcNow.ToFileTimeUtc();
            //string token = O.GetCurrentOTP();
            //UIContext.RegisterToken(token, "originip", regUser.user_identifier);
            long Datetimenow = DateTime.UtcNow.Ticks;
            string t = regUser.RegistereduserId.ToString() + Guid.NewGuid().ToString();
            string tokens = ClsFunction.GenerateResetToken(t + ":" + Datetimenow);
            string new_token = Regex.Replace(tokens, "[^0-9a-zA-Z]+", "");
            string link = ConfigurationManager.AppSettings["URL_BASE_ResetPassword"];
            string regLink = string.Format(link, HttpUtility.UrlEncode(new_token));
            var resetToken = "<a href='" + regLink + "'>Activation Link</a>";
            RegistrationContext.ResetPasswordToken(regUser.RegistereduserId, new_token, originIp);

            var content = MailerContext.SelectEmailContent("ss");
            if (content == null)
                content = new EmailTemplate()
                {
                    content = @"Hi [NAME], Your Account has been blocked.
                                <br/> Please use the below link to reset your account.<br/>[LINK]<br/>Please note this link will expire in 24 hours",
                    subject = "Your Account has been blocked"
                };

            else if (string.IsNullOrEmpty(content.content))
            {
                content.content = @"Hi [USERNAME],Your Account has been blocked.
                                <br/>Please use the below OTP to reset your account.<br/>[LINK]<br/>Please note this OTP will expire in 24 hours";
                content.subject = content.subject == "" || content.subject == null ? "Invalid Logn Attempt" : content.subject;
            }
            //var resetToken = "<a href='" + resetTokenAction + (DateTime.UtcNow.ToFileTimeUtc() + ":" + regUser.username).GenerateResetToken() + "'>Activation Link</a>";

            await Mailer.SendMailAsync(new MailContent()
            {
                ccAddress = "",
                Content = HttpUtility.UrlDecode(HttpUtility.HtmlDecode(content.content))
                .Replace("[NAME]", regUser.First_name + regUser.Middle_name + regUser.Last_name)
                .Replace("[USERNAME]", regUser.Username)
                .Replace("[LINK]", ""),
                Email = regUser.Email_address,
                Subject = content.subject,
                TypeOfMail = 1
            }, regUser.Username, regUser.RegistereduserId);
        }
        public static Response LoginFailedCheck(string user_name, string ipAddress)
        {
            var refId = DateTime.UtcNow.Ticks.ToString();
            var logId = SmtpProvider.Info(refId, "LoginFailedCheck", "Update on login failure", "LoginFailedCheck", "LoginFailedCheck", "");

            try
            {
                var MsgReponse = RegistrationContext.LoginFailedCheck(user_name);
                string[] res = null;
                res = MsgReponse.Split('|');
                string msgNo = res[0];

                RegisteredUser user = new RegisteredUser();
                user = RegistrationContext.GetUserInfo(user_name);

                if (msgNo == "M")
                {
                    SendInvalidLoginMail(user, ipAddress);
                }
                else if (msgNo == "OB" || msgNo == "B")
                {
                    if (user != null)
                    {
                        SendBlockedMail(user, ipAddress);
                    }
                }
                return new Response() { IsValid = true, ErrorCode = ErrorProvider.ErrorCode.Success.ToString(), ErrorMessage = MsgReponse };
            }
            catch (SqlException sqex)
            {
                RegisteredUser user = new RegisteredUser()
                {
                    Username = user_name,
                    Registration_ip = ipAddress
                };
                SmtpProvider.Error<RegisteredUser, string>(sqex, "", refId, "LoginFailedCheck", "Update on login failure", "LoginFailedCheck", sqex.Message, user, "", "999", logId);

                return new Response() { IsValid = false, ErrorCode = ErrorProvider.ErrorCode.Failure.ToString(), ErrorMessage = sqex.ToString() };
            }
            catch (Exception ex)
            {
                RegisteredUser user = new RegisteredUser()
                {
                    Username = user_name,
                    Registration_ip = ipAddress
                };
                SmtpProvider.Error<RegisteredUser, string>(ex, "", refId, "LoginFailedCheck", "Update on login failure", "LoginFailedCheck", ex.Message, user, "", "999", logId);

                return new Response() { IsValid = false, ErrorCode = ErrorProvider.ErrorCode.Failure.ToString(), ErrorMessage = ex.ToString() };
            }
        }
        #endregion

        #region GetuseridfromUsername
        public static bool GetEmployer(string username)
        {
            return RegistrationContext.GetEmployer(username);
        }
        public static long GetUseridfromUsername(string username)
        {
            return RegistrationContext.GetUseridfromUsername(username);
        }
        #endregion

        #region ResetPassword
        public static async Task SendResetMail(RegisteredUser regUser, string deviceId)
        {
            //long Datetimenow = DateTime.UtcNow.ToFileTimeUtc();
            //OTP p = new OTP();
            //string token = otp.GetNextOTP();
            //OTP O = new OTP(x, Guid.NewGuid().ToByteArray());
            long Datetimenow = DateTime.UtcNow.Ticks;
            //string token = O.GetCurrentOTP();
            string t = regUser.RegistereduserId.ToString() + Guid.NewGuid().ToString();
            string token = ClsFunction.GenerateResetToken(t + ":" + Datetimenow);
            string new_token = Regex.Replace(token, "[^0-9a-zA-Z]+", "");
            string link = ConfigurationManager.AppSettings["URL_BASE_ResetPassword"];
            string regLink = string.Format(link, HttpUtility.UrlEncode(new_token));
            var resetToken = "<a href='" + regLink + "'>Activation Link</a>";
            //UIContext.RegisterToken(token, "originip", regUser.user_identifier);
            RegistrationContext.ResetPasswordToken(regUser.RegistereduserId, new_token, deviceId);

            var content = MailerContext.SelectEmailContent("ss");
            if (content == null)
                content = new EmailTemplate()
                {
                    content = @"Hi [NAME],  Please use the below link to reset your account.<br/> [LINK] <br/>Please note this Reset link will expire in 24 hours",
                    subject = "reset Your Password"
                };

            else if (string.IsNullOrEmpty(content.content))
            {
                content.content = @"Hi [USERNAME], Please use the below link to reset your account.<br/> [LINK] <br/>Please note this Reset link will expire in 24 hours";
                content.subject = content.subject == "" || content.subject == null ? "Reset your Password" : content.subject;
            }
            //var resetToken = "<a href='" + resetTokenAction + (DateTime.UtcNow.ToFileTimeUtc() + ":" + regUser.username).GenerateResetToken() + "'>Activation Link</a>";

            await Mailer.SendMailAsync(new MailContent()
            {
                ccAddress = "",
                Content = HttpUtility.UrlDecode(HttpUtility.HtmlDecode(content.content))
                .Replace("[NAME]", regUser.First_name + regUser.Middle_name + regUser.Last_name)
                .Replace("[USERNAME]", regUser.Username)
                .Replace("[LINK]", resetToken),
                Email = regUser.Email_address,
                Subject = content.subject,
                TypeOfMail = 1
            }, regUser.Username, regUser.RegistereduserId);
        }
        public static string CheckpasswordToken(string token)
        {
            return RegistrationContext.CheckpasswordToken(token);
        }
        public static string ResetPassword(string password, string token, string used_on_ip)
        {
            password = MDD5.CalculateMD5Hash(password);
            return RegistrationContext.ResetPassword(password, token, used_on_ip);
        }
        #endregion


        #region CheckUsername
        public static bool CheckUsername(string username)
        {
            return RegistrationContext.CheckUsername(username);
        }
        #endregion

        #region GetUserName
        public static string GetNameOfUser(string username)
        {
            return RegistrationContext.GetNameOfUser(username);
        }
        #endregion
    }
}