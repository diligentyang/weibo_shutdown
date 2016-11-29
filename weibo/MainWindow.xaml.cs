using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
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
            UserUrl = getUserUrl(username);

        }

        private string getUserUrl(string username) {
            //username = UrlEncode("当天空有了夜De颜色");
            username = System.Web.HttpUtility.UrlEncode(username, System.Text.Encoding.UTF8);
            username = System.Web.HttpUtility.UrlEncode(username, System.Text.Encoding.UTF8).ToUpper();
            string UserUrl = string.Empty;
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
            File.WriteAllText(@"C:\wampserver\test.html", retString);

            return UserUrl;
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
    }
}
