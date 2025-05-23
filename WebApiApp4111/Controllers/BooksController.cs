using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Data.SqlClient;
using WebApiApp4111.Data;
using WebApiApp4111.Models;
using WebApiApp4111.Models;

namespace WebApiApp4101.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public BooksController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // GET: api/Books
        [HttpGet]
        public IActionResult GetAllBooks()
        {
            List<Book> books = new List<Book>();
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("LibraryConnection")))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Books", con);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    books.Add(new Book
                    {
                        BookID = (int)reader["BookID"],
                        Title = reader["Title"].ToString(),
                        Author = reader["Author"].ToString(),
                        ISBN = reader["ISBN"].ToString(),
                        YearPublished = (int)reader["YearPublished"]
                    });
                }
            }
            return Ok(books);
        }

        // GET: api/Books/5
        [HttpGet("{id}")]
        public IActionResult GetBookById(int id)
        {
            Book book = null;
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("LibraryConnection")))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Books WHERE BookID = @id", con);
                cmd.Parameters.AddWithValue("@id", id);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    book = new Book
                    {
                        BookID = (int)reader["BookID"],
                        Title = reader["Title"].ToString(),
                        Author = reader["Author"].ToString(),
                        ISBN = reader["ISBN"].ToString(),
                        YearPublished = (int)reader["YearPublished"]
                    };
                }
            }
            return book != null ? Ok(book) : NotFound();
        }

        // POST: api/Books
        [HttpPost]
        public IActionResult AddBook([FromBody] Book book)
        {
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("LibraryConnection")))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO Books (Title, Author, ISBN, YearPublished) VALUES (@Title, @Author, @ISBN, @YearPublished)", con);
                cmd.Parameters.AddWithValue("@Title", book.Title);
                cmd.Parameters.AddWithValue("@Author", book.Author);
                cmd.Parameters.AddWithValue("@ISBN", book.ISBN);
                cmd.Parameters.AddWithValue("@YearPublished", book.YearPublished);
                cmd.ExecuteNonQuery();
            }
            return Ok("Book added successfully.");
        }

        // DELETE: api/Books/5
        [HttpDelete("{id}")]
        public IActionResult DeleteBook(int id)
        {
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("LibraryConnection")))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("DELETE FROM Books WHERE BookID = @id", con);
                cmd.Parameters.AddWithValue("@id", id);
                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected > 0)
                    return Ok("Book deleted.");
                else
                    return NotFound();
            }
        }
    }
}
