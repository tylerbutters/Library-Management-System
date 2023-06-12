using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS
{
    public class Member
    {
        public int id { get; set; }
        public int pin { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }

        //public Member(int _id, int _pin, string _firstName, string _lastName, string _email) {
        //    id = _id;
        //    pin = _pin;
        //    firstName = _firstName;
        //    lastName = _lastName;
        //    email = _email;
        //}
    }
}
