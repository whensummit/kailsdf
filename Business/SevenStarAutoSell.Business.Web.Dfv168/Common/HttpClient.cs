using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
using System.Net;
using System.Threading;
using System.IO.Compression;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Collections;

namespace SevenStarAutoSell.Business.Web.Dfv168.Common
{
    public delegate void HttpClientExceptionEventHandler(Exception ex);
    public delegate void HttpClientRequestEventHandler(HttpClientEventArgs args);

    /// <summary>
    /// 采集核心类
    /// </summary>
    public class HttpClient
    {
        #region 字段&属性

        log4net.ILog log = log4net.LogManager.GetLogger(typeof(HttpClient));
        private const int Timeout = 10000;

        private long _ticks;

        private object _lockOpenSerialize = new object();
        private object _lockImageSerialize = new object();

        private string _charset = "UTF-8";
        private string _accept = "*/*";
        private string _contentType = "application/x-www-form-urlencoded";
        private string _userAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64; Trident/7.0; rv:11.0) like Gecko";
        private RequestSpeed _requestSpeed = RequestSpeed.Fast;

        private string _referer;
        private WebProxy _proxy;
        //private HttpWebRequest _request;
        private CookieCollection _cookieCollection = new CookieCollection();
        private CookieContainer _cookieContainer = new CookieContainer();

        public event HttpClientExceptionEventHandler Error;
        public event HttpClientRequestEventHandler BeginRequest;
        public event HttpClientRequestEventHandler EndRequest;

        public HttpClient()
        {
            ServicePointManager.Expect100Continue = false;
            ServicePointManager.DefaultConnectionLimit = 3000;
            ServicePointManager.UseNagleAlgorithm = false;
            AllowAutoRedirect = true;
        }

        public string ContentType
        {
            get { return _contentType; }
            set { _contentType = value; }
        }

        public string Charset
        {
            get { return _charset; }
            set { _charset = value; }
        }

        public string Accept
        {
            get { return _accept; }
            set { _accept = value; }
        }

        public string UserAgent
        {
            get { return _userAgent; }
            set { _userAgent = value; }
        }

        public RequestSpeed RequestSpeed
        {
            get { return _requestSpeed; }
            set { _requestSpeed = value; }
        }

        /// <summary>
        /// 是否自动跳转
        /// </summary>
        public bool AllowAutoRedirect { get; set; }
        

        public double RequestInterval
        {
            get; set;
        }

        //获取返回的header
        public WebHeaderCollection ResponeHeader
        {
            get; set;
        }

        //设置请求的header
        public Dictionary<string, string> AddRequestHeader
        {
            get; set;
        }

        //public HttpWebRequest Request
        //{
        //    get { return _request; }
        //    set { _request = value; }
        //}

        public CookieCollection CookieCollection
        {
            get { return _cookieCollection; }
            set { _cookieCollection = value; }
        }

        public CookieContainer CookieContainer
        {
            get { return _cookieContainer; }
            set { _cookieContainer = value; }
        }

        /// <summary>
        /// 设置代理（如果是未指定代理，则使用浏览器默认代理；设置null表示不用代理；）
        /// </summary>
        public WebProxy Proxy
        {
            get { return _proxy; }
            set { _proxy = value; }
        }

        public string CookiesString { get; private set; }

        #endregion

        #region 请求html

        public string Open(string uri)
        {
            return Open(uri, null);
        }

        public string Open(string uri, string postData)
        {
            return Open(uri, postData, null);
        }

