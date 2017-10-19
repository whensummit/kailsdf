using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SevenStarAutoSell.Models
{
    public class SysMemberModel
    {
        public string Account { get; set; }
        public int Id { get; set; }
        public string Pwd { get; set; }
    }

    public class SysMemberLoginParam
    {
        public string UserName { get; set; }

        public string Password { get; set; }
    }


    public class ChangePasswordParam
    {
        public string UserName { get; set; }

        public string OldPassword { get; set; }

        public string NewPassword { get; set; }
    }
}
