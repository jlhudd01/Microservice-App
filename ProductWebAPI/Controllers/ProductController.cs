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
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> PostProduct([FromBody]CreateProductCommand command, [FromHeader(Name = "x-requestid")] string requestId)
        {
            bool result = false;
            if (Guid.TryParse(requestId, out Guid guid) && guid != Guid.Empty)
            {
                var createProductCommand = new IdentifiedCommand<CreateProductCommand, bool>(command, guid);
                result = await _mediator.Send(createProductCommand);
            }

            return result ? (IActionResult)Ok() : (IActionResult)BadRequest();
        }

        [HttpPut]
        [Route("[action]")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Put([FromBody]UpdateProductCommand command, [FromHeader(Name = "x-requestid")] string requestId)
        {
            bool result = false;
            if (Guid.TryParse(requestId, out Guid guid) && guid != Guid.Empty)
            {
                var updateProductCommand = new IdentifiedCommand<UpdateProductCommand, bool>(command, guid);
                result = await _mediator.Send(updateProductCommand);
            }

            return result ? (IActionResult)Ok() : (IActionResult)BadRequest();
        }

        // DELETE api/values/5
        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Delete([FromBody]DeleteProductCommand command, [FromHeader(Name = "x-requestid")] string requestId)
        {
            bool result = false;
            if (Guid.TryParse(requestId, out Guid guid) && guid != Guid.Empty)
            {
                var deleteProductCommand = new IdentifiedCommand<DeleteProductCommand, bool>(command, guid);
                result = await _mediator.Send(deleteProductCommand);
            }

            return result ? (IActionResult)Ok() : (IActionResult)BadRequest();
        }
    }
}
