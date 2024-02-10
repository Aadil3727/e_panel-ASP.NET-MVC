//using AdminData.Migrations;
using AdminData.Models.Product;
using AdminServices.Interfaces.Admin;
using AdminServices.Interfaces.Category;
using AdminServices.Interfaces.Product;
using AdminServices.Services.Product;
using AutoMapper;
using DTO_s_Layer.DTO_Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Web_Admin.Data;

namespace Web_Admin.Controllers.Product
{
    public class ProductController : Controller
    {
        private readonly ApplicationContext _context;
        private readonly IMapper _mapper;

        private readonly IProductService _productService;

        public ProductController(IProductService productService, ApplicationContext context, IMapper imapper)
        {
            _productService = productService;
            _context = context;
            _mapper = imapper;
        }
        public async Task<IActionResult> Index()
        {
            //var products = await _productService.GetProductsAsync();
            //return View(products);
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
            var categories = _context.Category
        .Select(c => new SelectListItem
        {
            Value = c.Id.ToString(),
            Text = c.Name
        })
        .ToList();

            // Add a default option with an empty value
            categories.Insert(0, new SelectListItem { Value = "", Text = "Select a Category" });

            ViewBag.Categories = categories;

            // Create an empty ProductDTO to bind to the form
            var product = new ProductDTO();
            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductDTO product, IFormFile[] ProImg)
        {
            try
            {
                product.Adminid = GetCookie("userid");

                var response = await _productService.Create(product, ProImg);

                if (response == "Success")
                {
                    return RedirectToAction("List", "Product");
                }
                else
                {
                    // Log or print out the actual response for debugging purposes
                    Console.WriteLine($"Error response: {response}");
                    // Log any additional information that may help in diagnosing the issue

                    // Repopulate the ViewBag.Categories before returning the view
                    ViewBag.Categories = _context.Category
                        .Select(c => new SelectListItem
                        {
                            Value = c.Id.ToString(),
                            Text = c.Name
                        })
                        .ToList();

                    // Add a default option with an empty value
                    ViewBag.Categories.Insert(0, new SelectListItem { Value = "", Text = "Select a Category" });

                    return View(product);
                }
            }
            catch (Exception ex)
            {
                // Log the exception details
                Console.WriteLine($"Exception: {ex}");
                // You may want to log ex.Message, ex.StackTrace, or use a logging framework like ILogger

                return View(product);
            }
        }

        public IActionResult Details(Guid id)
        {
            var userIdString = GetCookie("userid");

            if (id != null)
            {
                var product = _productService.GetProductById(id, userIdString);

                if (product == null)
                {
                    return RedirectToAction("Error");
                }

                var productDTO = new ProductDTO
                {
                    Id = product.Id,
                    Name = product.Name,
                    Images = product.Images,
                    price = product.price,
                    ProImg = product.ProImg,
                    Description = product.Description
                    // Map other properties...
                };

                return View(productDTO);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }


        [HttpGet]
        public IActionResult Edit(Guid id)
        {
            var userIdString = GetCookie("userid");
            var productEntity = _productService.GetProductById(id, userIdString);

            if (productEntity != null)
            {
                var categories = _context.Category
                    .Select(c => new SelectListItem
                    {
                        Value = c.Id.ToString(),
                        Text = c.Name
                    })
                    .ToList();

                categories.Insert(0, new SelectListItem { Value = "", Text = "Select a Category" });
                ViewBag.Categories = categories;

                // Map the entity to DTO
                var productDTO = _mapper.Map<ProductDTO>(productEntity);

                return View(productDTO);
            }

            // Handle the case where the product is not found
            return NotFound();
        }


        [HttpPost]
        public async Task<IActionResult> Edit(ProductDTO product, IFormFile[] ProImg)
        {
            //if (ModelState.IsValid)
            //{
            var response = await _productService.Edit(product, ProImg);

            if (response == "Success")
            {
                return RedirectToAction("List", "Product");
            }
            else
            {
                // Log or print out the actual response for debugging purposes
                Console.WriteLine($"Error response: {response}");
                // Log any additional information that may help in diagnosing the issue

                return View(product);
            }
        }
      
        [HttpPost]  
        public async Task<IActionResult> DeleteProduct(Guid productId)

        {
            var adminid = GetCookie("userid");
            var result = await _productService.Delete(productId, adminid);

            if (result == "Success")
            {
                return RedirectToAction("List", "Product"); // 200 OK
                

            }
            else
            {
                return BadRequest(); // 400 Bad Request (or you can use NotFound() if it fits better)
            }
        }





        public IActionResult List()
        {

            var user = GetCookie("userid");

            if (user != null)
            {
                var product = _productService.List(user);
                return View(product);

            }
            else
            {
                // Redirect to login or show an unauthorized page
                return RedirectToAction("Login", "AdminView");
            }
        }
    }


    //public IActionResult Edit()
    //    {
    //        var categories = _context.Category
    //    .Select(c => new SelectListItem
    //    {
    //        Value = c.Id.ToString(),
    //        Text = c.Name
    //    })
    //    .ToList();

    //        // Add a default option with an empty value
    //        categories.Insert(0, new SelectListItem { Value = "", Text = "Select a Category" });

    //        ViewBag.Categories = categories;

    //        // Create an empty ProductDTO to bind to the form
    //        var product = new ProductDTO();
    //        return View(product);
    //    }
    //    [HttpPost]
    //    public async Task<IActionResult> Edit(ProductDTO product, IFormFile[] ProImg)
    //    {
    //        try
    //        {
    //            product.Adminid = GetCookie("userid");
    //            var responce = await _productService.Create(product, ProImg);

    //            if (responce == "Success")
    //            {
    //                return RedirectToAction("List", "Product");
    //            }
    //            else
    //            {
    //                // Log or print out the actual response for debugging purposes
    //                Console.WriteLine($"Error response: {responce}");
    //                // Log any additional information that may help in diagnosing the issue

    //                return View(product);
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            // Log the exception details
    //            Console.WriteLine($"Exception: {ex}");
    //            // You may want to log ex.Message, ex.StackTrace, or use a logging framework like ILogger

    //            return View(product);
    //        }
    //    }




}


