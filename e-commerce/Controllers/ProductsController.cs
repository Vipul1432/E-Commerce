using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using e_commerce.Data;
using e_commerce.Domain.Models;
using e_commerce.Domain.Interfaces;

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
        public async Task<IActionResult> Index()
        {
            List<Product> products = await _productRepository.GetAllAsync();
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
        public IActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        [HttpPost]
        [Route("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,Name,Price")] Product product)
         {
            if (ModelState.IsValid)
            {
                bool isAdded  = await _productRepository.AddAsync(product);
                if(isAdded)
                    TempData["success"] = $"Product {product.Name} Added Successfully";
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products/Edit/5
        [Route("Edit/{id}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                Product product = await _productRepository.GetByIdAsync(id ?? 1);
                return View(product);
            }                      
        }

        // POST: Products/Edit/5
        [HttpPost]
        [Route("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,Name,Price")] Product product)
        {
            if (id != product.ProductId)
            {
                return RedirectToAction("Index");
            }

            if (ModelState.IsValid)
            {
                try
                {
                   bool IsAdded = await _productRepository.UpdateAsync(product);
                    if(IsAdded)
                        TempData["success"] = "Category Updated Successfully";
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
            return IsDeleted;
        }
    }
}
