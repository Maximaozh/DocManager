using Shared.Dto;
using DocManager.Services;

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

        private static async Task<IResult> Creation(DocumentService documentService, DocInfo document, HttpContext httpContext)
        {
            IResult response = await documentService.Create(document, httpContext);
            return response;
        }

        private static async Task<IResult> GettingByUser(DocumentService documentService, HttpContext httpContext)
        {
            IResult response = await documentService.GetByUser(httpContext);
            return response;
        }
    }
}