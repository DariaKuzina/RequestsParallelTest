using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace TasksTests.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class RandomNumberController : ControllerBase
	{
		private const string address = "http://www.randomnumberapi.com/api/v1.0/random";
		private HttpClient client = new HttpClient();


		[HttpGet]
		[Route("WhenAll")]
		public async Task<IEnumerable<string>> GetWhenAll()
		{
			var tasks = Enumerable.Range(0, 1000).Select(i => GetRandomNumberAsync(i));
			return await Task.WhenAll(tasks);
		}

		[HttpGet]
		[Route("AwaitInLoop")]
		public async Task<IEnumerable<string>> GetAwaitInLoop()
		{
			var indexes = Enumerable.Range(0, 100);
			var result = new List<string>();

			foreach (var i in indexes)
				 result.Add(await GetRandomNumberAsync(i));

			return result;
		}

		private async Task<string> GetRandomNumberAsync(int min)
		{
			var result = "";
			HttpResponseMessage response = await client.GetAsync($"{address}?min={min}");
			if (response.IsSuccessStatusCode)
			{
				result = await response.Content.ReadAsStringAsync();
			}
			return result;
		}
	}
}
