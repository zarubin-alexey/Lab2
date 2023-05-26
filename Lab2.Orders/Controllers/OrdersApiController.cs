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
    public class OrdersApiController : ControllerBase
    {
        private readonly EF_DataContext context;

        public OrdersApiController(EF_DataContext dataContext)
        {
            context = dataContext;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var data = context.Orders.Select(p => Convert(p)).ToList();
            if (!data.Any())
            {
                throw new NotFoundException(ErrorCodes.EmptyTable, "The orders table is empty!");
            }

            return Ok(data);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var data = context.Orders.Where(p => p.id == id).FirstOrDefault();
            if (data == null)
            {
                throw new NotFoundException(ErrorCodes.EmptyResult, $"The order {id} was not found!");
            }

            return Ok(Convert(data));
        }

        [HttpPost()]
        public IActionResult Post([FromBody] Order model)
        {
            var order = Convert(model);
            if (context.Orders.Any(p => p.id == order.id))
            {
                throw new BadRequestException(ErrorCodes.DataAlreadyExists, $"The order {order.id} is already added!");
            }

            try
            {
                context.Orders.Add(order);
                context.SaveChanges();

                return Ok();
            }
            catch (Exception ex)
            {
                throw new InternalServerErrorException(ErrorCodes.UnknownError, ex.Message);
            }
        }

        [HttpPut()]
        public IActionResult Put([FromBody] Order model)
        {
            var order = Convert(model);
            if (!context.Orders.Any(p => p.id == order.id))
            {
                throw new BadRequestException(ErrorCodes.EmptyResult, $"The order {order.id} has not added yet!");
            }

            try
            {
                var old = context.Orders.Where(p => p.id == order.id).FirstOrDefault();
                context.Orders.Remove(old);

                context.Orders.Add(order);
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
            if (!context.Orders.Any(p => p.id == id))
            {
                throw new BadRequestException(ErrorCodes.EmptyResult, $"The order {id} has not added yet!");
            }

            try
            {
                var order = context.Orders.Where(p => p.id == id).FirstOrDefault();
                context.Orders.Remove(order);
                context.SaveChanges();

                return Ok();
            }
            catch (Exception ex)
            {
                throw new InternalServerErrorException(ErrorCodes.UnknownError, ex.Message);
            }
        }

        public static Order Convert(TbOrder order)
        {
            return new Order()
            {
                Id = order.id,
                UserId = order.user_id,
                ProductId = order.product_id,
                Count = order.count,
                Phone = order.phone,
                Address = order.address,
                Created = order.created
            };
        }

        public static TbOrder Convert(Order order)
        {
            return new TbOrder()
            {
                id = order.Id,
                user_id = order.UserId,
                product_id = order.ProductId,
                count = order.Count,
                phone = order.Phone,
                address = order.Address,
                created = order.Created
            };
        }
    }
}
