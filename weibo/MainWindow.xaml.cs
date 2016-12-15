using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace weibo
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        string strCookies = string.Empty;
        CookieContainer cookies = null;
        static Boolean flag = true;
        public MainWindow()
        {
            InitializeComponent();
            /*string s = "12341243561253";
            Regex reg = new Regex("(1.*?3)");
            Match match = reg.Match(s);
            while (match.Success) {
                string value = match.Groups[1].Value;
                MessageBox.Show(value);
                match = match.NextMatch();
            }*/

            /*
            Regex reg = new Regex(@"(?is)<li\s+class=""l1\s*"">([^<]+)</li>[^<]*<[^>]*>([^<]+)</li>(?:[^<]*<li[^>]*>\s*</li>)*[^<]*<li[^>]*>(\d{2})</li>");
            MatchCollection mc = reg.Matches(yourStr);
            foreach (Match m in mc)
            {
                richTextBox2.Text += m.Groups[1].Value + "\n";
                richTextBox2.Text += m.Groups[2].Value + "\n";
                richTextBox2.Text += m.Groups[3].Value + "\n";
            }
             */

            //DateTime DT = System.DateTime.Now;
            //MessageBox.Show(DT.ToString());
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            
            string username = textBox.Text;
            string UserUrl = string.Empty;
            string content = string.Empty;

            //得到用户首页的URL地址
            UserUrl = getUserUrl(username);
            //获取用户首页的所有内容
            if (UserUrl == "") {
                MessageBox.Show("微博名字输入错误请修正！");
            }
            else {
                flag = true;
                Thread t = new Thread(getUserContent);
                t.Start((object)UserUrl);
                button.Visibility = Visibility.Hidden;
                MessageBox.Show("绑定完毕");
                //this.Close();
                button1.Visibility = Visibility.Visible;
            }
            
            //File.WriteAllText(@"C:\wampserver\test.html", content);


        }

        /*private string getUserContent(string UserUrl) {
            string content = string.Empty;
            string url = UserUrl;
            cookies = new CookieContainer();
            cookies.Add(new Uri(UserUrl), new Cookie("YF-V5-G0", "da1eb9ea7ccc47f9e865137ccb4cf9f3"));
            cookies.Add(new Uri(UserUrl), new Cookie("YF-Page-G0", "8fee13afa53da91ff99fc89cc7829b07"));
            cookies.Add(new Uri(UserUrl), new Cookie("SUB", "_2AkMvYrpCf8NhqwJRmP4UyW_rbot0yQvEieLBAH7sJRMxHRl-yT83qk4ktRAKQx4PE5vwZZT70h16amGD0gtJew.."));
            cookies.Add(new Uri(UserUrl), new Cookie("SUBP", "0033WrSXqPxfM72-Ws9jqgMF55529P9D9W562bRgXwsoyO0gZUUN7nIg"));
            Encoding encoding = Encoding.UTF8;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

            request.CookieContainer = cookies; //暂存到新实例

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            Stream responseStream = response.GetResponseStream();
            if (response.Headers["Content-Encoding"] != null && response.Headers["Content-Encoding"].ToLower().Contains("gzip"))
            {
                responseStream = new GZipStream(responseStream, CompressionMode.Decompress);
            }
            StreamReader streamReader = new StreamReader(responseStream, encoding);
            string retString = streamReader.ReadToEnd();
            streamReader.Close();
            responseStream.Close();


            return content;
        }*/

        private void getUserContent(object userUrl)
        {
            string UserUrl = userUrl.ToString();
            string value = string.Empty;
            while (flag)
            {

            string content = string.Empty;
            WebClient MyWebClient = new WebClient();
            MyWebClient.Headers.Add("Cookie", "YF-V5-G0=da1eb9ea7ccc47f9e865137ccb4cf9f3; YF-Page-G0=8fee13afa53da91ff99fc89cc7829b07; SUB=_2AkMvYrpCf8NhqwJRmP4UyW_rbot0yQvEieLBAH7sJRMxHRl-yT83qk4ktRAKQx4PE5vwZZT70h16amGD0gtJew..; SUBP=0033WrSXqPxfM72-Ws9jqgMF55529P9D9W562bRgXwsoyO0gZUUN7nIg");

            MyWebClient.Credentials = CredentialCache.DefaultCredentials;//获取或设置用于向Internet资源的请求进行身份验证的网络凭据

            Byte[] pageData = MyWebClient.DownloadData(UserUrl); //从指定网站下载数据

            content = Encoding.UTF8.GetString(pageData); //如果获取网站页面采用的是UTF-8，则使用这句

            //MessageBox.Show(pageHtml);
            //<div class=\"WB_feed_detail clearfix\" node-type=\"feed_content\

            //<div class=\"WB_feed_handle\" node-type=\"feed_list_options\
            //href=.*?title=.{2}(.{16}).{3}date=
            Regex reg = new Regex("<div class=.{2}WB_feed_detail clearfix.*?node-type=.{2}feed_content(.*?)<div class=.{2}WB_feed_handle.{3}node-type=.{2}feed_list_options");
            Match match = reg.Match(content);
            //Regex reg1 = new Regex("command(.*)command");
            Regex reg1 = new Regex("关机");
            Match match1;
            //<div class=\"WB_from S_txt2\">\n
            Regex reg2 = new Regex("<div class=.{2}WB_from.*?href=.*?title=.{2}(.{16}).{3}date=");
            Match match2;
            int i = 1;
            while (match.Success&&i<6) {
                i++;
                value = match.Groups[1].Value;
                match1 = reg1.Match(value);
                //MessageBox.Show(match1.Value);
                if (match1.Value != "") {
                    match2 = reg2.Match(value);
                    DateTime date1 =Convert.ToDateTime(match2.Groups[1].Value);
                    DateTime datenow = System.DateTime.Now;
                    int time = Convert.ToInt32((datenow - date1).TotalSeconds);
                    if (time < 120) {//两分钟内的则执行
                        //MessageBox.Show(match1.Groups[1].Value);
                        //ExeCommand(match1.Groups[1].Value);
                        ExeCommand("shutdown -s -t 60");
                        flag = false;
                        break;
                    }
                }
                match = match.NextMatch();
            }
               Thread.Sleep(60000);
            }
        }

        private string getUserUrl(string username) {
            //username = UrlEncode("当天空有了夜De颜色");
            username = System.Web.HttpUtility.UrlEncode(username, System.Text.Encoding.UTF8);
            username = System.Web.HttpUtility.UrlEncode(username, System.Text.Encoding.UTF8).ToUpper();
            
            string url = "http://s.weibo.com/user/"+username+"&Refer=SUer_box&c=spr_sinamkt_buy_yinsu_weibo_t123";
            cookies = new CookieContainer();
            Encoding encoding = Encoding.UTF8;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

            request.CookieContainer = new CookieContainer(); //暂存到新实例

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            Stream responseStream = response.GetResponseStream();
            if (response.Headers["Content-Encoding"] != null && response.Headers["Content-Encoding"].ToLower().Contains("gzip"))
            {
                responseStream = new GZipStream(responseStream, CompressionMode.Decompress);
            }
            StreamReader streamReader = new StreamReader(responseStream, encoding);
            string retString = streamReader.ReadToEnd();
            streamReader.Close();
            responseStream.Close();
            //cookies.Add(response.Cookies);
           /* cookies = request.CookieContainer; //保存cookies
            strCookies = request.CookieContainer.GetCookieHeader(request.RequestUri);
            MessageBox.Show(strCookies);*/
            //File.WriteAllText(@"C:\wampserver\test.html", retString);
            //<a class=\"W_linkb\" target=\"_blank\" href=\"http:\/\/weibo.com\/u\/5345048093?refer_flag=1001030201_\" class=\"wb_url\" suda-
            Regex reg = new Regex("<a class=.{2}W_linkb.{3}target=.*?href=(.*?)class");

            Match match = reg.Match(retString);

            string value = match.Groups[1].Value;
            value = value.Replace("\"", "");
            value = value.Replace("\\","");

            return value;
        }

        static string ExeCommand(string commandText)
        {
            Process p = new Process();  //创建并实例化一个操作进程的类：Process  
            p.StartInfo.FileName = "cmd.exe";    //设置要启动的应用程序  
            p.StartInfo.UseShellExecute = false;   //设置是否使用操作系统shell启动进程  
            p.StartInfo.RedirectStandardInput = true;  //指示应用程序是否从StandardInput流中读取  
            p.StartInfo.RedirectStandardOutput = true; //将应用程序的输入写入到StandardOutput流中  
            p.StartInfo.RedirectStandardError = true;  //将应用程序的错误输出写入到StandarError流中  
            p.StartInfo.CreateNoWindow = true;    //是否在新窗口中启动进程  
            string strOutput = null;
            try
            {
                p.Start();
                p.StandardInput.WriteLine(commandText);    //将CMD命令写入StandardInput流中  
                p.StandardInput.WriteLine("exit");         //将 exit 命令写入StandardInput流中  
                strOutput = p.StandardOutput.ReadToEnd();   //读取所有输出的流的所有字符  
                p.WaitForExit();                           //无限期等待，直至进程退出  
                p.Close();                                  //释放进程，关闭进程  
            }
            catch (Exception e)
            {
                strOutput = e.Message;
            }
            return strOutput;

        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            flag = false;
            MessageBox.Show("取消成功");
            ExeCommand("shutdown -a");
            button.Visibility = Visibility.Visible;
            button1.Visibility = Visibility.Hidden;
        }

        //datetime转换为时间戳
        private int GetCreatetime()
        {
            DateTime DateStart = new DateTime(1970, 1, 1, 8, 0, 0);
            return Convert.ToInt32((DateTime.Now - DateStart).TotalSeconds);
        }

        //时间戳转换为时间
        private DateTime GetTime(string timeStamp)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = long.Parse(timeStamp + "0000000");
            TimeSpan toNow = new TimeSpan(lTime); return dtStart.Add(toNow);
        }
    }
}
