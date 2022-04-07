using Core.Repository;
using Core.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TKMS.Abstraction.Models;

namespace TKMS.Repository.Interfaces
{
    public interface ICardTypeRepository : IRepository<CardType>
    {
        Task<PagedList> GetCardTypePaged(Pagination pagination);
        Task<PagedList> GetDropdwon(long? id = null, long? c5CodeId = null);
    }
}