        public string Open(string uri, string postData, string referer)
        {
            HttpWebRequest _request;
            int i = 0;
            var sw = new Stopwatch();
            sw.Start();
            //WaitAMoment();
            //lock (_lockOpenSerialize)
            {
                string html = null;

                HttpWebResponse response = null;
                Stream responseStream = null;
                StreamReader sr = null;

                try
                {
                    if (RequestInterval > 0)
                    {
                        TimeSpan ts = new TimeSpan(DateTime.Now.Ticks - _ticks);
                        double sleep = ts.TotalMilliseconds;
                        if (sleep < RequestInterval)
                        {
                            Thread.Sleep((int)(RequestInterval - sleep));
                        }

                        _ticks = DateTime.Now.Ticks;
                    }

                    if (referer == null)
                    {
                        referer = _referer;
                    }

                    if (BeginRequest != null)
                    {
                        HttpClientEventArgs e = new HttpClientEventArgs();
                        e.Uri = uri;
                        e.PostData = postData;
                        e.Referer = referer;
                        BeginRequest(e);
                    }

                    if (uri.StartsWith("https", StringComparison.OrdinalIgnoreCase))
                    {
                        ServicePointManager.ServerCertificateValidationCallback = CheckValidationResult;
                        _request = (HttpWebRequest)WebRequest.Create(uri);
                        _request.ProtocolVersion = HttpVersion.Version10;
                    }
                    else
                    {
                        _request = (HttpWebRequest)WebRequest.Create(uri);
                    }
                    _request.Referer = referer;
                    _request.Headers.Add("x-requested-with", "XMLHttpRequest");
                    _request.Accept = _accept;
                    _request.UserAgent = _userAgent;
                    _request.CookieContainer = _cookieContainer;
                    _request.Timeout = Timeout;

                    if (_proxy != null)
                    {
                        _request.Proxy = _proxy;
                    }
                    _request.AllowWriteStreamBuffering = true;
                    _request.AllowAutoRedirect = AllowAutoRedirect;
                    _request.KeepAlive = true;

                    if (AddRequestHeader != null)
                    {
                        foreach (var addheader in AddRequestHeader)
                        {
                            _request.Headers.Add(addheader.Key, addheader.Value);
                        }
                    }

                    foreach (Cookie cookie in _cookieCollection)
                    {
                        _cookieContainer.SetCookies(_request.RequestUri, cookie.Name + "=" + cookie.Value);
                    }

                    if (string.IsNullOrEmpty(postData))
                    {
                        _request.Method = "GET";
                    }
                    else
                    {
                        byte[] bytes = Encoding.GetEncoding(_charset).GetBytes(postData);

                        _request.Method = "POST";
                        _request.ContentType = _contentType;
                        _request.ContentLength = bytes.Length;

                        using (Stream writer = _request.GetRequestStream())
                        {
                            writer.Write(bytes, 0, bytes.Length);
                        }
                    }
                    response = (HttpWebResponse)_request.GetResponse();

                    ResponeHeader = response.Headers;

                    #region Cookie管理

                    // 1.当前地址的cookie （基础类自动处理，不会处理非当前路径的cookie）
                    _cookieContainer = _request.CookieContainer;

                    // 2. 处理response返回的所有cookie(非当前url路径的)
                    string allCookies = response.Headers["Set-Cookie"];
                    if (!string.IsNullOrWhiteSpace(allCookies))
                    {
                        string pathSplit = "path=";
                        while (allCookies.Contains(pathSplit))
                        {
                            #region 分解["Set-Cookie"]字符串

                            string cookieText =
                                allCookies.Substring(0, allCookies.IndexOf(pathSplit,
                                    StringComparison.CurrentCultureIgnoreCase));
                            allCookies = allCookies.Substring($"{cookieText}{pathSplit}".Length);

                            int nextCookieSplitIndex = allCookies.IndexOf(",",
                                StringComparison.CurrentCultureIgnoreCase);

                            string cookiePath = nextCookieSplitIndex == -1
                                ? allCookies
                                : allCookies.Substring(0, nextCookieSplitIndex);

                            allCookies = nextCookieSplitIndex == -1
                                ? string.Empty
                                : allCookies.Substring($"{cookiePath},".Length);

                            #endregion

                            #region 注入 _cookieContainer

                            string cookiePathFull;
                            cookieText = cookieText.Trim();
                            cookiePath = cookiePath.Trim();
                            if (cookiePath.Contains("://"))
                            {
                                cookiePathFull = cookiePath;
                            }
                            else
                            {
                                cookiePathFull = $"{_request.RequestUri.Scheme}://{_request.RequestUri.Authority}{cookiePath}";
                            }
                            _cookieContainer.SetCookies(new Uri(cookiePathFull), cookieText);

                            #endregion
                        }
                    }

                    #endregion


                    if (response.ContentEncoding.ToLower().Contains("gzip"))
                    {
                        responseStream = new GZipStream(response.GetResponseStream(), CompressionMode.Decompress);
                    }
                    else if (response.ContentEncoding.ToLower().Contains("deflate"))
                    {
                        responseStream = new DeflateStream(response.GetResponseStream(), CompressionMode.Decompress);
                    }
                    else
                    {
                        responseStream = response.GetResponseStream();
                    }

                    sr = new StreamReader(responseStream, Encoding.GetEncoding(_charset));
                    html = sr.ReadToEnd();

                    if (EndRequest != null)
                    {
                        HttpClientEventArgs e = new HttpClientEventArgs();
                        e.Uri = uri;
                        e.PostData = postData;
                        e.Referer = referer;
                        e.ResponseText = html;
                        EndRequest(e);
                    }
                }
                catch (Exception ex)
                {
                    //LogCache.Instance.AddInfo(
                    //    $"HttpClient.Open() 发生异常。(url:{uri},postdata:{postData},referer{referer}) \n异常详情：{ex}\n{ex.StackTrace}");
                    //if (Error != null)
                    //{
                    //    Error(ex);
                    //}
                    log.Error("请求发生异常", ex);
                }
                finally
                {
                   
                    _referer = uri;

                    if (response != null)
                    {
                        response.Close();
                        response = null;
                    }

                    if (responseStream != null)
                    {
                        responseStream.Close();
                        responseStream = null;
                    }

                    if (sr != null)
                    {
                        sr.Close();
                        sr = null;
                    }
                    if (html == null)
                    {
                        log.Error($"请求html为null,url:{uri},postdata:{postData}");
                        //LogCache.Instance.AddInfo(
                        //    $"HttpClient.Open() 请求html为null。(invokmethod{new StackFrame(1).GetMethod()}, url:{uri},postdata:{postData},referer{referer}) ");
                    }
                }

                sw.Stop();
                //log.Debug($"{i}请求服务花费了{sw.ElapsedMilliseconds}ms。");
                i++;
                return html;
            }
        }

