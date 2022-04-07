using Core.Repository.Models;
using Core.Security;
using Core.Utility.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TKMS.Abstraction.ComplexModels;
using TKMS.Abstraction.Constants;
using TKMS.Abstraction.Enums;
using TKMS.Abstraction.Models;
using TKMS.Repository.Interfaces;
using TKMS.Service.Interfaces;

namespace TKMS.Service.Services
{
    public class IndentService : IIndentService
    {
        private readonly IIndentRepository _indentRepository;
        private readonly IUserProviderService _userProviderService;
        private readonly ISettingService _settingService;

        public IndentService(
            IIndentRepository indentRepository,
            IUserProviderService userProviderService,
            ISettingService settingService
        )
        {
            _indentRepository = indentRepository;
            _userProviderService = userProviderService;
            _settingService = settingService;
        }

        public async Task<ResponseModel> CreateIndent(Indent entity)
        {
            var validateResult = await ValidateIndant(new List<Indent> { entity });
            if (!validateResult.Success)
            {
                return validateResult;
            }

            entity.CreatedBy = _userProviderService.UserClaim.UserId;
            entity.UpdatedBy = _userProviderService.UserClaim.UserId;
            await _indentRepository.AddAsync(entity);
            var result = await _indentRepository.SaveChangesAsync();
            if (result > 0)
            {
                return new ResponseModel
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Indent created successfully.",
                    Data = entity
                };
            }
            return new ResponseModel { Success = false, StatusCode = StatusCodes.Status500InternalServerError, Message = "Indent does not created." };
        }

