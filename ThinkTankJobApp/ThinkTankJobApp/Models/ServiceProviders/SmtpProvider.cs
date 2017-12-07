using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ThinkTankJobApp.Models.ServiceContext;

namespace ThinkTankJobApp.Models.ServiceProviders
{
    public class SmtpProvider
    {
        public static SmtpSettingModel GetSmtp()
        {
            return MailerContext.GetSmtp();
        }
        public static void Error<Req, Res>(Exception ex, string userId, string referenceId, string functionName, string messageFunction,
          string messageType, string shortDesc, Req Request, Res Response, string errorcode = "999", long logId = 0)
        {

            MailerContext.AddException(new tb_logs()
            {
                // Holds the response data
                description = JsonConvert.SerializeObject(Request),
                error_code = errorcode,
                error_description = ex.ToString(),
                function_name = functionName,
                msg_function = messageFunction,
                msg_type = messageType,
                short_description = shortDesc,
                stack_trace = ex.StackTrace,
                status = "F",
                trace_data = Response == null ? "" : JsonConvert.SerializeObject(Response),
                transaction_ref_id = referenceId,
                user_Identifier = userId,
                log_id = logId,
                created_on = DateTime.UtcNow,

            });
        }

        public static long Info(string TxRefId, string description, string shortDescription, string msgFunction, string msgType, string userId)
        {



            return MailerContext.AddLog(new tb_logs()
            {
                description = string.Format(description, userId),
                short_description = shortDescription,
                msg_function = msgFunction,
                msg_type = msgType,
                status = "S",
                user_Identifier = userId,
                transaction_ref_id = TxRefId,
                created_on = DateTime.UtcNow


            });
        }

        public static SmtpSettingModel GetSmtpSettingFor(int smtpType)
        {
            return MailerContext.GetSmtpFor(smtpType);
        }

        public static bool EditSmtpSetting(SmtpSettingModel model)
        {
            return MailerContext.EditSmtpSetting(new tb_smtp_settings()
            {
                active = model.IsActive,
                address = model.Address,
                from_address = model.FromAddress,
                password = model.Password,
                last_modified_by = model.LastModifiedBy,
                port = model.Port,
                smtp_type = model.Type,
                smtp_setting_id = model.SMTPSettingId,
                use_anonymous = model.UseAnonymous,
                username = model.UserName
            });

        }
    }
}