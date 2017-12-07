using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ThinkTankJobApp.Models.ServiceContext;

namespace ThinkTankJobApp.Models.ServiceProviders
{
    public static class JobsProvider
    {
        public static bool PostJob(JobDetailsModel model)
        {
            var post = JobsContext.PostJob(new JobDetail()
                {
                    job_details_id = model.Job_details_id,
                    job_organization = model.Job_organization,
                    job_img = "",
                    employee_id = RegistrationProvider.GetUseridfromUsername(model.username),
                    job_date = model.Job_date,
                    job_applied_date = DateTime.Now,
                    job_description = model.Job_description,
                    job_title = model.Job_title,
                    jo_date_modified = DateTime.Now,
                    job_city = model.Job_city,
                    job_type = model.Job_type
                });
            if(post>0)
            {
                return true;
            }
            return false;
        }

        public static JobList GetAllJobs(long userId)
        {
            return JobsContext.GetAllJobs(userId);
        }
        public static bool ApplicationStatus(MultipleJobsModel model)
        {
            var post = JobsContext.ApplicationStatus(new tb_multiple_jobs()
                {
                    employee_id = model.Employee_id,
                    multiple_jobs_id = model.Multiple_jobs_id,
                    applied_date = model.Applied_date,
                    registered_user_id = model.Registered_user_id,
                    jobs_details_id = model.Jobs_details_id,
                });
            if(post>0)
            {
                return true;
            }
            return false;
        }
    }
}