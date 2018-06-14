using Microsoft.AspNetCore.Mvc;
using SOOIDN.Api.Logging;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace SOOIDN.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public sealed class CollatzController 
		: ControllerBase
	{
		private readonly ILogger logger;

		public CollatzController(ILogger logger) => 
			this.logger = logger ?? throw new ArgumentNullException(nameof(logger));

		[HttpGet("{value}")]
		public ActionResult<string> Get(string value)
		{
			this.logger.Log(
				$"{nameof(CollatzController)}.{nameof(CollatzController.Get)}, value is {value}");

			if(BigInteger.TryParse(value, out var number))
			{
				var sequence = new List<BigInteger>() { number };

				while (number > 1)
				{
					number = number % 2 == 0 ?
						number / 2 : ((3 * number) + 1) / 2;
					sequence.Add(number);
				}

				this.logger.Log(
					$"{nameof(CollatzController)}.{nameof(CollatzController.Get)}, sequence length for {value} is {sequence.Count}");

				return Ok(sequence);
			}
			else
			{
				return BadRequest($"The given value, {value}, is not a valid integer.");
			}
		}
	}
}
