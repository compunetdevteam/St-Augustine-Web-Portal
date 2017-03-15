using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StAugustine.Models.Objects
{
    public class Tag
    {
        
        public Tag()
        {
            this.Posts = new HashSet<Post>();
        }

        public int ID { get; set; }
        public string Name { get; set; }

       
        public virtual ICollection<Post> Posts { get; set; }
    }
}
