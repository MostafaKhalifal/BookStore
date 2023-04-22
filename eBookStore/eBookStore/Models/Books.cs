using System.ComponentModel.DataAnnotations;

namespace eBookStore.Models
{
	public class Books
	{
		[Key]
		public int Id { get; set; }
		[Required]
		public string Title { get; set; }
		[Required]
		public string Description { get; set; }
		public string Author { get; set; }
		[Required]
		public double Price { get; set; }
		public float Rating { get; set; }
		public string Category { get; set; }
	}
}
