using backend.Models;
using System.Collections.Generic;

namespace backend.Services
{
    public static class DocumentStore
    {
        public static List<Document> Documents { get; set; } = new();
    }
}