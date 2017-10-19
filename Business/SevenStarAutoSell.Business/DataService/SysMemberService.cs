using AutoMapper;
using Framework.DataAccess;
using SevenStarAutoSell.Common.Funcs;
using SevenStarAutoSell.DataAccess;
using SevenStarAutoSell.DataAccess.DbEntity;
using SevenStarAutoSell.Models;

namespace SevenStarAutoSell.Business.DataService
{
    public class SysMemberService : Singleton<SysMemberService>
    {
        public  SysMemberModel FindByUserName(string userName)
        {
            return SysMemberDataAccess.FindByUserName(userName);
        }

        public int UpdateLoginTime(int id)
        {
            return DbUtil.Master.ExecuteNonQuery("update system_user set last_login_time = NOW() where user_id = ?id;", new Parameter().AddMore("id", id));
        }

        /// <summary>
        /// 更改系统会员密码
        /// </summary>
        public string ChangePassword(int id, ChangePasswordParam param)
        {
            return "1";
        }
    }
}
