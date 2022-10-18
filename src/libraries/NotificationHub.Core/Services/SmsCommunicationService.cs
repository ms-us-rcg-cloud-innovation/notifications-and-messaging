using Azure;
using Azure.Communication.Sms;
using NotificationHub.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationHub.Core.Services
{
    public class SmsCommunicationService
    {
        private readonly SmsClient _smsClient;

        public SmsCommunicationService(SmsClient smsClient)
        {
            _smsClient = smsClient;
        }

        public async Task<IReadOnlyList<SmsSendResult>> SendSmsMessageAsync(SmsMessage message)
        {
            Response<IReadOnlyList<SmsSendResult>> smsSendResult = await _smsClient.SendAsync(
                from: message.FromPhone.ToString(),
                to: message.ToPhone.Select(x => x.E164Number).ToArray(),
                message: message.Body);

            var rawResponse = smsSendResult?.GetRawResponse();

            if(rawResponse is null || rawResponse.Status > 299)
            {
                throw new Exception("Failed to send messages to receipients");
            }

            return smsSendResult.Value;
        }
    }
}
