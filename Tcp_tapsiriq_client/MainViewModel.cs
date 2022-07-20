
using GalaSoft.MvvmLight.Command;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Tcp_tapsiriq_client
{

    public class MainViewModel : INotifyPropertyChanged
    {
        private List<string> list_combo;

        public List<string> List_Combo
        {
            get { return list_combo; }
            set { list_combo = value; OnPropertyChanged(); }
        }

        private string textbox_text;

        public string Textbox_text
        {
            get { return textbox_text; }
            set { textbox_text = value; OnPropertyChanged(); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public ObservableCollection<string> processes { get; set; }

        private string selected_combobox;

        public string Selected_combobox
        {
            get { return selected_combobox; }
            set { selected_combobox = value; OnPropertyChanged(); }
        }

        public RelayCommand Run
        {
            get => new RelayCommand(() =>
            {

                Task.Run(() =>
                {

                    if (Selected_combobox == "List")
                    {
                        var client = new TcpClient();
                        var ip = IPAddress.Parse("10.2.27.46");
                        var ep = new IPEndPoint(ip, 61932);

                        try
                        {
                            client.Connect(ep);
                            if (client.Connected)
                            {
                                while (true)
                                {

                                    var stream = client.GetStream();
                                    //var bw = new BinaryWriter(stream);



                                    //foreach (var process in processes)
                                    //{
                                    //    bw.Write(process.Id);
                                    //    bw.Write(process.ProcessName.ToString());
                                    //}
                                    var bw = new BinaryWriter(stream);
                                    var json = JsonConvert.SerializeObject(processes, Newtonsoft.Json.Formatting.Indented);
                                    bw.Write(json);


                                }
                            }

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                    else if (Selected_combobox == "kill")
                    {
                        var client = new TcpClient();
                        var ip = IPAddress.Parse("10.2.27.46");
                        var ep = new IPEndPoint(ip, 61932);

                        try
                        {
                            client.Connect(ep);
                            if (client.Connected)
                            {
                                while (true)
                                {

                                    var stream = client.GetStream();
                                    try
                                    {
                                       
                                        var lst = Process.GetProcessesByName(Textbox_text);
                                        foreach (var proc in lst)
                                        {
                                            proc.Kill();
                                        }
                                        processes.Clear();
                                        foreach (var process in Process.GetProcesses())
                                        {
                                            processes.Add(process.ProcessName);

                                        }
                                    }
                                    catch (Exception)
                                    {

                                        throw;
                                    }

                                    var bw = new BinaryWriter(stream);
                                    var json = JsonConvert.SerializeObject(processes, Newtonsoft.Json.Formatting.Indented);
                                    bw.Write(json);


                                }
                            }

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                    else if (Selected_combobox == "Create")
                    {
                        var client = new TcpClient();
                        var ip = IPAddress.Parse("10.2.27.46");
                        var ep = new IPEndPoint(ip, 61932);

                        try
                        {
                            client.Connect(ep);
                            if (client.Connected)
                            {
                                while (true)
                                {

                                    var stream = client.GetStream();
                                    try
                                    {
                                        Process.Start(Textbox_text);
                                  
                                       
                                        processes.Clear();
                                        foreach (var process in Process.GetProcesses())
                                        {
                                            processes.Add(process.ProcessName);

                                        }
                                    }
                                    catch (Exception)
                                    {

                                        throw;
                                    }

                                    var bw = new BinaryWriter(stream);
                                    var json = JsonConvert.SerializeObject(processes, Newtonsoft.Json.Formatting.Indented);
                                    bw.Write(json);


                                }
                            }

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                });
            });
        }
        public MainViewModel()
        {

            processes = new ObservableCollection<string>();
            foreach (var process in Process.GetProcesses())
            {
                processes.Add(process.ProcessName);

            }
            List_Combo = new List<string>();
            List_Combo.Add("List");
            List_Combo.Add("Kill");
            List_Combo.Add("Create");
            List_Combo.Add("help");


        }
    }
}
