using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController(IProductRepository repo) : ControllerBase
    {
       

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts()
        {
           return  Ok( await repo.GetProductsAsync());
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Product>>GetProduct(int id)
        {
            var product = await repo.GetProductByIdAsync(id);
            if (product==null) return NotFound();
            return product;
        }

        [HttpPost]
        public async Task<ActionResult<Product>> AddProduct(Product product)
        {
            repo.AddProduct(product);
             if( await repo.SaveChangesAsync()){
                 return CreatedAtAction("GetProduct",new {id=product.Id},product);
             }
             return BadRequest("Problem creating Product");
        }
        [HttpPut("{id:int}")]
        public async Task<ActionResult>UpdateProduct(int id,Product product)
        {
            if (product.Id != id || !ProductExist(id))
            {
                return BadRequest("Can not update this product");
            }
            repo.UpdateProduct(product);
            if(await  repo.SaveChangesAsync()){
                return NoContent();
            }
            return BadRequest();
        }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteProduct(int id)
    {
        var product =  await repo.GetProductByIdAsync(id);
        if(product == null) return NotFound();
        repo.DeleteProduct(product);
        if(await repo.SaveChangesAsync()){
            return CreatedAtAction("GetProduct",new {id=product.Id},product);
        }
        return BadRequest("Unable to Delete");
    }

        public bool ProductExist(int id)
        {
            return repo.ProductExists(id);
        }
    }
}
