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
            ArticleView articleView = ArticleView.ArticleData(article);

            var email = User.Identity.Name;
            var currUserId = db.Users.Where(x => x.Email == email).First().Id;
            var authorId = db.Authors.First(x => x.UserId == currUserId).Id;
            ViewBag.CurrentAuthorId = authorId;

            if (article == null)
            {
                return HttpNotFound();
            }
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
                return RedirectToAction("Index");
            }

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
                return RedirectToAction("Index");
            }
            return View(article);
        }

        // GET: Articles/Delete/5
        public ActionResult Delete(int? id)
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
            if (article == null)
            {
                return HttpNotFound();
            }
            return View(article);
        }

        // POST: Articles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Article article = db.Articles.Find(id);
            db.Articles.Remove(article);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        public bool IsAuthor(int id)
        {
            var email = User.Identity.Name;
            var currUserId = db.Users.Where(x => x.Email == email).First().Id;
            var authorId = db.Authors.First(x => x.UserId == currUserId).Id;
            bool isAuthor = db.Articles.Any(x => x.Id == id && x.AuthorId == authorId);

            if (isAuthor)
            {
                return true;
            }
            return false;
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
