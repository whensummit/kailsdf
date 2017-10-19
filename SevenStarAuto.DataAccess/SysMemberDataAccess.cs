using Framework.DataAccess;
using SevenStarAutoSell.DataAccess.DbEntity;
using SevenStarAutoSell.Models;

namespace SevenStarAutoSell.DataAccess
{
    public class SysMemberDataAccess
    {
        public static SysMemberModel FindByUserName(string userName)
        {
            var user_id = DbUtil.Slave.ExecuteScalar($"select user_id from system_user  where `user_account`=?account LIMIT 1",
               new Parameter().AddMore("account", userName))?.ToString();
            if (string.IsNullOrWhiteSpace(user_id))
                return null;

            int id = int.Parse(user_id.ToString());
            var entity = DbUtil.Slave.Retrieve<SystemMemberDbEntity>(id);
            //Mapper.Initialize(cfg => cfg.CreateMap<SystemMemberDbEntity, SysMemberModel>());
            ////var config = new MapperConfiguration(cfg => cfg.CreateMap<SystemMemberDbEntity, SysMemberModel>());
            //var result = Mapper.Map<SystemMemberDbEntity, SysMemberModel>(entity);
            return new SysMemberModel
            {
                Id = entity.Id.Value,
                Account = entity.Account,
                Pwd = entity.Pwd
            };
        }
    }
}
