using DataAccess.Repository.IRepository;
using DataObject.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PizzaManagement.Pages.Staff.Categories
{
    public class DeleteModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [BindProperty]
        public Category Category { get; set; }
        public void OnGet(int id)
        {
            Category = _unitOfWork.Category.GetFirstOrDefault(u => u.CategoryID == id);
            
        }
        public async Task<IActionResult> OnPost()
        {
            var objFromDb = _unitOfWork.Category.GetFirstOrDefault(u => u.CategoryID == Category.CategoryID);
            if (objFromDb != null)
            {
                _unitOfWork.Category.Remove(objFromDb);
                _unitOfWork.Save();
                RedirectToPage("Index");
            }
            else
            {
                return Page();
            }
            return RedirectToPage("Index");
        }
    }
}
