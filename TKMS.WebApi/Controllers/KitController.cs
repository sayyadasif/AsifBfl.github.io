using Core.Repository.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TKMS.Abstraction.ComplexModels;
using TKMS.Abstraction.Enums;
using TKMS.Abstraction.Models;
using TKMS.Service.Interfaces;

namespace TKMS.WebApi.Controllers
{
    /// <summary>
    /// Kit entity management class
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class KitController : ControllerBase
    {
        private readonly IKitService _kitService;
        private readonly IBranchTransferService _branchTransferService;
        private readonly IKitDamageReasonService _kitDamageReasonService;

        /// <summary>
        /// Kit constructor
        /// </summary>
        /// <param name="kitService"></param>
        public KitController(
            IKitService kitService,
            IBranchTransferService branchTransferService,
            IKitDamageReasonService kitDamageReasonService
            )
        {
            _kitService = kitService;
            _branchTransferService = branchTransferService;
            _kitDamageReasonService = kitDamageReasonService;
        }

        /// <summary>
        /// Get Kit By Account No
        /// </summary>
        /// <param name="accountNo"></param>
        /// <returns></returns>
        [HttpGet("{accountNo}")]
        public async Task<IActionResult> GetKitByAccountNo(string accountNo)
        {
            var result = await _kitService.GetKitByAccountNo(accountNo, KitStatuses.Dispatched.GetHashCode());
            return Ok(result);
        }

        /// <summary>
        /// Get Kit By Account No
        /// </summary>
        /// <param name="accountNo"></param>
        /// <returns></returns>
        [HttpGet("Transfer/{accountNo}")]
        public async Task<IActionResult> GetTransferKitByAccountNo(string accountNo)
        {
            var result = await _branchTransferService.GetTransferKitByAccountNo(accountNo);
            return Ok(result);
        }

        /// <summary>
        /// Update Kit Details
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> UpdateKit(KitUpdateRequest model)
        {
            var result = await _kitService.UpdateKitDetails(model);
            return Ok(result);
        }

        /// <summary>
        /// Update Transfer Kit Details
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("Transfer")]
        public async Task<IActionResult> UpdateTransferKit(BranchTransferUpdateRequest model)
        {
            var result = await _branchTransferService.UpdateBranchTransfer(model);
            return Ok(result);
        }

        /// <summary>
        /// Get All Damage Reasons
        /// </summary>
        /// <returns></returns>
        [HttpGet("DamageReasons")]
        public async Task<IActionResult> DamageReasons()
        {
            var result = await _kitDamageReasonService.GetDropdwon();
            return Ok(result);
        }
    }
}
