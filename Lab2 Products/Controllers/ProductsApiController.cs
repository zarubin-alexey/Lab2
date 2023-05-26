using Lab2.DAL;
using Lab2.Exceptions;
using Lab2.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace Lab2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsApiController : ControllerBase
    {
        private readonly EF_DataContext context;

        public ProductsApiController(EF_DataContext dataContext)
        {
            context = dataContext;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var data = context.Products.Select(p => Convert(p)).ToList();
            if (!data.Any())
            {
                throw new NotFoundException(ErrorCodes.EmptyTable, "The products table is empty!");
            }

            return Ok(data);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var data = context.Products.Where(p => p.id == id).FirstOrDefault();
            if (data == null)
            {
                throw new NotFoundException(ErrorCodes.EmptyResult, $"The product {id} was not found!");
            }

            return Ok(Convert(data));
        }

        [HttpPost()]
        public IActionResult Post([FromBody] Product model)
        {
            var product = Convert(model);
            if (context.Products.Any(p => p.id == product.id))
            {
                throw new BadRequestException(ErrorCodes.DataAlreadyExists, $"The product {product.id} is already added!");
            }

            try
            {
                context.Products.Add(product);
                context.SaveChanges();

                return Ok();
            }
            catch (Exception ex)
            {
                throw new InternalServerErrorException(ErrorCodes.UnknownError, ex.Message);
            }
        }

        [HttpPut()]
        public IActionResult Put([FromBody] Product model)
        {
            var product = Convert(model);
            if (!context.Products.Any(p => p.id == product.id))
            {
                throw new BadRequestException(ErrorCodes.EmptyResult, $"The product {product.id} has not added yet!");
            }

            try
            {
                var old = context.Products.Where(p => p.id == product.id).FirstOrDefault();
                context.Products.Remove(old);

                context.Products.Add(product);
                context.SaveChanges();

                return Ok();
            }
            catch (Exception ex)
            {
                throw new InternalServerErrorException(ErrorCodes.UnknownError, ex.Message);
            }
        }

        [HttpDelete()]
        public IActionResult Delete(int id)
        {
            if (!context.Products.Any(p => p.id == id))
            {
                throw new BadRequestException(ErrorCodes.EmptyResult, $"The product {id} has not added yet!");
            }

            try
            {
                var product = context.Products.Where(p => p.id == id).FirstOrDefault();
                context.Products.Remove(product);
                context.SaveChanges();

                return Ok();
            }
            catch (Exception ex)
            {
                throw new InternalServerErrorException(ErrorCodes.UnknownError, ex.Message);
            }
        }

        public static Product Convert(TbProduct product)
        {
            return new Product()
            {
                Id = product.id,
                Name = product.name,
                Count = product.count,
                Brand = product.brand,
                Size = product.size,
                Price = product.price
            };
        }

        public static TbProduct Convert(Product product)
        {
            return new TbProduct()
            {
                id = product.Id,
                name = product.Name,
                count = product.Count,
                brand = product.Brand,
                size = product.Size,
                price = product.Price
            };
        }
    }
}