        public string OpenEx(string uri, string postData, string referer,string gid)
        {         
            return this.Open(uri, postData, referer);

        }

        #endregion

        #region 请求验证码图片

        public Image GetImage(string uri)
        {
            return GetImage(uri, null, null);
        }

        public Image GetImage(string uri, string postData)
        {
            return GetImage(uri, postData, null);
        }

        public Image GetImage(string uri, string postData, string referer)
        {
            WaitAMoment();
            lock (_lockImageSerialize)
            {
                Image img = null;

                HttpWebResponse response = null;
                Stream responseStream = null;
                StreamReader sr = null;
           
                try
                {
                    if (referer == null)
                    {
                        referer = _referer;
                    }

                    if (BeginRequest != null)
                    {
                        HttpClientEventArgs e = new HttpClientEventArgs();
                        e.Uri = uri;
                        e.PostData = postData;
                        e.Referer = referer;
                        BeginRequest(e);
                    }

                    var _request = (HttpWebRequest)WebRequest.Create(uri);
                    _request.Referer = referer;
                    _request.Accept = _accept;
                    _request.UserAgent = _userAgent;
                    _request.CookieContainer = _cookieContainer;
                    _request.Timeout = Timeout;
                    if (_proxy != null)
                    {
                        _request.Proxy = _proxy;
                    }
                    _request.AllowWriteStreamBuffering = true;
                    _request.AllowAutoRedirect = true;
                    _request.KeepAlive = true;

                    foreach (Cookie cookie in _cookieCollection)
                    {
                        _cookieContainer.SetCookies(_request.RequestUri, cookie.Name + "=" + cookie.Value);
                    }

                    if (string.IsNullOrEmpty(postData))
                    {
                        _request.Method = "GET";
                    }
                    else
                    {
                        byte[] bytes = Encoding.GetEncoding(_charset).GetBytes(postData);

                        _request.Method = "POST";
                        _request.ContentType = "application/x-www-form-urlencoded";
                        _request.ContentLength = bytes.Length;
                        using (Stream writer = _request.GetRequestStream())
                        {
                            writer.Write(bytes, 0, bytes.Length);
                        }
                    }

                    response = (HttpWebResponse)_request.GetResponse();
                    _cookieContainer = _request.CookieContainer;
                    responseStream = response.GetResponseStream();

                    img = Image.FromStream(responseStream);

                    if (EndRequest != null)
                    {
                        HttpClientEventArgs e = new HttpClientEventArgs();
                        e.Uri = uri;
                        e.PostData = postData;
                        e.Referer = referer;
                        e.ResponseImage = img;
                        EndRequest(e);
                    }
                }
                catch (Exception ex)
                {
                    log.Error("获取图片出现异常！", ex);
                    //LogCache.Instance.AddInfo(
                    //    $"HttpClient.GetImage() 发生异常。(url:{uri},postdata:{postData},referer{referer}) \n异常详情：{ex}\n{ex.StackTrace}");
                    if (Error != null)
                    {
                        Error(ex);
                    }
                }
                finally
                {
                    if (response != null)
                    {
                        response.Close();
                        response = null;
                    }

                    if (responseStream != null)
                    {
                        responseStream.Close();
                        responseStream = null;
                    }

                    if (sr != null)
                    {
                        sr.Close();
                        sr = null;
                    }
                    if (img == null)
                    {
                        //LogCache.Instance.AddInfo(
                        //    $"HttpClient.GetImage() 请求image为null。(invokmethod{new StackFrame(1).GetMethod()}, url:{uri},postdata:{postData},referer{referer}) ");
                        log.Error("图片为空！");
                    }
                }

                return img;
            }
        }

