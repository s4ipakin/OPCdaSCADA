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
                    RunGraf(_itemDict);
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
                LineSmoothness = 0, //0: straight lines, 1: really smooth lines
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
            if (pointsonScreen > 10)
            {
                pointsonScreen--;
                SeriesCollection[0].Values.RemoveAt(0);
                queue.Dequeue();
            }
            Labels = queue.ToArray();
            
        }

    }
}