        public async Task<ResponseModel> CreateIndents(List<Indent> entities, bool validateIndent = true)
        {
            if (validateIndent)
            {
                var validateResult = await ValidateIndant(entities);
                if (!validateResult.Success)
                {
                    return validateResult;
                }
            }

            foreach (var entity in entities)
            {
                entity.CreatedBy = _userProviderService.UserClaim.UserId;
                entity.UpdatedBy = _userProviderService.UserClaim.UserId;
                await _indentRepository.AddAsync(entity);
            }
            var result = await _indentRepository.SaveChangesAsync();
            if (result > 0)
            {
                return new ResponseModel
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Indents created successfully.",
                    Data = validateIndent ? null : entities,
                };
            }
            return new ResponseModel { Success = false, StatusCode = StatusCodes.Status500InternalServerError, Message = "Indents does not created." };
        }

        public async Task<ResponseModel> DeleteIndent(long id)
        {
            var entityResult = await GetIndentById(id);

            if (!entityResult.Success) { return entityResult; }

            var entity = entityResult.Data as Indent;
            entity.UpdatedBy = _userProviderService.UserClaim.UserId;
            entity.IsDeleted = true;
            var result = await _indentRepository.SaveChangesAsync();

            if (result > 0)
            {
                return new ResponseModel
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Indent deleted successfully.",
                    Data = entity
                };
            }
            return new ResponseModel { Success = false, StatusCode = StatusCodes.Status500InternalServerError, Message = "Indent does not deleted." };
        }

        public async Task<ResponseModel> DeleteIndents(List<Indent> entities)
        {
            _indentRepository.RemoveRange(entities);
            var result = await _indentRepository.SaveChangesAsync();

            if (result > 0)
            {
                return new ResponseModel
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Indents deleted successfully.",
                };
            }
            return new ResponseModel { Success = false, StatusCode = StatusCodes.Status500InternalServerError, Message = "Indents does not deleted." };
        }

        public async Task<ResponseModel> GetAllIndent(bool? isActive = null)
        {
            return new ResponseModel
            {
                Success = true,
                StatusCode = StatusCodes.Status200OK,
                Data = await _indentRepository.Find(a => a.IsDeleted == false && (!isActive.HasValue || a.IsActive == isActive))
            };
        }

        public async Task<ResponseModel> GetIndentPaged(Pagination pagination)
        {
            return new ResponseModel
            {
                Success = true,
                StatusCode = StatusCodes.Status200OK,
                Data = await _indentRepository.GetIndentPaged(pagination)
            };
        }

        public async Task<ResponseModel> GetIndentById(long id)
        {
            var result = await _indentRepository.SingleOrDefaultAsync(a => a.IndentId == id);
            if (result != null)
            {
                return new ResponseModel { Success = true, StatusCode = StatusCodes.Status200OK, Data = result };
            }
            else
            {
                return new ResponseModel { Success = false, StatusCode = StatusCodes.Status404NotFound, Message = "Indent does not exists." };
            }
        }

        public async Task<IndentDetailModel> GetIndentDetailById(long id)
        {
            return await _indentRepository.GetIndentDetailById(id);
        }

        public async Task<List<IndentDetailModel>> GetIndentDetailByIndentNos(List<string> indentNos)
        {
            return await _indentRepository.GetIndentDetailByIndentNos(indentNos);
        }

        public async Task<ResponseModel> UpdateIndentStatus(Indent updateEntity)
        {
            var entityResult = await GetIndentById(updateEntity.IndentId);

            if (!entityResult.Success) { return entityResult; }

            var entity = entityResult.Data as Indent;
            entity.IndentStatusId = updateEntity.IndentStatusId;
            entity.RejectionReasonId = updateEntity.RejectionReasonId;
            if (_userProviderService.HOApprover() &&
               (updateEntity.IndentStatusId == IndentStatuses.Approved.GetHashCode() ||
                updateEntity.IndentStatusId == IndentStatuses.Rejected.GetHashCode()))
            {
                entity.HoApproveStatusId = updateEntity.IndentStatusId;
                entity.HoApproveBy = _userProviderService.UserClaim.UserId;
                entity.HoApproveDate = CommonUtils.GetDefaultDateTime();
            }
            if (_userProviderService.IBLCPU() &&
               (updateEntity.IndentStatusId == IndentStatuses.IndentForDispatch.GetHashCode() ||
                updateEntity.IndentStatusId == IndentStatuses.Rejected.GetHashCode()))
            {
                entity.CpuApproveStatusId = updateEntity.IndentStatusId;
                entity.CpuApproveBy = _userProviderService.UserClaim.UserId;
                entity.CpuApproveDate = CommonUtils.GetDefaultDateTime();
            }
            entity.Remarks = updateEntity.Remarks;
            entity.UpdatedDate = CommonUtils.GetDefaultDateTime();
            entity.UpdatedBy = _userProviderService.UserClaim.UserId;

            var result = await _indentRepository.SaveChangesAsync();

            if (result > 0)
            {
                return new ResponseModel
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Indent updated successfully.",
                    Data = entity
                };
            }
            return new ResponseModel { Success = false, StatusCode = StatusCodes.Status500InternalServerError, Message = "Indent does not updated." };
        }


        public async Task<ResponseModel> UpdateIndent(Indent updateEntity)
        {
            var entityResult = await GetIndentById(updateEntity.IndentId);

            if (!entityResult.Success) { return entityResult; }

            var entity = entityResult.Data as Indent;
            entity.IblBranchId = updateEntity.IblBranchId;
            entity.SchemeCodeId = updateEntity.SchemeCodeId;
            entity.C5CodeId = updateEntity.C5CodeId;
            entity.CardTypeId = updateEntity.CardTypeId;
            entity.BfilBranchTypeId = updateEntity.BfilBranchTypeId;
            entity.BfilBranchId = updateEntity.BfilBranchId;
            entity.BfilRegionId = updateEntity.BfilRegionId;
            entity.DispatchAddressId = updateEntity.DispatchAddressId;
            entity.IndentStatusId = updateEntity.IndentStatusId;
            entity.IndentNo = updateEntity.IndentNo;
            entity.IndentDate = updateEntity.IndentDate;
            entity.NoOfKit = updateEntity.NoOfKit;
            entity.ContactName = updateEntity.ContactName;
            entity.ContactNo = updateEntity.ContactNo;
            entity.RejectionReasonId = updateEntity.RejectionReasonId;
            entity.Remarks = updateEntity.Remarks;
            entity.IsActive = updateEntity.IsActive;
            entity.UpdatedDate = CommonUtils.GetDefaultDateTime();
            entity.UpdatedBy = _userProviderService.UserClaim.UserId;

            var result = await _indentRepository.SaveChangesAsync();

            if (result > 0)
            {
                return new ResponseModel
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Indent updated successfully.",
                    Data = entity
                };
            }
            return new ResponseModel { Success = false, StatusCode = StatusCodes.Status500InternalServerError, Message = "Indent does not updated." };
        }

        public async Task<string> GetNewIndentNo()
        {
            var indentCountSetting = await _settingService.GetSettingsByKey(SettingKeys.IndentCount);
            var indentDateSetting = await _settingService.GetSettingsByKey(SettingKeys.IndentDate);

            var indentCount = 0;
            var indentDate = DateTime.Now.Date;

            if (!string.IsNullOrEmpty(indentCountSetting.SettingValue))
            {
                indentCount = Convert.ToInt32(indentCountSetting.SettingValue);
            }

            if (!string.IsNullOrEmpty(indentDateSetting.SettingValue))
            {
                indentDate = Convert.ToDateTime(indentDateSetting.SettingValue);
            }

            if (indentDate.Date != DateTime.Now.Date || string.IsNullOrEmpty(indentDateSetting.SettingValue))
            {
                indentCount = 0;
                indentDate = DateTime.Now.Date;

                indentDateSetting.SettingValue = indentDate.ToShortDateString();
                await _settingService.UpdateSetting(indentDateSetting);
            }

            indentCount++;
            indentCountSetting.SettingValue = indentCount.ToString();
            await _settingService.UpdateSetting(indentCountSetting);

            return $"IN{indentDate:MMdd}{indentCount:d3}";
        }

        public Task<long> GetIndentCount(dynamic filters = null)
        {
            return _indentRepository.GetIndentCount(filters);
        }

        public Task<long> GetIndentKitTotal(dynamic filters = null)
        {
            return _indentRepository.GetIndentKitTotal(filters);
        }

        private async Task<ResponseModel> ValidateIndant(List<Indent> entities)
        {
            var indents = (from i in entities
                           group new { i } by new { i.IndentDate, i.BfilBranchId } into indent
                           select new
                           {
                               indent.Key.BfilBranchId,
                               indent.Key.IndentDate,
                               Count = indent.Count(),
                           });

            var branchIndentsPerDay = await _settingService.GetSettingsByKey(SettingKeys.BranchIndentsPerDay);

            foreach (var indent in indents)
            {
                var existIndents = await _indentRepository.Find(i => i.IndentDate.Date == indent.IndentDate.Date && i.BfilBranchId == indent.BfilBranchId);
                if (Convert.ToInt32(branchIndentsPerDay.SettingValue) < (indent.Count + existIndents.Count()))
                {
                    return new ResponseModel { Success = false, StatusCode = StatusCodes.Status400BadRequest, Message = $"Indent reached at maximum limit {branchIndentsPerDay.SettingValue} for the day for date {CommonUtils.GetFormatedDate(indent.IndentDate)}" };
                }
            }

            return new ResponseModel { Success = true, StatusCode = StatusCodes.Status200OK, Message = "Indents are validated" };
        }
    }
}
