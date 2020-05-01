using ETutor.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;

namespace ETutor.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {

        ETutorEntities context;
        IIdentity _identity;
        UserViewModel user;

        public ActionResult Index()
        {
            GetUser();
            return View();
        }

        [HttpGet]
        public ActionResult GetAttachments()
        {
            GetUser();
            var uploadAttachments = new List<UploadAttachmentViewModel>();
            using(context = new ETutorEntities())
            {
                uploadAttachments = context.Document_tbl
                    .Where(w => w.IsActive == true)
                    .Select(s => new UploadAttachmentViewModel
                    {
                        attachmentId = s.Id,
                        description = s.Description,
                        name = s.FileName,
                        uploadById = s.CreatedBy,
                        isOwner = user.userId == s.CreatedBy,
                        comments = s.DocumentComment_tbl.Where(w => w.IsActive == true).Select(ss => new CommentViewModel
                        {
                            commentId = ss.Id,
                            commentById = ss.CreatedBy,
                            commentedOn = ss.UpdateOn,
                            commentBy = ss.User_tbl.UserName,
                            isOwner = user.userId == ss.CreatedBy,
                            text = ss.Comment
                        }).ToList()
                    }).ToList();
            }

            return new JsonResult { Data = uploadAttachments, JsonRequestBehavior = JsonRequestBehavior.AllowGet , MaxJsonLength = int.MaxValue };
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        #region private
        private void GetUser()
        {
            _identity = User.Identity;
            using(context = new ETutorEntities())
            {
                user = context.User_tbl.Where(w => w.UserName == _identity.Name).Select(s => new UserViewModel
                {
                    userId = s.Id,
                    userName = s.UserName,
                    email = s.Email,
                    roleId = s.UserRole_tbl.Select(ss=>ss.Role_tbl).FirstOrDefault().Id,
                    roleName = s.UserRole_tbl.Select(ss=>ss.Role_tbl).FirstOrDefault().Name
                }).FirstOrDefault();
            }
        }
        #endregion
    }
}