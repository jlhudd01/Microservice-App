using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductWebAPI.Commands;
using ProductWebAPI.Contexts;
using ProductWebAPI.Models;
using ProductWebAPI.Repositories;

namespace ProductWebAPI.Controllers
{
    [EnableCors("CorsPolicy")]
    [Route("api/[controller]")]
    public class ProductController : Controller
    {
        private readonly IProductRepository _repository;
        private readonly IMediator _mediator;

        public ProductController(IProductRepository repository, IMediator mediator)
        {
            _repository = repository;
            _mediator = mediator;
        }
        // GET api/controller/products
        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
        public IActionResult Products()
        {
            return Ok(_repository.Get());
        }

        // GET api/product/5
        [HttpGet]
        [Route("[action]/{id}")]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public IActionResult GetProduct(int id)
        {
            try
            {
                var product = _repository.GetProduct(id);

                return Ok(product);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<IActionResult> PostProduct([FromBody]CreateProductCommand command)
        {
            await _mediator.Send(command);

            return Created("http://localhost:5000/Product/PostProduct", command);
        }

        [HttpPut]
        [Route("[action]")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> Put([FromBody]UpdateProductCommand command)
        {
            await _mediator.Send(command);

            return Ok();
        }

        // DELETE api/values/5
        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> Delete([FromBody]DeleteProductCommand command)
        {
            await _mediator.Send(command);

            return Ok();
        }
    }
}
