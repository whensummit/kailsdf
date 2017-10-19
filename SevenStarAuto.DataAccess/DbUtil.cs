using System;
using System.Configuration;
using Framework.DataAccess;

namespace SevenStarAutoSell.DataAccess
{
    public class MySqlDataMaster : AbstractData
    {
        public override string ConnectionString => ConfigurationManager.ConnectionStrings["DBConnectionStringMaster"].ConnectionString;

        protected override DatabaseTypeEnum DatabaseType => DatabaseTypeEnum.MySql;

        protected override string ContextID => "MySqlDataMaster";
    }

    public class MySqlDataSlave : AbstractData
    {
        public override string ConnectionString => ConfigurationManager.ConnectionStrings["DBConnectionStringSlave"].ConnectionString;

        protected override DatabaseTypeEnum DatabaseType => DatabaseTypeEnum.MySql;

        protected override string ContextID => "MySqlDataSlave";
    }

    public static class DbUtil
    {
        /// <summary>
        /// 主库
        /// </summary>
        public static MySqlDataMaster Master { get; } = new MySqlDataMaster();

        /// <summary>
        /// 从库
        /// </summary>
        public static MySqlDataSlave Slave { get; } = new MySqlDataSlave();
    }
}
