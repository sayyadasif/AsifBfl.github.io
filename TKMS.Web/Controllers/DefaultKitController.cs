using Core.Repository.Models;
using Core.Utility.Utils;
using ExcelDataReader;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TKMS.Abstraction.ComplexModels;
using TKMS.Abstraction.Constants;
using TKMS.Abstraction.Enums;
using TKMS.Abstraction.Models;
using TKMS.Repository.Interfaces;
using TKMS.Service.Interfaces;
using TKMS.Web.Models;

namespace TKMS.Web.Controllers
{
    public class DefaultKitController : BaseController
    {
        private readonly IIndentService _indentService;
        private readonly IBranchService _branchService;
        private readonly IIblBranchService _iblBranchService;
        private readonly ISchemeCodeService _schemeCodeService;
        private readonly IC5CodeService _c5CodeService;
        private readonly IUserProviderService _userProviderService;
        private readonly IDispatchService _dispatchService;
        private readonly IFileService _fileService;

        public DefaultKitController(
            IIndentService indentService,
            IBranchService branchService,
            IIblBranchService iblBranchService,
            ISchemeCodeService schemeCodeService,
            IC5CodeService c5CodeService,
            IUserProviderService userProviderService,
            IDispatchService dispatchService,
            IFileService fileService
        )
        {
            _indentService = indentService;
            _branchService = branchService;
            _iblBranchService = iblBranchService;
            _schemeCodeService = schemeCodeService;
            _c5CodeService = c5CodeService;
            _userProviderService = userProviderService;
            _dispatchService = dispatchService;
            _fileService = fileService;
        }

        public async Task<IActionResult> Index()
        {
            if (!AuthorizeUser()) { return AccessDeniedView(); }

            return View();
        }

