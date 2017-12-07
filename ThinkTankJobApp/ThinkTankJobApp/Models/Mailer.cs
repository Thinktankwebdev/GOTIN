using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using ThinkTankJobApp.Models.ServiceContext;
using ThinkTankJobApp.Models.ServiceProviders;

namespace ThinkTankJobApp.Models
{
    public static class Mailer
    {
        internal static void SendMail(MailContent mailContent, string smsContent, string username, long userid)
        {
            StartMailer(mailContent.Email, mailContent.ccAddress, mailContent.Subject, mailContent.Content, mailContent.TransactionId, mailContent.TypeOfMail, username, userid, mailContent.TypeOfMail);
        }

        internal static async Task SendMailAsync(MailContent mailContent, string username, long userid)
        {
            await Task.Run(() =>
            {
                StartMailer(mailContent.Email, mailContent.ccAddress, mailContent.Subject, mailContent.Content, mailContent.TransactionId, mailContent.TypeOfMail, username, userid, mailContent.TypeOfMail);
            });
        }

        internal static async Task SendMailWithAttachmentAsync(MailContent mailContent, string[] fileName, string username, long userId)
        {
            await Task.Run(() =>
            {
                StartMailerWithAttachment(mailContent.Email, mailContent.ccAddress, mailContent.Subject, mailContent.Content, mailContent.TransactionId, mailContent.TypeOfMail, username, userId, fileName);
            });
        }


        private static void StartMailer(string email, string ccAddress, string subject, string content, string txnId, int mailType, string username, long user_id, int type)
        {
            mailType = 1;
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += (s, e) =>
            {
                tb_email_logs log = new tb_email_logs();
                try
                {

                    log = MailerContext.LogEmail(new tb_email_logs()
                    {
                        cc = ccAddress ?? "",
                        content = HttpUtility.UrlDecode(content ?? ""),
                        error = "",
                        has_attachement = false,
                        subject = subject ?? "",
                        to = email ?? "",
                        log_entered_date = DateTime.UtcNow,
                        user_identifier = user_id.ToString()
                    });
                }
                catch (Exception)
                {

                }

                try
                {
                    var smtp = SmtpProvider.GetSmtpSettingFor(mailType);
                    // TODO : Fetch the details from the Web-Admin
                    MailMessage message = new MailMessage();
                    if (email.Contains(";"))
                    {
                        message.From = new MailAddress(smtp.FromAddress);
                        foreach (var item in email.Split(';'))
                        {
                            if (string.IsNullOrEmpty(item))
                                continue;
                            message.To.Add(new MailAddress(item));
                        }
                        message.Subject = subject;
                        message.Body = HttpUtility.UrlDecode(content);
                    }
                    else if (email.Contains(","))
                    {
                        message.From = new MailAddress(smtp.FromAddress);
                        foreach (var item in email.Split(','))
                        {
                            if (string.IsNullOrEmpty(item))
                                continue;
                            message.To.Add(new MailAddress(item));
                        }
                        message.Subject = subject;
                        message.Body = HttpUtility.UrlDecode(content);
                    }
                    else
                    {
                        message = new MailMessage(smtp.FromAddress, email, subject, HttpUtility.UrlDecode(content));
                    }

                    message.IsBodyHtml = true;
                    //message.Attachments.Add(attachment);
                    SmtpClient cnt = new SmtpClient(smtp.Address, Convert.ToInt16(smtp.Port));
                    cnt.Timeout = 2000000;
                    cnt.UseDefaultCredentials = false;
                    cnt.EnableSsl = true;
                    //cnt.Host = smtp.Address;

                    if (!smtp.UseAnonymous)
                        cnt.Credentials = new System.Net.NetworkCredential(smtp.UserName, smtp.Password);
                    cnt.Send(message);
                }
                catch (Exception ex)
                {
                    try
                    {
                        e.Result = email;
                        log.error = ex.ToString();
                        MailerContext.UpdateEmailLog(log);
                    }
                    catch (Exception)
                    {

                    }
                    //throw ex;
                }

            };
            worker.RunWorkerAsync();
            worker.RunWorkerCompleted += worker_RunWorkerCompleted;
        }

