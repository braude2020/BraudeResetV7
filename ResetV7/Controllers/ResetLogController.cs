using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ResetV7.Controllers
{
    public class ResetLogController : Controller
    {
        public async Task OnGet()
        {
           // zoom HOst di
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}