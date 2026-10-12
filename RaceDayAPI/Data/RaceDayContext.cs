using Microsoft.EntityFrameworkCore;
using RaceDayAPI.Models;

namespace RaceDayAPI.Data
{
	public class RaceDayContext : DbContext
	{
		public RaceDayContext(DbContextOptions<RaceDayContext> options)
			: base(options)
		{
		}

		public DbSet<Event> Events { get; set; }
	}
}