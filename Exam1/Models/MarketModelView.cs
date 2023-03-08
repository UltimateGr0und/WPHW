using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Exam1.Models
{
    public class MarketModelView
    {
        public Account User { get; set; }
        public string Category { get; set; }
        public string SearchRequest { get; set; }
        public PageInfo PageInfo { get; set; }
        public List<ProductInfo> products { get; set; }
        public List<string> Categories { get; set; }
    }

    public class PageInfo
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages
        {
            get { return (int)Math.Ceiling((decimal)TotalItems / PageSize); }
        }

    }
}