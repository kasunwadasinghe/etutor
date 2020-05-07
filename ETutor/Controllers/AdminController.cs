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
    public class AdminController : Controller
    {
        ETutorEntities context;
        IIdentity _identity;
        UserViewModel user;
        // GET: Admin
        public ActionResult Index()
        {
            GetUser();
            ViewBag.IsAdmin = user.roleName == "Admin";
            if (user.roleName != "Admin")
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        public ActionResult Dashboard(string userId)
        {
            ViewBag.UserId = userId;
            return View();
        }

        public ActionResult _Message()
        {
            return PartialView();
        }

        [HttpGet]
        public ActionResult GetAttachments()
        {
            GetUser();
            var uploadAttachments = new List<UploadAttachmentViewModel>();
            using (context = new ETutorEntities())
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
                            attachmentId = s.Id,
                            commentId = ss.Id,
                            commentById = ss.CreatedBy,
                            commentedOn = ss.UpdateOn,
                            commentBy = ss.User_tbl.UserName,
                            isOwner = user.userId == ss.CreatedBy,
                            text = ss.Comment
                        }).OrderByDescending(o => o.commentId).ToList(),
                        comment = new CommentViewModel
                        {
                            attachmentId = s.Id,
                            commentId = 0,
                            commentById = user.userId,
                            commentedOn = DateTime.Now,
                            commentBy = user.userName,
                            isOwner = true,
                            text = ""
                        }
                    }).OrderByDescending(o => o.attachmentId).ToList();
            }

            return new JsonResult { Data = uploadAttachments, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = int.MaxValue };
        }

        [HttpGet]
        public ActionResult GetMeetings(string UserId)
        {
            GetUser();
            var Meetings = new List<MeetingViewModel>();
            using (context = new ETutorEntities())
            {
                Meetings = context.User_tbl.Where(w => w.Id == UserId).SelectMany(sm => sm.Meeting_tbl2)
                    .Where(w => w.IsCanceled == false)
                    .Select(s => new MeetingViewModel
                    {
                        meetingId = s.Id,
                        description = s.Description,
                        place = s.Venue,
                        date = s.MeetingDate.ToString(),
                        time = s.MeetingDate.ToString(),
                        createdBy = s.User_tbl.UserName,
                        createdById = s.CreatedBy,
                        isOwner = user.userId == s.CreatedBy,
                    }).OrderByDescending(o => o.meetingId).ToList();
            }

            return new JsonResult { Data = Meetings, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = int.MaxValue };
        }

        [HttpGet]
        public ActionResult GetUsers()
        {
            GetUser();
            var tutors = new List<DropdownViewModel>();
            var students = new List<DropdownViewModel>();
            var userlist = new List<DropdownViewModel>();
            using (context = new ETutorEntities())
            {
                tutors = context.Role_tbl.Where(w => w.Name == "Tutor").SelectMany(s => s.UserRole_tbl).Select(s => s.User_tbl).Select(s => new DropdownViewModel
                {
                    id = s.Id,
                    label = s.UserName
                }).Distinct().ToList();

                students = context.Role_tbl.Where(w => w.Name == "Student").SelectMany(s => s.UserRole_tbl).Select(s => s.User_tbl).Select(s => new DropdownViewModel
                {
                    id = s.Id,
                    label = s.UserName
                }).Distinct().ToList();

                userlist = context.Role_tbl.Where(w => w.Name != "Admin").SelectMany(s => s.UserRole_tbl).Select(s => s.User_tbl).Select(s => new DropdownViewModel
                {
                    id = s.Id,
                    label = s.UserName
                }).Distinct().ToList();
            }

            var users = new StudentTutorViewModel();
            users.tutors = tutors;
            users.students = students;
            users.users = userlist;
            return new JsonResult { Data = users, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = int.MaxValue };
        }

        [HttpPost]
        public ActionResult SaveStudentTutor(StudentTutorViewModel studentTutor)
        {
            GetUser();
            var result = new ResultViewModel();
            using (context = new ETutorEntities())
            {
                try
                {
                    studentTutor.students.ForEach(f =>
                    {
                        var studenttutor = context.StudentTutor_tbl.Where(w => w.StudentId == f.id).FirstOrDefault();
                        if (studenttutor != null)
                        {
                            studenttutor.IsCurrent = false;
                            studenttutor.UpdatedBy = user.userId;
                            studenttutor.UpdateOn = DateTime.Now;
                        }

                        var StudentTutor = new StudentTutor_tbl
                        {
                            StudentId = f.id,
                            TutorId = studentTutor.tutorId != null ? studentTutor.tutorId.id : null,
                            CreatedBy = user.userId,
                            CreateOn = DateTime.Now,
                            UpdatedBy = user.userId,
                            UpdateOn = DateTime.Now,
                            IsCurrent = true
                        };

                        context.StudentTutor_tbl.Add(StudentTutor);
                        context.SaveChanges();
                    });

                    result.isSuccess = true;
                    result.message = "Save success";
                }
                catch (Exception e)
                {
                    result.isSuccess = false;
                    result.message = e.Message;
                }
            }
            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = int.MaxValue };
        }

        [HttpGet]
        public ActionResult GetStudentWithOutTutor()
        {
            GetUser();
            var studentwithouttutor = new List<StudentWithOutTutorViewModel>();
            using (context = new ETutorEntities())
            {
                studentwithouttutor = context.Role_tbl.Where(w => w.Name == "Student")
                    .SelectMany(s => s.UserRole_tbl)
                    .Select(s => s.User_tbl).Except(context.StudentTutor_tbl.Where(w=>w.IsCurrent == true)
                    .Select(s=>s.User_tbl3)).Select(s => new StudentWithOutTutorViewModel
                {
                    studentId = s.Id,
                    studentUserName = s.UserName
                }).ToList();

            }

            return new JsonResult { Data = studentwithouttutor, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpGet]
        public ActionResult GetNonActiveStudents(string from,string to)
        {
            GetUser();
            var documentusers = new List<UserViewModel>();
            var documentcommentusers = new List<UserViewModel>();
            var blogusers = new List<UserViewModel>();
            var blogcommentusers = new List<UserViewModel>();
            var messageusers = new List<UserViewModel>();
            var nonactivestudent = new List<UserViewModel>();

            var fromdate = DateTime.Now;
            var todate = DateTime.Now;

            var i = DateTime.TryParse(from, out fromdate);
            i = DateTime.TryParse(to, out todate);

            using (context = new ETutorEntities())
            {
                documentusers = context.Role_tbl.Where(w => w.Name == "Student").SelectMany(s => s.UserRole_tbl).Select(s => s.User_tbl).Except(context.Document_tbl.Where(w => w.UpdateOn >=fromdate && w.UpdateOn <=todate).Select(s => s.User_tbl)).Select(s => new UserViewModel
                {
                    userId = s.Id,
                    userName = s.UserName
                }).ToList();

                documentcommentusers = context.Role_tbl.Where(w => w.Name == "Student").SelectMany(s => s.UserRole_tbl).Select(s => s.User_tbl).Except(context.DocumentComment_tbl.Where(w => w.UpdateOn >= fromdate && w.UpdateOn <= todate).Select(s => s.User_tbl)).Select(s => new UserViewModel
                {
                    userId = s.Id,
                    userName = s.UserName
                }).ToList();

                blogusers = context.Role_tbl.Where(w => w.Name == "Student").SelectMany(s => s.UserRole_tbl).Select(s => s.User_tbl).Except(context.Post_tbl.Where(w => w.UpdateOn >= fromdate && w.UpdateOn <= todate).Select(s => s.User_tbl)).Select(s => new UserViewModel
                {
                    userId = s.Id,
                    userName = s.UserName
                }).ToList();

                blogcommentusers = context.Role_tbl.Where(w => w.Name == "Student").SelectMany(s => s.UserRole_tbl).Select(s => s.User_tbl).Except(context.PostComment_tbl.Where(w => w.UpdateOn >= fromdate && w.UpdateOn <= todate).Select(s => s.User_tbl)).Select(s => new UserViewModel
                {
                    userId = s.Id,
                    userName = s.UserName
                }).ToList();

                messageusers = context.Role_tbl.Where(w => w.Name == "Student").SelectMany(s => s.UserRole_tbl).Select(s => s.User_tbl).Except(context.Message_tbl.Where(w => w.UpdateOn >= fromdate && w.UpdateOn <= todate).Select(s => s.User_tbl)).Select(s => new UserViewModel
                {
                    userId = s.Id,
                    userName = s.UserName
                }).ToList();

                nonactivestudent = documentusers
                    .Join(documentcommentusers, d => d.userId, dc => dc.userId, (d, dc) => new { document = d, documentcomment = dc })
                    .Join(blogusers, d => d.document.userId, b => b.userId, (d, b) => new { document = d, blog = b })
                    .Join(blogcommentusers, b => b.blog.userId, bc => bc.userId, (b, bc) => new { blog = b, blogcomment = bc })
                    .Join(messageusers,b=>b.blogcomment.userId,m=>m.userId,(b,m)=>new UserViewModel {userId=m.userId,userName=m.userName})
                    .ToList();
            }

            return new JsonResult { Data = nonactivestudent, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpGet]
        public ActionResult GetMessageHistory()
        {
            GetUser();
            var messagehistory = new List<MessageHistoryViewModel>();
            var weekbefore = DateTime.Now.AddDays(-7);

            using (context = new ETutorEntities())
            {
                messagehistory = context.User_tbl.Select(s => new MessageHistoryViewModel
                {
                    UserId = s.Id,
                    UserName = s.UserName,
                    UserType = s.UserRole_tbl.Select(ss => ss.Role_tbl.Name).FirstOrDefault(),
                    overallNoMessage = s.Message_tbl2.Where(w => w.CreatedBy != s.Id && w.IsActive == true ).Count(),
                    last7NoMessages = s.Message_tbl2.Where(w=>w.CreatedBy != s.Id && w.IsActive == true && w.UpdateOn >= weekbefore).Count()
                }).ToList();
            }

            return new JsonResult { Data = messagehistory, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpGet]
        public ActionResult GetMessageForPersonalTutor()
        {
            GetUser();
            var messagehistory = new List<MessageHistoryViewModel>();
            var weekbefore = DateTime.Now.AddDays(-7);

            using (context = new ETutorEntities())
            {
                messagehistory = context.StudentTutor_tbl.Where(w => w.IsCurrent == true)
                    .Join(context.Message_tbl, s => s.StudentId, m => m.CreatedBy, (s, m) => new MessageHistoryViewModel
                    {
                        student = s.User_tbl3.UserName,
                        studentId = s.User_tbl3.Id,
                        tutor = s.User_tbl2.UserName,
                        tutorId = s.User_tbl2.Id,
                        overallNoMessage = s.User_tbl3.Message_tbl2.Count(),
                        tutorMessages = m.User_tbl2.Where(w => w.Id == s.TutorId).Count(),
                        avgNoMessages = (m.User_tbl2.Where(w => w.Id == s.TutorId).Count() != 0 && s.User_tbl3.Message_tbl2.Count() !=0 ?  (m.User_tbl2.Where(w => w.Id == s.TutorId).Count() / s.User_tbl3.Message_tbl2.Count()) : 0)
                    }).ToList();

                messagehistory = messagehistory.GroupBy(g => g.studentId).Select(s => new MessageHistoryViewModel
                {
                    student = s.Select(ss => ss.studentId).FirstOrDefault(),
                    studentId = s.Select(ss => ss.studentId).FirstOrDefault(),
                    tutor = s.Select(ss => ss.tutor).FirstOrDefault(),
                    tutorId = s.Select(ss=>ss.tutorId).FirstOrDefault(),
                    overallNoMessage = s.Select(ss => ss.overallNoMessage).FirstOrDefault(),
                    tutorMessages = s.Sum(ss => ss.tutorMessages),
                }).ToList();
            }

            return new JsonResult { Data = messagehistory, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpGet]
        public ActionResult GetMeetings(DropdownViewModel selecteduser)
        {
            GetUser();
            var Meetings = new List<MeetingViewModel>();
            using (context = new ETutorEntities())
            {
                Meetings = context.User_tbl.Where(w => w.Id == selecteduser.id).SelectMany(sm => sm.Meeting_tbl2)
                    .Where(w => w.IsCanceled == false)
                    .Select(s => new MeetingViewModel
                    {
                        meetingId = s.Id,
                        description = s.Description,
                        place = s.Venue,
                        date = s.MeetingDate.ToString(),
                        time = s.MeetingDate.ToString(),
                        createdBy = s.User_tbl.UserName,
                        createdById = s.CreatedBy,
                        isOwner = user.userId == s.CreatedBy,
                        members = s.User_tbl2.Select(ss => new DropdownViewModel
                        {
                            id = ss.Id,
                            label = ss.UserName
                        }).ToList()
                    }).OrderByDescending(o => o.meetingId).ToList();
            }

            return new JsonResult { Data = Meetings, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = int.MaxValue };
        }

        #region private
        private void GetUser()
        {
            _identity = User.Identity;
            using (context = new ETutorEntities())
            {
                user = context.User_tbl.Where(w => w.UserName == _identity.Name).Select(s => new UserViewModel
                {
                    userId = s.Id,
                    userName = s.UserName,
                    email = s.Email,
                    roleId = s.UserRole_tbl.Select(ss => ss.Role_tbl).FirstOrDefault().Id,
                    roleName = s.UserRole_tbl.Select(ss => ss.Role_tbl).FirstOrDefault().Name
                }).FirstOrDefault();
            }
        }
        #endregion
    }
}
