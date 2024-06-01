using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using MinimalAPIAssignment.Data;
using MinimalAPIAssignment.Models;
using System.Text.RegularExpressions;

var builder = WebApplication.CreateBuilder(args);
//Adding DBContext Class to the Services
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DbConnection") ?? throw new InvalidOperationException("Connection string 'DbContext' not found."));
});
builder.Services.AddEndpointsApiExplorer();
//Adding Swagger Service to the Web Application
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Books And Authors API",
        Version = "v1"
    });
});

var app = builder.Build();

app.UseSwagger();

// Enable middleware to serve Swagger UI (HTML, JS, CSS, etc.),
// specifying the Swagger JSON endpoint.
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Library Management API v1");
    c.RoutePrefix = string.Empty; // Serve the Swagger UI at the app's root
});

// get All Books
app.MapGet("/books", async (AppDbContext db) => await db.Books.ToListAsync());

// get Book on Id
app.MapGet("/books/{id}", async (int id, AppDbContext db) =>
{
    var book = await db.Books.FindAsync(id);
    return book is not null ? Results.Ok(book) : Results.NotFound();
});

// Creating Book Details
app.MapPost("/books", async (Book book, AppDbContext db) =>
{

    if (string.IsNullOrEmpty(book.Title) || book.AuthorId == null || string.IsNullOrEmpty(book.ISBN))
    {
        return Results.BadRequest("Title, Author, and ISBN cannot be empty");
    }
    var existingBook = await db.Books.FirstOrDefaultAsync(b => b.ISBN == book.ISBN);
    if (existingBook != null)
    {
        return Results.Conflict("A book with the same ISBN already exists");
    }
    if (!IsValidIsbn(book.ISBN))
    {
        return Results.BadRequest("Not a Valid ISBN Value");
    }
    var author = await db.Authors.FirstOrDefaultAsync(b => b.Id == book.AuthorId);
    if (author is null)
    {
        return Results.BadRequest($"Author with ID {book.AuthorId} not found");
    }
    db.Books.Add(book);
    await db.SaveChangesAsync();
    return Results.Created($"/books/{book.Id}", book);
});

// Updating the Book Details
app.MapPut("/books/{id}", async (int id, Book updatedBook, AppDbContext db) =>
{

    var book = await db.Books.FindAsync(id);
    if (book is null)
    {
        return Results.NotFound();
    }
    if (updatedBook.ISBN != book.ISBN & updatedBook.ISBN != null)
    {
        return Results.BadRequest("ISBN cannot be changed");
    }

    if (updatedBook.AuthorId != book.AuthorId & updatedBook.AuthorId != null)
    {
        return Results.BadRequest("Author of the book cannot be changed");
    }
    book.Title = updatedBook.Title;
    book.PublicationDate = updatedBook.PublicationDate;

    await db.SaveChangesAsync();
    return Results.Ok(book);
});

// Deleting Book on Id
app.MapDelete("/books/{id}", async (int id, AppDbContext db) =>
{
    var book = await db.Books.FindAsync(id);
    if (book is null)
    {
        return Results.NotFound();
    }
    db.Books.Remove(book);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

// Get all Authors
app.MapGet("/authors", async (AppDbContext db) => await db.Authors.Include(a => a.Books).ToListAsync());

//Get Author Based on ID
app.MapGet("/authors/{id}", async (int id, AppDbContext db) =>
{
    var author = await db.Authors.Include(a => a.Books).FirstOrDefaultAsync(a => a.Id == id);
    return author is not null ? Results.Ok(author) : Results.NotFound();
});

// Creating Author Details
app.MapPost("/authors", async (Author author, AppDbContext db) =>
{
    if (string.IsNullOrEmpty(author.Name))
    {
        return Results.BadRequest("Name cannot be empty");
    }
    db.Authors.Add(author);
    await db.SaveChangesAsync();
    return Results.Created($"/authors/{author.Id}", author);
});

// Updating the Author Details.
app.MapPut("/authors/{id}", async (int id, Author updatedAuthor, AppDbContext db) =>
{
    var author = await db.Authors.Include(a => a.Books).FirstOrDefaultAsync(a => a.Id == id);
    if (author is null)
    {
        return Results.NotFound();
    }

    if (string.IsNullOrEmpty(updatedAuthor.Name))
    {
        return Results.BadRequest("Name cannot be empty");
    }

    author.Name = updatedAuthor.Name;
    author.Books = updatedAuthor.Books;

    await db.SaveChangesAsync();
    return Results.Ok(author);
});

// Delete Author Based on Id
app.MapDelete("/authors/{id}", async (int id, AppDbContext db) =>
{
    var author = await db.Authors.FindAsync(id);
    if (author is null)
    {
        return Results.NotFound();
    }
    db.Authors.Remove(author);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

 static bool IsValidIsbn(string isbn)
{
    
    if (string.IsNullOrEmpty(isbn))
        return false;

    isbn = isbn.Replace("-", "");

    // ISBN-10 validation
    if (Regex.IsMatch(isbn, @"^\d{9}[\dX]$"))
    {
        return true;
    }
    // ISBN-13 validation
    if (Regex.IsMatch(isbn, @"^\d{13}$"))
    {
        return true;
    }

    return false;
}
app.Run();
