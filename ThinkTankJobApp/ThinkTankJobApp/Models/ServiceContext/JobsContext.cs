using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ThinkTankJobApp.Models.ServiceContext
{
    public static class JobsContext
    {
        #region PostJob
        public static long PostJob(JobDetail model)
        {
            using (var ctx = new JobAppDBEntities())
            {
                try
                {
                    if (model!=null)
                    {
                        model.job_date = DateTime.Now;
                        model.is_active = true;
                        ctx.JobDetails.Add(model);
                        ctx.SaveChanges();
                        return model.job_details_id;
                    }
                    return -1;
                   
                }
                catch (Exception ex)
                {
                    return -1;
                }
            }
        }

        public static JobList GetAllJobs(long userId)
        {
            JobList list = new JobList();
            using(var ctx = new JobAppDBEntities())
            {
                list.Jobs = ctx.JobDetails.Select(x => new JobDetailsModel()
                {
                Employee_id = x.employee_id,
                Job_details_id = x.job_details_id,
                Job_description = x.job_description,
                Job_city = x.job_city,
                Job_date = x.job_date,
                Job_organization = x.job_organization,
                Job_title = x.job_title,
                Job_type = x.job_type,
                Jo_date_modified = x.jo_date_modified,
                Job_applied_date = x.job_applied_date,
                Job_img = x.job_img,
                Applied = ctx.tb_multiple_jobs.Where(y=>y.registered_user_id==userId && y.jobs_details_id==x.job_details_id).Select(c => new MultipleJobsModel(){
                    Multiple_jobs_id = c.multiple_jobs_id,
                    Applied_date = c.applied_date,
                    Registered_user_id = c.registered_user_id,
                    Jobs_details_id = c.jobs_details_id,
                    Employee_id = c.employee_id
                }).FirstOrDefault()
                }).ToList();
                
            }
            if(list.Jobs.Count>0)
            {
                return list;
            }
            return new JobList();
        }

        public static long ApplicationStatus(tb_multiple_jobs jobs)
        {
            using(var ctx = new JobAppDBEntities())
            {
                var tb = ctx.tb_multiple_jobs.Where(x => x.registered_user_id == jobs.registered_user_id && x.multiple_jobs_id == jobs.multiple_jobs_id && x.jobs_details_id==jobs.jobs_details_id).FirstOrDefault();
                if(tb==null)
                {
                    jobs.applied_date = DateTime.Now;
                    ctx.tb_multiple_jobs.Add(jobs);
                    ctx.SaveChanges();
                     return jobs.multiple_jobs_id;
                }
               
            }
            return -1;
        }
        
        #endregion
    }
}