using MyBlog.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyBlog.ViewModels
{
    public class ArticleView
    {

        public int Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public DateTime DateCreated { get; set; }

        [Display(Name = "Author")]
        public string FirstName { get; set; }

        public static List<ArticleView> ArticleList(IEnumerable<Article> data)
        {
            List<ArticleView> list = new List<ArticleView>();

            foreach (var item in data)
            {
                ArticleView tmp = new ArticleView();
                tmp.Id = item.Id;
                tmp.Title = item.Title;
                tmp.Content = item.Content;
                tmp.DateCreated = item.DateCreated;
                tmp.FirstName = item.Author.FirstName;

                list.Add(tmp);
            }

            return list;
        }

        public static ArticleView ArticleData(Article data)
        {

            ArticleView tmp = new ArticleView();
            tmp.Id = data.Id;
            tmp.Title = data.Title;
            tmp.Content = data.Content;
            tmp.DateCreated = data.DateCreated;
            tmp.FirstName = data.Author.FirstName;

            return tmp;
        }

    }
}