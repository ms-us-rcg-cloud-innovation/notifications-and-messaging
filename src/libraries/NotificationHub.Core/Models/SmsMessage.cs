using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationHub.Core.Models
{
    public class PhoneNumber
    {
        public PhoneNumber(string e164Number)
        {
            E164Number = e164Number;
        }

        [RegularExpression("\\(?\\+?\\d{1}\\)\\?-?\\(?\\d{3}\\)?-? *\\d{3}-? *-?\\d{4}")]
        public string E164Number { get; }

        public override string ToString()
            => this.E164Number;
    }

    public class SmsMessage
        : NotificationMessageBase
    {

        public PhoneNumber FromPhone { get; set; }

        public PhoneNumber[] ToPhone { get; set; }
    }
}
