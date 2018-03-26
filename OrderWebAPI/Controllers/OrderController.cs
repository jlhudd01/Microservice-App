using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderWebAPI.Commands;
using OrderWebAPI.Contexts;
using OrderWebAPI.Models;
using OrderWebAPI.Repositories;

namespace OrderWebAPI.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("CorsPolicy")]
    public class OrderController : Controller
    {
        private readonly IOrderRepository _repository;
        private readonly IMediator _mediator;

        public OrderController(IOrderRepository repository, IMediator mediator)
        {
            _repository = repository;
            _mediator = mediator;
        }

        // GET api/values
        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(IEnumerable<Order>), (int)HttpStatusCode.OK)]
        public IActionResult Orders()
        {
            return Ok(_repository.Get());
        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public IActionResult Delete([FromBody]RemoveOrderItemFromOrderCommand command)
        {
            _mediator.Send(command);

            return Ok();
        }
    }
}
