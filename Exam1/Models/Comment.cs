using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Exam1.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public Account Account { get; set; }
        public string Text { get; set; }
        public ProductInfo Product { get; set; }
    }
}