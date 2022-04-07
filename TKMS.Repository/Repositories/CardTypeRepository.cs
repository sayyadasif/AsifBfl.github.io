using Core.Repository;
using Core.Repository.Models;
using Core.Utility.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TKMS.Abstraction.ComplexModels;
using TKMS.Abstraction.Models;
using TKMS.Repository.Contexts;
using TKMS.Repository.Interfaces;

namespace TKMS.Repository.Repositories
{
    public class CardTypeRepository : Repository<CardType>, ICardTypeRepository
    {
        public CardTypeRepository(TkmsDbContext context)
            : base(context)
        { }

        private TkmsDbContext TkmsDbContext
        {
            get { return _dbContext as TkmsDbContext; }
        }

        public async Task<PagedList> GetCardTypePaged(Pagination pagination)
        {
            string cardTypeName = IsPropertyExist(pagination.Filters, "cardTypeName") ? pagination.Filters?.cardTypeName : null;

            IRepository<CardTypeModel> repositoryCardTypeModel = new Repository<CardTypeModel>(TkmsDbContext);
            var query = (from ct in TkmsDbContext.CardTypes
                         where !ct.IsDeleted
                          && (string.IsNullOrEmpty(cardTypeName) || ct.CardTypeName.Contains(cardTypeName))
                         select new CardTypeModel
                         {
                             CardTypeId = ct.CardTypeId,
                             CardTypeName = ct.CardTypeName,
                         });

            return await repositoryCardTypeModel.GetPagedReponseAsync(pagination, null, query);
        }

        public async Task<PagedList> GetDropdwon(long? id = null, long? c5CodeId = null)
        {
            if (c5CodeId.HasValue && c5CodeId > 0)
            {
                id = TkmsDbContext.C5Codes.FirstOrDefault(sc => sc.C5CodeId == c5CodeId).CardTypeId;
            }

            Repository<DropdownModel> repositoryDropdownModel = new(TkmsDbContext);
            var query = (from r in TkmsDbContext.CardTypes
                         where !r.IsDeleted &&
                         (!id.HasValue || r.CardTypeId == id)
                         select new DropdownModel()
                         {
                             Id = r.CardTypeId,
                             Text = r.CardTypeName,
                             IsActive = r.IsActive
                         });

            return await repositoryDropdownModel.GetPagedReponseAsync(new Pagination { SortOrderColumn = "Text" }, null, query);
        }
    }
}