        private static void StartMailerWithAttachment(string email, string ccAddress, string subject, string content, string txnId, int mailType, string username, long user_id, string[] filename)
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += (s, e) =>
            {
                tb_email_logs log = new tb_email_logs();
                try
                {
                    log = MailerContext.LogEmail(new tb_email_logs()
                    {
                        cc = ccAddress ?? "",
                        content = HttpUtility.UrlDecode(content ?? ""),
                        error = "",
                        has_attachement = false,
                        subject = subject ?? "",
                        to = email ?? "",
                        log_entered_date = DateTime.UtcNow,
                        user_identifier = user_id.ToString()
                    });
                }
                catch (Exception)
                {

                }

                try
                {
                    var smtp = SmtpProvider.GetSmtpSettingFor(mailType);
                    // TODO : Fetch the details from the Web-Admin
                    MailMessage message = new MailMessage();
                    if (email.Contains(";"))
                    {
                        message.From = new MailAddress(smtp.FromAddress);
                        foreach (var item in email.Split(';'))
                        {
                            if (string.IsNullOrEmpty(item))
                                continue;
                            message.To.Add(new MailAddress(item));
                        }
                        message.Subject = subject;
                        message.Body = HttpUtility.UrlDecode(content);
                    }
                    else if (email.Contains(","))
                    {
                        message.From = new MailAddress(smtp.FromAddress);
                        foreach (var item in email.Split(','))
                        {
                            if (string.IsNullOrEmpty(item))
                                continue;
                            message.To.Add(new MailAddress(item));
                        }
                        message.Subject = subject;
                        message.Body = HttpUtility.UrlDecode(content);
                    }
                    else
                    {
                        message = new MailMessage(smtp.FromAddress, email, subject, HttpUtility.UrlDecode(content));
                    }

                    message.IsBodyHtml = true;
                    foreach (var file in filename)
                    {
                        message.Attachments.Add(new Attachment(file, "appliation/pdf"));
                    }

                    //message.Attachments.Add(attachment);
                    SmtpClient cnt = new SmtpClient();

                    cnt.Host = smtp.Address;
                    cnt.Port = Convert.ToInt16(smtp.Port);
                    if (!smtp.UseAnonymous)
                        cnt.Credentials = new System.Net.NetworkCredential(smtp.UserName, smtp.Password);
                    cnt.Send(message);
                }
                catch (Exception ex)
                {
                    try
                    {
                        e.Result = email;
                        log.error = ex.ToString();
                        MailerContext.UpdateEmailLog(log);
                    }
                    catch (Exception)
                    {

                    }
                    //throw ex;
                }

            };
            worker.RunWorkerAsync();
            worker.RunWorkerCompleted += worker_RunWorkerCompleted;
        }

        static void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MailerContext.AddException(new tb_logs()
                {
                    description = e.Error.Message,
                    error_code = "999",
                    error_description = e.Error.Data != null ? JsonConvert.SerializeObject(e.Error.Data) : "",
                    function_name = "MAILER",
                    msg_function = "MAILER",
                    msg_type = "SEND MAIL",
                    short_description = "Error while sending mail",
                    stack_trace = e.Error.StackTrace,
                    status = "F",
                    trace_data = e.Error.InnerException == null ? "" : e.Error.InnerException.ToString(),
                    transaction_ref_id = DateTime.UtcNow.Ticks.ToString(),
                    user_Identifier = null ///e.Result
                });
            }
        }

        //public static IEnumerable<IMailContent> GetSentMail(string userId, string filterQuery = "")
        //{
        //    throw new System.NotImplementedException();
        //}
    }
}