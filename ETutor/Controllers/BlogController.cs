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
    public class BlogController : Controller
    {
        ETutorEntities context;
        IIdentity _identity;
        UserViewModel user;

        // GET: Blog
        public ActionResult Index()
        {
            GetUser();
            ViewBag.IsAdmin = user.roleName == "Admin";
            return View();
        }

        [HttpGet]
        public ActionResult GetPost(int? Id = null)
        {
            GetUser();
            var post = new PostViewModel();
            using (context = new ETutorEntities())
            {
                post = context.Post_tbl
                    .Where(w => w.IsActive == true && (Id == null || w.Id == Id))
                    .Select(s => new PostViewModel
                    {
                        postId = s.Id,
                        description = s.PostBody,
                        title = s.PostTitle,
                        autherById = s.CreatedBy,
                        autherBy = s.User_tbl.UserName,
                        isOwner = user.userId == s.CreatedBy,
                        createdOn = s.CreateOn.ToString(),
                        comments = s.PostComment_tbl.Where(w => w.IsActive == true).Select(ss => new CommentViewModel
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
                    }).FirstOrDefault();
            }
            return new JsonResult { Data = post, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = int.MaxValue };
        }

        [HttpGet]
        public ActionResult GetFeaturePost()
        {
            GetUser();
            var post = new List<PostViewModel>();
            using (context = new ETutorEntities())
            {
                post = context.Post_tbl
                    .Where(w => w.IsActive == true)
                    .Select(s => new PostViewModel
                    {
                        postId = s.Id,
                        description = s.PostBody,
                        title = s.PostTitle,
                        autherById = s.CreatedBy,
                        isOwner = user.userId == s.CreatedBy
                    }).OrderByDescending(o => o.postId).Take(2).ToList();
            }
            return new JsonResult { Data = post, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = int.MaxValue };
        }

        [HttpGet]
        public ActionResult GetPostList()
        {
            GetUser();
            var post = new List<PostViewModel>();
            using (context = new ETutorEntities())
            {
                post = context.Post_tbl
                    .Where(w => w.IsActive == true)
                    .Select(s => new PostViewModel
                    {
                        postId = s.Id,
                        description = s.PostBody,
                        title = s.PostTitle,
                        autherById = s.CreatedBy,
                        isOwner = user.userId == s.CreatedBy
                    }).OrderByDescending(o => o.postId).ToList();
            }
            return new JsonResult { Data = post, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = int.MaxValue };
        }

        [HttpPost]
        public ActionResult CreateBlogPost(string Title,string BodyText)
        {
            GetUser();
            var result = new ResultViewModel();
            using (context = new ETutorEntities())
            {
                try
                {
                    var post = new Post_tbl
                    {
                        PostBody = BodyText,
                        PostTitle = Title,
                        CreatedBy = user.userId,
                        CreateOn = DateTime.Now,
                        UpdateOn = DateTime.Now,
                        IsActive = true,
                    };

                    context.Post_tbl.Add(post);
                    context.SaveChanges();

                    result.isSuccess = true;
                    result.message = "Save Success";
                }
                catch (Exception e)
                {
                    result.isSuccess = false;
                    result.message = e.Message;
                }
            }


            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet};
        }

        [HttpPost]
        public ActionResult EditBlogPost(string Title, string BodyText , int Id)
        {
            var result = new ResultViewModel();
            using (context = new ETutorEntities())
            {
                var Post = context.Post_tbl.Where(w => w.Id == Id).FirstOrDefault();
                try
                {
                    if (Post != null)
                    {
                        Post.PostBody = BodyText;
                        Post.PostTitle = Title;
                        Post.UpdateOn = DateTime.Now;
                        context.SaveChanges();
                    }

                    result.isSuccess = true;
                    result.message = "Update Success";
                }
                catch (Exception e)
                {
                    result.isSuccess = false;
                    result.message = e.Message;
                }
            }
            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpGet]
        public ActionResult DeleteBlogPost(int postId)
        {
            var result = new ResultViewModel();
            using (context = new ETutorEntities())
            {
                    var post = context.Post_tbl.Where(w => w.Id == postId).FirstOrDefault();
                    try
                    {
                        if (post != null)
                        {
                            post.IsActive = false;
                            post.UpdateOn = DateTime.Now;
                        }
                        context.SaveChanges();

                        result.isSuccess = true;
                        result.message = "Delete Success";
                    }
                    catch (Exception e)
                    {
                        result.isSuccess = false;
                        result.message = e.Message;
                    }

                    context.SaveChanges();
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
                    var newcomment = new PostComment_tbl
                    {
                        PostId = comment.attachmentId,
                        Comment = comment.text,
                        IsActive = true,
                        CreatedBy = user.userId,
                        CreateOn = DateTime.Now,
                        UpdateOn = DateTime.Now
                    };

                    context.PostComment_tbl.Add(newcomment);
                    context.SaveChanges();

                    comment.commentId = newcomment.Id;
                }
                catch (Exception)
                {
                }
                return new JsonResult { Data = comment, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

        }

        [HttpPost]
        public ActionResult EidtComment(CommentViewModel comment)
        {
            var result = new ResultViewModel();
            using (context = new ETutorEntities())
            {
                var postComment = context.PostComment_tbl.Where(w => w.Id == comment.commentId).FirstOrDefault();
                try
                {
                    if (postComment != null)
                    {
                        postComment.Comment = comment.text;
                        postComment.UpdateOn = DateTime.Now;
                        context.SaveChanges();
                    }

                    result.isSuccess = true;
                    result.message = "Update Success";
                }
                catch (Exception e)
                {
                    result.isSuccess = false;
                    result.message = e.Message;
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
                    var postcomment = context.PostComment_tbl.Where(w => w.Id == id).FirstOrDefault();
                    try
                    {
                        if (postcomment != null)
                        {
                            context.PostComment_tbl.Remove(postcomment);
                            context.SaveChanges();
                        }

                        result.isSuccess = true;
                        result.message = "Delete Success";
                    }
                    catch (Exception e)
                    {
                        result.isSuccess = false;
                        result.message = e.Message;
                    }

                    context.SaveChanges();
                }
            }

            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public ActionResult _Message()
        {
            return PartialView();
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