using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyBlog.Models;

namespace MyBlog.ViewModels
{
    public class CommentView
    {
        public int Id { get; set; }

        public string CommentText { get; set; }

        public DateTime DateAdded { get; set; }

        public string Author { get; set; }


        public static List<CommentView> CommentsList(IEnumerable<Comment> data)
        {
            List<CommentView> list = new List<CommentView>();

            foreach (var item in data)
            {
                CommentView tmp = new CommentView();
                tmp.Id = item.Id;
                tmp.CommentText = item.CommentText;
                tmp.DateAdded = item.DateAdded;
                tmp.Author = item.Author.FirstName;
            }

            return list;
        }
    }
}