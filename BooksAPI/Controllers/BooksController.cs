#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BooksAPI.Data;
using BooksAPI.Models;
using Microsoft.AspNetCore.JsonPatch;

namespace BooksAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public BooksController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Books
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> GetBooks()
        {
            return await _context.Books.ToListAsync();
        }

        // GET: api/Books/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetBook(int id)
        {
            var book = await _context.Books.FindAsync(id);

            if (book == null)
            {
                return NotFound();
            }

            return book;
        }

        // PUT: api/Books/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBook(int id, Book book)
        {
            if (id != book.BookId)
            {
                return BadRequest();
            }

            _context.Entry(book).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Books
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Book>> PostBook(Book book)
        {
            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBook", new { id = book.BookId }, book);
        }

        // DELETE: api/Books/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // PATCH: api/Books/5
        [HttpPatch("{id}")]
        public async Task<ActionResult<Book>> PatchBook(int id, [FromBody] JsonPatchDocument<Book> patch)
        {
            if (patch == null)
            {
                return BadRequest();
            }
            Book item = _context.Books.SingleOrDefault(b => b.BookId == id);
            if (item != null)
            {
                patch.ApplyTo(item, ModelState);

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                foreach (var op in patch.Operations)
                {
                    if (op.path.ToLower() == "/bookid")
                    {
                        return BadRequest();
                    }
                }

                _context.Entry(item).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok(item);
            }
            else
            {
                return NotFound();
            }
        }

/*
 [
  {
    "path": "/pages",
    "op": "replace",
    "value": 130
  },
{
    "path": "/title",
    "op": "replace",
    "value": "Dědeček"
  }
] 
*/



        private bool BookExists(int id)
        {
            return _context.Books.Any(e => e.BookId == id);
        }
    }

    public class PatchBookViewModel
    {
        public List<Book> books { get; set; }
    }
}
