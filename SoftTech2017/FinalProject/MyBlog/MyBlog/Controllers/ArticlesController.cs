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

            var email = User.Identity.Name;
            var currUserId = db.Users.Where(x => x.Email == email).First().Id;
            var authorId = db.Authors.First(x => x.UserId == currUserId).Id;
            ViewBag.CurrentAuthorId = authorId;


            return View(articleView);
        }

        // GET: Articles/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Articles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Content")] ArticleView article)
        {
            if (ModelState.IsValid)
            {
                Article tmp = new Article();
                tmp.Title = article.Title;
                tmp.Content = article.Content;
                var email = User.Identity.Name;
                var currUserId = db.Users.Where(x => x.Email == email).First().Id;
                var authorId = db.Authors.First(x => x.UserId == currUserId).Id;
                tmp.AuthorId = authorId;

                db.Articles.Add(tmp);
                db.SaveChanges();

                string message = string.Format($"Статия {article.Id} е записана успешно!");
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

        // POST: Articles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
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
        public ActionResult Delete(int id)
        {
            Article article = db.Articles.Find(id);
            db.Articles.Remove(article);
            db.SaveChanges();

            string message = string.Format($"Статия {id} е изтрита успешно!");
            this.AddNotification(message, NotificationType.SUCCESS);

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

        //public bool IsAdmin()
        //{

        //}

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
