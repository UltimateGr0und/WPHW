using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Exam1.Models
{
    public class Category
    {
        public Category() { }
        public Category(string name) { Name = name; }
        public int Id { get; set; }
        public string Name { get; set; }
    }
}