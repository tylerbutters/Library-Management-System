using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace LMS
{ 
    class FileManager
    {

        private List<Member> LoadMembers()
        {

            var rows = File.ReadAllLines(@"./Database/memberInformation.csv");
            List<Member> members = new List<Member>();
            
            var membersvar = from l in rows.Skip(1) //skips first row
                       let split = l.Split(',') //splits cell at comma
                       select new Member()
                       {
                           id = int.Parse(split[0]),
                           pin = int.Parse(split[1]),
                           firstName = split[2],
                           lastName = split[3],
                           email = split[4],
                       };
            members = membersvar.ToList();
            return members;
        }
    }
}
