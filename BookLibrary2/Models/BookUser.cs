using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookLibrary2.Models
{
    public class BookUser
    {
        public long IdBook { get; set; }
        public long User { get; set; }
    }
}