using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using OPC_DA_Test;
using System.Collections.Specialized;

namespace OPC_DA_SCADA
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        StartPage startPage;
        GrafPage grafPage;
        OPC_DA_Client oPC_DA_Client = new OPC_DA_Client("CoDeSys.OPC");
        List<string> itemList = new List<string>();
        private delegate void SetUIelements(Dictionary<string, string> itemDict);
        

        public MainWindow()
        {
            InitializeComponent();
            startPage = new StartPage();
            grafPage = new GrafPage();
            Main.Content = startPage;            
            ListOfItemsOPC listOfItemsOPC = new ListOfItemsOPC();
            oPC_DA_Client.Connect();
            oPC_DA_Client.CreateGroup(listOfItemsOPC.GetOPCitems());
            oPC_DA_Client.ItemsChanged += OPC_DA_Client_ItemsChanged;
            this.Closing += MainWindow_Closing;
            startPage.ItemsChanged += StartPage_ItemsChanged;
            Timer myTimer = new Timer();
            myTimer.Elapsed += new ElapsedEventHandler(DisplayTimeEvent);
            myTimer.Interval = 1000; // 1000 ms is one second
            myTimer.Start();
        }

        private void StartPage_ItemsChanged(KeyValuePair<string, string> itemDict)
        {
            oPC_DA_Client.WriteValue(itemDict.Key, itemDict.Value);
            
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            oPC_DA_Client.Disconnect();
        }

        private void MainBtcClck(object sender, RoutedEventArgs e)
        {
            Main.Content = startPage;
        }

        private void GrafBtnClck(object sender, RoutedEventArgs e)
        {
            Main.Content = grafPage;
        }

        private void OPC_DA_Client_ItemsChanged(Dictionary<string, string> itemDict)
        {
            startPage.opcItemDict = itemDict;
        }

        public void DisplayTimeEvent(object source, ElapsedEventArgs e)
        {
            //StartPage startPage = 
            Dictionary<string, string> itemDict = startPage.opcItemDict;
            startPage.ItemDict = WriteOPCToCSV("PLC1:.rPA_WorkTime_Acc", itemDict, @"D:\Chart\Chart1.csv");
            grafPage.ItemDict = startPage.ItemDict;
        }

        private OrderedDictionary WriteOPCToCSV(string key, Dictionary<string, string> itemDict, string filePath)
        {
            ReadWriteCSV readWriteCSV = new ReadWriteCSV(filePath);
            try
            {
                readWriteCSV.WriteToCSV(System.DateTime.Now.ToString(), itemDict[key]);
            }
            catch(Exception ex) { }
            OrderedDictionary readItems = new OrderedDictionary();
            readItems = readWriteCSV.ReadFromCSV();
            return readItems;
        }

        private void WriteToOPC(KeyValuePair<string, string> item)
        {
            oPC_DA_Client.WriteValue(item.Key, item.Value);
        }
    }
}
