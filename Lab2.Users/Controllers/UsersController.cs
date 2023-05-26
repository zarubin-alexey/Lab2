using Lab2.DAL;
using Lab2.Exceptions;
using Lab2.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using Newtonsoft.Json;
using Lab2.RabbitMq;

namespace Lab2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly EF_DataContext context;
        private readonly IRabbitMqService rabbitService;

        public UsersController(EF_DataContext dataContext, IRabbitMqService rabbitMqService)
        {
            context = dataContext;
            rabbitService = rabbitMqService;
        }

        [HttpPost("CreateOrder")]
        public IActionResult Post([FromBody] Order model)
        {
            try
            {
                rabbitService.SendMessage(model);

                return Ok();
            }
            catch (Exception ex)
            {
                throw new InternalServerErrorException(ErrorCodes.UnknownError, ex.Message);
            }
        }

        /// <summary>
        /// Returns the list of all users.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Get()
        {
            var data = context.Users.Select(p => Convert(p)).ToList();
            if (!data.Any())
            {
                throw new NotFoundException(ErrorCodes.EmptyTable, "The Users table is empty!");
            }

            return Ok(data);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var data = context.Users.Where(p => p.id == id).FirstOrDefault();
            if (data == null)
            {
                throw new NotFoundException(ErrorCodes.EmptyResult, $"The user {id} was not found!");
            }

            return Ok(Convert(data));
        }

        [HttpPost()]
        public IActionResult Post([FromBody] User model)
        {
            var user = Convert(model);
            if (context.Users.Any(p => p.id == user.id))
            {
                throw new BadRequestException(ErrorCodes.DataAlreadyExists, $"The user {user.id} is already added!");
            }

            try
            {
                context.Users.Add(user);
                context.SaveChanges();

                return Ok();
            }
            catch (Exception ex)
            {
                throw new InternalServerErrorException(ErrorCodes.UnknownError, ex.Message);
            }
        }

        [HttpPut()]
        public IActionResult Put([FromBody] User model)
        {
            var user = Convert(model);
            if (!context.Users.Any(p => p.id == user.id))
            {
                throw new BadRequestException(ErrorCodes.EmptyResult, $"The user {user.id} has not added yet!");
            }

            try
            {
                var old = context.Users.Where(p => p.id == user.id).FirstOrDefault();
                context.Users.Remove(old);

                context.Users.Add(user);
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
            if (!context.Users.Any(p => p.id == id))
            {
                throw new BadRequestException(ErrorCodes.EmptyResult, $"The user {id} has not added yet!");
            }

            try
            {
                var user = context.Users.Where(p => p.id == id).FirstOrDefault();
                context.Users.Remove(user);
                context.SaveChanges();

                return Ok();
            }
            catch (Exception ex)
            {
                throw new InternalServerErrorException(ErrorCodes.UnknownError, ex.Message);
            }
        }

        private static User Convert(TbUser user)
        {
            return new User()
            {
                Id = user.id,
                Name = user.name,
                Surname = user.surname,
                Phone = user.phone
            };
        }

        private static TbUser Convert(User user)
        {
            return new TbUser()
            {
                id = user.Id,
                name = user.Name,
                surname = user.Surname,
                phone = user.Phone
            };
        }
    }
}
