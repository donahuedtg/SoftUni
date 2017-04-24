using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MyBlog.Models;
using MyBlog.ViewModels;
using MyBlog.Extensions;

namespace MyBlog.Controllers
{
    [Authorize]
    public class ArticlesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Articles
        [AllowAnonymous]
        public ActionResult Index()
        {
            IEnumerable<Article> data = db.Articles.Include(x => x.Author);
            List<ArticleView> list = ArticleView.ArticleList(data);

            return View(list);
        }

        // GET: Articles/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Article article = db.Articles.Find(id);

            if (article == null)
            {
                return HttpNotFound();
            }

            ArticleView articleView = ArticleView.ArticleData(article);

            string userName = User.Identity.Name;
            int currentAuthorId = GetCurrentAuthorId(userName);
            ViewBag.CurrentAuthorId = currentAuthorId;

            return View(articleView);
        }

        // GET: Articles/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Articles/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Content")] ArticleView article)
        {
            if (ModelState.IsValid)
            {
                Article tmp = new Article();
                tmp.Title = article.Title;
                tmp.Content = article.Content;
                string userName = User.Identity.Name;
                int authorId = GetCurrentAuthorId(userName);
                tmp.AuthorId = authorId;

                db.Articles.Add(tmp);
                db.SaveChanges();

                string message = string.Format($"Статия {tmp.Id} е записана успешно!");
                this.AddNotification(message, NotificationType.SUCCESS);
                return RedirectToAction("Index");
            }

            string messageError = string.Format($"Статията не беше записана, моля проверете данните!");
            this.AddNotification(messageError, NotificationType.ERROR);
            return View(article);
        }

        // GET: Articles/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            bool isAdmin = IsAdmin();

            if (isAdmin)
            {
                Article article = db.Articles.Find(id);
                ArticleView articleView = ArticleView.ArticleData(article);

                if (article == null)
                {
                    return HttpNotFound();
                }
                return View(articleView);
            }
            else
            {
                bool isAuthor = IsAuthor((int)id);

                if (!isAuthor)
                {
                    return RedirectToAction("Index");
                }

                Article article = db.Articles.Find(id);
                ArticleView articleView = ArticleView.ArticleData(article);

                if (article == null)
                {
                    return HttpNotFound();
                }
                return View(articleView);
            }

        }

        // POST: Articles/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Content")] ArticleView article)
        {
            if (ModelState.IsValid)
            {
                Article articleForEdit = db.Articles.Find(article.Id);
                articleForEdit.Title = article.Title;
                articleForEdit.Content = article.Content;


                db.Entry(articleForEdit).State = EntityState.Modified;
                db.SaveChanges();

                string message = string.Format($"Статия {article.Id} е редактирана успешно!");
                this.AddNotification(message, NotificationType.SUCCESS);
                return RedirectToAction("Index");
            }

            string messageError = string.Format($"Статия {article.Id} не беше редактирана, моля проверете данните!");
            this.AddNotification(messageError, NotificationType.ERROR);
            return View(article);
        }

        //// GET: Articles/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }

        //    bool isAuthor = IsAuthor((int)id);

        //    if (!isAuthor)
        //    {
        //        return RedirectToAction("Index");
        //    }

        //    Article article = db.Articles.Find(id);
        //    if (article == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(article);
        //}

        // POST: Articles/Delete/5
        [HttpPost]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            bool isAdmin = IsAdmin();

            if (isAdmin)
            {
                Article article = db.Articles.Find(id);

                if (article == null)
                {
                    return HttpNotFound();
                }

                db.Articles.Remove(article);
                db.SaveChanges();

                string message = string.Format($"Статия {id} е изтрита успешно!");
                this.AddNotification(message, NotificationType.SUCCESS);

                return RedirectToAction("Index");
            }
            else
            {
                bool isAuthor = IsAuthor((int)id);

                if (!isAuthor)
                {
                    return RedirectToAction("Index");
                }

                Article article = db.Articles.Find(id);

                if (article == null)
                {
                    return HttpNotFound();
                }

                db.Articles.Remove(article);
                db.SaveChanges();

                string message = string.Format($"Статия {id} е изтрита успешно!");
                this.AddNotification(message, NotificationType.SUCCESS);

                return RedirectToAction("Index");
            }


        }

        [HttpPost]
        public ActionResult DeleteComment(int? id, int? articleId)
        {
            if (id == null || articleId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            bool isAdmin = IsAdmin();

            if (isAdmin)
            {
                Comment comment = db.Comments.Find(id);

                if (comment == null)
                {
                    return HttpNotFound();
                }

                db.Comments.Remove(comment);
                db.SaveChanges();

                string message = string.Format($"Коментар {id} е изтрит успешно!");
                this.AddNotification(message, NotificationType.SUCCESS);

                return RedirectToAction("Details", "Articles", new { id = articleId });
            }

            string messageError = string.Format($"Коментар {id} не е изтрит успешно! Нямате права.");
            this.AddNotification(messageError, NotificationType.ERROR);
            return RedirectToAction("Index");
        }

        //AjaxAddComments
        public ActionResult AddNewComment(int id, string text)
        {
            string userName = User.Identity.Name;
            int currentAuthorId = GetCurrentAuthorId(userName);
            ViewBag.CurrentAuthorId = currentAuthorId;

            Comment tmp = new Comment();
            tmp.CommentText = text;
            tmp.AuthorId = currentAuthorId;
            tmp.ArticleId = id;

            db.Comments.Add(tmp);
            db.SaveChanges();

            Article article = db.Articles.Find(id);

            if (article == null)
            {
                return HttpNotFound();
            }

            ArticleView articleView = ArticleView.ArticleData(article);

            return PartialView("_ShowComments", articleView);
        }


        public bool IsAuthor(int id)
        {
            string userName = User.Identity.Name;
            int currentAuthorId = GetCurrentAuthorId(userName);
            bool isAuthor = db.Articles.Any(x => x.Id == id && x.AuthorId == currentAuthorId);

            if (isAuthor)
            {
                return true;
            }
            return false;
        }

        public int GetCurrentAuthorId(string userName)
        {
            string userId = db.Users.Where(x => x.UserName == userName).First().Id;
            int currentAuthorId = db.Authors.Where(a => a.UserId == userId).First().Id;
            return currentAuthorId;
        }

        public bool IsAdmin()
        {
            bool isAdmin = User.IsInRole("Admin");

            if (isAdmin)
            {
                return true;
            }

            return false;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}
