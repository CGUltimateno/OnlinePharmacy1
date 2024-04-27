
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using OnlinePharmacy.DAL;

using Microsoft.EntityFrameworkCore;

using OnlinePharmacy.DTOs;
using OnlinePharmacy.Models;

namespace OnlinePharmacy.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {


        private readonly DBConn _context;


        public ProductController(DBConn context)
        {
            _context = context;
     
        }


        // List all the products
        [HttpGet]
        public IActionResult ListProducts()
        {
            var products = _context.Products.ToList();
            if (products.Count == 0)
            {
                return NotFound("Products Not available. ");
            }
            return Ok(products);
        }

        //list the product by id 
        [HttpGet("{id}")]
        public IActionResult ListProductsWithId(int id)
        {
            var product = _context.Products.Find(id);
            if (product == null)
            {
                return NotFound($"Product details Not Found with id {id} ");
            }
            return Ok(product);
        }
        [HttpGet("ProductsWithCategory/{CategoryID}")]
        public async Task<IActionResult> ListProductsWithCategory(int CategoryID)
        {
            var products = await _context.Products.Where(x => x.CategoryId == CategoryID).ToListAsync();
            if (products == null || !products.Any())
            {
                return NotFound($"Products not found for category ID {CategoryID}.");
            }
            return Ok(products);
        }

        //Add product
        [HttpPost]
        public IActionResult AddProduct([FromForm] ProductDto model)
        {
            if (model == null)
            {
                return BadRequest("Product model is null.");
            }

            var product = new Product
            {
                ProductName = model.ProductName,
                Price = model.Price,
                Qty = model.Qty,
                CategoryId = model.CategoryId

               
            };

            _context.Products.Add(product);
            _context.SaveChanges();

            return Ok(product);
        }



        //Update product info
        [HttpPut]
        public IActionResult UpdateProduct(ProductDto model)
        {
            if (model == null || model.ProductId == 0)
            {
                if (model == null)
                {
                    return BadRequest($"Product Id {model.ProductId} is invalid ");
                }
            }
            var product = _context.Products.Find(model.ProductId);
            if (product == null)
            {
                return BadRequest($"Product  was not found with id {model.ProductId}");
            }
            product.ProductName = model.ProductName;
            product.Price = model.Price;
            product.Qty = model.Qty;
            product.CategoryId = model.CategoryId;
            _context.SaveChanges();
            return Ok("Product details updated ");
        }
        [HttpPatch("{id}")]
        public IActionResult UpdateProductPatch(int id, [FromBody] JsonPatchDocument<Product> patchDocument)
        {
            if (patchDocument == null)
            {
                return BadRequest();
            }

            var product = _context.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            var productToPatch = new Product
            {
                
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                Price = product.Price,
                Qty = product.Qty
            };

            patchDocument.ApplyTo(productToPatch, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            product.ProductName = productToPatch.ProductName;
            product.Price = productToPatch.Price;
            product.Qty = productToPatch.Qty;

            _context.SaveChanges();

            return Ok(product);
        }

        //delete product
        [HttpDelete]
        public IActionResult DeleteProduct(int id)
        {
            var product = _context.Products.Find(id);
            if (product == null)
            {
                return NotFound($"Product not found with {id} ");
            }
            _context.Products.Remove(product);
            _context.SaveChanges();
            return Ok("Product details deleted.");
        }

    }
}
