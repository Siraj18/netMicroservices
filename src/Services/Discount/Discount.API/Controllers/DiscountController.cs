﻿using Discount.API.Entities;
using Discount.API.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Discount.API.Controllers
{
	[Route("api/v1/[controller]")]
	[ApiController]
	public class DiscountController : ControllerBase
	{
		private readonly IDiscountRepository _repository;
		public DiscountController(IDiscountRepository repository)
		{
			_repository = repository;
		}

		[HttpGet("{productName}", Name = "GetDiscount")]
		[ProducesResponseType((int)HttpStatusCode.OK)]
		public async Task<ActionResult<Coupon>> GetDiscount(string productName)
		{
			var coupon = await _repository.GetDiscount(productName);
			return Ok(coupon);
		}

		[HttpPost]
		[ProducesResponseType((int)HttpStatusCode.Created)]
		public async Task<ActionResult<Coupon>> CreateDiscount([FromBody]Coupon coupon)
		{
			await _repository.CreateDiscount(coupon);
			return CreatedAtRoute("GetDiscount", new { ProductName = coupon.ProductName }, coupon);
		}

		[HttpPut]
		[ProducesResponseType((int)HttpStatusCode.OK)]
		public async Task<ActionResult<bool>> UpdateDiscount([FromBody] Coupon coupon)
		{
			return Ok(await _repository.UpdateDiscount(coupon));
		}

		[HttpDelete("{productName}", Name = "DeleteDiscount")]
		[ProducesResponseType((int)HttpStatusCode.OK)]
		public async Task<ActionResult<bool>> DeleteDiscount(string productName)
		{
			return Ok(await _repository.DeleteDiscount(productName));
		}
	}
}