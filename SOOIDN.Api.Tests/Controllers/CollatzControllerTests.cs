using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using Rocks;
using SOOIDN.Api.Controllers;
using SOOIDN.Api.Logging;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace SOOIDN.Api.Tests.Controllers
{
	[TestFixture]
	public static class CollatzControllerTests
	{
		[Test]
		public static void CreateWithNull() => 
			Assert.That(() => new CollatzController(null), Throws.TypeOf<ArgumentNullException>());

		[Test]
		public static void CalculateWithValue()
		{
			var value = "5";
			var logger = Rock.Create<ILogger>();
			logger.Handle(_ => _.Log(Arg.IsAny<string>()), 2);

			var controller = new CollatzController(logger.Make());
			var result = controller.Get(value).Result as OkObjectResult;
			var resultValue = result.Value as List<BigInteger>;

			Assert.That(result.StatusCode, Is.EqualTo(200));
			Assert.That(resultValue.Count, Is.EqualTo(5));

			logger.Verify();
		}

		[Test]
		public static void CalculateWithInvalidValue()
		{
			var value = "wrong";
			var logger = Rock.Create<ILogger>();
			logger.Handle(_ => _.Log(Arg.IsAny<string>()));

			var controller = new CollatzController(logger.Make());
			var result = controller.Get(value).Result as BadRequestObjectResult;

			Assert.That(result.StatusCode, Is.EqualTo(400));

			logger.Verify();
		}
	}
}
