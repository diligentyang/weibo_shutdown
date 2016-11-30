using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
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
        public MainWindow()
        {
            InitializeComponent();

        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            string username = textBox.Text;
            string UserUrl = string.Empty;
            string content = string.Empty;
            UserUrl = getUserUrl(username);
            //MessageBox.Show(UserUrl);
            //ExeCommand("start "+UserUrl);
            content = getUserContent(UserUrl);
            File.WriteAllText(@"C:\wampserver\test.html", content);
            MessageBox.Show("OK");

        }

        private string getUserContent(string UserUrl) {
            string content = string.Empty;
            string url = UserUrl;
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


            return content;
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

        /*public static string UrlEncode(string str)
        {
            StringBuilder sb = new StringBuilder();
            byte[] byStr = System.Text.Encoding.UTF8.GetBytes(str); //默认是System.Text.Encoding.Default.GetBytes(str)
            for (int i = 0; i < byStr.Length; i++)
            {
                sb.Append(@"%" + Convert.ToString(byStr[i], 16));
            }

            return (sb.ToString());
        }*/

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
    }
}
