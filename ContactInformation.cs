using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddressBook
{
    public class ContactInformation
    {
        public string firstName { get; set; }
        public string secondName { get; set; }
        public string telephoneNumber { get; set; }
        public string eMail { get; set; }

        public ContactInformation(string firstName_, string secondName_,
          string telephoneNumber_, string eMail_)
        {
            firstName = firstName_;
            secondName = secondName_;
            telephoneNumber = telephoneNumber_;
            eMail = eMail_;
        }

        public override string ToString()
        {
            return this.firstName + "," + this.secondName + "," + this.telephoneNumber + "," + this.eMail;
        }
    }
}
