﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makale.Entities
{
    public class Note:EntityBase
    {
        public string Title{ get; set; }
        public string Text { get; set; }
        public bool IsDraft{ get; set; }
        public bool LikeCount{ get; set; }
        public int CategoryId{ get; set; }

        public virtual User Owner{ get; set; }
        public virtual Category Category { get; set; }
        public virtual List<Comment> Comments { get; set; }
        public virtual List<Liked> Likes{ get; set; }
      
    }
}
