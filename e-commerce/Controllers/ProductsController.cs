using e_commerce.Domain.Interfaces;
using e_commerce.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace e_commerce.Controllers
{
    [Route("Products")]
    public class ProductsController : Controller
    {
        private readonly IRepository<Product> _productRepository;

        public ProductsController(IRepository<Product> productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet]
        public IActionResult Index(string sortOrder, int? categoryId)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParam"] = sortOrder == "name" ? "name_desc" : "name";
            ViewData["PriceSortParam"] = sortOrder == "price" ? "price_desc" : "price";

            IQueryable<Product> products = _productRepository.GetAllProduct();
            // finding product category by categoryId
            if (categoryId != null)
            {
                products = products.Where(products => products.CategoryId == categoryId);
            }
            // sorting product by name and price
            switch (sortOrder)
            {
                case "name_desc":
                    products = products.OrderByDescending(p => p.Name);
                    break;
                case "price":
                    products = products.OrderBy(p => p.Price);
                    break;
                case "price_desc":
                    products = products.OrderByDescending(p => p.Price);
                    break;
                default:
                    products = products.OrderBy(p => p.Name);
                    break;
            }

            return View(products);
        }

        // GET: Products/Details/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Details(int id)
        {
            Product product = await _productRepository.GetByIdAsync(id);

            if (product == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return View(product);
        }


        // GET: Products/Create
        [Route("Create")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            List<Category> categories = await _productRepository.GetAllCategory();
            List<SelectListItem> CategoryList = categories.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.CategoryId.ToString()
            }).ToList();
            ViewBag.CategoryList = CategoryList;
            return View();
        }

        // POST: Products/Create
        [HttpPost]
        [Route("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Price, CategoryId")] Product product)
        {
            if (ModelState.IsValid)
            {
                bool isAdded = await _productRepository.AddAsync(product);
                if (isAdded)
                    TempData["success"] = $"Product {product.Name} Added Successfully";
                else
                    TempData["error"] = $"{product.Name} can't added try again!!!";
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        // GET: Products/Edit/5
        [Route("Edit/{id}")]
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                Product product = await _productRepository.GetByIdAsync(id ?? 1);
                List<Category> categories = await _productRepository.GetAllCategory();
                List<SelectListItem> CategoryList = categories.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.CategoryId.ToString(),
                    Selected = x.CategoryId == product.CategoryId
                }).ToList();
                ViewBag.CategoryList = CategoryList;
                return View(product);
            }
        }

        // POST: Products/Edit/5
        [HttpPost]
        [Route("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Name,Price, CategoryId")] Product product)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Product existingproduct = await _productRepository.GetByIdAsync(id);
                    existingproduct.Name = product.Name;
                    existingproduct.Price = product.Price;
                    existingproduct.Description = product.Description;
                    existingproduct.CategoryId = product.CategoryId;
                    bool IsAdded = await _productRepository.UpdateAsync(existingproduct);
                    if (IsAdded)
                        TempData["success"] = "Category Updated Successfully";
                    else
                        TempData["error"] = $"{existingproduct.Name} can't updated try again!!!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    return RedirectToAction("Error");
                }
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products/Delete/5
        [HttpPost]
        [Route("Delete")]
        public async Task<bool> Delete(int id)
        {
            Product product = await _productRepository.GetByIdAsync(id);
            bool IsDeleted = await _productRepository.RemoveAsync(product);
            if (IsDeleted)
                TempData["success"] = $"{product.Name} Deleted Successfully";
            else
                TempData["error"] = $"{product.Name} can't deleted try again!!!";
            return IsDeleted;
        }
    }
}
