using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Newtonsoft.Json.Linq;
using SevenStarAutoSell.Business.Web.Dfv168.Model;
using SevenStarAutoSell.Business.Web.Dfv168.Common;
using SevenStarAutoSell.Model.Web;

namespace SevenStarAutoSell.Business.Web.Dfv168.Service
{
    /// <summary>
    /// dfv适配器
    /// </summary>
    public class QiXingAdapter
    {

        int gateway = 0;

        //脚本引擎
        ScriptEngine engine = new ScriptEngine();

        protected log4net.ILog log = log4net.LogManager.GetLogger(typeof(QiXingAdapter));

        #region 公开属性


        /// <summary>
        /// 登陆票
        /// </summary>
        public LoginToken LoginToken { get; set; }

        /// <summary>
        /// 登录参数
        /// </summary>
        public UserLogin LoginModel { get; set; }

        /// <summary>
        /// httpClient  对象
        /// </summary>
        protected HttpClient HttpClient { get; set; }

        /// <summary>
        /// 失败类型
        /// </summary>
        public FailType FailType { get; set; }

        #endregion

        /// <summary>
        /// 构造函数
        /// </summary>
        public QiXingAdapter()
        {
            HttpClient = new HttpClient();
            this.InitScriptEngine();
        }

        #region 公开方法

        /// <summary>
        /// 用户登陆
        /// </summary>
        /// <param name="userLogin">登陆信息</param>
        /// <returns></returns>
        public UserLoginResult Login(UserLogin userLogin)
        {
            LoginModel = userLogin;
            UserLoginResult result = new UserLoginResult();

            //获取登陆信息
            var token = GetLoginToken();

            if (null != token)
            {
                //登陆网关
                token = LoginGateway(token);
                if (null != token)
                {
                    this.LoginToken = token;
                    result.Successed = true;
                }
                else
                {
                    result.Successed = false;
                    result.Message = "登录网关失败";
                    result.LoginTime = DateTime.Now.Ticks;
                }
            }
            else
            {
                result.Successed = false;
                result.Message = "登录失败";
                result.LoginTime = DateTime.Now.Ticks;
            }

            return result;
        }

        /// <summary>
        /// 获取账号信息
        /// </summary>
        /// <returns></returns>
        public AccountInformation GetAccountInformation()
        {
            var result = HttpClient.Open("http:" + LoginToken.PathName + "/Front/A/A04", "http:" + LoginToken.PathName + "/Front/Shared/Index");
            if (null != result)
            {
                if (result.Contains("\"QuotaList\":[") && result.Contains("],\"GameStatus\""))
                {
                    var jsonData = result.Remove(0, result.LastIndexOf("\"QuotaList\":[") + 13);
                    jsonData = jsonData.Remove(jsonData.IndexOf("],\"GameStatus\""));
                    return Newtonsoft.Json.JsonConvert.DeserializeObject<AccountInformation>(jsonData);
                }
                else
                {
                    //获取到的数据不包含账号数据
                    return null;
                }
            }
            else
            {
                //网络请求失败,需要修改查询接口
                return null;
            }
        }

        /// <summary>
        /// 批量扫水
        /// </summary>
        /// <param name="numbers">批量扫水</param>
        /// <returns></returns>
        public IList<QueryInformation> BatchSeekWater(IList<string> numbers)
        {
            IList<Task> tasks = new List<Task>();
            IList<QueryInformation> querylist = new List<QueryInformation>();

            foreach (var number in numbers)
            {
                var task = Task.Run(() =>
                {
                    var result = this.SeekWater(number);
                    result.Number = number;
                    querylist.Add(result);
                });
                tasks.Add(task);
            }
            Task.WaitAll(tasks.ToArray());
            return querylist;
        }

        /// <summary>
        /// 批量押注
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public BatchOrderResult SubmitBatchOrder(BatchOrders order)
        {
            string order_data = "";

            order.ApiAddress = string.Format(order.ApiAddress, LoginToken.PathName);
            order.Referer = string.Format(order.Referer, LoginToken.PathName);

            if (order.ItemID > 0)
            {
                order_data = $"ItemID={order.ItemID}&IsBatch={order.IsBatch}&";
            }

            foreach (var item in order.OrderData)
            {
                int index = order.OrderData.IndexOf(item);
                order_data += $"OrderData[{index}][Bet]={item.Bet}&OrderData[{index}][No]={item.No}&";
            }
            order_data = order_data.Remove(order_data.LastIndexOf('&'));

            var result = HttpClient.Open(order.ApiAddress, order_data, order.Referer);
            if (null != result)
            {
                return Newtonsoft.Json.JsonConvert.DeserializeObject<BatchOrderResult>(result);
            }

            return null;
        }

