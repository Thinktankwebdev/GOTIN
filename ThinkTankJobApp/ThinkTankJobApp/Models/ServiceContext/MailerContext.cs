using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ThinkTankJobApp.Models.ServiceContext
{
    public class MailerContext
    {
        #region email
        public static EmailTemplate SelectEmailContent(string EmailCode)
        {
            using (var content = new JobAppDBEntities())
            {
                var response = content.proc_GetEmailTemplate(EmailCode);
                var data = response.FirstOrDefault();
                if (data == null)
                    return new EmailTemplate();
                return new EmailTemplate()
                {
                    email_template_id = data.email_template_id,
                    subject = data.subject,
                    content = data.content

                };
            }
        }

        /// <summary>
        /// Updates the email log.
        /// </summary>
        /// <param name="data">The data.</param>
        public static void UpdateEmailLog(tb_email_logs data)
        {
            using (var context = new JobAppDBEntities())
            {
                var log = context.tb_email_logs.Find(data.email_log_id);
                if (log == null)
                    return;
                log.error = data.error;
                log.content = data.content;
                context.SaveChanges();
            }
        }
        public static tb_email_logs LogEmail(tb_email_logs data)
        {
            using (var context = new JobAppDBEntities())
            {
                data.log_entered_date = DateTime.UtcNow;
                var log = context.tb_email_logs.Add(data);
                context.SaveChanges();
                return log;
            }
        }
        #endregion

        #region SmtpSettings
        public static int ID = 0;
        public static bool EditSmtpSetting(tb_smtp_settings smtp_setting)
        {
            using (var context = new JobAppDBEntities())
            {
                var smtpSetting = context.tb_smtp_settings.Find(smtp_setting.smtp_setting_id);
                if (smtpSetting == null)
                {
                    smtp_setting.last_modified_on = DateTime.UtcNow;
                    smtp_setting.active = true;
                    smtp_setting.status = true;

                    context.tb_smtp_settings.Add(smtp_setting);
                }
                else
                {
                    smtpSetting.username = smtp_setting.username;

                    smtpSetting.port = smtp_setting.port;
                    smtpSetting.last_modified_by = smtp_setting.last_modified_by;
                    smtpSetting.last_modified_on = DateTime.UtcNow;
                    smtpSetting.password = smtp_setting.password;
                    smtpSetting.from_address = smtp_setting.from_address;
                    smtpSetting.address = smtp_setting.address;

                }
                context.SaveChanges();
                ID = smtp_setting.smtp_setting_id;
                return true;
            }
        }

        public static SmtpSettingModel GetSmtp()
        {

            using (var context = new JobAppDBEntities())
            {
                var smtps = context.tb_smtp_settings.Select(p => new SmtpSettingModel()
                {
                    UserName = p.username,
                    UseAnonymous = p.use_anonymous,
                    Type = p.smtp_type,
                    SMTPSettingId = p.smtp_setting_id,
                    Port = p.port,
                    Password = p.password,
                    IsActive = p.active,
                    FromAddress = p.from_address,
                    Address = p.address
                }).FirstOrDefault(p => p.SMTPSettingId == p.SMTPSettingId);

                return smtps;
            }
        }

        /// <summary>
        /// Gets the SMTP for.
        /// </summary>
        /// <param name="smtpType">Type of the SMTP.</param>
        /// <returns></returns>
        public static SmtpSettingModel GetSmtpFor(int smtpType)
        {

            using (var context = new JobAppDBEntities())
            {
                var smtpSetting = context.tb_smtp_settings.FirstOrDefault(o => o.smtp_type == smtpType);
                if (smtpSetting == null)
                    return new SmtpSettingModel() { Type = smtpType };
                return new SmtpSettingModel()
                {
                    UserName = smtpSetting.username,
                    UseAnonymous = smtpSetting.use_anonymous,
                    Type = smtpSetting.smtp_type,
                    SMTPSettingId = smtpSetting.smtp_setting_id,
                    Port = smtpSetting.port,
                    Password = smtpSetting.password,
                    IsActive = smtpSetting.active,
                    FromAddress = smtpSetting.from_address,
                    Address = smtpSetting.address
                };
            }
        }
        #endregion

        #region Email Templates

        /// <summary>
        /// Gets the email template.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static EmailTemplateModel GetEmailTemplate(string type)
        {
            using (var context = new JobAppDBEntities())
            {
                var content = context.tb_email_templates.FirstOrDefault(e => e.@for == type);
                if (content == null)
                    return new EmailTemplateModel()
                    {
                        For = (ThinkTankJobApp.Models.EmailTemplateModel.EmailTemplateType)Enum.Parse(typeof(ThinkTankJobApp.Models.EmailTemplateModel.EmailTemplateType), type, true),
                        Content = "",
                        SmsContent = "",
                        Subject = ""
                    };
                return new EmailTemplateModel()
                {
                    Subject = content.subject,
                    Content = content.content,
                    SmsContent = content.sms_content,
                    For = (ThinkTankJobApp.Models.EmailTemplateModel.EmailTemplateType)Enum.Parse(typeof(ThinkTankJobApp.Models.EmailTemplateModel.EmailTemplateType), content.@for, true),
                    Id = content.email_template_id
                };
            }
        }

        /// <summary>
        /// Edits the email template.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public static bool EditEmailTemplate(tb_email_templates model)
        {
            using (var context = new JobAppDBEntities())
            {
                var content = context.tb_email_templates.Find(model.email_template_id);
                if (content == null)
                    context.tb_email_templates.Add(model);
                else
                {
                    content.subject = model.subject;
                    content.content = model.content;
                    content.@for = model.@for.ToString();
                    content.sms_content = model.sms_content;
                }
                context.SaveChanges();
                return true;
            }
        }

        #endregion Email Templates

        #region Add Exception
        public static long AddLog(tb_logs log)
        {
            try
            {
                using (var context = new JobAppDBEntities())
                {
                    log.transaction_ref_id = log.transaction_ref_id ?? DateTime.UtcNow.Ticks.ToString();
                    log.created_on = DateTime.UtcNow;
                    log.status = "S";
                    var header = context.tb_logs.Add(log);
                    context.SaveChanges();
                    return header.log_id;
                }
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public static void AddException(tb_logs tb_log)
        {
            try
            {
                using (var context = new JobAppDBEntities())
                {
                    if (tb_log.log_id == 0)
                        context.tb_logs.Add(tb_log);
                    else
                    {

                        var log = context.tb_logs.Find(tb_log.log_id);
                        if (log != null)
                        {
                            log.function_name = tb_log.function_name;
                            log.stack_trace = tb_log.stack_trace;
                            log.trace_data = string.IsNullOrEmpty(tb_log.trace_data) ? (tb_log.description ?? "") : tb_log.trace_data;
                            log.error_code = tb_log.error_code;
                            log.error_description = tb_log.error_description;
                            log.created_on = DateTime.UtcNow;
                            log.status = "F";
                        }

                    }
                    context.SaveChanges();
                }
            }
            catch (Exception)
            {
            }
        }

       
        #endregion
    }
}