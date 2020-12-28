using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using topcyTurvyCake_RazorPages.Models;

namespace topcyTurvyCake_RazorPages.Pages.Admin
{
    public class AddEditRecipeModel : PageModel
    {
        private readonly IRecipesService recipesService;

        [FromRoute]
        public long? Id { set; get; }


        public bool IsNewEecipe
        {
            get { return Id == null; }
        }

        [BindProperty]
        public Recipe Recipe { get; set; }

        [BindProperty]
        public IFormFile Image { get; set; }

        public AddEditRecipeModel(IRecipesService recipesService)
        {
            this.recipesService = recipesService;
        }
        public async Task OnGet()
        {
            Recipe =await recipesService.FindAsync(Id.GetValueOrDefault()) ?? new Recipe();
        }


        public async Task<IActionResult> OnPost()
        {
            if(!ModelState.IsValid)
            {
                return Page();
            }
            //Recipe.Id = Id.GetValueOrDefault();

            //await recipesService.SaveAsync(Recipe);

            var res = await recipesService.FindAsync(Id.GetValueOrDefault()) ?? new Recipe();

            res.Name = Recipe.Name;
            res.Description = Recipe.Description;
            res.Ingredients = Recipe.Ingredients;
            res.Directions = Recipe.Directions;

            if(Image!=null)
            {
                using var stream = new System.IO.MemoryStream();
                await Image.CopyToAsync(stream);

                res.Image = stream.ToArray();
                res.ImageContentType = Image.ContentType;
            }


            return RedirectToPage("/Recipe", new { id = res.Id });
        }


        public async Task<IActionResult> OnPostDelete()
        {
            
            await recipesService.DeleteAsync(Id.Value);

            return RedirectToPage("/Index");
        }




    }
}
