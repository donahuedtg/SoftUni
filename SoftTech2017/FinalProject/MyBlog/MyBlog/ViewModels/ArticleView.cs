using MyBlog.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyBlog.ViewModels
{
    public class ArticleView
    {

        public int Id { get; set; }


        [DisplayName("Заглавие")]
        public string Title { get; set; }

        [DisplayName("Съдържание")]
        public string Content { get; set; }

        public DateTime DateCreated { get; set; }

        public string Author { get; set; }

        public int AuthorId { get; set; }

        public ICollection<Comment> Comments { get; set; }

        public int Count { get; set; }

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
                tmp.Author = item.Author.FirstName;
                tmp.AuthorId = item.AuthorId;
                tmp.Count = item.Comments.Count;
                             

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
            tmp.Author = data.Author.FirstName;
            tmp.AuthorId = data.AuthorId;
            tmp.Comments = data.Comments;

            return tmp;
        }

    }
}