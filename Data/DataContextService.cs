using System;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text;
using System.Net.Http.Headers;
using Radzen;

using BFGBlazor.Data;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using System.Linq.Dynamic.Core;
using System.Data;

namespace BFGBlazor.Data
{
    public partial class DataContextService
    {
        private readonly ApplicationDbContext _context;
        private readonly IServiceProvider _serviceProvider;

        public DataContextService(IServiceProvider serviceProvider, ApplicationDbContext context)
        {
            //_context = context;
            // db = new ApplicationDbContext(services, dbOptions);
            _serviceProvider = serviceProvider;
            _context = context;
        }


        private static readonly HttpClient client = new HttpClient();

        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        public Task<WeatherForecast[]> GetForecastAsync(DateTime startDate)
        {

            var rng = new Random();
            return Task.FromResult(Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = startDate.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            }).ToArray());
        }

        partial void OnHomeLoansRead(ref IQueryable<HomeLoans> homeLoans);

        public async Task<IEnumerable<HomeLoans>> GetHomeLoans(Query query = null)
        {
            var items = _context.HomeLoans.AsQueryable();

            items = items.Include(i => i.Residential);
            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Filter))
                {
                    items = items.Where(query.Filter);
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach (var p in propertiesToExpand)
                    {
                        items = items.Include(p);
                    }
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }


            OnHomeLoansRead(ref items);

           return await Task.FromResult(items);
        }
    }
}
