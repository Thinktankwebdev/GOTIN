using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ThinkTankJobApp.Models;
using ThinkTankJobApp.Models.ServiceProviders;
using PagedList;

namespace ThinkTankJobApp.Controllers
{
    public class JobsController : Controller
    {
        //
        // GET: /Jobs/

        public ActionResult Index(string search, string city,int? page)
        {
            long userId = 0;
            if(User.Identity.IsAuthenticated)
            {
                userId = RegistrationProvider.GetUseridfromUsername(User.Identity.Name);
            }
            int pageSize = 5;
            int pageIndex = 1;

            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            JobList model = new JobList();
            model = JobsProvider.GetAllJobs(userId);
            Session["ListOfJobs"] = model;
            
            JobList mod = new JobList();
            mod.Jobs = new List<JobDetailsModel>();
            var list = model.Jobs.Where(x => x.Applied == null).ToList();
            if((search!=null && search !="")&&(city!=null && city!=""))
            {
                mod.Jobs= list.Where(x => x.Job_city == city && x.Job_title.Contains(search)).OrderByDescending(x=>x.Job_details_id).ToList();
            }
            else if(search!=null && search !="")
            {
                mod.Jobs = list.Where(x => x.Job_title.Contains(search)).OrderByDescending(x => x.Job_details_id).ToList();
            }
            else if(city!=null && city!="")
            {
                mod.Jobs = list.Where(x => x.Job_city == city).OrderByDescending(x => x.Job_details_id).ToList();
            }
            else
            {
                mod.Jobs = list.ToList();
            }
            return View(mod.Jobs.ToPagedList(pageIndex, pageSize));
        }

       
        public ActionResult Details(string ID)
        {
            long userId = 0;
            if (User.Identity.IsAuthenticated)
            {
                userId = RegistrationProvider.GetUseridfromUsername(User.Identity.Name);
            }
            var id = long.Parse(ID);
            JobList model = new JobList();
            if (Session["ListOfJobs"] != null)
            {
                model = (JobList)Session["ListOfJobs"];

            }
            else
            {
                model = JobsProvider.GetAllJobs(userId);

                Session["ListOfJobs"] = model;
            }
            var result = model.Jobs.Where(x => x.Job_details_id == id).FirstOrDefault();
            return View(result);
        }
        public ActionResult PostJobs()
        {
            return View();
        }
        [HttpPost]
        public ActionResult PostJobs(JobDetailsModel model)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (model != null)
                {
                    model.username = User.Identity.Name;
                    var res = JobsProvider.PostJob(model);
                    if (res)
                    {
                        TempData["Success"] = "Successfully Posted a Job!"; 
                        return Json(new { success = true, message = "Successfully Posted a Job!" }, JsonRequestBehavior.DenyGet);
                    }

                }
            }
            TempData["Error"] = "Failed! Please try again";
            return Json(new { success = true, message = "Failed! Please try again" }, JsonRequestBehavior.DenyGet);
        }

        public ActionResult SearchJobsPost()
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

            return PartialView("_MainSearchJobs", model.Jobs);
        }

        public ActionResult ListofServices(int? page)
        {
            long userId = 0;
            if (User.Identity.IsAuthenticated)
            {
                userId = RegistrationProvider.GetUseridfromUsername(User.Identity.Name);
            }
            int pageSize = 5;
            int pageIndex = 1;
            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            JobList model = new JobList();
            if (Session["ListOfJobs"] != null)
            {
                model = (JobList)Session["ListOfJobs"];
            }
            else
            {
                model = JobsProvider.GetAllJobs(userId);
                Session["ListOfJobs"] = model;
            }
            model.Jobs = model.Jobs.OrderByDescending(x => x.Job_details_id).ToList();
            //ServiceDomainModel domains = new ServiceDomainModel();
            // domains = model.ServiceDomain.Where(x => x.Service_domain_id == domainid).FirstOrDefault();
            return PartialView("_Employers", model.Jobs.ToPagedList(pageIndex, pageSize));
        }

        public ActionResult FilterList(string jobId, string city, int?page)
        {
            long userId = 0;
            if (User.Identity.IsAuthenticated)
            {
                userId = RegistrationProvider.GetUseridfromUsername(User.Identity.Name);
            }
            int pageSize = 5;
            int pageIndex = 1;
            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            JobList model = new JobList();
            if (Session["ListOfJobs"] != null)
            {
                model = (JobList)Session["ListOfJobs"];
            }
            else
            {
                model = JobsProvider.GetAllJobs(userId);
                Session["ListOfJobs"] = model;
            }
            model.Jobs = model.Jobs.Where(x=>x.Job_details_id==Convert.ToInt32(jobId) || x.Job_city==city).OrderByDescending(x => x.Job_details_id).ToList();
            return PartialView("_Employers", model.Jobs.ToPagedList(pageIndex, pageSize));
        }
        [HttpPost]
        public ActionResult ApplyJobs(MultipleJobsModel model)
        {
            long userId = 0;
            if (User.Identity.IsAuthenticated)
            {
                userId = RegistrationProvider.GetUseridfromUsername(User.Identity.Name);
                if (model != null)
                {
                    model.Registered_user_id = userId;
                    model.Applied_date = DateTime.Now;
                    var res = JobsProvider.ApplicationStatus(model);
                    if(res)
                    {
                        TempData["Success"] = "Job has been applied we may get back to you.";
                        return Json(new { title = "Favrequest", success = true, message="S"}, JsonRequestBehavior.DenyGet);
                    }
                    TempData["Error"] = "Failed.";
                    return Json(new { title = "Favrequest", success = true, message = "F" }, JsonRequestBehavior.DenyGet);
                }
            }
            return Json(new { title = "Favrequest", success = true, message="NA" }, JsonRequestBehavior.DenyGet);
            
        }

        public ActionResult AppliedJobs(int? page)
        {
            long userId = 0;
            if (User.Identity.IsAuthenticated)
            {
                userId = RegistrationProvider.GetUseridfromUsername(User.Identity.Name);
            }
            int pageSize = 5;
            int pageIndex = 1;

            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            JobList model = new JobList();
            model = JobsProvider.GetAllJobs(userId);
            Session["ListOfJobs"] = model;

            JobList mod = new JobList();
            mod.Jobs = new List<JobDetailsModel>();
            var list = model.Jobs.Where(x => x.Applied != null).ToList();
            return View(list.ToPagedList(pageIndex, pageSize));
        }


    }
}
