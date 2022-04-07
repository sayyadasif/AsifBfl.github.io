using Core.Repository.Models;
using Core.Security;
using Core.Utility.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TKMS.Abstraction.ComplexModels;
using TKMS.Abstraction.Models;
using TKMS.Repository.Interfaces;
using TKMS.Service.Interfaces;

namespace TKMS.Service.Services
{
    public class SmsService : ISmsService
    {
        private readonly IUserProviderService _userProviderService;
        private readonly ISentSmsService _sentSmsService;
        private readonly BfilSmsSettings _bfilSmsSettings;

        public SmsService(
            IUserProviderService userProviderService,
            ISentSmsService sentSmsService,
            IOptions<BfilSmsSettings> bfilSmsSettings
        )
        {
            _userProviderService = userProviderService;
            _sentSmsService = sentSmsService;
            _bfilSmsSettings = bfilSmsSettings.Value;
        }

        public async Task<ResponseModel> SendSms(SentSmsRequest model)
        {
            try
            {
                var restClient = new RestClient(_bfilSmsSettings.GatewayUrl);
                var smsMessage = string.Format(model.IsAllocate ? _bfilSmsSettings.AllocateTemplate : _bfilSmsSettings.ReturnTemplate, model.Otp);
                var request = new RestRequest(Method.POST)
                {
                    Resource = "",
                    RequestFormat = DataFormat.Json
                }
                .AddJsonBody(new SmsRequest
                {
                    apikey = _bfilSmsSettings.Apikey,
                    senderid = _bfilSmsSettings.SenderId,
                    number = model.MobileNumbers,
                    message = smsMessage,
                    format = _bfilSmsSettings.Format,
                });

                var response = await restClient.ExecuteAsync<dynamic>(request);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var sentSmsResult = JsonConvert.DeserializeObject<SmsResult>(response.Content);
                    var isSuccess = sentSmsResult.status == "OK";

                    var smsResult = await _sentSmsService.CreateSentSms(new SentSms
                    {
                        MobileNumber = model.MobileNumbers,
                        Otp = model.Otp,
                        Message = smsMessage,
                        IsSuccess = isSuccess,
                        ResponseMessage = sentSmsResult.message,
                    });

                    return new ResponseModel
                    {
                        Success = isSuccess,
                        StatusCode = StatusCodes.Status200OK,
                        Message = sentSmsResult.message,
                        Data = smsResult.Data
                    };
                }

            }
            catch (Exception ex)
            {
                Serilog.Log.Error(ex, ex.Message);
            }

            return new ResponseModel { Success = false, StatusCode = StatusCodes.Status500InternalServerError, Message = "Sms does not sent" };
        }
    }
}
