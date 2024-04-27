
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlinePharmacy.DAL;
using OnlinePharmacy.Models;
using System.Threading.Tasks;
using System.Linq;
using OnlinePharmacy.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace OnlinePharmacy.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoriesController : ControllerBase
    {
        private readonly DBConn _context;

        public CategoriesController(DBConn context)
        {
            _context = context;
        }



        // List all the Categories
        [HttpGet]
        public IActionResult ListCategories()
        {
            var categories = _context.Categories.ToList();
            if (categories.Count == 0)
            {
                return NotFound("Categories Not available. ");
            }
            return Ok(categories);



        }
        [HttpGet("{CategoryId}", Name = "ListCatgeoriesWithId")]
        public IActionResult ListCategorieswithId(int CategoryId)
        {
            var category = _context.Categories.Find(CategoryId);
            if (category == null)
            {
                return NotFound($"category details Not Found with id {CategoryId} ");
            }
            return Ok(category);
        }
        [HttpPost]
        public IActionResult AddCategory([FromForm] CategoryDto model)
        {
            if (model == null)
            {
                return BadRequest("Category model is null.");
            }

            var category = new Category
            {
                CategoryId = model.CategoryId,
                CategoryName = model.CategoryName

            };

            _context.Categories.Add(category);
            _context.SaveChanges();

            return Ok(category);
        }
        [HttpPut]
        public IActionResult UpdateCategory(CategoryDto model)
        {
            if (model == null || model.CategoryId == 0)
            {
                if (model == null)
                {
                    return BadRequest($"Category Id {model.CategoryId} is invalid ");
                }
            }
            var category = _context.Categories.Find(model.CategoryId);
            if (category == null)
            {
                return BadRequest($"Category  was not found with id {model.CategoryId}");
            }
            category.CategoryName = model.CategoryName;
            category.CategoryId = model.CategoryId;

            _context.SaveChanges();
            return Ok("Category details updated ");
        }

        [HttpDelete]
        public IActionResult DeleteCategory(int id)
        {
            var category = _context.Categories.Find(id);
            if (category == null)
            {
                return NotFound($"Product not found with {id} ");
            }
            _context.Categories.Remove(category);
            _context.SaveChanges();
            return Ok("Product details deleted.");
        }
    }



}

