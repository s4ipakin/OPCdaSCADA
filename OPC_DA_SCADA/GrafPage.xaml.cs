using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
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

namespace OPC_DA_SCADA
{
    /// <summary>
    /// Interaction logic for GrafPage.xaml
    /// </summary>
    public partial class GrafPage : Page, INotifyPropertyChanged
    {
        OrderedDictionary _itemDict;
        double additionalpoint = 2d;
        string[] _labels;
        List<string> labelList = new List<string>();
        string[] appended = new string[] { "" };
        Queue<string> queue = new Queue<string>();
        int pointsonScreen;
        bool read;
        public event PropertyChangedEventHandler PropertyChanged;
        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels
        {
            get { return _labels; }
            set
            {
                _labels = value;
                OnPropertyChanged("Labels");
            }
        }
        public Func<double, string> YFormatter { get; set; }
        public OrderedDictionary ItemDict
        {
            get { return _itemDict; }
            set
            {
                _itemDict = value;
                Dispatcher.BeginInvoke(new Action(() =>
                {   
                    if (read)
                    {
                        RunGraf(_itemDict);
                    }
                    else
                    {
                        
                    }
                    
                }));
            }
        }


        public GrafPage()
        {
            InitializeComponent();
            SeriesCollection = new SeriesCollection();
            Labels = new[] { System.DateTime.Now.ToString() };
            YFormatter = value => value.ToString("C");

            SeriesCollection.Add(new LineSeries
            {
                Title = "Series 4",
                Values = new ChartValues<double> { 0/*, 3, 2, 4*/ },
                LineSmoothness = 0.8, //0: straight lines, 1: really smooth lines
                PointGeometry = Geometry.Parse("m 25 70.36218 20 -28 -20 22 -8 -6 z"),
                PointGeometrySize = 5,
                PointForeground = Brushes.Gray
            });

            DataContext = this;
        }

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            if (PropertyChanged != null) // if subrscribed to event
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void RunGraf(OrderedDictionary itemDict)
        {
            
            pointsonScreen++;
            IDictionaryEnumerator myEnumerator = itemDict.GetEnumerator();
            double newValue = 5d;
            string newTime = "";
            while (myEnumerator.MoveNext())
            {
                newValue = Convert.ToDouble(myEnumerator.Value);
                newTime = myEnumerator.Key.ToString();
            }
            SeriesCollection[0].Values.Add(newValue);
             
            textBox.Text = pointsonScreen.ToString();
            queue.Enqueue(newTime);
            if (pointsonScreen > 60)
            {
                pointsonScreen--;
                SeriesCollection[0].Values.RemoveAt(0);
                queue.Dequeue();
            }
            Labels = queue.ToArray();           
        }

        private void ShowDays(System.DateTime dateTime, OrderedDictionary itemDict, int hoursStart = 0, int minStart = 0, int hoursEnd = 23, int minEnd = 59)
        {
            try
            {
                queue.Clear();
                SeriesCollection[0].Values.Clear();
                Dictionary<string, string> entries = new Dictionary<string, string>();
                
                System.DateTime stopTime = dateTime;
                stopTime = dateTime.AddHours(hoursEnd);
                stopTime = stopTime.AddMinutes(minEnd);
                dateTime = dateTime.AddHours(hoursStart);
                dateTime = dateTime.AddMinutes(minStart);
                //stopTime.AddMinutes(minEnd - minStart);
                ICollection keyCOllection = itemDict.Keys;
                ICollection valueCollection = itemDict.Values;
                String[] myKeys = new String[itemDict.Count];
                String[] myValues = new String[itemDict.Count];
                keyCOllection.CopyTo(myKeys, 0);
                valueCollection.CopyTo(myValues, 0);
                for (int k = 0; k < itemDict.Count; k++)
                {
                    entries.Add(myKeys[k], myValues[k]);
                }
                int increment =(int)Math.Ceiling(((stopTime - dateTime).TotalSeconds)/720);
                int i = 0;                
                while (dateTime < stopTime)
                {                    
                    queue.Enqueue(dateTime.ToString());
                    if (entries.ContainsKey(dateTime.ToString()))
                    {                       
                        SeriesCollection[0].Values.Add(Convert.ToDouble(entries[dateTime.ToString()]));
                    }
                    else
                    {
                        SeriesCollection[0].Values.Add(0d);
                    }
                    dateTime = dateTime.AddSeconds(increment);
                    i++;
                }
                Labels = queue.ToArray();
                textBox.Text = Convert.ToString(increment);
            }
            catch(Exception ex) { }
        }

        private void btrStart_Click(object sender, RoutedEventArgs e)
        {
            
            if (read)
            {
                read = false;               
            }
                
            else
            {
                queue.Clear();
                SeriesCollection[0].Values.Clear();
                //SeriesCollection[0].Values.Add(0d);
                pointsonScreen = 0;
                read = true;
            }
                               
        }

        private void btnFromDays_Click(object sender, RoutedEventArgs e)
        {
            read = false;
            ShowDays((System.DateTime)pickedData.SelectedDate, _itemDict, Convert.ToInt32(txtbxStartHour.Text), Convert.ToInt32(txtbxStartMin.Text), Convert.ToInt32(txtbxEndHour.Text), Convert.ToInt32(txtbxEndMin.Text));
        }
    }
}
