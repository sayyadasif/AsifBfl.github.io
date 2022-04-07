using Core.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TKMS.Abstraction.ComplexModels;
using TKMS.Abstraction.Models;

namespace TKMS.Service.Interfaces
{
    public interface IAddressService
    {
        Task<ResponseModel> GetAddressById(long id);
        Task<ResponseModel> GetAllAddress(bool? isActive = null);
        Task<ResponseModel> GetAddressPaged(Pagination pagination);
        Task<ResponseModel> CreateAddress(Address entity);
        Task<ResponseModel> UpdateAddress(Address updateEntity);
        Task<ResponseModel> DeleteAddress(long id);
    }
}
