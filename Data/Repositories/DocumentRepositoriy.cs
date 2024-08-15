using ApplicationDB;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using Shared.Dto;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
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

            DocumentAccess access = new DocumentAccess()
            {
                Access_level = 0,
                Document = document,
                User = user,
            };

            await _Context.AddAsync(access);

            await _Context.AddAsync(document);
            await _Context.SaveChangesAsync();
        }

        public async Task<List<DocInfoGet>> Get()
        {
            //return await _Context.Documents
            //    .AsNoTracking()
            //    .OrderBy(x => x.Id)
            //    .ToListAsync();

            var docs = from d in _Context.Documents
                       select new DocInfoGet()
                       {
                           Id = d.Id,
                           Name = d.Name,
                           Description = d.Desc,
                           CreatedDate = d.Created,
                           ExpireDate = d.ExpireDate,
                           Path = d.Path,
                           UserId = d.User.Id,
                       };
            var doc_list = docs.ToList();
            return doc_list;
        }

        public Task<Document?> GetByID(int id)
        {
            throw new NotImplementedException();
        }
    }
}
