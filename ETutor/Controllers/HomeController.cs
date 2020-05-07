using ETutor.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Web.Mvc;
using ETutor.Common.Mail;
using ETutor.Common;

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
            ViewBag.IsAdmin = user.roleName == "Admin";
            if (user.roleName == "Admin")
            {
                return RedirectToAction("Index", "Admin");
            }
            return View();
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
                        uploadBy = s.User_tbl.UserName,
                        isOwner = user.userId == s.CreatedBy || user.roleName == "Admin",
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
        public ActionResult GetPermission()
        {
            GetUser();
            var isTutor = user.roleName == "Tutor";

            return new JsonResult { Data = isTutor, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = int.MaxValue };
        }

        [HttpPost]
        public ActionResult UploadFiles(object obj)
        {
            GetUser();
            var result = new ResultViewModel();
            try
            {
                var length = Request.ContentLength;
                var bytes = new byte[length];
                Request.InputStream.Read(bytes, 0, length);

                var fileName = Request.Headers["X-File-Name"];
                var fileSize = Request.Headers["X-File-Size"];
                var fileType = Request.Headers["X-File-Type"];
                var fileDescription = Request.Headers["X-File-Description"];

                using (context = new ETutorEntities())
                {
                    var document = new Document_tbl
                    {
                        FileName = fileName,
                        IsActive = true,
                        CreatedBy = user.userId,
                        CreateOn = DateTime.Now,
                        Description = fileDescription,
                        UpdateOn = DateTime.Now
                    };

                    context.Document_tbl.Add(document);
                    context.SaveChanges();
                }
                var saveToFileLoc = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/UploadFile/"), fileName);
                var fileStream = new FileStream(saveToFileLoc, FileMode.Create, FileAccess.ReadWrite);
                fileStream.Write(bytes, 0, length);
                fileStream.Close();

                result.isSuccess = true;
                result.message = "Upload Success";
            }
            catch (Exception e)
            {
                result.isSuccess = false;
                result.message = e.Message;
            }

            return new JsonResult { Data = "", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpGet]
        public HttpResponseMessage DownLoadFile(string FileName, string fileType)
        {
            Byte[] bytes = null;
            if (FileName != null)
            {
                string filePath = Path.GetFullPath(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.InternetCache), FileName));
                FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fs);
                bytes = br.ReadBytes((Int32)fs.Length);
                br.Close();
                fs.Close();
            }

            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            System.IO.MemoryStream stream = new MemoryStream(bytes);
            result.Content = new StreamContent(stream);
            result.Content.Headers.ContentType = new MediaTypeHeaderValue(fileType);
            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = FileName
            };
            return (result);
        }

        [HttpGet]
        public ActionResult GetUsers()
        {
            GetUser();
            var users = new List<DropdownViewModel>();
            using (context = new ETutorEntities())
            {
                users = context.Role_tbl.Where(w => w.Name != "Admin").SelectMany(s => s.UserRole_tbl).Where(w => w.IdentityUser_Id != user.userId).Select(s => s.User_tbl).Select(s => new DropdownViewModel
                {
                    id = s.Id,
                    label = s.UserName
                }).Distinct().ToList();
            }
            return new JsonResult { Data = users, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = int.MaxValue };
        }

        [HttpPost]
        public ActionResult EidtAttachment(UploadAttachmentViewModel attachment)
        {
            var result = new ResultViewModel();
            using (context = new ETutorEntities())
            {
                var document = context.Document_tbl.Where(w => w.Id == attachment.attachmentId).FirstOrDefault();
                try
                {
                    if (document != null)
                    {
                        document.Description = attachment.description;
                        document.UpdateOn = DateTime.Now;
                        context.SaveChanges();
                    }

                    result.isSuccess = true;
                    result.message = "Update Success";
                }
                catch (Exception)
                {
                    result.isSuccess = false;
                    result.message = "Update fail";
                }
            }

            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpGet]
        public ActionResult DeleteAttachment(string attachmentId)
        {
            var result = new ResultViewModel();
            using (context = new ETutorEntities())
            {
                var id = 0;
                if (int.TryParse(attachmentId, out id))
                {
                    var document = context.Document_tbl.Where(w => w.Id == id).FirstOrDefault();
                    try
                    {
                        if (document != null)
                        {
                            document.IsActive = false;
                            document.UpdateOn = DateTime.Now;
                        }
                        context.SaveChanges();

                        result.isSuccess = true;
                        result.message = "Delete Success";
                    }
                    catch (Exception)
                    {
                        result.isSuccess = false;
                        result.message = "Delete fail";
                    }

                    context.SaveChanges();
                }
            }

            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpPost]
        public ActionResult EidtComment(CommentViewModel comment)
        {
            var result = new ResultViewModel();
            using (context = new ETutorEntities())
            {
                var documentcoment = context.DocumentComment_tbl.Where(w => w.Id == comment.commentId).FirstOrDefault();
                try
                {
                    if (documentcoment != null)
                    {
                        documentcoment.Comment = comment.text;
                        documentcoment.UpdateOn = DateTime.Now;
                        context.SaveChanges();
                    }

                    result.isSuccess = true;
                    result.message = "Update Success";
                }
                catch (Exception)
                {
                    result.isSuccess = false;
                    result.message = "Update fail";
                }
            }

            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpGet]
        public ActionResult DeleteComment(string commentId)
        {
            var result = new ResultViewModel();
            using (context = new ETutorEntities())
            {
                var id = 0;

                if (int.TryParse(commentId, out id))
                {
                    var documentcomment = context.DocumentComment_tbl.Where(w => w.Id == id).FirstOrDefault();
                    try
                    {
                        if (documentcomment != null)
                        {
                            context.DocumentComment_tbl.Remove(documentcomment);
                            context.SaveChanges();
                        }

                        result.isSuccess = true;
                        result.message = "Delete Success";
                    }
                    catch (Exception)
                    {
                        result.isSuccess = false;
                        result.message = "Delete fail";
                    }

                    context.SaveChanges();
                }
            }

            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpPost]
        public ActionResult AddComment(CommentViewModel comment)
        {
            GetUser();
            using (context = new ETutorEntities())
            {
                try
                {
                    var newcomment = new DocumentComment_tbl
                    {
                        DocumentId = comment.attachmentId,
                        Comment = comment.text,
                        IsActive = true,
                        CreatedBy = user.userId,
                        CreateOn = DateTime.Now,
                        UpdateOn = DateTime.Now
                    };

                    context.DocumentComment_tbl.Add(newcomment);
                    context.SaveChanges();

                    comment.commentId = newcomment.Id;
                }
                catch (Exception)
                {
                }
                return new JsonResult { Data = comment, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

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

        [HttpPost]
        public ActionResult AddMeeting(MeetingViewModel meeting)
        {
            GetUser();
            using (context = new ETutorEntities())
            {
                try
                {
                    var Date = DateTime.Parse(meeting.date);
                    var Time = DateTime.Parse(meeting.time);
                    var MeetingDate = new DateTime(Date.Year, Date.Month, Date.Day, Time.Hour, Time.Minute, 0);
                    var Members = meeting.members.Select(s => s.id).ToList();
                    var newmeeting = new Meeting_tbl
                    {
                        Description = meeting.description,
                        Venue = meeting.place,
                        MeetingDate = MeetingDate,
                        IsCanceled = false,
                        CreatedBy = user.userId,
                        UpdatedBy = user.userId,
                        CreateOn = DateTime.Now,
                        UpdateOn = DateTime.Now,

                    };
                    newmeeting.User_tbl2 = context.User_tbl.Where(w => Members.Contains(w.Id)).ToList();
                    newmeeting.User_tbl2.Add(context.User_tbl.Where(w => w.Id == user.userId).First());
                    context.Meeting_tbl.Add(newmeeting);
                    context.SaveChanges();

                    meeting.meetingId = newmeeting.Id;
                    meeting.date = newmeeting.MeetingDate.ToString();
                    meeting.createdBy = user.userName;
                    meeting.members = context.User_tbl.Where(w => Members.Contains(w.Id)).Select(s => new DropdownViewModel
                    {
                        id = s.Id,
                        label = s.UserName
                    }).ToList();

                    var mailhistory = new Mail_tbl
                    {
                        FromId = user.userId,
                        Subject = "New meeting arrangement",
                        MailBody = "You have a new meeting arrangement",
                        IsFail = false,
                        CreateOn = DateTime.Now,
                        TriggerBy = "Meeting"
                    };
                    context.Mail_tbl.Add(mailhistory);
                    context.SaveChanges();

                    var mailId = mailhistory.Id;


                    newmeeting.User_tbl2.ToList().ForEach(f =>
                    {
                        var isFail = false;
                        try
                        {
                            Mail.SendMail(f.UserName, user.userName, "New meeting arrangement", "You have a new meeting arrangement");
                        }
                        catch (Exception ex)
                        {
                            isFail = true;
                        }
                        finally
                        {
                            var mailrecipt = new MailRecipit_tbl
                            {
                                IsFail = isFail,
                                MailId = mailId,
                                ToId = f.Id
                            };

                            context.MailRecipit_tbl.Add(mailrecipt);
                        }

                    });

                    context.SaveChanges();
                }
                catch (Exception e)
                {
                    Logger.log(e.Source,e.Message);
                }
                return new JsonResult { Data = meeting, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }

        [HttpGet]
        public ActionResult GetMeetings()
        {
            GetUser();
            var Meetings = new List<MeetingViewModel>();
            using (context = new ETutorEntities())
            {
                Meetings = context.User_tbl.Where(w => w.Id == user.userId).SelectMany(sm => sm.Meeting_tbl2)
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

        [HttpGet]
        public ActionResult CancelMeetings(int Id)
        {
            GetUser();
            var result = new ResultViewModel();

            using (context = new ETutorEntities())
            {
                try
                {
                    var meeting = context.Meeting_tbl.Where(w => w.Id == Id).FirstOrDefault();
                    if (meeting != null)
                    {
                        meeting.IsCanceled = true;
                        meeting.UpdateOn = DateTime.Now;

                        context.SaveChanges();

                        result.isSuccess = true;
                        result.message = "Cancel Success !";
                    }
                }
                catch (Exception e)
                {
                    result.isSuccess = false;
                    result.message = e.Message;
                }
            }

            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public ActionResult _Message()
        {
            return PartialView();
        }

        [HttpGet]
        public ActionResult GetMessages()
        {
            GetUser();
            var Messages = new List<MessageViewModel>();
            using (context = new ETutorEntities())
            {
                Messages = context.User_tbl.Where(w => w.Id == user.userId).SelectMany(sm => sm.Message_tbl2)
                    .Where(w => w.IsActive == true)
                    .Select(s => new MessageViewModel
                    {
                        messageId = s.Id,
                        description = s.Description,
                        sendBy = s.User_tbl.UserName,
                        sendById = s.CreatedBy,
                        isOwner = user.userId == s.CreatedBy,
                        date = s.CreateOn.ToString(),
                        members = s.User_tbl2.Where(w => w.Id != s.CreatedBy).Select(ss => new DropdownViewModel
                        {
                            id = ss.Id,
                            label = ss.UserName
                        }).ToList()
                    }).OrderByDescending(o => o.messageId).ToList();
            }

            return new JsonResult { Data = Messages, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = int.MaxValue };
        }

        [HttpPost]
        public ActionResult AddMessage(MessageViewModel message)
        {
            GetUser();
            using (context = new ETutorEntities())
            {
                try
                {
                    var Members = message.members.Select(s => s.id).ToList();
                    var newmessage = new Message_tbl
                    {
                        Description = message.description,
                        IsActive = true,
                        CreatedBy = user.userId,
                        UpdatedBy = user.userId,
                        CreateOn = DateTime.Now,
                        UpdateOn = DateTime.Now,

                    };
                    newmessage.User_tbl2 = context.User_tbl.Where(w => Members.Contains(w.Id)).ToList();
                    newmessage.User_tbl2.Add(context.User_tbl.Where(w => w.Id == user.userId).First());
                    context.Message_tbl.Add(newmessage);
                    context.SaveChanges();

                    var mailhistory = new Mail_tbl
                    {
                        FromId = user.userId,
                        Subject = "New Message",
                        MailBody = "You have a new message",
                        IsFail = false,
                        CreateOn = DateTime.Now,
                        TriggerBy = "Message"
                    };
                    context.Mail_tbl.Add(mailhistory);
                    context.SaveChanges();

                    var mailId = mailhistory.Id;

                    
                    newmessage.User_tbl2.ToList().ForEach(f =>
                    {
                        var isFail = false;
                        try
                        {
                            Mail.SendMail(f.UserName, user.userName, "New Message", "You have a new message");
                        }
                        catch (Exception ex)
                        {
                            isFail = true;
                        }
                        finally
                        {
                            var mailrecipt = new MailRecipit_tbl
                            {
                                IsFail = isFail,
                                MailId = mailId,
                                ToId = f.Id
                            };

                            context.MailRecipit_tbl.Add(mailrecipt);
                        }
                        
                    });

                    context.SaveChanges();


                    message.messageId = newmessage.Id;
                    message.date = newmessage.CreateOn.ToString();
                    message.sendBy = user.userName;
                    message.sendById = user.userId;
                    message.members = context.User_tbl.Where(w => Members.Contains(w.Id)).Select(s => new DropdownViewModel
                    {
                        id = s.Id,
                        label = s.UserName
                    }).ToList();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                return new JsonResult { Data = message, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }

        [HttpGet]
        public ActionResult DeleteMessage(string Id)
        {
            GetUser();
            var response = new ResultViewModel();
            using (context = new ETutorEntities())
            {
                try
                {
                    var messageId = 0;
                    if (int.TryParse(Id, out messageId))
                    {
                        var message = context.Message_tbl.Where(w => w.Id == messageId).FirstOrDefault();
                        if (message != null)
                        {

                            message.IsActive = false;
                            message.UpdatedBy = user.userId;
                            message.UpdateOn = DateTime.Now;

                            context.SaveChanges();

                            response.isSuccess = true;
                            response.message = "Deleate success !!";
                        }
                    }
                }
                catch (Exception e)
                {
                    response.isSuccess = false;
                    response.message = e.Message;
                }
            }

            return new JsonResult { Data = response, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = int.MaxValue };
        }

        [HttpPost]
        public ActionResult AddMeetingMinute(MeetingMinutesViewModel meetingminute)
        {
            GetUser();
            using (context = new ETutorEntities())
            {
                try
                {
                    var newmeetingmiute = new MeetingMinits_tbl
                    {
                        MeetingId = meetingminute.meetingId,
                        Description = meetingminute.description,
                        IsActive = true,
                        CreatedBy = user.userId,
                        UpdatedBy = user.userId,
                        CreateOn = DateTime.Now,
                        UpdateOn = DateTime.Now
                    };
                    newmeetingmiute.User_tbl2 = context.User_tbl.Where(w => w.Id == meetingminute.presenter.id).ToList();
                    context.MeetingMinits_tbl.Add(newmeetingmiute);
                    context.SaveChanges();

                    meetingminute.meetingMinuteId = newmeetingmiute.Id;
                    meetingminute.presenter = context.User_tbl.Where(w => w.Id == meetingminute.presenter.id).Select(s => new DropdownViewModel
                    {
                        id = s.Id,
                        label = s.UserName
                    }).FirstOrDefault();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                return new JsonResult { Data = meetingminute, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }

        [HttpPost]
        public ActionResult EidtMeetingMinute(MeetingMinutesViewModel meetingminute)
        {
            GetUser();
            var result = new ResultViewModel();
            using (context = new ETutorEntities())
            {
                var meetingMinute = context.MeetingMinits_tbl.Where(w => w.Id == meetingminute.meetingMinuteId).FirstOrDefault();
                try
                {
                    if (meetingMinute != null)
                    {
                        meetingMinute.Description = meetingminute.description;
                        if(!meetingMinute.User_tbl2.Any(w=>w.Id == meetingminute.presenter.id))
                            meetingMinute.User_tbl2 = context.User_tbl.Where(w=>w.Id == meetingminute.presenter.id).ToList();
                        meetingMinute.UpdateOn = DateTime.Now;
                        meetingMinute.UpdatedBy = user.userId;
                        context.SaveChanges();
                    }

                    result.isSuccess = true;
                    result.message = "Update Success";
                }
                catch (Exception e)
                {
                    result.isSuccess = false;
                    result.message = "Update fail";
                }
            }

            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpGet]
        public ActionResult DeleteMeetingMinute(int Id)
        {
            GetUser();
            var response = new ResultViewModel();
            using (context = new ETutorEntities())
            {
                try
                {
                    var meetingMinute = context.MeetingMinits_tbl.Where(w => w.Id == Id).FirstOrDefault();
                    if (meetingMinute != null)
                    {

                        meetingMinute.IsActive = false;
                        meetingMinute.UpdatedBy = user.userId;
                        meetingMinute.UpdateOn = DateTime.Now;

                        context.SaveChanges();

                        response.isSuccess = true;
                        response.message = "Deleate success !!";
                    }
                }
                catch (Exception e)
                {
                    response.isSuccess = false;
                    response.message = e.Message;
                }
            }

            return new JsonResult { Data = response, JsonRequestBehavior = JsonRequestBehavior.AllowGet};
        }

        [HttpGet]
        public ActionResult GetMeetingMinutes(int meetingId)
        {
            GetUser();
            var MeetingMinutes = new List<MeetingMinutesViewModel>();
            using (context = new ETutorEntities())
            {
                MeetingMinutes = context.User_tbl.SelectMany(sm => sm.MeetingMinits_tbl2)
                    .Where(w => w.IsActive == true && w.MeetingId == meetingId)
                    .Select(s => new MeetingMinutesViewModel
                    {
                        meetingMinuteId = s.Id,
                        meetingId = s.MeetingId,
                        description = s.Description,
                        isEdit =false,
                        presenter = s.User_tbl2.Select(ss=>new DropdownViewModel
                        {
                            id = ss.Id,
                            label = ss.UserName,
                        }).FirstOrDefault()
                    }).OrderByDescending(o => o.meetingId).ToList();
            }

            return new JsonResult { Data = MeetingMinutes, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = int.MaxValue };
        }

        [HttpGet]
        public ActionResult GetPresenters(int meetingId)
        {
            var presenters = new List<DropdownViewModel>();
            using (context = new ETutorEntities())
            {
                presenters = context.Meeting_tbl
                    .Where(w => w.IsCanceled == false && w.Id == meetingId).SelectMany(s=>s.User_tbl2)
                    .Select(s => new DropdownViewModel
                    {
                        id = s.Id,
                        label = s.UserName
                    }).ToList();
            }

            return new JsonResult { Data = presenters, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = int.MaxValue };
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