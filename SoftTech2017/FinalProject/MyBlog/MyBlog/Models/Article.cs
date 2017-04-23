using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace MyBlog.Models
{
    public class Article
    {
        public Article()
        {
            DateCreated = DateTime.Now;
            Comments = new HashSet<Comment>();
        }


        public int Id { get; set; }

        [DisplayName("Заглавие")]
        public string Title { get; set; }

        [DisplayName("Съдържание")]
        public string Content { get; set; }

        public DateTime DateCreated { get; set; }

        public int AuthorId { get; set; }

        public virtual Author Author { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }

    }
}