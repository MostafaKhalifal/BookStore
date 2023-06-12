using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using WebApplication5.Data;
using WebApplication5.Models;

namespace WebApplication5.Controllers
{
	public class ordersController : Controller
	{
		private readonly WebApplication5Context _context;

		public ordersController(WebApplication5Context context)
		{
			_context = context;
		}

		// GET: orders
		public async Task<IActionResult> Index()
		{
			return _context.orders != null ?
						View(await _context.orders.ToListAsync()) :
						Problem("Entity set 'WebApplication5Context.orders'  is null.");
		}

		// GET: orders/Details/5
		public async Task<IActionResult> Details(int? id)
		{
			if (id == null || _context.orders == null)
			{
				return NotFound();
			}

			var orders = await _context.orders
				.FirstOrDefaultAsync(m => m.Id == id);
			if (orders == null)
			{
				return NotFound();
			}

			return View(orders);
		}

		// GET: orders/Create
		public async Task<IActionResult> Create(int? id)
		{
			var book = await _context.book.FindAsync(id);

			return View(book);
		}


		// POST: orders/Create
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(int bookId, int quantity)
		{
			orders order = new orders();
			order.bookId = bookId;
			order.quantity = quantity;

			order.userid = Convert.ToInt32(HttpContext.Session.GetString("userid")); ;
			order.orderdate = DateTime.Today;

			SqlConnection conn = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\EL10_gazy\\Documents\\mynewdb3.mdf;Integrated Security=True;Connect Timeout=30");
			string sql;
			sql = "UPDATE book  SET bookquantity  = bookquantity   - '" + order.quantity + "'  where (id ='" + order.bookId + "' )";
			SqlCommand comm = new SqlCommand(sql, conn);
			conn.Open();
			comm.ExecuteNonQuery();



			_context.Add(order);
			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(myorders));


		}


		public async Task<IActionResult> myorders()
		{

			int id = Convert.ToInt32(HttpContext.Session.GetString("userid")); ;
			var orItems = await _context.orders.FromSqlRaw("select *  from orders where  userid = '" + id + "'  ").ToListAsync();
			return View(orItems);

		}

		public async Task<IActionResult> customerOrders(int? id)
		{


			var orItems = await _context.orders.FromSqlRaw("select *  from orders where  userid = '" + id + "'  ").ToListAsync();
			return View(orItems);

		}

		public async Task<IActionResult> customerreport()
		{
			var orItems = await _context.report.FromSqlRaw("select usersaccounts.id as Id, name as customername, sum (quantity * Price)  as total from book, orders,usersaccounts  where usersaccounts.id = orders.userid  and bookid= book.Id group by name,usersaccounts.id ").ToListAsync();
			return View(orItems);

		}


		// GET: orders/Edit/5
		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null || _context.orders == null)
			{
				return NotFound();
			}

			var orders = await _context.orders.FindAsync(id);
			if (orders == null)
			{
				return NotFound();
			}
			return View(orders);
		}

		// POST: orders/Edit/5
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("Id,bookId,userid,quantity,orderdate")] orders orders)
		{
			if (id != orders.Id)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					_context.Update(orders);
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!ordersExists(orders.Id))
					{
						return NotFound();
					}
					else
					{
						throw;
					}
				}
				return RedirectToAction(nameof(Index));
			}
			return View(orders);
		}

		// GET: orders/Delete/5
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null || _context.orders == null)
			{
				return NotFound();
			}

			var orders = await _context.orders
				.FirstOrDefaultAsync(m => m.Id == id);
			if (orders == null)
			{
				return NotFound();
			}

			return View(orders);
		}

		// POST: orders/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			if (_context.orders == null)
			{
				return Problem("Entity set 'WebApplication5Context.orders'  is null.");
			}
			var orders = await _context.orders.FindAsync(id);
			if (orders != null)
			{
				_context.orders.Remove(orders);
			}

			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		private bool ordersExists(int id)
		{
			return (_context.orders?.Any(e => e.Id == id)).GetValueOrDefault();
		}

		[HttpPost]
        public IActionResult AddToCart(int bookId, int quantity)
        {
            // Get the cart from the session.
            Cart cart = HttpContext.Session.GetObject<Cart>("cart");

            // If the cart doesn't exist, create a new one.
            if (cart == null)
            {
                cart = new Cart();
                HttpContext.Session.SetObject("cart", cart);
            }

            // Find the book in the database.
            book book = _context.book.FirstOrDefault(b => b.Id == bookId);

            if (book != null)
            {
                // Add the item to the cart.
                CartItem cartItem = new CartItem
                {
                    Book = book,
                    Quantity = quantity
                };

                cart.Items.Add(cartItem);

                // Update the total amount of the cart.
                cart.Total += cartItem.Book.price * cartItem.Quantity;

                // Update the cart in the session.
                HttpContext.Session.SetObject("cart", cart);
            }

            // Redirect to the cart view.
            return RedirectToAction("Cart");
        }


        public IActionResult Cart()
		{
			// Get the cart from the session.
			Cart cart = HttpContext.Session.GetObject<Cart>("cart");

			// If the cart doesn't exist, create a new one.
			if (cart == null)
			{
				cart = new Cart();
				HttpContext.Session.SetObject("cart", cart);
			}

			// Return the cart view.
			return View(cart);
		}
	}
}
