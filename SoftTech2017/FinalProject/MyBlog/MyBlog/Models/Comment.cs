using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyBlog.Models
{
    public class Comment
    {
        public Comment()
        {
            DateAdded = DateTime.Now;  
        }

        public int Id { get; set; }

        [Required(ErrorMessage = "Полето не може да бъде празно!")]
        public string CommentText { get; set; }

        public DateTime DateAdded { get; set; }

        public int? AuthorId { get; set; }

        public virtual Author Author { get; set; }

        public int ArticleId { get; set; }

        public virtual Article Article { get; set; }
    }
}