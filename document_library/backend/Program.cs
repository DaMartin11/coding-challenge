using backend.Models;
using backend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAntiforgery();

var app = builder.Build();
app.UseAntiforgery();
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseStaticFiles();

// --- Upload single document ---
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
        FilePath = filePath,
        UploadDate = DateTime.UtcNow,
        DownloadCount = 0,
        PreviewUrl = "/previews/default.jpg",
        IconUrl = GetIconForType(file.ContentType)
    };

    DocumentStore.Documents.Add(doc);

    return Results.Ok(doc);
})
.DisableAntiforgery();

// --- Upload several documents ---
app.MapPost("/api/documents/upload-multiple", async ([FromForm] List<IFormFile> files) =>
{
    if (files == null || files.Count == 0)
    {
        return Results.BadRequest("No files uploaded.");
    }

    var uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
    Directory.CreateDirectory(uploadDir);

    var uploadedDocs = new List<Document>();

    foreach (var file in files)
    {
        var filePath = Path.Combine(uploadDir, file.FileName);

        using var stream = new FileStream(filePath, FileMode.Create);
        await file.CopyToAsync(stream);

        var doc = new Document
        {
            Name = file.FileName,
            Type = file.ContentType,
            FilePath = filePath,
            UploadDate = DateTime.UtcNow,
            DownloadCount = 0,
            PreviewUrl = "/previews/default.jpg",
            IconUrl = GetIconForType(file.ContentType)
        };

        DocumentStore.Documents.Add(doc);
        uploadedDocs.Add(doc);
    }

    return Results.Ok(uploadedDocs);
})
.Accepts<List<IFormFile>>("multipart/form-data")
.DisableAntiforgery();

// --- Get all documents ---
app.MapGet("/api/documents", () =>
{
    return DocumentStore.Documents;
});

// --- Download a document by ID ---
app.MapGet("/api/documents/{id}/download", (Guid id) =>
{
    var doc = DocumentStore.Documents.FirstOrDefault(d => d.Id == id);

    if (doc == null)
    {
        return Results.NotFound();
    }

    if (!System.IO.File.Exists(doc.FilePath))
    {
        return Results.Problem("File not found on server.", statusCode: 404);
    }

    doc.DownloadCount++;

    return Results.File(
        doc.FilePath,
        contentType: doc.Type,
        fileDownloadName: doc.Name
    );
});
app.MapPost("/api/share/{documentId}/create", (Guid documentId, [FromQuery] int expiresInHours) =>
{
    var doc = DocumentStore.Documents.FirstOrDefault(d => d.Id == documentId);
    if (doc == null)
    {
        return Results.NotFound("Document not found.");
    }

    var newLink = new ShareLink
    {
        DocumentId = documentId,
        ExpiresAt = DateTime.UtcNow.AddHours(expiresInHours)
    };

    ShareLinkStore.Links.Add(newLink);

    return Results.Ok(new
    {
        Token = newLink.Token,
        ExpiresAt = newLink.ExpiresAt
    });
});
// --- Access shared document by token ---
app.MapGet("/api/share/{token}", (Guid token) =>
{
    var link = ShareLinkStore.Links.FirstOrDefault(l => l.Token == token);

    if (link == null)
    {
        return Results.NotFound("Share link not found.");
    }

    if (link.IsExpired)
    {
        return Results.Problem("This share link has expired.", statusCode: 403);
    }

    var doc = DocumentStore.Documents.FirstOrDefault(d => d.Id == link.DocumentId);
    if (doc == null)
    {
        return Results.NotFound("Document not found.");
    }

    if (!System.IO.File.Exists(doc.FilePath))
    {
        return Results.Problem("File not found on server.", statusCode: 404);
    }

    doc.DownloadCount++;

    return Results.File(
        doc.FilePath,
        contentType: doc.Type,
        fileDownloadName: doc.Name
    );
});

app.Run();

// --- Helper function ---
static string GetIconForType(string contentType)
{
    if (contentType.Contains("pdf"))
        return "/icons/pdf.png";
    if (contentType.Contains("word"))
        return "/icons/word.png";
    if (contentType.Contains("excel"))
        return "/icons/excel.png";
    if (contentType.Contains("image"))
        return "/icons/image.png";
    return "/icons/file.png";
}
