//// Assuming you're using ASP.NET MVC with a controller named "CartController"
//using Microsoft.AspNetCore.Mvc;
//using WebApplication5.Models;

//public class CartController : Controller
//{
//	// Add to Cart button click handler
//	[HttpPost]
//	public ActionResult AddToCart(int bookId)
//	{
//		// Fetch book details from the database using the bookId
//		var book = _db.Books.FirstOrDefault(b => b.BookId == bookId);

//		if (book != null)
//		{
//			// Create a new cart item
//			var cartItem = new CartItem
//			{
//				BookId = book.BookId,
//				BookName = book.BookName,
//				Price = book.Price,
//				Quantity = 1
//			};

//			// Save the cart item to the database
//			_db.Cart.Add(cartItem);
//			_db.SaveChanges();
//		}

//		return RedirectToAction("Index", "Home"); // Redirect to the product listing page
//	}
//}
