using ConstructionSaaSBackend.Models;
using ConstructionSaaSBackend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ConstructionSaaSBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class DocumentController : BaseController
    {
        private readonly IDocumentService _documentService;

        public DocumentController(IDocumentService documentService)
        {
            _documentService = documentService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDocuments()
        {
            var tenantId = GetCurrentTenantId();
            var documents = await _documentService.GetDocumentsByTenantAsync(tenantId);
            return Ok(documents);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDocument(int id)
        {
            var tenantId = GetCurrentTenantId();
            var document = await _documentService.GetDocumentByIdAsync(id);
            
            if (document == null || document.TenantId != tenantId)
            {
                return NotFound();
            }
            
            return Ok(document);
        }

        [HttpGet("project/{projectId}")]
        public async Task<IActionResult> GetDocumentsByProject(int projectId)
        {
            var tenantId = GetCurrentTenantId();
            var documents = await _documentService.GetDocumentsByProjectAsync(projectId);
            
            // Filter by tenant to ensure security
            var tenantDocuments = documents.Where(d => d.TenantId == tenantId);
            
            return Ok(tenantDocuments);
        }

        [HttpPost]
        public async Task<IActionResult> CreateDocument([FromBody] Document document)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Set tenant ID from current context
            document.TenantId = GetCurrentTenantId();

            var createdDocument = await _documentService.CreateDocumentAsync(document);
            return CreatedAtAction(nameof(GetDocument), new { id = createdDocument.DocumentId }, createdDocument);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDocument(int id, [FromBody] Document document)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var tenantId = GetCurrentTenantId();
            var existingDocument = await _documentService.GetDocumentByIdAsync(id);
            
            if (existingDocument == null || existingDocument.TenantId != tenantId)
            {
                return NotFound();
            }

            try
            {
                var updatedDocument = await _documentService.UpdateDocumentAsync(id, document);
                return Ok(updatedDocument);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDocument(int id)
        {
            var tenantId = GetCurrentTenantId();
            var document = await _documentService.GetDocumentByIdAsync(id);
            
            if (document == null || document.TenantId != tenantId)
            {
                return NotFound();
            }

            var result = await _documentService.DeleteDocumentAsync(id);
            if (!result)
            {
                return NotFound();
            }
            
            return NoContent();
        }
    }
}
