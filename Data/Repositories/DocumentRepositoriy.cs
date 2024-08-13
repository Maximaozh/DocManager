using ApplicationDB;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using Shared.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class DocumentRepositoriy : IDocumentRepositoriy
    {
        public AppContextDB _Context { get; set; }

        public DocumentRepositoriy(AppContextDB appContextDB)
        {
            _Context = appContextDB;
        }
        public async Task Add(DocInfo documentInfo)
        {
            var user = await _Context.Users.FirstOrDefaultAsync(u => u.Id == documentInfo.UserId)
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
            
            await _Context.AddAsync(document);
            await _Context.SaveChangesAsync();
        }

        public async Task<List<Document>> Get()
        {
            return await _Context.Documents
                .AsNoTracking()
                .OrderBy(x => x.Id)
                .ToListAsync();
        }

        public Task<Document?> GetByID(int id)
        {
            throw new NotImplementedException();
        }
    }
}
