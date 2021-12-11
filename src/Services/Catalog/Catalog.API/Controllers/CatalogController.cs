using Catalog.API.Entities;
using Catalog.API.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Catalog.API.Controllers
{
	[ApiController]
	[Route("api/v1/[controller]")]
	public class CatalogController: ControllerBase
	{
		private readonly IProductRepository _repository;
		private readonly ILogger<CatalogController> _logger;
		public CatalogController(IProductRepository repository, ILogger<CatalogController> logger)
		{
			_repository = repository;
			_logger = logger;
		}

		[HttpGet]
		[ProducesResponseType((int) HttpStatusCode.OK)]
		public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
		{
			var products = await _repository.GetProducts();
			var s = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
			return Ok(products);
		}


		[HttpGet("{id:length(24)}", Name = "GetProduct"),]
		[ProducesResponseType((int)HttpStatusCode.NotFound)]
		[ProducesResponseType((int)HttpStatusCode.OK)]
		public async Task<ActionResult<Product>> GetProductById(string id)
		{
			var product = await _repository.GetProduct(id);

			if (product == null)
			{
				_logger.LogError($"product with id = {id} not found");
				return NotFound();
			}

			return Ok(product);
		}


		[Route("[action]/{name}", Name = "GetProductByName")]
		[HttpGet]
		[ProducesResponseType((int)HttpStatusCode.NotFound)]
		[ProducesResponseType((int)HttpStatusCode.OK)]
		public async Task<ActionResult<Product>> GetProductByName(string name)
		{
			var product = await _repository.GetProductByName(name);

			if (product == null)
			{
				_logger.LogError($"product with id = {name} not found");
				return NotFound();
			}

			return Ok(product);
		}


		[Route("[action]/{category}", Name = "GetProductByCategory")]
		[HttpGet]
		[ProducesResponseType((int)HttpStatusCode.OK)]
		public async Task<ActionResult<IEnumerable<Product>>> GetProductsByCategory(string category)
		{
			var products = await _repository.GetProductsByCategory(category);

			return Ok(products);
		}


		[HttpPost]
		[ProducesResponseType((int)HttpStatusCode.OK)]
		public async Task<ActionResult<Product>> CreateProduct([FromBody] Product product)
		{
			await _repository.CreateProduct(product);

			return CreatedAtRoute("GetProduct", new { id = product.Id }, product);
		}
		

		[HttpPut]
		[ProducesResponseType((int)HttpStatusCode.OK)]
		public async Task<IActionResult> UpdateProducts([FromBody] Product product)
		{
			

			return Ok(await _repository.UpdateProduct(product));
		}

		[HttpDelete("{id:length(24)}", Name = "DeleteProduct")]
		[ProducesResponseType((int)HttpStatusCode.OK)]
		public async Task<IActionResult> DeleteProduct(string id)
		{

			return Ok(await _repository.DeleteProduct(id));
			
		}
	}
}
