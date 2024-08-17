using ApplicationDB;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using Shared.Dto.Document;

namespace DocManager.Services;


public class DocumentService(AppContextDB dbContext)
{


    public async Task<int> Create(DocInfo documentInfo)
    {
        if (documentInfo is null)
        {
            return -1;
        }

        User user = await dbContext.Users.FirstOrDefaultAsync(u => u.Id == documentInfo.Id)
            ?? throw new Exception();
        Document document = new Document()
        {
            Name = documentInfo.Name,
            Path = documentInfo.Path,
            Created = documentInfo.CreatedDate,
            ExpireDate = documentInfo.ExpireDate,
            Desc = documentInfo.Description,
        };

        user.Documents.Add(document);

        DocumentAccess access = new DocumentAccess()
        {
            Access_level = 0,
            Document = document,
            User = user,
        };

        await dbContext.AddAsync(access);

        await dbContext.AddAsync(document);
        await dbContext.SaveChangesAsync();
        return 0;
    }


    // Данил, тут пока возвращаются все доки, доделай чтобы было по ID
    public async Task<List<DocInfoGet>> GetByUser()
    {
        List<DocInfoGet> docs = await dbContext
        .Documents
        .AsNoTracking()
        .Select(d => new DocInfoGet()
        {
            Id = d.Id,
            Name = d.Name,
            Description = d.Desc,
            CreatedDate = d.Created,
            ExpireDate = d.ExpireDate,
            Path = d.Path,
            UserId = d.User.Id
        })
        .ToListAsync();

        return docs;
    }
}
