using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyBlog.Models
{
    public class Article
    {
        public Article()
        {
            DateCreated = DateTime.Now;
        }


        public int Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public DateTime DateCreated { get; set; }

        public int AuthorId { get; set; }

        public virtual Author Author { get; set; }

    }
}