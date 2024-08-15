using Data.Repositories;
using Shared.Dto;
using Shared;

namespace DocManager.Services
{
    public interface IDocumentService
    {
        public Task<IResult> Create(DocInfo doc, HttpContext httpContext);
        public Task<IResult> GetByUser(HttpContext httpContext);
    }

    public class DocumentService : IDocumentService
    {
        private readonly IConfiguration _configuration;
        private readonly IDocumentRepositoriy _documentRepositoriy;

        public DocumentService(IConfiguration configuration, IDocumentRepositoriy documentRepositoriy)
        {
            _configuration = configuration;
            _documentRepositoriy = documentRepositoriy;
        }

        public async Task<IResult> Create(DocInfo doc, HttpContext httpContext)
        {
            if (doc is null)
                return Results.BadRequest(new { details = "Ошибка, документ не был получен" });

            await _documentRepositoriy.Add(doc);
            return Results.Ok();
        }

        public async Task<IResult> GetByUser(HttpContext httpContext)
        {
            var documents = await _documentRepositoriy.Get();
            if (documents is null)
                return Results.BadRequest(new { details = "Ошибка, документы не были получены" });
            return Results.Json(documents);
        }
    }
}

