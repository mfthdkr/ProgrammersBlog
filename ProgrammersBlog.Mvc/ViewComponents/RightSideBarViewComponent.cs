using Microsoft.AspNetCore.Mvc;
using ProgrammersBlog.Mvc.Models;
using ProgrammersBlog.Services.Abstract;
using System.Threading.Tasks;

namespace ProgrammersBlog.Mvc.ViewComponents
{
    public class RightSideBarViewComponent: ViewComponent
    {
        private readonly IArticleService _articleService;
        private readonly ICategoryService _categoryService;

        public RightSideBarViewComponent(ICategoryService categoryService, IArticleService articleService)
        {
            _categoryService = categoryService;
            _articleService = articleService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categoriesResult= await _categoryService.GetAllByNonDeletedAndActiveAsync();
            var articlecResult = await _articleService.GetAllByViewCountAsync(isAscending: false, takeSize: 5);

            return View(new RightSideBarViewModel
            {
                Categories= categoriesResult.Data.Categories,
                Articles = articlecResult.Data.Articles
            });
        }
    }
}
