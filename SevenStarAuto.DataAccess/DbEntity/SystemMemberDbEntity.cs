using Framework.DataAccess;
using System;

namespace SevenStarAutoSell.DataAccess.DbEntity
{
    [Serializable]
    [TableMapping(TableName = "system_user")]
    public class SystemMemberDbEntity : Framework.DataAccess.Model
    {
        private int? _id = null;
        private string _account = null;
        private string _pwd = null;

        /// <summary>
        /// 用户ID
        /// </summary>
        [TableMapping(FieldName = "user_id", PrimaryKey = true)]
        public int? Id
        {
            get { return _id; }
            set { _id = value; }
        }

        /// <summary>
        /// 账号
        /// </summary>
        [TableMapping(FieldName = "user_account", PrimaryKey = false)]
        public string Account
        {
            get { return _account; }
            set { _account = value; }
        }

        /// <summary>
        /// 加密的密码
        /// </summary>
        [TableMapping(FieldName = "user_pwd", PrimaryKey = false)]
        public string Pwd
        {
            get { return _pwd; }
            set { _pwd = value; }
        }
    }
}