        public string SerializeCookies()
        {
            List<Cookie> listCookies = new List<Cookie>();

            Hashtable table = (Hashtable)_cookieContainer.GetType().InvokeMember("m_domainTable", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.GetField | System.Reflection.BindingFlags.Instance, null, _cookieContainer, new object[] { });
            foreach (object pathList in table.Values)
            {
                SortedList lstCookieCol = (SortedList)pathList.GetType().InvokeMember("m_list", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.GetField | System.Reflection.BindingFlags.Instance, null, pathList, new object[] { });
                foreach (CookieCollection colCookies in lstCookieCol.Values)
                {
                    foreach (Cookie c in colCookies)
                    {
                        listCookies.Add(c);
                    }
                }
            }

            StringBuilder sb = new StringBuilder();
            foreach (Cookie cookie in listCookies)
            {
                sb.AppendFormat("{0};{1};{2};{3};{4};{5}\r\n", cookie.Domain, cookie.Name, cookie.Path, cookie.Port, cookie.Secure.ToString(), cookie.Value);
            }

            return sb.ToString();
        }

        #endregion

        public void DeserializeCookies(string cookiesString)
        {
            string[] cookies = cookiesString.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            foreach (string c in cookies)
            {
                string[] cc = c.Split(";".ToCharArray());

                Cookie ck = new Cookie();
                ck.Domain = cc[0];
                ck.Name = cc[1];
                ck.Path = cc[2];
                ck.Port = cc[3];
                ck.Secure = bool.Parse(cc[4]);
                ck.Value = cc[5];

                _cookieContainer.Add(ck);
            }
        }

        private bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true; //总是接受  
        }

        /// <summary>
        /// 请求之前等待一会，避免ip被封
        /// </summary>
        private void WaitAMoment()
        {
            if (_requestSpeed == RequestSpeed.Slow)
            {
                Thread.Sleep(800);
            }
            else
            {
                Thread.Sleep(100);
            }
        }
    }
    
    public class HttpClientEventArgs : EventArgs
    {
        public string Uri { get; set; }
        public string PostData { get; set; }
        public string Referer { get; set; }
        public string ResponseText { get; set; }
        public Image ResponseImage { get; set; }
    }

    public class PostData
    {
        private Dictionary<string, string> _dict = new Dictionary<string, string>();

        public void Add(string name, string value)
        {
            _dict[name] = value;
        }

        public void Remove(string name)
        {
            _dict.Remove(name);
        }

        public string this[string key]
        {
            get { return _dict[key]; }
            set { _dict[key] = value; }
        }

        public string ToPostDataString()
        {
            string postData = "";

            foreach (KeyValuePair<string, string> kv in _dict)
            {
                postData += "&" + kv.Key + "=" + kv.Value;
            }

            if (postData.Length > 0)
            {
                postData = postData.Substring(1);
            }

            return postData;
        }

        public string ToJsonString()
        {
            string postData = "";

            foreach (KeyValuePair<string, string> kv in _dict)
            {
                string value = kv.Value.Replace("\\", "\\\\");
                postData += ",\"" + kv.Key + "\":" + "\"" + value + "\"";
            }

            if (postData.Length > 0)
            {
                postData = postData.Substring(1);
            }

            postData = "{" + postData + "} ";

            return postData;
        }
    }

    /// <summary>
    /// 请求速度（速度越快，IP越容易被封。不同网站用不同速度）
    /// </summary>
    public enum RequestSpeed
    {
        Slow,
        Fast
    }
}
