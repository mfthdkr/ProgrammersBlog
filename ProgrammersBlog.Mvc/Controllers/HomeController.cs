﻿using Microsoft.AspNetCore.Mvc;
using ProgrammersBlog.Services.Abstract;
using System.Threading.Tasks;

namespace ProgrammersBlog.Mvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly IArticleService _articleService;
        public HomeController(IArticleService articleService)
        {
            _articleService = articleService;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var articleListDto = await _articleService.GetAllByNonDeletedAndActiveAsync();
            return View(articleListDto.Data);
        }
    }
}
