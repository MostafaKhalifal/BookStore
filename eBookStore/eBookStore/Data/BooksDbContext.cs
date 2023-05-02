using eBookStore.Models;
using Microsoft.EntityFrameworkCore;

namespace eBookStore.Data
{
	public class BooksDbContext : DbContext
	{
        public BooksDbContext(DbContextOptions<BooksDbContext> options) : base(options)
        {
        }
        
        public DbSet<Book> books { get; set; }
        
        public DbSet<eBookStore.Models.usersaccounts>? usersaccounts { get; set; }
         
    }
}
