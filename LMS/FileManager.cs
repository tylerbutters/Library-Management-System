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
        
        public static List<Member> ReadFile(string memberInfoFile)
        {
            var lines = File.ReadAllLines(memberInfoFile);
            var data = from l in lines.Skip(1)
                       let split = l.Split(',')
                       select new Member
                       {
                           id = int.Parse(split[0]),
                           pin = int.Parse(split[1]),
                           firstName = split[2],
                           lastName = split[3],
                           email = split[4],
                       };
            return data.ToList();
        }
        
    }
}
