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
            string url = "http://s.weibo.com/user/%25E5%25BD%2593%25E5%25A4%25A9%25E7%25A9%25BA%25E6%259C%2589%25E4%25BA%2586%25E5%25A4%259CDe%25E9%25A2%259C&Refer=SUer_box&c=spr_sinamkt_buy_yinsu_weibo_t123";
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
            MessageBox.Show(retString);

        }
    }
}
