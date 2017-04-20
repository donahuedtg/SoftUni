using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyBlog.Models
{
    public class Author
    {
        public Author()
        {
            Articles = new HashSet<Article>();
            Comments = new HashSet<Comment>();
        }

        public int Id { get; set; }

        public string FirstName { get; set; }

        public string UserId { get; set; }

        public virtual ICollection<Article> Articles { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
    }
}