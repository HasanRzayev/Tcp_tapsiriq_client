using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
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
using System.Windows.Threading;


namespace Tcp_server_TAPSIRIQ
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
      
      
            static TcpListener listener = null;
            static
            public BinaryReader br
            { get; set; }
            public ObservableCollection<string> processes { get; set; }

            private void StartServer()
            {
                var ip = IPAddress.Parse("10.2.27.46");
                var ep = new IPEndPoint(ip, 61932);
                listener = new TcpListener(ep);
                listener.Start();
                var client = listener.AcceptTcpClient();



                Task.Run(() =>
                {

                    try
                    {

                        var json_name = "";
                        var stream = client.GetStream();
                        br = new BinaryReader(stream);
                        json_name = br.ReadString();
                        var lazimli = JsonConvert.DeserializeObject<ObservableCollection<string>>(json_name);
                        foreach (var pr in lazimli)
                        {
                            dispatcher.Invoke(() => processes.Add(pr.ToString()));
                        }

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                });
            }
            private Dispatcher dispatcher = Dispatcher.CurrentDispatcher;

            public MainWindow()
            {

                InitializeComponent();
                processes = new ObservableCollection<string>();
                Thread newThread = new Thread(new ThreadStart(StartServer));
                newThread.Start();
                DataContext = this;
            }
        }
    
}
