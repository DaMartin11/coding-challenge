using backend.Models;
using backend.Services;
using Microsoft.AspNetCore.Http;
// No need for using Microsoft.AspNetCore.Mvc; specifically for DisableAntiforgery() here.

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAntiforgery(); // Keep this for the service registration

var app = builder.Build();
app.UseAntiforgery(); // Keep this for the middleware

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseStaticFiles();


// --- In-memory document store endpoints ---

// Upload single file
app.MapPost("/api/documents/upload", async (IFormFile file) =>
{
    var uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
    Directory.CreateDirectory(uploadDir);

    var filePath = Path.Combine(uploadDir, file.FileName);

    using var stream = new FileStream(filePath, FileMode.Create);
    await file.CopyToAsync(stream);

    var doc = new Document
    {
        Name = file.FileName,
        Type = file.ContentType,
        FilePath = filePath
    };

    DocumentStore.Documents.Add(doc);

    return Results.Ok(doc);
})
.DisableAntiforgery(); // <--- THIS IS THE CORRECT WAY to disable anti-forgery for Minimal APIs!

// Get all documents
app.MapGet("/api/documents", () =>
{
    return DocumentStore.Documents;
});

// Download a document
app.MapGet("/api/documents/{id}/download", (Guid id) =>
{
    var doc = DocumentStore.Documents.FirstOrDefault(d => d.Id == id);
    if (doc == null) return Results.NotFound();

    return Results.File(doc.FilePath, contentType: doc.Type, fileDownloadName: doc.Name);
});

app.Run();