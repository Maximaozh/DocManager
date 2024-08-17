using DocManager.Services;
using Shared.Dto.Document;

namespace DocManager.Endpoints
{
    public static class DocumentEndpoints
    {
        public static IEndpointRouteBuilder MapDocEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapPost("api/Document", Creation);
            app.MapGet("api/DocWork", GettingByUser);

            return app;
        }

        private static async Task<IResult> Creation(DocumentService documentService, DocInfo document)
        {
            int response = await documentService.Create(document);
            return Results.Ok(response);
        }

        private static async Task<IResult> GettingByUser(DocumentService documentService)
        {
            List<DocInfoGet> response = await documentService.GetByUser();
            return Results.Json(response);
        }
    }
}