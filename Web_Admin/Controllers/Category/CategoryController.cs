using AdminData.Models.Category;
using AdminServices.Interfaces.Admin;
using AdminServices.Interfaces.Category;
using AdminServices.Services.Category;
using DTO_s_Layer.DTO_Model;
using Microsoft.AspNetCore.Mvc;
using Web_Admin.Data;

namespace Web_Admin.Controllers.Category
{
    public class CategoryController : Controller
    {
        private readonly ApplicationContext _context;
        private readonly Icategory _icategory;
        private readonly IAdmin _admin;
        private readonly CateFileUpload _fileUpload;

        public CategoryController(ApplicationContext context, Icategory icategory, IAdmin admin, CateFileUpload fileUpload)
        {
            _context = context;
            _icategory = icategory;
            _admin = admin;
            _fileUpload = fileUpload;
        }

        public IActionResult Index()
        {
            return View();
        }
        public string GetCookie(string userId)
        {
            var cookieKey = userId.ToString();
            var cookieValue = Request.Cookies[cookieKey];
            return cookieValue;
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Category_db category_Db, IFormFile CategoryImg)
        {
            try
            {
                category_Db.Adminid = GetCookie("userid");
                var response = await _icategory.Create(category_Db, CategoryImg); // Await the asynchronous task
                if (response == "Success")
                {
                    return RedirectToAction("Create", "Category");
                }
                else
                {
                    ViewBag.message = "Not Valid";
                    return View();
                }
            }
            catch (Exception ex)
            {
                return View();
            }

        }
        public async Task<IActionResult> List()
        {
            string userId = GetCookie("userid");
            var userCategories = await _icategory.List(userId);
            return View(userCategories);
        }
        public IActionResult Edit(Guid id)
        {
            var adminId = GetCookie("userid");
            var categoryEditModel =  _icategory.GetCategoryById(id, adminId);

            if (categoryEditModel == null)
            {
                // Handle not found scenario
                return NotFound();
            }

            return View(categoryEditModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CategoryDTO categoryEditModel, IFormFile CategoryImg)
        {
            try
            {
                var response = await _icategory.Edit(categoryEditModel, CategoryImg);

                if (response == "Success")
                {
                    return RedirectToAction("List", "Category"); // Redirect to the desired page after successful update
                }
                else
                {
                    ViewBag.message = "Not Valid";
                    return View(categoryEditModel);
                }
            }
            catch (Exception ex)
            {
                return View(categoryEditModel);
            }
        }


        [HttpPost]
        public async Task<IActionResult> DeleteCategory(Guid categoryid)

        {
            var adminid = GetCookie("userid");
            var result = await _icategory.Delete(categoryid, adminid);

            if (result == "Success")
            {
                return RedirectToAction("List", "Category"); // 200 OK


            }
            else
            {
                return BadRequest(); // 400 Bad Request (or you can use NotFound() if it fits better)
            }
        }
    }
}
