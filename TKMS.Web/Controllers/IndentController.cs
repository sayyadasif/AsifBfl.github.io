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
    public class IndentController : BaseController
    {
        private readonly IIndentService _indentService;
        private readonly IBranchService _branchService;
        private readonly IIblBranchService _iblBranchService;
        private readonly ISchemeCodeService _schemeCodeService;
        private readonly IC5CodeService _c5CodeService;
        private readonly ICardTypeService _cardTypeService;
        private readonly IBranchTypeService _branchTypeService;
        private readonly IRegionService _regionService;
        private readonly IAddressService _addressService;
        private readonly IUserProviderService _userProviderService;
        private readonly IIndentStatusService _indentStatusService;
        private readonly IRejectionReasonService _rejectionReasonService;
        private readonly IDispatchService _dispatchService;
        private readonly IBranchDispatchService _branchDispatchService;
        private readonly ICourierStatusService _courierStatusService;
        private readonly IAddressRepository _addressRepository;
        private readonly IFileService _fileService;
        private readonly IKitService _kitService;
        private readonly ISettingService _settingService;

        public IndentController(
            IIndentService indentService,
            IBranchService branchService,
            IIblBranchService iblBranchService,
            ISchemeCodeService schemeCodeService,
            IC5CodeService c5CodeService,
            ICardTypeService cardTypeService,
            IBranchTypeService branchTypeService,
            IRegionService regionService,
            IAddressService addressService,
            IUserProviderService userProviderService,
            IIndentStatusService indentStatusService,
            IRejectionReasonService rejectionReasonService,
            IDispatchService dispatchService,
            ICourierStatusService courierStatusService,
            IBranchDispatchService branchDispatchService,
            IAddressRepository addressRepository,
            IFileService fileService,
            IKitService kitService,
            ISettingService settingService
        )
        {
            _indentService = indentService;
            _branchService = branchService;
            _iblBranchService = iblBranchService;
            _schemeCodeService = schemeCodeService;
            _c5CodeService = c5CodeService;
            _cardTypeService = cardTypeService;
            _branchTypeService = branchTypeService;
            _regionService = regionService;
            _addressService = addressService;
            _userProviderService = userProviderService;
            _indentStatusService = indentStatusService;
            _rejectionReasonService = rejectionReasonService;
            _dispatchService = dispatchService;
            _courierStatusService = courierStatusService;
            _branchDispatchService = branchDispatchService;
            _addressRepository = addressRepository;
            _fileService = fileService;
            _kitService = kitService;
            _settingService = settingService;
        }

        public async Task<IActionResult> Index()
        {
            if (!AuthorizeUser()) { return AccessDeniedView(); }

            await SetFilterComboBoxes();

            var model = new IndentDisplayModel
            {
                AllowIndentCreate = _userProviderService.AllowIndentCreate(),

                IndentRaised = _userProviderService.HOIndentMaker() || _userProviderService.ROIndentMaker() ||
                               _userProviderService.ROKitManagement(),

                IndentRejected = _userProviderService.HOIndentMaker() || _userProviderService.ROIndentMaker(),

                IndentRejectedDetail = (!_userProviderService.HOIndentMaker() && _userProviderService.HOApprover()) ||
                                       (!_userProviderService.ROIndentMaker() && _userProviderService.ROKitManagement()) ||
                                         _userProviderService.IBLCPU(),

                IndentDispatched = _userProviderService.HOIndentMaker() || _userProviderService.ROIndentMaker() ||
                                   _userProviderService.ROKitManagement() || _userProviderService.IBLCPU(),

                IndentApproved = _userProviderService.HOApprover(),

                IndentReceived = _userProviderService.HOApprover(),

                IndentCpuReceived = _userProviderService.IBLCPU(),

                IndentReceivedAtBranch = _userProviderService.BM() || _userProviderService.BCM(),

                IndentCancelled = _userProviderService.HOApprover() || _userProviderService.HOIndentMaker() ||
                                  _userProviderService.ROIndentMaker() || _userProviderService.ROKitManagement() ||
                                  _userProviderService.IBLCPU() || _userProviderService.BM() || _userProviderService.BCM(),

                IndentCpuDispatched = _userProviderService.IBLCPU(),
            };

            return View(model);
        }

        public async Task<IActionResult> IndentDetail(long id)
        {
            if (!AuthorizeUser()) { return AccessDeniedView(); }

            return View(await _indentService.GetIndentDetailById(id));
        }

        public async Task<IActionResult> DispatchDetail(long id)
        {
            if (!AuthorizeUser()) { return AccessDeniedView(); }
            return View(await GetDispatchedDetail(id));
        }

        public async Task<IActionResult> DispatchedDetail(long id)
        {
            if (!AuthorizeUser()) { return AccessDeniedView(); }
            return View(await GetDispatchedDetail(id));
        }

        public async Task<IActionResult> CancelledDetail(long id)
        {
            if (!AuthorizeUser()) { return AccessDeniedView(); }
            return View(await GetDispatchedDetail(id));
        }

        public async Task<IActionResult> ReceivedDetail(long id)
        {
            if (!AuthorizeUser()) { return AccessDeniedView(); }
            ViewBag.RejectedReasons = GetSelectList(await _rejectionReasonService.GetDropdwon(), "Select Rejection Reason");
            return View(await _indentService.GetIndentDetailById(id));
        }

        public async Task<IActionResult> ApprovedDetail(long id)
        {
            if (!AuthorizeUser()) { return AccessDeniedView(); }
            return View(await _indentService.GetIndentDetailById(id));
        }

        public async Task<IActionResult> RejectedDetail(long id)
        {
            if (!AuthorizeUser()) { return AccessDeniedView(); }
            return View(await _indentService.GetIndentDetailById(id));
        }

        public async Task<IActionResult> Dispatch(long id = 0)
        {
            if (!AuthorizeUser()) { return AccessDeniedView(); }

            var model = new DispatchRequestModel { IndentId = id };
            if (id > 0)
            {
                var indentDetails = await _indentService.GetIndentDetailById(id);
                model.IndentId = indentDetails.IndentId;
                model.IndentStatusId = indentDetails.IndentStatusId;
                model.IndentStatus = indentDetails.IndentStatus;
                model.IndentNo = indentDetails.IndentNo;
                model.NoOfKit = indentDetails.NoOfKit;
                model.DispatchedNoOfKit = indentDetails.NoOfKitDispatched;
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Dispatch(DispatchRequestModel model)
        {
            if (!ModelState.IsValid)
            {
                SetNotification("Please enter valid form detail", NotificationTypes.Error, "Indent Dispatch");
                return View(model);
            }

            if (model.BranchDispatch == null)
            {
                SetNotification("Please Upload BFIL Branch Level Dispatch Details file!", NotificationTypes.Error, "Indent Dispatch");
                return View(model);
            }

            if (model.KitDispatch == null)
            {
                SetNotification("Please Upload BFIL Kit Account Level Dispatch Details file!", NotificationTypes.Error, "Indent Dispatch");
                return View(model);
            }

            if (model.IndentId > 0 && model.NoOfKit < (model.DispatchedNoOfKit + model.DispatchNoOfKit))
            {
                SetNotification("Dispatch Kit Qty is more than Indented Kit Qty!", NotificationTypes.Error, "Indent Dispatch");
                return View(model);
            }


            var branchDispatchError = string.Empty;
            var kitDispatchError = string.Empty;
            var dispatches = new List<Dispatch>();
            var kits = new List<Kit>();

            #region Branch Dispatch

            var branchDispachPath = await _fileService.CreateFile(model.BranchDispatch);

            try
            {
                System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
                using (var stream = System.IO.File.Open(branchDispachPath, FileMode.Open, FileAccess.Read))
                {
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        int index = 0;
                        int indentNoIndex = 0;
                        int indentDateIndex = 1;
                        int branchCodeIndex = 2;
                        int branchNameIndex = 3;
                        int schemeCodeIndex = 4;
                        int cardTypeIndex = 5;
                        int vendorIndex = 6;
                        int qtyIndex = 7;
                        int acStartIndex = 8;
                        int acEndIndex = 9;
                        int bfilBranchCodeIndex = 10;
                        int schemeTypeIndex = 11;
                        int dispatchDateIndex = 12;
                        int refNoIndex = 13;
                        int wayBill1Index = 14;
                        int wayBill2Index = 15;
                        int wayBill3Index = 16;
                        int wayBill4Index = 17;
                        int wayBill5Index = 18;

                        while (reader.Read()) //Each row of the file
                        {
                            if (index == 0)
                            {
                            }
                            else
                            {
                                var indentNo = reader.GetValue(indentNoIndex)?.ToString().Trim() ?? "";
                                var indentDate = reader.GetValue(indentDateIndex)?.ToString().Trim() ?? "";
                                var branchCode = reader.GetValue(branchCodeIndex)?.ToString().Trim() ?? "";
                                var branchName = reader.GetValue(branchNameIndex)?.ToString().Trim() ?? "";
                                var schemeCode = reader.GetValue(schemeCodeIndex)?.ToString().Trim() ?? "";
                                var cardType = reader.GetValue(cardTypeIndex)?.ToString().Trim() ?? "";
                                var vendor = reader.GetValue(vendorIndex)?.ToString().Trim() ?? "";
                                var qty = reader.GetValue(qtyIndex)?.ToString().Trim() ?? "";
                                var acStart = reader.GetValue(acStartIndex)?.ToString().Trim() ?? "";
                                var acEnd = reader.GetValue(acEndIndex)?.ToString().Trim() ?? "";
                                var bfilBranchCode = reader.GetValue(bfilBranchCodeIndex)?.ToString().Trim() ?? "";
                                var schemeType = reader.GetValue(schemeTypeIndex)?.ToString().Trim() ?? "";
                                var dispatchDate = reader.GetValue(dispatchDateIndex)?.ToString().Trim() ?? "";
                                var refNo = reader.GetValue(refNoIndex)?.ToString().Trim() ?? "";
                                var wayBill1 = reader.GetValue(wayBill1Index)?.ToString().Trim() ?? "";
                                var wayBill2 = reader.GetValue(wayBill2Index)?.ToString().Trim() ?? "";
                                var wayBill3 = reader.GetValue(wayBill3Index)?.ToString().Trim() ?? "";
                                var wayBill4 = reader.GetValue(wayBill4Index)?.ToString().Trim() ?? "";
                                var wayBill5 = reader.GetValue(wayBill5Index)?.ToString().Trim() ?? "";

                                if (string.IsNullOrEmpty(indentNo) && string.IsNullOrEmpty(indentDate) &&
                                    string.IsNullOrEmpty(branchCode) && string.IsNullOrEmpty(branchName) &&
                                    string.IsNullOrEmpty(schemeCode) && string.IsNullOrEmpty(cardType) &&
                                    string.IsNullOrEmpty(vendor) && string.IsNullOrEmpty(qty) &&
                                    string.IsNullOrEmpty(acStart) && string.IsNullOrEmpty(acEnd) &&
                                    string.IsNullOrEmpty(bfilBranchCode) && string.IsNullOrEmpty(schemeType) &&
                                    string.IsNullOrEmpty(dispatchDate) && string.IsNullOrEmpty(refNo) &&
                                    string.IsNullOrEmpty(wayBill1) && string.IsNullOrEmpty(wayBill2) &&
                                    string.IsNullOrEmpty(wayBill3) && string.IsNullOrEmpty(wayBill4) &&
                                    string.IsNullOrEmpty(wayBill5))
                                {
                                    continue;
                                }

                                var dispatchQty = 0;
                                var rowErrorMessage = string.Empty;

                                if (string.IsNullOrEmpty(indentNo))
                                {
                                    rowErrorMessage += "<li>Indent Number has no content</li>";
                                }

                                if (string.IsNullOrEmpty(indentDate))
                                {
                                    rowErrorMessage += "<li>Indent Date has no content</li>";
                                }

                                if (string.IsNullOrEmpty(branchCode))
                                {
                                    rowErrorMessage += "<li>Branch Code has no content</li>";
                                }

                                if (string.IsNullOrEmpty(schemeCode))
                                {
                                    rowErrorMessage += "<li>Scheme Code has no content</li>";
                                }

                                if (string.IsNullOrEmpty(cardType))
                                {
                                    rowErrorMessage += "<li>Card Type has no content</li>";
                                }

                                if (string.IsNullOrEmpty(qty))
                                {
                                    rowErrorMessage += "<li>Dispatch Qty has no content</li>";
                                }
                                else if (!int.TryParse(qty, out dispatchQty))
                                {
                                    rowErrorMessage += "<li>Dispatch Qty is not valid</li>";
                                }
                                if (dispatchQty == 0)
                                {
                                    rowErrorMessage += "<li>Dispatch Qty is 0</li>";
                                }

                                if (string.IsNullOrEmpty(acStart))
                                {
                                    rowErrorMessage += "<li>Ac Start has no content</li>";
                                }

                                if (string.IsNullOrEmpty(acEnd))
                                {
                                    rowErrorMessage += "<li>Ac End has no content</li>";
                                }

                                if (string.IsNullOrEmpty(bfilBranchCode))
                                {
                                    rowErrorMessage += "<li>Bfil Branch Code has no content</li>";
                                }

                                if (string.IsNullOrEmpty(schemeType))
                                {
                                    rowErrorMessage += "<li>Scheme Type has no content</li>";
                                }

                                if (string.IsNullOrEmpty(dispatchDate))
                                {
                                    rowErrorMessage += "<li>Dispatch Date has no content</li>";
                                }

                                if (string.IsNullOrEmpty(refNo))
                                {
                                    rowErrorMessage += "<li>Reference Number has no content</li>";
                                }

                                if (!string.IsNullOrEmpty(rowErrorMessage))
                                {
                                    rowErrorMessage = $"<ul>Row {index} has error{rowErrorMessage}</ul>";
                                    branchDispatchError += rowErrorMessage;
                                    index++;
                                    continue;
                                }

                                var wayBills = new List<DispatchWayBill>();

                                foreach (var wayBill in new List<string> { wayBill1, wayBill2, wayBill3, wayBill4, wayBill5 })
                                {
                                    if (!string.IsNullOrEmpty(wayBill))
                                    {
                                        wayBills.Add(new DispatchWayBill
                                        {
                                            WayBillNo = wayBill,
                                            DispatchDate = CommonUtils.GetParseDate(dispatchDate),
                                            CourierStatusId = CourierStatuses.NewReordAdded.GetHashCode(),
                                            DispatchStatusId = DispatchStatuses.Dispatched.GetHashCode(),
                                        });
                                    }
                                }

                                dispatches.Add(new Dispatch
                                {
                                    IndentNo = indentNo,
                                    IndentDate = CommonUtils.GetParseDate(indentDate),
                                    IblBranchCode = branchCode,
                                    SchemeCode = schemeCode,
                                    CardType = cardType,
                                    BfilBranchCode = bfilBranchCode,
                                    Vendor = vendor,
                                    DispatchQty = dispatchQty,
                                    AccountStart = acStart,
                                    AccountEnd = acEnd,
                                    SchemeType = schemeType,
                                    DispatchDate = CommonUtils.GetParseDate(dispatchDate),
                                    ReferenceNo = refNo,
                                    DispatchStatusId = DispatchStatuses.Dispatched.GetHashCode(),
                                    DispatchWayBills = wayBills,
                                });
                            }
                            index++;
                        }
                    }
                }
            }
            catch (Exception)
            {
                branchDispatchError = "Please upload valid content for BFIL Branch Level Dispatch!";
            }

            _fileService.DeleteFile(branchDispachPath);

            var sum = dispatches.Sum(d => d.DispatchQty);
            if (model.IndentId > 0)
            {
                sum = dispatches.Where(d => d.IndentNo == model.IndentNo).Sum(d => d.DispatchQty);
            }

            if (sum != model.DispatchNoOfKit)
            {
                branchDispatchError += "BFIL Branch Level Dispatch Kit Qty is not match with excel Qty.";
            }

            var indents = await _indentService.GetIndentDetailByIndentNos(dispatches.Select(d => d.IndentNo).ToList());

            int dispatchIndex = 0;
            foreach (var dispatch in dispatches)
            {
                dispatchIndex++;

                var rowErrorMessage = string.Empty;
                var indentDetail = indents.FirstOrDefault(i => i.IndentNo == dispatch.IndentNo);

                if (indentDetail == null)
                {
                    rowErrorMessage += $"<li>Indent no {dispatch.IndentNo} did not match</li>";
                    continue;
                }

                dispatch.IndentId = indentDetail.IndentId;

                if (dispatch.IndentDate.Date != indentDetail.IndentDate.Date)
                {
                    rowErrorMessage += "<li>Indent Date did not matched</li>";
                }

                if (dispatch.IblBranchCode != indentDetail.IblBranchCode)
                {
                    rowErrorMessage += "<li>IBL Branch Code did not match</li>";
                }

                if (dispatch.SchemeCode != indentDetail.SchemeCode)
                {
                    rowErrorMessage += "<li>Scheme Code did not match</li>";
                }

                if (dispatch.CardType != indentDetail.CardType)
                {
                    rowErrorMessage += "<li>Card Type did not match</li>";
                }

                if (dispatch.BfilBranchCode != indentDetail.BfilBranchCode)
                {
                    rowErrorMessage += "<li>Bfil Branch Code did not match</li>";
                }

                if (!string.IsNullOrEmpty(rowErrorMessage))
                {
                    rowErrorMessage = $"<ul>Row {dispatchIndex} has error{rowErrorMessage}</ul>";
                    branchDispatchError += rowErrorMessage;
                    continue;
                }

            }

            if (!string.IsNullOrEmpty(branchDispatchError))
            {
                model.BranchDispatchError = branchDispatchError;
                SetNotification("BFIL Branch Level Dispatch Details Error", NotificationTypes.Error, "Indent Dispatch");
                return View(model);
            }
            else
            {
                model.BranchDispatchSuccess = "Upload Successful";
            }

            #endregion

            #region Kit Dispatch

            var kitDispachPath = await _fileService.CreateFile(model.KitDispatch);

            try
            {
                System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
                using (var stream = System.IO.File.Open(kitDispachPath, FileMode.Open, FileAccess.Read))
                {
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        int index = 0;
                        int indentNoIndex = 0;
                        int indentDateIndex = 1;
                        int branchCodeIndex = 2;
                        int branchNameIndex = 3;
                        int bfilBranchCodeIndex = 4;
                        int cifNoIndex = 5;
                        int acStatIndex = 6;
                        int refNoIndex = 7;

                        while (reader.Read()) //Each row of the file
                        {
                            if (index == 0)
                            {
                            }
                            else
                            {
                                var indentNo = reader.GetValue(indentNoIndex)?.ToString().Trim() ?? "";
                                var indentDate = reader.GetValue(indentDateIndex)?.ToString().Trim() ?? "";
                                var branchCode = reader.GetValue(branchCodeIndex)?.ToString().Trim() ?? "";
                                var branchName = reader.GetValue(branchNameIndex)?.ToString().Trim() ?? "";
                                var bfilBranchCode = reader.GetValue(bfilBranchCodeIndex)?.ToString().Trim() ?? "";
                                var cifNo = reader.GetValue(cifNoIndex)?.ToString().Trim() ?? "";
                                var acStat = reader.GetValue(acStatIndex)?.ToString().Trim() ?? "";
                                var refNo = reader.GetValue(refNoIndex)?.ToString().Trim() ?? "";

                                if (string.IsNullOrEmpty(indentNo) && string.IsNullOrEmpty(indentDate) &&
                                    string.IsNullOrEmpty(branchCode) && string.IsNullOrEmpty(branchName) &&
                                    string.IsNullOrEmpty(bfilBranchCode) && string.IsNullOrEmpty(cifNo) &&
                                    string.IsNullOrEmpty(acStat) && string.IsNullOrEmpty(refNo))
                                {
                                    continue;
                                }

                                var indentDetail = indents.FirstOrDefault(i => i.IndentNo == indentNo);

                                if (indentDetail == null || indentNo != indentDetail.IndentNo)
                                {
                                    index++;
                                    continue;
                                }

                                var rowErrorMessage = string.Empty;

                                if (string.IsNullOrEmpty(indentDate))
                                {
                                    rowErrorMessage += "<li>Indent Date has no content</li>";
                                }
                                else if (CommonUtils.GetParseDate(indentDate).Date != indentDetail.IndentDate.Date)
                                {
                                    rowErrorMessage += "<li>Indent Date did not match</li>";
                                }

                                if (string.IsNullOrEmpty(branchCode))
                                {
                                    rowErrorMessage += "<li>Branch Code has no content</li>";
                                }
                                else if (branchCode != indentDetail.IblBranchCode)
                                {
                                    rowErrorMessage += "<li>Branch Code did not match</li>";
                                }

                                if (string.IsNullOrEmpty(bfilBranchCode))
                                {
                                    rowErrorMessage += "<li>Bfil Branch Code has no content</li>";
                                }
                                else if (bfilBranchCode != indentDetail.BfilBranchCode)
                                {
                                    rowErrorMessage += "<li>Bfil Branch Code did not match</li>";
                                }

                                if (string.IsNullOrEmpty(cifNo))
                                {
                                    rowErrorMessage += "<li>CIF no has no content </li>";
                                }

                                if (string.IsNullOrEmpty(acStat))
                                {
                                    rowErrorMessage += "<li>Ac Stat has no content</li>";
                                }

                                if (string.IsNullOrEmpty(refNo))
                                {
                                    rowErrorMessage += "<li>Reference Number has no content</li>";
                                }

                                if (!string.IsNullOrEmpty(rowErrorMessage))
                                {
                                    rowErrorMessage = $"<ul>Row {index} has error{rowErrorMessage}</ul>";
                                    kitDispatchError += rowErrorMessage;
                                    index++;
                                    continue;
                                }

                                kits.Add(new Kit
                                {
                                    IndentId = indentDetail.IndentId,
                                    BranchId = indentDetail.BfilBranchId,
                                    CifNo = cifNo,
                                    AccountNo = acStat,
                                    ReferenceNo = refNo,
                                    KitStatusId = KitStatuses.Dispatched.GetHashCode()
                                }); ;
                            }
                            index++;
                        }
                    }
                }
            }
            catch (Exception)
            {
                kitDispatchError = "Please upload valid content for BFIL Kit Account Level Dispatch!";
            }

            _fileService.DeleteFile(kitDispachPath);

            if (kits.Count() != model.DispatchNoOfKit)
            {
                kitDispatchError += "BFIL Kit Account Level Dispatch Qty is not match with excel Qty.";
            }

            var accNos = kits.Select(k => k.AccountNo).ToList();
            var existAccounts = await _kitService.ValidateKitAccountNo(accNos);
            if (existAccounts.Any())
            {
                kitDispatchError += $"BFIL Kit Accounts exists. {string.Join(", ", existAccounts)}";
            }

            var duplicateAccNos = accNos.GroupBy(x => x).Where(group => group.Count() > 1).Select(group => group.Key);
            if (duplicateAccNos.Any())
            {
                kitDispatchError += $"BFIL Kit Accounts duplicate. {string.Join(", ", duplicateAccNos)}";
            }

            dispatches.ForEach(d => kits.Where(k => k.IndentId == d.IndentId && k.ReferenceNo == d.ReferenceNo).ToList().ForEach(k => d.Kits.Add(k)));

            dispatches.ForEach(d =>
            {
                if (d.DispatchQty != d.Kits.Count)
                {
                    kitDispatchError += $"BFIL Indent No {d.IndentNo} Dispatch Qty {d.DispatchQty} Reference No {d.ReferenceNo} is not match with Kit Qty {d.Kits.Count}.";
                }
            });

            if (!string.IsNullOrEmpty(kitDispatchError))
            {
                model.KitDispatchError = kitDispatchError;
                SetNotification("BFIL Kit Account Level Dispatch Error", NotificationTypes.Error, "Indent Dispatch");
                return View(model);
            }
            else
            {
                model.KitDispatchSuccess = "Upload Successful";
            }

            #endregion

            var result = await _dispatchService.CreateDispatches(dispatches);
            if (result.Success)
            {
                result = await _indentService.UpdateIndentStatus(new Indent
                {
                    IndentId = model.IndentId,
                    IndentStatusId = model.NoOfKit == model.DispatchedNoOfKit + model.DispatchNoOfKit ?
                                                        IndentStatuses.Dispatched.GetHashCode() :
                                                        IndentStatuses.PartialDispatched.GetHashCode(),
                });

                SetNotification("Indent Dispatch saved!", NotificationTypes.Success, "Indent Dispatch");
                return Redirect(Url.Action("Index") + "#indentForDispatched");
            }

            SetNotification("Indent Dispatch not saved!", NotificationTypes.Error, "Indent Dispatch");
            return View(model);
        }

        public async Task<IActionResult> BranchDispatch(long id)
        {
            if (!AuthorizeUser()) { return AccessDeniedView(); }
            var dispatch = await SetDispatchDetail(id);
            return View(new BranchDispatch { DispatchId = id, DispatchMode = "Staff", IndentId = dispatch.IndentId });
        }

        [HttpPost]
        public async Task<IActionResult> BranchDispatch(BranchDispatch model)
        {
            if (model.DispatchMode == "Staff")
            {
                model.CourierName = null;
                model.WayBillNo = null;
            }
            else
            {
                model.StaffId = null;
                model.StaffName = null;
                model.StaffContactNo = null;
            }

            var result = await _branchDispatchService.CreateBranchDispatch(model);
            if (result.Success)
            {
                await UpdateDispatchStatus(new DispatchUpdateModel
                {
                    IndentId = model.IndentId,
                    DispatchId = model.DispatchId,
                    IsDispatchToBranch = true
                });

                SetNotification("Indent Dispatch to Branch saved!", NotificationTypes.Success, "Indent Dispatch to Branch");
                return Redirect(Url.Action("DispatchedDetail", new { id = model.IndentId }));
            }

            await SetDispatchDetail(model.DispatchId);
            return View(model);
        }

        public async Task<IActionResult> Manage(long id = 0)
        {
            if (!AuthorizeUser()) { return AccessDeniedView(); }

            var model = new Indent();
            if (id > 0)
            {
                var indentResult = await _indentService.GetIndentById(id);
                if (indentResult.Success)
                {
                    model = indentResult.Data;
                    var addressResult = await _addressService.GetAddressById(model.DispatchAddressId);
                    if (addressResult.Success)
                    {
                        model.DispatchAddress = addressResult.Data;
                    }
                    if (model.RejectionReasonId.HasValue && model.RejectionReasonId > 0)
                    {
                        var reason = await _rejectionReasonService.GetDropdwon(model.RejectionReasonId);
                        model.RejectionReason = reason.FirstOrDefault().Text;
                    }
                    model.IfscCode = (await _branchService.GetBranchById(model.BfilBranchId)).Data?.IfscCode;
                }
                await SetComboBoxes(model);
            }
            else
            {
                model.IndentNo = await _indentService.GetNewIndentNo();
                model.IndentDate = CommonUtils.GetDefaultDateTime();
                model.IndentStatusId = IndentStatuses.PendingApproval.GetHashCode();
                await SetComboBoxes(model);
                if (model.BfilRegionId > 0)
                {
                    var region = await _regionService.GetRegionById(model.BfilRegionId);
                    model.DispatchAddressId = region.Data?.Address?.AddressId;
                    model.DispatchAddress = region.Data?.Address;
                }
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Manage(Indent model)
        {
            if (!ModelState.IsValid)
            {
                await SetComboBoxes(model);
                SetNotification("Please enter valid form detail", NotificationTypes.Error, "Indent");
                return View(model);
            }

            if (model.IsAddressUpdated)
            {
                var addressResult = await _addressService.CreateAddress(model.DispatchAddress);
                if (addressResult.Success)
                {
                    model.DispatchAddressId = addressResult.Data?.AddressId;
                }
            }

            model.IndentStatusId = IndentStatuses.PendingApproval.GetHashCode();

            var indentResult = new ResponseModel();
            if (model.IndentId == 0)
            {
                indentResult = await _indentService.CreateIndent(model);
            }
            else
            {
                indentResult = await _indentService.UpdateIndent(model);
            }

            if (indentResult.Success)
            {
                SetNotification($"Indent saved successfully!", NotificationTypes.Success, "Indent");
                return RedirectToAction("Index", "Indent");
            }

            await SetComboBoxes(model);
            SetNotification(indentResult.Message, NotificationTypes.Error, "Indent");
            return View(model);
        }

        public async Task<IActionResult> GetIndents(
            string indentStatusIds = null,
            string indentNo = null,
            string indentStartDate = null,
            string indentEndDate = null,
            long? branchId = null,
            long? schemeCodeId = null,
            long? c5CodeId = null,
            long? cardTypeId = null,
            long? bfilBranchTypeId = null,
            long? regionId = null,
            long? bfilBranchId = null,
            bool? dispatchQty = null,
            bool? showStock = null
        )
        {
            try
            {
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;
                int currentPage = skip / Convert.ToInt32(length) + 1;

                dynamic filters = new ExpandoObject();
                filters.indentStatusIds = indentStatusIds;
                filters.indentNo = indentNo;
                filters.indentStartDate = indentStartDate;
                filters.indentEndDate = indentEndDate;
                filters.branchId = branchId;
                filters.schemeCodeId = schemeCodeId;
                filters.c5CodeId = c5CodeId;
                filters.cardTypeId = cardTypeId;
                filters.bfilBranchTypeId = bfilBranchTypeId;
                filters.regionId = regionId.HasValue ? regionId : _userProviderService.GetRegionId();
                filters.bfilBranchId = bfilBranchId.HasValue ? bfilBranchId : _userProviderService.GetBranchId();
                filters.dispatchQty = dispatchQty;
                filters.showStock = showStock;

                var indentResult = await _indentService.GetIndentPaged(
                    new Pagination
                    {
                        PageNumber = currentPage,
                        PageSize = pageSize,
                        SortOrderBy = sortColumnDirection,
                        SortOrderColumn = sortColumn,
                        Filters = filters
                    });
                var indentPaged = indentResult.Data as PagedList;
                recordsTotal = indentPaged.TotalCount;
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = indentPaged.Data };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<IActionResult> GetBranches(long regionId)
        {
            return Ok(await _branchService.GetDropdwon(regionId: regionId));
        }

        public async Task<IActionResult> GetAddressDetails(long regionId)
        {
            var region = await _regionService.GetRegionById(regionId);
            return Ok(region.Data?.Address);
        }

        public async Task<IActionResult> GetBranchDetails(long branchId)
        {
            var branchResult = await _branchService.GetBranchById(branchId);
            if (!branchResult.Success)
            {
                return Ok(branchResult);
            }
            var branch = branchResult.Data as Branch;
            var iblBranches = (await _iblBranchService.GetDropdwon(id: branch.IblBranchId))
                          .Select(b => new DropdownModel { Id = b.Id, Text = $"{b.IblBranchCode} - {b.Text} ", IsActive = b.IsActive }).ToList();

            var schemeCodes = await _schemeCodeService.GetDropdwon(id: branch.SchemeCodeId);
            var _c5Codes = await _c5CodeService.GetDropdwon(id: branch.C5CodeId);
            var c5Codes = _c5Codes.Select(b => new DropdownModel { Id = b.Id, Text = $"{b.Text} ", IsActive = b.IsActive }).ToList();
            var cardTypes = new List<DropdownModel>();
            if (c5Codes.Any())
            {
                cardTypes.Add(new DropdownModel { Id = _c5Codes.FirstOrDefault().CardTypeId, Text = _c5Codes.FirstOrDefault().CardTypeName });
            }
            if (!iblBranches.Any())
            {
                iblBranches.Add(new DropdownModel { Id = 0, Text = "Select IBL Branch" });
            }
            if (!schemeCodes.Any())
            {
                schemeCodes.Add(new DropdownModel { Id = 0, Text = "Select Scheme Code" });
            }
            if (!c5Codes.Any())
            {
                c5Codes.Add(new DropdownModel { Id = 0, Text = "Select C5 Code" });
            }
            if (!cardTypes.Any())
            {
                cardTypes.Add(new DropdownModel { Id = 0, Text = "Select Card Type" });
            }
            branchResult.Data = new { branch, iblBranches, schemeCodes, c5Codes, cardTypes };
            return Ok(branchResult);
        }

        public async Task<IActionResult> UpdateIndentStatus(IndentUpdateModel indentUpdateModel)
        {
            var indent = new Indent
            {
                IndentId = indentUpdateModel.IndentId,
                RejectionReasonId = indentUpdateModel.RejectionReasonId,
                Remarks = indentUpdateModel.Remarks,
            };

            if (indentUpdateModel.IsApproved)
            {
                indent.IndentStatusId = indentUpdateModel.IndentStatusId == IndentStatuses.Approved.GetHashCode() ?
                                                                 IndentStatuses.IndentForDispatch.GetHashCode() :
                                                                 IndentStatuses.Approved.GetHashCode();


            }
            else if (indentUpdateModel.IsCancelled)
            {
                indent.IndentStatusId = IndentStatuses.Cancelled.GetHashCode();
            }
            else if (indentUpdateModel.IsRejected)
            {
                indent.IndentStatusId = IndentStatuses.Rejected.GetHashCode();
            }

            var result = await _indentService.UpdateIndentStatus(indent);
            return Ok(result);
        }

        public async Task<IActionResult> UpdateDispatchStatus(DispatchUpdateModel dispatchUpdateModel)
        {
            var dispatch = new Dispatch
            {
                IndentId = dispatchUpdateModel.IndentId,
                DispatchId = dispatchUpdateModel.DispatchId
            };

            if (dispatchUpdateModel.IsReceivedAtRo)
            {
                dispatch.DispatchStatusId = DispatchStatuses.ReceivedAtRo.GetHashCode();
            }
            else if (dispatchUpdateModel.IsDispatchToBranch)
            {
                dispatch.DispatchStatusId = DispatchStatuses.DispatchToBranch.GetHashCode();
            }
            else if (dispatchUpdateModel.IsReceivedAtBranch)
            {
                dispatch.DispatchStatusId = DispatchStatuses.Received.GetHashCode();
            }

            var result = await _dispatchService.UpdateDispatchStatus(dispatch);

            if (result.Success)
            {
                if (dispatchUpdateModel.IsReceivedAtBranch)
                {
                    var isAllDispatch = await _dispatchService.IsAllDispatchReceived(dispatch.IndentId);
                    if (isAllDispatch)
                    {
                        await _indentService.UpdateIndentStatus(new Indent
                        {
                            IndentId = dispatch.IndentId,
                            RejectionReasonId = null,
                            IndentStatusId = IndentStatuses.IndentBoxReceived.GetHashCode()
                        });
                    }
                    result.Data = new { isAllDispatch };
                }
            }

            return Ok(result);
        }

        [HttpPost]
        public async Task<ResponseModel> UploadIndents(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return new ResponseModel { Success = false, Message = "Please select the file!", StatusCode = StatusCodes.Status400BadRequest };

            var path = await _fileService.CreateFile(file);

            var indents = new List<Indent>();
            var schemeCodes = (await _schemeCodeService.GetAllSchemeCode()).Data as List<SchemeCode>;
            var c5Codes = await _c5CodeService.GetDropdwon();
            var branches = await _branchService.GetDropdwon();
            var iblBranches = await _iblBranchService.GetDropdwon();
            var branchTypes = await _branchTypeService.GetDropdwon();
            var indentFileError = string.Empty;

            try
            {
                System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
                using (var stream = System.IO.File.Open(path, FileMode.Open, FileAccess.Read))
                {
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        int index = 0;
                        int dateIndex = 0;
                        int branchIndex = 1;
                        int schemeCodeIndex = 2;
                        int c5CodeIndex = 3;
                        int cardTypeIndex = 4;
                        int noOfKitIndex = 5;
                        int bfilBranchTypeIndex = 6;
                        int addressDetailIndex = 7;
                        int bfilBranchIndex = 8;
                        int pincodeIndex = 9;
                        int contactNameIndex = 10;
                        int contactNoIndex = 11;

                        while (reader.Read()) //Each row of the file
                        {
                            if (index == 0)
                            {
                            }
                            else
                            {
                                var indentDate = reader.GetValue(dateIndex)?.ToString().Trim() ?? "";
                                var branch = reader.GetValue(branchIndex)?.ToString().Trim() ?? "";
                                var schemeCode = reader.GetValue(schemeCodeIndex)?.ToString().Trim() ?? "";
                                var c5Code = reader.GetValue(c5CodeIndex)?.ToString().Trim() ?? "";
                                var cardType = reader.GetValue(cardTypeIndex)?.ToString().Trim() ?? "";
                                var numberOfKit = reader.GetValue(noOfKitIndex)?.ToString().Trim() ?? "";
                                var bfilBranchType = reader.GetValue(bfilBranchTypeIndex)?.ToString().Trim() ?? "";
                                var addressDetail = reader.GetValue(addressDetailIndex)?.ToString().Trim() ?? "";
                                var bfilBranch = reader.GetValue(bfilBranchIndex)?.ToString().Trim() ?? "";
                                var pincode = reader.GetValue(pincodeIndex)?.ToString().Trim() ?? "";
                                var contactName = reader.GetValue(contactNameIndex)?.ToString().Trim() ?? "";
                                var contactNo = reader.GetValue(contactNoIndex)?.ToString().Trim() ?? "";

                                if (string.IsNullOrEmpty(indentDate) && string.IsNullOrEmpty(branch) &&
                                    string.IsNullOrEmpty(schemeCode) && string.IsNullOrEmpty(c5Code) &&
                                    string.IsNullOrEmpty(cardType) && string.IsNullOrEmpty(numberOfKit) &&
                                    string.IsNullOrEmpty(bfilBranchType) && string.IsNullOrEmpty(addressDetail) &&
                                    string.IsNullOrEmpty(bfilBranch) && string.IsNullOrEmpty(pincode) &&
                                    string.IsNullOrEmpty(contactName) && string.IsNullOrEmpty(contactNo))
                                {
                                    continue;
                                }

                                var rowErrorMessage = string.Empty;

                                if (string.IsNullOrEmpty(indentDate))
                                {
                                    rowErrorMessage += "<li>Date has no content</li>";
                                }
                                else if (CommonUtils.GetParseDate(indentDate) < CommonUtils.GetDefaultDateTime().Date)
                                {
                                    rowErrorMessage += "<li>Indent Date does not allowed for past date</li>";
                                }

                                var _iblBranch = iblBranches.FirstOrDefault(s => s.IblBranchCode == branch);

                                if (string.IsNullOrEmpty(branch))
                                {
                                    rowErrorMessage += "<li>Indent Branch Code has no content</li>";
                                }
                                else if (_iblBranch == null)
                                {
                                    rowErrorMessage += "<li>Indent Branch Code did not match</li>";
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

                                var noOfKit = 0;
                                int.TryParse(numberOfKit, out noOfKit);

                                if (string.IsNullOrEmpty(numberOfKit))
                                {
                                    rowErrorMessage += "<li>Number of Kit has no content</li>";
                                }
                                else if (noOfKit == 0)
                                {
                                    rowErrorMessage += "<li>Number of Kit should be more than 0</li>";
                                }

                                var _bfilBranchType = branchTypes.FirstOrDefault(s => s.Text == bfilBranchType);

                                if (string.IsNullOrEmpty(bfilBranchType))
                                {
                                    rowErrorMessage += "<li>BFIL Address Type has no content</li>";
                                }
                                else if (_bfilBranchType == null)
                                {
                                    rowErrorMessage += "<li>BFIL Address Type did not match</li>";
                                }

                                var _bfilBranch = branches.FirstOrDefault(s => s.BranchCode == bfilBranch);
                                if (string.IsNullOrEmpty(bfilBranch))
                                {
                                    rowErrorMessage += "<li>BFIL Branch Code has no content</li>";
                                }
                                else if (_bfilBranch == null)
                                {
                                    rowErrorMessage += "<li>BFIL Branch Code did not match</li>";
                                }

                                if (string.IsNullOrEmpty(addressDetail))
                                {
                                    rowErrorMessage += "<li>Address has no content</li>";
                                }

                                if (string.IsNullOrEmpty(pincode))
                                {
                                    rowErrorMessage += "<li>Pincode has no content</li>";
                                }

                                if (string.IsNullOrEmpty(contactName))
                                {
                                    rowErrorMessage += "<li>Contact Name has no content</li>";
                                }

                                if (string.IsNullOrEmpty(contactNo))
                                {
                                    rowErrorMessage += "<li>Contact Number has no content</li>";
                                }

                                var _regionId = _userProviderService.GetRegionId();
                                if (_regionId != null && _bfilBranch != null && _bfilBranch.RegionId != _regionId.Value)
                                {
                                    rowErrorMessage += "<li>Indent not allowed for other Region/Branch</li>";
                                }

                                if (!string.IsNullOrEmpty(rowErrorMessage))
                                {
                                    rowErrorMessage = $"<ul>Row {index} has error{rowErrorMessage}</ul>";
                                    indentFileError += rowErrorMessage;
                                    index++;
                                    continue;
                                }

                                var address = new Address { AddressDetail = addressDetail, PinCode = pincode };
                                await _addressRepository.AddAsync(address);

                                indents.Add(new Indent
                                {
                                    IndentNo = await _indentService.GetNewIndentNo(),
                                    IndentDate = CommonUtils.GetParseDate(indentDate),
                                    IndentStatusId = IndentStatuses.PendingApproval.GetHashCode(),
                                    SchemeCodeId = _schemeCode.SchemeCodeId,
                                    C5CodeId = _c5Code.Id,
                                    CardTypeId = _c5Code.CardTypeId,
                                    NoOfKit = noOfKit,
                                    IblBranchId = _iblBranch != null ? _iblBranch.Id : 0,
                                    BfilBranchTypeId = _bfilBranchType.Id,
                                    BfilRegionId = _bfilBranch.RegionId,
                                    BfilBranchId = _bfilBranch.Id,
                                    DispatchAddressId = address.AddressId,
                                    ContactName = contactName,
                                    ContactNo = contactNo,
                                    DispatchAddress = new Address { AddressDetail = addressDetail, PinCode = pincode },
                                });
                            }
                            index++;
                        }
                    }
                }
            }
            catch (Exception)
            {
                indentFileError = "Please upload valid content for Indents!";
            }

            _fileService.DeleteFile(path);

            if (!string.IsNullOrEmpty(indentFileError))
            {
                return new ResponseModel { Success = false, StatusCode = StatusCodes.Status400BadRequest, Message = indentFileError };
            }

            return await _indentService.CreateIndents(indents);
        }

        [HttpPost]
        public async Task<ResponseModel> UploadDeliveryReport(long dispatchId, IFormFile file)
        {
            if (file == null || file.Length == 0)
                return new ResponseModel { Success = false, Message = "Please select the file!", StatusCode = StatusCodes.Status400BadRequest };

            var path = await _fileService.CreateFile(file);

            var errorResponse = new ResponseModel
            {
                Success = false,
                StatusCode = StatusCodes.Status400BadRequest
            };

            var dispatch = (await _dispatchService.GetDispatchById(dispatchId)).Data as Dispatch;
            var couriedStatuses = await _courierStatusService.GetDropdwon();

            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            try
            {
                using (var stream = System.IO.File.Open(path, FileMode.Open, FileAccess.Read))
                {
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        int index = 0;
                        int wayBillNoIndex = 0;
                        int dispatchDateIndex = 1;
                        int courierStatusIndex = 2;
                        int deliveryDateIndex = 3;
                        int receiverNameIndex = 4;
                        int courierPartnerIndex = 5;
                        int referenceNoIndex = 6;

                        while (reader.Read()) //Each row of the file
                        {
                            if (index == 0)
                            {
                            }
                            else
                            {
                                var wayBillNo = reader.GetValue(wayBillNoIndex)?.ToString().Trim() ?? "";
                                var dispatchDate = reader.GetValue(dispatchDateIndex)?.ToString().Trim() ?? "";
                                var courierStatus = reader.GetValue(courierStatusIndex)?.ToString().Trim() ?? "";
                                var deliveryDate = reader.GetValue(deliveryDateIndex)?.ToString().Trim() ?? "";
                                var receiverName = reader.GetValue(receiverNameIndex)?.ToString().Trim() ?? "";
                                var courierPartner = reader.GetValue(courierPartnerIndex)?.ToString().Trim() ?? "";
                                var referenceNo = reader.GetValue(referenceNoIndex)?.ToString().Trim() ?? "";


                                if (string.IsNullOrEmpty(wayBillNo) && string.IsNullOrEmpty(dispatchDate) &&
                                    string.IsNullOrEmpty(courierStatus) && string.IsNullOrEmpty(deliveryDate) &&
                                    string.IsNullOrEmpty(receiverName) && string.IsNullOrEmpty(courierPartner) &&
                                    string.IsNullOrEmpty(referenceNo))
                                {
                                    continue;
                                }

                                var rowErrorMessage = string.Empty;

                                var dispatchWayBill = dispatch.DispatchWayBills.FirstOrDefault(w => w.WayBillNo == wayBillNo);

                                if (dispatchWayBill == null)
                                {
                                    index++;
                                    continue;
                                }

                                if (dispatch.ReferenceNo != referenceNo)
                                {
                                    rowErrorMessage += "<li>Reference Number did not match</li>";
                                }

                                if (string.IsNullOrEmpty(dispatchDate))
                                {
                                    rowErrorMessage += "<li>Dispatch Date has no content</li>";
                                }
                                else if (CommonUtils.GetParseDate(dispatchDate).Date != dispatchWayBill.DispatchDate.Date)
                                {
                                    rowErrorMessage += "<li>Dispatch Date did not match</li>";
                                }

                                if (string.IsNullOrEmpty(courierStatus))
                                {
                                    rowErrorMessage += "<li>Status from Courier has no content</li>";
                                }
                                else
                                {
                                    var couriedStatus = couriedStatuses.FirstOrDefault(s => s.Text.Equals(courierStatus, StringComparison.InvariantCultureIgnoreCase));
                                    if (couriedStatus != null)
                                    {
                                        dispatchWayBill.CourierStatusId = couriedStatus.Id;
                                    }
                                    else
                                    {
                                        rowErrorMessage += "<li>Status from Courier did not match</li>";
                                    }
                                }

                                if (string.IsNullOrEmpty(deliveryDate))
                                {
                                    rowErrorMessage += "<li>Delivery Date has no content</li>";
                                }
                                else
                                {
                                    dispatchWayBill.DeliveryDate = CommonUtils.GetParseDate(deliveryDate);
                                }

                                if (string.IsNullOrEmpty(receiverName))
                                {
                                    rowErrorMessage += "<li>Receiver Name has no content</li>";
                                }
                                else
                                {
                                    dispatchWayBill.ReceiveBy = receiverName;
                                }

                                if (string.IsNullOrEmpty(courierPartner))
                                {
                                    rowErrorMessage += "<li>Courier Partner has no content</li>";
                                }
                                else
                                {
                                    dispatchWayBill.CourierPartner = courierPartner;
                                }

                                if (!string.IsNullOrEmpty(rowErrorMessage))
                                {
                                    rowErrorMessage = $"<ul>Row {index} has error{rowErrorMessage}</ul>";
                                    errorResponse.Message += rowErrorMessage;
                                    index++;
                                    continue;
                                }

                                dispatchWayBill.ReportUploadBy = _userProviderService.UserClaim.UserId;
                                dispatchWayBill.ReportUploadDate = CommonUtils.GetDefaultDateTime();
                                dispatchWayBill.ReceiveStatusId = DispatchStatuses.Received.GetHashCode();
                                dispatchWayBill.UpdatedDate = CommonUtils.GetDefaultDateTime();
                                dispatchWayBill.UpdatedBy = _userProviderService.UserClaim.UserId;
                            }
                            index++;
                        }
                    }
                }
            }
            catch (Exception)
            {
                errorResponse.Message = "Please upload valid file!";
            }

            _fileService.DeleteFile(path);

            if (!string.IsNullOrEmpty(errorResponse.Message))
            {
                return errorResponse;
            }

            var result = await _dispatchService.UpdateDispatch(dispatch);
            if (result.Success)
            {
                return new ResponseModel
                {
                    Success = true,
                    Message = "Dispatch Uploaded Report Successfully!",
                    StatusCode = StatusCodes.Status200OK,
                    Data = dispatch.DispatchWayBills.Select(w => new { w.DispatchWayBillId, w.DeliveryDate, w.CourierPartner, w.ReceiveBy })
                };
            }

            return errorResponse;
        }

        private async Task<IndentDetailModel> GetDispatchedDetail(long id)
        {
            var indentDetail = await _indentService.GetIndentDetailById(id);

            dynamic filters = new ExpandoObject();
            filters.indentId = id;

            indentDetail.DispatchModels = ((await _dispatchService.GetDispatchPaged(new Pagination { Filters = filters })).Data).Data;
            return indentDetail;
        }

        private async Task<DispatchModel> SetDispatchDetail(long dispathcId)
        {
            dynamic filters = new ExpandoObject();
            filters.dispatchId = dispathcId;

            var dispatches = ((await _dispatchService.GetDispatchPaged(new Pagination { Filters = filters })).Data).Data as List<DispatchModel>;
            var dispatch = dispatches.FirstOrDefault();
            ViewBag.Dispatch = dispatch;
            return dispatch;
        }

        private async Task SetComboBoxes(Indent indentModel)
        {
            long? regionId = _userProviderService.GetRegionId();

            var allBranches = (await _branchService.GetDropdwon(regionId: regionId));

            var branches = allBranches.Select(b => new DropdownModel { Id = b.Id, Text = $"{b.BranchCode} - {b.Text} ", IsActive = b.IsActive }).ToList();

            var regions = await _regionService.GetDropdwon(id: regionId);
            if (regions.Count == 1)
            {
                indentModel.BfilRegionId = regions.FirstOrDefault().Id;
            }

            var iblBranches = new List<DropdownModel>();
            var bfilBranches = new List<DropdownModel>();
            var schemeCodes = new List<DropdownModel>();
            var c5Codes = new List<DropdownModel>();
            var cardTypes = new List<DropdownModel>();

            if (indentModel.IndentId > 0 || regions.Count == 1)
            {
                bfilBranches = branches;
            }
            else if (indentModel.BfilRegionId > 0)
            {
                bfilBranches = allBranches.Where(br => br.RegionId == indentModel.BfilRegionId)
                                          .Select(b => new DropdownModel { Id = b.Id, Text = $"{b.BranchCode} - {b.Text} ", IsActive = b.IsActive }).ToList();
            }

            if (indentModel.IndentId > 0 || (indentModel.SchemeCodeId > 0 && indentModel.C5CodeId > 0))
            {
                schemeCodes = await _schemeCodeService.GetDropdwon(id: indentModel.SchemeCodeId);
                var _c5Codes = await _c5CodeService.GetDropdwon(id: indentModel.C5CodeId);
                c5Codes = _c5Codes.Select(b => new DropdownModel { Id = b.Id, Text = $"{b.Text} ", IsActive = b.IsActive }).ToList();
                cardTypes.Add(new DropdownModel { Id = _c5Codes.FirstOrDefault().CardTypeId, Text = _c5Codes.FirstOrDefault().CardTypeName });
            }

            if (indentModel.IndentId > 0 || indentModel.IblBranchId > 0)
            {
                iblBranches = (await _iblBranchService.GetDropdwon(id: indentModel.IblBranchId))
                          .Select(b => new DropdownModel { Id = b.Id, Text = $"{b.IblBranchCode} - {b.Text} ", IsActive = b.IsActive }).ToList();
            }

            var indentBranches = GetSelectList(iblBranches, iblBranches.Count == 1 ? "" : "Select IBL Branch");
            if (iblBranches.Count != 1) { indentBranches.First().Value = "0"; };

            ViewBag.IblBranches = indentBranches;
            ViewBag.SchemeCodes = GetSelectList(schemeCodes, schemeCodes.Count == 1 ? "" : "Select Scheme Code");
            ViewBag.C5Codes = GetSelectList(c5Codes, c5Codes.Count == 1 ? "" : "Select C5 Code");
            ViewBag.CardTypes = GetSelectList(cardTypes, cardTypes.Count == 1 ? "" : "Select Card Type");
            ViewBag.BranchTypes = GetSelectList(await _branchTypeService.GetDropdwon(true), "Select Address Type");
            ViewBag.Regions = GetSelectList(regions, regions.Count == 1 ? "" : "Select Region");
            ViewBag.BfilBranches = GetSelectList(bfilBranches, bfilBranches.Count == 1 ? "" : "Select BFIL Branch");
        }

        private async Task SetFilterComboBoxes()
        {
            long? regionId = _userProviderService.GetRegionId();

            var branches = (await _branchService.GetDropdwon(regionId: regionId))
                           .Select(b => new DropdownModel { Id = b.Id, Text = $"{b.BranchCode} - {b.Text} ", IsActive = b.IsActive }).ToList();

            var iblBranches = (await _iblBranchService.GetDropdwon())
                           .Select(b => new DropdownModel { Id = b.Id, Text = $"{b.IblBranchCode} - {b.Text} ", IsActive = b.IsActive }).ToList();

            var c5Codes = (await _c5CodeService.GetDropdwon())
                         .Select(b => new DropdownModel { Id = b.Id, Text = $"{b.Text} ", IsActive = b.IsActive }).ToList();

            ViewBag.Branches = GetSelectList(iblBranches, "All IBL Branch");
            ViewBag.SchemeCodes = GetSelectList(await _schemeCodeService.GetDropdwon(), "All Scheme Code");
            ViewBag.C5Codes = GetSelectList(c5Codes, "All C5 Code");
            ViewBag.CardTypes = GetSelectList(await _cardTypeService.GetDropdwon(), "All Card Type");
            ViewBag.BranchTypes = GetSelectList(await _branchTypeService.GetDropdwon(true), "All Address Type");
            ViewBag.BfilBranches = GetSelectList(branches, "All BFIL Branch");
            ViewBag.Regions = GetSelectList(await _regionService.GetDropdwon(id: regionId), "All Region");
        }

        private bool AuthorizeUser()
        {
            return
                _userProviderService.ROIndentMaker() || _userProviderService.ROKitManagement() ||
                _userProviderService.HOIndentMaker() || _userProviderService.HOApprover() ||
                _userProviderService.IBLCPU() || _userProviderService.BM() || _userProviderService.BCM();
        }
    }
}
