﻿using Polly;
using Polly.Retry;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace SOOIDN.Client
{
	class Program
	{
		private const int RetryCount = 4;
		private static readonly TimeSpan Retry = TimeSpan.FromMilliseconds(100);
		// Under IIS Express...
		//private static readonly Uri BaseUri = new Uri("http://localhost:61730");

		// Otherwise...
		private static readonly Uri BaseUri = new Uri("http://localhost:5000");
		private const string CollatzGetApi = "/api/collatz";

		static async Task Main()
		{
			var policy = Program.CreatePolicy();

			await Console.Out.WriteLineAsync("Enter integers to retrive its Collatz sequence.");

			using (var client = new HttpClient())
			{
				while (true)
				{
					var value = await Console.In.ReadLineAsync();

					var result = await policy.ExecuteAsync(
						async () => await client.GetAsync(new Uri(Program.BaseUri, $"{Program.CollatzGetApi}/{value}")));

					await Console.Out.WriteLineAsync(await result.Content.ReadAsStringAsync());
				}
			}
		}

		private static RetryPolicy CreatePolicy() =>
			Policy.Handle<HttpRequestException>(e =>
				{
					Console.WriteLine($"Could not call the service, sorry!");
					return true;
				}).WaitAndRetryAsync(
					Program.RetryCount, retryAttempt =>
					{
						Console.WriteLine($"Attempt {retryAttempt} of {Program.RetryCount} to call service...");
						return Program.Retry;
					});
	}
}
