using Data.Models;
using Shared.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public interface IDocumentRepositoriy
    {
        Task Add(DocInfo documentInfo);
        Task<List<DocInfoGet>> Get();
        Task<Document?> GetByID(int id);
    }
}