        [HttpPost]
        public async Task<ResponseModel> UploadDefaultKits(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return new ResponseModel { Success = false, Message = "Please select the file!", StatusCode = StatusCodes.Status400BadRequest };

            var path = await _fileService.CreateFile(file);

            var uploadKits = new List<KitUploadDetail>();
            var kitFileError = string.Empty;

            var schemeCodes = (await _schemeCodeService.GetAllSchemeCode()).Data as List<SchemeCode>;
            var c5Codes = await _c5CodeService.GetDropdwon();
            var branches = (await _branchService.GetAllBranch()).Data as List<Branch>;
            var iblBranches = await _iblBranchService.GetDropdwon();

            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            using (var stream = System.IO.File.Open(path, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    int index = 0;
                    int dateIndex = 0;
                    int iblBranchCodeIndex = 1;
                    int bfilBranchCodeIndex = 2;
                    int schemeCodeIndex = 3;
                    int c5CodeIndex = 4;
                    int cardTypeIndex = 5;
                    int iblBranchNameIndex = 6;
                    int acTypeIndex = 7;
                    int cifNoIndex = 8;
                    int acNoIndex = 9;

                    while (reader.Read()) //Each row of the file
                    {
                        if (index == 0)
                        {
                        }
                        else
                        {

                            var date = reader.GetValue(dateIndex)?.ToString().Trim() ?? "";
                            var iblBranchCode = reader.GetValue(iblBranchCodeIndex)?.ToString().Trim() ?? "";
                            var bfilBranchCode = reader.GetValue(bfilBranchCodeIndex)?.ToString().Trim() ?? "";
                            var schemeCode = reader.GetValue(schemeCodeIndex)?.ToString().Trim() ?? "";
                            var c5Code = reader.GetValue(c5CodeIndex)?.ToString().Trim() ?? "";
                            var cardType = reader.GetValue(cardTypeIndex)?.ToString().Trim() ?? "";
                            var iblBranchName = reader.GetValue(iblBranchNameIndex)?.ToString().Trim() ?? "";
                            var acType = reader.GetValue(acTypeIndex)?.ToString().Trim() ?? "";
                            var cifNo = reader.GetValue(cifNoIndex)?.ToString().Trim() ?? "";
                            var acNo = reader.GetValue(acNoIndex)?.ToString().Trim() ?? "";

                            if (string.IsNullOrEmpty(date) && string.IsNullOrEmpty(iblBranchCode) &&
                                string.IsNullOrEmpty(bfilBranchCode) && string.IsNullOrEmpty(schemeCode) &&
                                string.IsNullOrEmpty(c5Code) && string.IsNullOrEmpty(cardType) &&
                                string.IsNullOrEmpty(cifNo) && string.IsNullOrEmpty(acNo))
                            {
                                continue;
                            }

                            var rowErrorMessage = string.Empty;

                            if (string.IsNullOrEmpty(date))
                            {
                                rowErrorMessage += "<li>Date has no content</li>";
                            }

                            var _iblBranch = iblBranches.FirstOrDefault(s => s.IblBranchCode == iblBranchCode);

                            if (string.IsNullOrEmpty(iblBranchCode))
                            {
                                rowErrorMessage += "<li>Indent Branch Code has no content</li>";
                            }
                            else if (_iblBranch == null)
                            {
                                rowErrorMessage += "<li>Indent Branch Code did not match</li>";
                            }

                            var _bfilBranch = branches.FirstOrDefault(s => s.BranchCode == bfilBranchCode);
                            if (string.IsNullOrEmpty(bfilBranchCode))
                            {
                                rowErrorMessage += "<li>BFIL Branch Code has no content</li>";
                            }
                            else if (_bfilBranch == null)
                            {
                                rowErrorMessage += "<li>BFIL Branch Code did not match</li>";
                            }

                            var _schemeCode = schemeCodes.FirstOrDefault(sc => sc.SchemeCodeName == schemeCode);

                            if (string.IsNullOrEmpty(schemeCode))
                            {
                                rowErrorMessage += "<li>Scheme Code has no content</li>";
                            }
                            else if (_schemeCode == null)
                            {
                                rowErrorMessage += "<li>Scheme Code did not match</li>";
                            }

                            var _c5Code = c5Codes.FirstOrDefault(c => c.Text == c5Code);

                            if (string.IsNullOrEmpty(c5Code))
                            {
                                rowErrorMessage += "<li>C5 Code has no content</li>";
                            }
                            else if (_c5Code == null)
                            {
                                rowErrorMessage += "<li>C5 Code did not match</li>";
                            }
                            else if (_schemeCode != null && _c5Code != null && !_schemeCode.SchemeC5Codes.Select(s => s.C5CodeId).Contains(_c5Code.Id))
                            {
                                rowErrorMessage += "<li>C5 Code did not match with Scheme Code</li>";
                            }

                            if (string.IsNullOrEmpty(cardType))
                            {
                                rowErrorMessage += "<li>Card Type has no content</li>";
                            }
                            else if (_c5Code != null && _c5Code.CardTypeName != cardType)
                            {
                                rowErrorMessage += "<li>Card Type did not match with C5 Code</li>";
                            }

                            if (string.IsNullOrEmpty(cifNo))
                            {
                                rowErrorMessage += "<li>IF No has no content</li>";
                            }

                            if (string.IsNullOrEmpty(acNo))
                            {
                                rowErrorMessage += "<li>A/c stat has no content</li>";
                            }

                            if (!string.IsNullOrEmpty(rowErrorMessage))
                            {
                                rowErrorMessage = $"<ul>Row {index} has error{rowErrorMessage}</ul>";
                                kitFileError += rowErrorMessage;
                                index++;
                                continue;
                            }

                            uploadKits.Add(new KitUploadDetail
                            {
                                KitDate = date,
                                IblBranchCode = iblBranchCode,
                                BfilBranchCode = bfilBranchCode,
                                SchemeCode = schemeCode,
                                C5Code = c5Code,
                                CardType = cardType,
                                CifNo = cifNo,
                                AccountNumber = acNo,
                            });
                        }
                        index++;
                    }
                }
            }

            _fileService.DeleteFile(path);

            if (!string.IsNullOrEmpty(kitFileError))
            {
                return new ResponseModel { Success = false, StatusCode = StatusCodes.Status400BadRequest, Message = kitFileError };
            }

            var indents = new List<Indent>();
            var dispatches = new List<Dispatch>();
            var kits = new List<Kit>();

            var groupKits = (from k in uploadKits
                             group new { k } by new { k.KitDate, k.IblBranchCode, k.BfilBranchCode } into ks
                             select new GroupKitUploadDetail
                             {
                                 KitDate = ks.Key.KitDate,
                                 IblBranchCode = ks.Key.IblBranchCode,
                                 BfilBranchCode = ks.Key.BfilBranchCode,
                                 TotalKits = ks.Count(),
                             }).ToList();

            foreach (var groupKit in groupKits)
            {
                var _iblBranch = iblBranches.FirstOrDefault(s => s.IblBranchCode == groupKit.IblBranchCode);
                var _bfilBranch = branches.FirstOrDefault(s => s.BranchCode == groupKit.BfilBranchCode);

                var _uploadKit = uploadKits.FirstOrDefault(k => k.KitDate == groupKit.KitDate &&
                                                                k.IblBranchCode == groupKit.IblBranchCode &&
                                                                k.BfilBranchCode == groupKit.BfilBranchCode);

                var _schemeCode = schemeCodes.FirstOrDefault(sc => sc.SchemeCodeName == _uploadKit.SchemeCode);
                var _c5Code = c5Codes.FirstOrDefault(c => c.Text == _uploadKit.C5Code);
                var indentNo = await _indentService.GetNewIndentNo();
                groupKit.IndentNo = indentNo;

                indents.Add(new Indent
                {
                    SchemeCodeId = _schemeCode.SchemeCodeId,
                    C5CodeId = _c5Code.Id,
                    CardTypeId = _c5Code.CardTypeId,
                    BfilBranchTypeId = BranchTypes.Branch.GetHashCode(),
                    BfilRegionId = _bfilBranch.RegionId,
                    BfilBranchId = _bfilBranch.BranchId,
                    IblBranchId = _iblBranch != null ? _iblBranch.Id : 0,
                    DispatchAddressId = _bfilBranch.AddressId,
                    IndentStatusId = IndentStatuses.IndentBoxReceived.GetHashCode(),
                    IndentNo = indentNo,
                    IndentDate = CommonUtils.GetParseDate(_uploadKit.KitDate),
                    NoOfKit = groupKit.TotalKits,
                    ContactName = string.Empty,
                    ContactNo = string.Empty,
                    HoApproveStatusId = IndentStatuses.Approved.GetHashCode(),
                    HoApproveBy = _userProviderService.UserClaim.UserId,
                    HoApproveDate = CommonUtils.GetDefaultDateTime(),
                    CpuApproveStatusId = IndentStatuses.IndentForDispatch.GetHashCode(),
                    CpuApproveBy = _userProviderService.UserClaim.UserId,
                    CpuApproveDate = CommonUtils.GetDefaultDateTime(),
                    Remarks = indentNo,
                });

                dispatches.Add(new Dispatch
                {
                    AccountStart = string.Empty,
                    AccountEnd = string.Empty,
                    Vendor = string.Empty,
                    DispatchQty = groupKit.TotalKits,
                    DispatchDate = CommonUtils.GetParseDate(_uploadKit.KitDate),
                    SchemeType = string.Empty,
                    ReferenceNo = indentNo,
                    DispatchStatusId = DispatchStatuses.Received.GetHashCode(),
                    BranchDispatchDate = CommonUtils.GetParseDate(_uploadKit.KitDate),
                    BranchReceiveDate = CommonUtils.GetParseDate(_uploadKit.KitDate),
                    Remarks = indentNo,
                    DispatchWayBills = new List<DispatchWayBill>
                                        {
                                            new DispatchWayBill
                                            {
                                                WayBillNo = indentNo,
                                                DispatchDate = CommonUtils.GetParseDate(_uploadKit.KitDate),
                                                DeliveryDate = CommonUtils.GetParseDate(_uploadKit.KitDate),
                                                CourierStatusId = CourierStatuses.Norecords.GetHashCode(),
                                                ReceiveBy = string.Empty,
                                                ReceiveStatusId = DispatchStatuses.Received.GetHashCode(),
                                                ReportUploadBy = _userProviderService.UserClaim.UserId,
                                                ReportUploadDate = CommonUtils.GetParseDate(_uploadKit.KitDate),
                                                DispatchStatusId = DispatchStatuses.Received.GetHashCode(),
                                                CourierPartner = string.Empty,
                                                Remarks = indentNo,
                                            }
                                        },
                });

            }

            var indentResult = await _indentService.CreateIndents(indents, false);
            if (indentResult.Success)
            {
                var _indents = indentResult.Data as List<Indent>;
                foreach (var indent in _indents)
                {
                    var _uploadKit = groupKits.FirstOrDefault(k => k.IndentNo == indent.IndentNo);
                    var _dispatch = dispatches.FirstOrDefault(d => d.ReferenceNo == indent.IndentNo);

                    _dispatch.IndentId = indent.IndentId;

                    foreach (var uploadKit in uploadKits.Where(k => k.KitDate == _uploadKit.KitDate &&
                                                                k.IblBranchCode == _uploadKit.IblBranchCode &&
                                                                k.BfilBranchCode == _uploadKit.BfilBranchCode))
                    {
                        _dispatch.Kits.Add(new Kit
                        {
                            IndentId = indent.IndentId,
                            BranchId = indent.BfilBranchId,
                            CifNo = uploadKit.CifNo,
                            AccountNo = uploadKit.AccountNumber,
                            KitStatusId = KitStatuses.Dispatched.GetHashCode(),
                            Remarks = _uploadKit.IndentNo,
                        });
                    }
                }
                var disptachResults = await _dispatchService.CreateDispatches(dispatches);
                if (disptachResults.Success)
                {
                    return new ResponseModel { Success = true, StatusCode = StatusCodes.Status200OK, Message = "Kits upload successfully" };
                }
                await _indentService.DeleteIndents(_indents);
            }

            return new ResponseModel { Success = false, StatusCode = StatusCodes.Status400BadRequest, Message = "Kits did not upload" };
        }

        private bool AuthorizeUser()
        {
            return _userProviderService.SystemAdmin();
        }

        internal class KitUploadDetail
        {
            public string KitDate { get; set; }
            public string IblBranchCode { get; set; }
            public string BfilBranchCode { get; set; }
            public string SchemeCode { get; set; }
            public string C5Code { get; set; }
            public string CardType { get; set; }
            public string CifNo { get; set; }
            public string AccountNumber { get; set; }
        }

        internal class GroupKitUploadDetail
        {
            public string KitDate { get; set; }
            public string IblBranchCode { get; set; }
            public string BfilBranchCode { get; set; }
            public string IndentNo { get; set; }
            public int TotalKits { get; set; }
        }
    }
}