        /// <summary>
        /// 批量退单
        /// </summary>
        /// <param name="orderId">订单号</param>
        /// <param name="apiAddress">API地址</param>
        /// <param name="referer">应用页面</param>
        /// <returns></returns>
        public BatchOrderDeleteResult DeleteBatchOrder(string orderId)
        {
            var result = HttpClient.Open($"http:{LoginToken.PathName}/api/FrontA10/OrderDel", $"ID={orderId}", $"http:{LoginToken.PathName}/Front/A/A10");
            if (null != result)
            {
                return Newtonsoft.Json.JsonConvert.DeserializeObject<BatchOrderDeleteResult>(result);
            }
            return null;
        }

        /// <summary>
        /// 扫水
        /// </summary>
        /// <typeparam name="T">实体信息</typeparam>
        /// <param name="num">编号</param>
        /// <returns></returns>
        public QueryInformation SeekWater(string num)
        {
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            var daraQuery = HttpClient.Open($"http:{LoginToken.PathName}/api/FrontC03/QueryOdds", string.Format("No={0}", num), $"http:{LoginToken.PathName}/Front/C/C03");
            sw.Stop();
            var ss = sw.ElapsedMilliseconds;
            if (daraQuery != null)
            {
                var info = Newtonsoft.Json.JsonConvert.DeserializeObject<QueryInformation>(daraQuery);
                return info;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 押注
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="num">押注号码</param>
        /// <param name="money">押注金额</param>
        /// <returns></returns>
        public SingleOrderResult SendOrder(string num, int money)
        {
            //下单
            var result = HttpClient.Open("http:" + LoginToken.PathName + "/api/FrontC03/OrderAdd", string.Format("No={0}&Bet={1}&Flags={2}", num, 1, 0), "http:" + LoginToken.PathName + "/Front/C/C03");
            if (null != result)
            {
                return Newtonsoft.Json.JsonConvert.DeserializeObject<SingleOrderResult>(result);
            }
            else
            {
                return null;
            }

        }

        /// <summary>
        /// 押注
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="num">押注号码</param>
        /// <param name="money">押注金额</param>
        /// <returns></returns>
        public SingleOrderResult SendOrder(string num, double money)
        {
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            //下单
            var result = HttpClient.Open("http:" + LoginToken.PathName + "/api/FrontC03/OrderAdd", string.Format("No={0}&Bet={1}&Flags={2}", num, 1, 0), "http:" + LoginToken.PathName + "/Front/C/C03");
            sw.Stop();
            var ss = sw.ElapsedMilliseconds;
            if (null != result)
            {
                var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<SingleOrderResult>(result);
                return obj;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 删单
        /// </summary>     
        /// <param name="orderId">押注单号</param>
        /// <returns></returns>
        public SingleOrderDelete CancelOrder(string orderId)
        {
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();

            var deleteResult = HttpClient.Open("http:" + LoginToken.PathName + "/api/FrontC03/OrderDel", string.Format("DelIDList[]={0}", orderId), "http:" + LoginToken.PathName + "/Front/C/C03");

            sw.Stop();

            var ss = sw.ElapsedMilliseconds;
            if (null != deleteResult)
            {
                return Newtonsoft.Json.JsonConvert.DeserializeObject<SingleOrderDelete>(deleteResult);
            }
            else
            {
                return null;
            }
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 初始化脚本引擎
        /// </summary>
        private void InitScriptEngine()
        {
            engine.AddScriptText(Scripts.prng4);
            engine.AddScriptText(Scripts.rng);
            engine.AddScriptText(Scripts.jsbn);
            engine.AddScriptText(Scripts.rsa);
        }

        /// <summary>
        /// 生成RSA加密字符串
        /// </summary>
        /// <param name="userName">账号</param>
        /// <param name="password">密码</param>
        /// <param name="code">验证码</param>
        /// <returns></returns>
        private string GenerateRSAString(string userName, string password, string code)
        {
            try
            {
                var rasKey1 = "B0C1D0E12B47DA93A24C7422D433170A4D5B126A27CED6F3087652723562889803E2280041F02E6B24A251C928EBE5BA12501D466D63A43AD5D88A5809A09271F14FD220C0DAE272CAAF9F1CF09DAF52005272529071723C0CB87B5A6392860CA2E72B23A4652910DB87BAC31D89E4DD23B3C56AAE685D9A14C4CC89862AAA27";
                var rasKey2 = "010001";

                var setkey = string.Format("rsa.setPublic('{0}', '{1}');", rasKey1, rasKey2); //设置公钥,加密位数
                var obj = new { Account = userName, Password = password, Code = code }; //登陆信息实体
                var jsonStr = Newtonsoft.Json.JsonConvert.SerializeObject(obj);                 //JSON序列化
                var account = "rsa.encrypt('" + jsonStr + "')";
                var encryptString = engine.Eval("var rsa = new RSAKey();" + setkey + account).ToString();

                log.InfoFormat("生成RSA密码成功{0}", encryptString);
                return encryptString;

            }
            catch (Exception exc)
            {
                log.Error(exc.Message, exc);
                return null;
            }

        }

        /// <summary>
        /// 获取LoginToken
        /// </summary>
        /// <returns></returns>
        private LoginToken GetLoginToken()
        {
            //根据用户名&密码&验证生成RSA加密字符串
            string encryptString = this.GenerateRSAString(LoginModel.LoginName, LoginModel.Password, "undefined");

            //用户登录
            var userInfo = HttpClient.Open($"http://{LoginModel.Domain}/api/shared/login", encryptString, $"http://{LoginModel.Domain}/");

            //获取到登陆信息
            if (null != userInfo)
            {
                //解析登陆信息
                var token = Newtonsoft.Json.JsonConvert.DeserializeObject<LoginToken>(userInfo);

                //登陆成功
                if (token.Code == 0)
                {
                    //保存Path 同时也是GID
                    token.PathName = token.Redirect.Replace("/Command/LoginGateway", "");
                    token.GID = token.PathName.Remove(0, token.Redirect.LastIndexOf(LoginModel.Domain.Replace("www.", "")) + 11);

                    log.Info(string.Format("获取身份票成功!token=[{0}]", token.Data));

                    return token;

                }
                else
                {
                    log.Error(string.Format("登陆失败:{0} 原因:{1}", token.Code, token.Message));
                    return null;
                }
            }
            else
            {
                log.Error(string.Format("登陆失败:{0}", "未找到执行的网址"));
                return null;
            }
        }

        /// <summary>
        /// 登陆网关
        /// </summary>
        /// <param name="token">登陆票</param>
        /// <returns>返回新的登陆票</returns>
        private LoginToken LoginGateway(LoginToken token)
        {
            gateway = gateway + 1;

            //提交登陆票
            var submit_Data = HttpClient.Open("http:" + token.Redirect, "+=" + token.Data, "http://pu6g.dfv168.com//");

            if (submit_Data.Length > 50)
            {
                //解析得到的数据
                submit_Data = submit_Data.Remove(0, submit_Data.IndexOf("value='") + 7);
                submit_Data = submit_Data.Remove(submit_Data.IndexOf("/>") - 2);
                token.SubmitData = submit_Data;

                return token;

                //登陆票据 保存
                //this.LoginToken = token;
                //var html = httpClient.Open("http://pu6g.dfv168.com/" + token.PathName + "/Front/Shared/Index", "+=" + submit_Data, "http://" + token.Redirect);
                //var Index = httpClient.Open("http://pu6g.dfv168.com/" + token.PathName + "/Front/C/C03", null, "http://pu6g.dfv168.com/" + token.PathName + "/Front/Shared/Description");
                //var daraQuery = HttpClient.Open("http://pu6g.dfv168.com/" + token.PathName + "/api/FrontC03/QueryOdds", "No=2345", "http://pu6g.dfv168.com/" + token.PathName + "/Front/C/C03");
            }
            else
            {
                log.Error("登陆网管失败");
                if (gateway <= 5)
                {
                    return LoginGateway(token);
                }
                else
                {
                    return null;
                }
            }

        }

        #endregion

    }
}
