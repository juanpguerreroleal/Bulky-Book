using System;
using System.Collections.Generic;
using System.Text;

namespace Bulky_Book.Models.ViewModels
{
    public class ProductViewModel
    {
        public Product Product { get; set; }
        public IEnumerable<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem> Categories { get; set; }
        public IEnumerable<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem> CoverTypes { get; set; }
        public List<Microsoft.AspNetCore.Http.IFormFile> Files { get; set; }
    }
}
