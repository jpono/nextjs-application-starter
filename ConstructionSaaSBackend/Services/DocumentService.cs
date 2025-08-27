using ConstructionSaaSBackend.Data;
using ConstructionSaaSBackend.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConstructionSaaSBackend.Services
{
    public interface IDocumentService
    {
        Task<Document?> GetDocumentByIdAsync(int documentId);
        Task<IEnumerable<Document>> GetDocumentsByProjectAsync(int projectId);
        Task<IEnumerable<Document>> GetDocumentsByTenantAsync(int tenantId);
        Task<Document> CreateDocumentAsync(Document document);
        Task<Document> UpdateDocumentAsync(int documentId, Document document);
        Task<bool> DeleteDocumentAsync(int documentId);
        Task<bool> DocumentExistsAsync(int documentId);
    }

    public class DocumentService : IDocumentService
    {
        private readonly ApplicationDbContext _context;

        public DocumentService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Document?> GetDocumentByIdAsync(int documentId)
        {
            return await _context.Documents
                .Include(d => d.Tenant)
                .Include(d => d.Project)
                .FirstOrDefaultAsync(d => d.DocumentId == documentId);
        }

        public async Task<IEnumerable<Document>> GetDocumentsByProjectAsync(int projectId)
        {
            return await _context.Documents
                .Include(d => d.Tenant)
                .Include(d => d.Project)
                .Where(d => d.ProjectId == projectId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Document>> GetDocumentsByTenantAsync(int tenantId)
        {
            return await _context.Documents
                .Include(d => d.Tenant)
                .Include(d => d.Project)
                .Where(d => d.TenantId == tenantId)
                .ToListAsync();
        }

        public async Task<Document> CreateDocumentAsync(Document document)
        {
            _context.Documents.Add(document);
            await _context.SaveChangesAsync();
            return document;
        }

        public async Task<Document> UpdateDocumentAsync(int documentId, Document document)
        {
            var existingDocument = await _context.Documents.FindAsync(documentId);
            if (existingDocument == null)
            {
                throw new KeyNotFoundException($"Document with ID {documentId} not found.");
            }

            existingDocument.FileName = document.FileName;
            existingDocument.OriginalFileName = document.OriginalFileName;
            existingDocument.FilePath = document.FilePath;
            existingDocument.ContentType = document.ContentType;
            existingDocument.FileSize = document.FileSize;
            existingDocument.ProjectId = document.ProjectId;
            existingDocument.Category = document.Category;
            existingDocument.Description = document.Description;
            existingDocument.UploadedBy = document.UploadedBy;
            existingDocument.IsActive = document.IsActive;

            await _context.SaveChangesAsync();
            return existingDocument;
        }

        public async Task<bool> DeleteDocumentAsync(int documentId)
        {
            var document = await _context.Documents.FindAsync(documentId);
            if (document == null)
            {
                return false;
            }

            _context.Documents.Remove(document);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DocumentExistsAsync(int documentId)
        {
            return await _context.Documents
                .AnyAsync(d => d.DocumentId == documentId);
        }
    }
}
