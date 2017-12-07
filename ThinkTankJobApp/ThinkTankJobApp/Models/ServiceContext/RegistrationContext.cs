using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ThinkTankJobApp.Models.ServiceContext
{
    public static class RegistrationContext
    {
        #region Registeruser
        public static long RegisterUser(tb_registered_users user)
        {
            using (var ctx = new JobAppDBEntities())
            {
                try
                {
                    
                    var tb = ctx.tb_registered_users.Where(x => x.username == user.username).FirstOrDefault();
                    if (tb == null)
                    {
                        user.registred_on = DateTime.Now;
                        user.is_active = false;
                        ctx.tb_registered_users.Add(user);
                        ctx.SaveChanges();
                        return user.registered_user_id;
                    }
                    else
                    {
                        tb.first_name = user.first_name;
                        tb.middle_name = user.middle_name;
                        tb.last_name = user.last_name;
                        tb.title = user.title;
                        tb.mobile_number = user.mobile_number;
                        tb.mobile_country_code = user.mobile_country_code;
                        tb.username = user.username;
                        tb.password = user.password;
                        tb.Country = user.Country;
                        tb.Province = user.Province;
                        tb.City = user.City;
                        tb.dob = user.dob;
                        tb.registration_ip = user.registration_ip;
                        tb.gender = user.gender;
                        tb.Address = user.Address;
                        //tb.ServiceId = user.ServiceId;
                        tb.isEmployer = user.isEmployer;
                        tb.is_active = false;
                        ctx.SaveChanges();
                        return tb.registered_user_id;
                    }
                }
                catch (Exception ex)
                {
                    return -1;
                }
            }
        }

        public static string ActivateRegistration(string token, string used_on_ip)
        {
            using (var ctx = new JobAppDBEntities())
            {
                var res = ctx.proc_registrationConfirmation(token, used_on_ip).FirstOrDefault();
                return res;
            }
        }


        #endregion

        #region Register/Token/OTP
        public static bool RegisterToken(string token, string origin_ip, long userid)
        {
            using (var ctx = new JobAppDBEntities())
            {
                var uservalid = ctx.tb_registered_users.Where(x => x.registered_user_id == userid).FirstOrDefault();
                if (uservalid != null)
                {
                    var tb = new tb_registered_users_registration_tokens();
                    tb.expired_on = DateTime.UtcNow.AddHours(24);
                    tb.registration_link = token;
                    tb.origin_ip = origin_ip;
                    tb.created_on = DateTime.UtcNow;
                    tb.registered_user_id = userid;
                    ctx.tb_registered_users_registration_tokens.Add(tb);
                    ctx.SaveChanges();
                    return true;
                }
                return false;

            }
        }
        #endregion

        #region CheckUserName
        public static bool CheckUsername(string username)
        {
            using (var ctx = new JobAppDBEntities())
            {
                var tb = ctx.tb_registered_users.Where(x => x.username == username).FirstOrDefault();
                if (tb == null)
                {
                    return false;
                }
                return true;
            }
        }
        public static string CheckUserAvailability(string username)
        {
            using (var ctx = new JobAppDBEntities())
            {
                var useractive = ctx.tb_registered_users.Where(o => o.username == username).FirstOrDefault();
                if (useractive == null)
                {
                    return "S";
                }
                else
                {
                    if (useractive.is_active)
                    {
                        return "U";
                    }
                    else
                    {
                        return "NA";
                    }
                }


            }
        }
        public static bool GetUsersNameCheck(this string user)
        {
            using (var ctx = new JobAppDBEntities())
            {
                var User = ctx.tb_registered_users.FirstOrDefault(o => (o.username == user || o.email_address == user));


                if (User == null)
                {
                    return false;
                }

                return true;
            }
        }
        #endregion

        #region UserDetailsFromToken
        public static RegisteredUser GetUserDetailsFromToken(string regToken)
        {
            using (var context = new JobAppDBEntities())
            {
                try
                {
                    var result = from a in context.tb_registered_users_registration_tokens
                                 join b in context.tb_registered_users on a.registered_user_id equals b.registered_user_id
                                 select new RegisteredUser()
                                 {
                                     token = a.registration_link,
                                     RegistereduserId = b.registered_user_id,
                                     Email_address = b.email_address,
                                     Username = b.username,
                                     First_name = b.first_name,
                                     Last_name = b.last_name,
                                     Middle_name = b.middle_name
                                 };
                    return result.Where(o => o.token == regToken).FirstOrDefault();


                }
                catch (Exception ex)
                {
                    return new RegisteredUser();
                }
            }
        }
        #endregion
        #region UserIdFromToken
        public static long GetRegTokenUserId(string token)
        {
            using (var ctx = new JobAppDBEntities())
            {
                var tb = ctx.tb_registered_users_registration_tokens.Where(x => x.registration_link == token && x.used_on == null).FirstOrDefault();
                if (tb != null)
                {
                    return tb.registered_user_id;
                }
                return -1;
            }
        }
        public static long GetResetTokenUserId(string token)
        {
            using (var ctx = new JobAppDBEntities())
            {
                var tb = ctx.tb_registered_users_resetpasswordtokens.Where(x => x.password_token == token).FirstOrDefault();
                if (tb != null)
                {
                    return tb.registered_user_id;
                }
                return -1;
            }
        }
        #endregion

        #region Login
        public static Response Login(LoginModel login)
        {
            using (var ctx = new JobAppDBEntities())
            {
                var response = new Response();
                var Usercount = ctx.tb_registered_users.AsNoTracking().Count(c => ((c.username == login.Username || c.email_address == login.Username) && c.password == login.Password));


                if (Usercount == 1)
                {

                    var active = login_checkRegisteruser(login.Username, "active");
                    if (active == true)
                    {
                        var lockuser = login_checkRegisteruser(login.Username, "block");
                        if (lockuser == true)
                        {

                            Usercount = 0;
                            response.IsValid = false;
                            response.ErrorMessage = "|U|Your Account is Blocked. Please reset your account to Login.|";
                            return response;
                        }
                        else
                        {
                            Usercount = 0;
                            response.IsValid = true;
                            response.ErrorMessage = "|S|Login Successfully.|";
                            return response;
                        }
                    }
                    else
                    {

                        Usercount = 0;
                        response.IsValid = false;
                        response.ErrorMessage = "|A|Your Account is Inactive. Please activate you account to login.|";
                        return response;
                    }

                }

                else
                {
                    response.IsValid = false;
                    response.ErrorMessage = "|R|Your password is Invalid Password.|";
                    return response;
                }


            }
        }

        public static bool login_checkRegisteruser(string Username, string type)
        {
            using (var ctx = new JobAppDBEntities())
            {
                if (type == "active")
                {
                    var active = ctx.tb_registered_users.Where(a => a.username == Username && a.email_address == Username).FirstOrDefault().is_active;
                    if (active == true)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    var RegLockedUser = from a in ctx.tb_registered_users
                                        join b in ctx.tb_login_details on a.registered_user_id equals b.registered_user_id
                                        select new
                                        {

                                            a.registered_user_id,
                                            a.username,
                                            b.login_locked,
                                            a.email_address
                                        };
                    var lockedUser =
                        RegLockedUser.Where(
                            o =>
                                o.login_locked == true &&
                                (o.username == Username || o.email_address == Username)).FirstOrDefault();
                    if (lockedUser != null)
                    {
                        return true;
                    }
                    else
                    {

                        return false;
                    }
                }


            }
        }
        #endregion

        #region LoginCheckPoint
        public static string LoginCheckPoint(string user_name, string ip_address, string ip_country)
        {
            using (var ctx = new JobAppDBEntities())
            {
                var response = ctx.proc_LoginCheckPoint(user_name, ip_address, ip_country);
                return response.FirstOrDefault();
            }
        }
        #endregion

        #region common
        public static RegisteredUser GetUserInfo(string UserName)
        {
            RegisteredUser user = new RegisteredUser();
            using (var context = new JobAppDBEntities())
            {
                var content = context.tb_registered_users.Where(m => m.username == UserName || m.email_address == UserName).FirstOrDefault();
                if (content != null)
                {
                    user.Email_address = content.email_address;
                    user.Username = content.username;
                    user.RegistereduserId = content.registered_user_id;
                    user.First_name = content.first_name;
                    user.Last_name = content.last_name;
                    user.Middle_name = content.middle_name;
                }

                return user;
            }
        }
        #endregion

        #region ResetPassword
        public static string ResetPasswordToken(long userid, string resettoken, string ipOrigin)
        {
            using (var context = new JobAppDBEntities())
            {
                try
                {
                    var user_id = context.tb_registered_users.Where(p => p.registered_user_id == userid).Select(p => p.registered_user_id).FirstOrDefault();
                    var token = context.proc_UpdatePasswordRegToken("I", user_id, resettoken, ipOrigin, null);
                    return token.FirstOrDefault();
                }
                catch (Exception ex)
                {
                    return ex.Message.ToString();
                }
            }
        }
        #endregion

        #region FirstLogin
        public static bool IsFirstLogin(string userName)
        {
            using (var ctx = new JobAppDBEntities())
            {
                var User = ctx.tb_registered_users.Where(o => o.username == userName || o.email_address == userName).FirstOrDefault();


                if (User != null)
                {
                    var Login = ctx.tb_login_details.Where(o => o.registered_user_id == User.registered_user_id).FirstOrDefault();
                    if (Login != null)
                    {
                        if (Login.last_login == null)
                            return true;
                    }
                    else
                    {
                        return true;
                    }
                }


                return false;
            }
        }
        #endregion

        #region UserDetails

        public static RegisteredUser GetUserDetails(string username)
        {
            using (var ctx = new JobAppDBEntities())
            {
                RegisteredUser model = new RegisteredUser();
                var userid = Convert.ToInt64(username);
                var user_data = ctx.tb_registered_users.Where(x => x.registered_user_id == userid).Select(x => new RegisteredUser()
                {
                    RegistereduserId = x.registered_user_id,
                    Username = x.username,
                    Email_address = x.email_address,
                    Is_active = x.is_active,
                    Country = x.Country,
                    City = x.City,
                    Province = x.Province,
                    Password = x.password,
                    Mbile_country_code = x.mobile_country_code,
                    Mobile_number = x.mobile_number,
                    Gender = x.gender,
                     isEmployer= x.isEmployer.Value,
                    Dob = x.dob.Value,
                    First_name = x.first_name,
                    Middle_name = x.middle_name,
                    Last_name = x.last_name,
                    Registration_ip = x.registration_ip,
                    Registred_on = x.registred_on,
                    Address = x.Address,
                    Title = x.title,
                    
                }).FirstOrDefault();
                if (user_data != null)
                {
                    return user_data;
                }
                return new RegisteredUser();
            }
        }
        #endregion

        #region LoginFailCheck
        public static string LoginFailedCheck(string user_name)
        {
            using (var ctx = new JobAppDBEntities())
            {
                var response = ctx.proc_LoginFailed(user_name);
                return response.FirstOrDefault();
            }
        }
        #endregion

        #region GetUserNameFromId
        public static bool GetEmployer(string username)
        {
            using (var ctx = new JobAppDBEntities())
            {
                if (username != "" && username != null)
                {
                    var tb = ctx.tb_registered_users.Where(x => x.username == username || x.email_address == username).FirstOrDefault().isEmployer;
                    if (tb.Value==true)
                    {
                        return tb.Value;
                    }
                    
                }
                return false;
            }
        }
        public static long GetUseridfromUsername(string username)
        {
            using (var ctx = new JobAppDBEntities())
            {
                if (username != "" && username != null)
                {
                    var tb = ctx.tb_registered_users.Where(x => x.username == username || x.email_address == username).FirstOrDefault();
                    if (tb != null)
                    {
                        return tb.registered_user_id;
                    }
                }
                return -1;
            }
        }
        #endregion

        #region ResetPassword
        public static string CheckpasswordToken(string token)
        {
            using (var ctx = new JobAppDBEntities())
            {
                var chckToken = ctx.tb_registered_users_resetpasswordtokens.Where(y => y.password_token == token).FirstOrDefault();
                if (chckToken != null)
                {
                    var res = ctx.tb_registered_users_resetpasswordtokens.Where(x => x.password_token == token && x.used_datetime == null && DateTime.UtcNow <= x.expires_on).FirstOrDefault();
                    if (res != null)
                    {
                        return "S";
                    }
                    else
                    {
                        return "LE";
                    }
                }
                else
                {
                    return "F";
                }

            }
        }
        public static string ResetPassword(string password, string token, string used_on_ip)
        {
            using (var ctx = new JobAppDBEntities())
            {
                var res = ctx.proc_ResetPassword(token, password, used_on_ip).FirstOrDefault();
                return res;
            }

        }



        #endregion

        #region NameOfuser
        public static string GetNameOfUser(string username)
        {
            using (var ctx = new JobAppDBEntities())
            {
                var res = ctx.tb_registered_users.Where(x => x.username == username || x.email_address == username).FirstOrDefault();
                if (res != null)
                {
                    return res.first_name + " " + res.last_name;
                }
                return "";
            }
        }
        #endregion
    }
}