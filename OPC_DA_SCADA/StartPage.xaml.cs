using OPC_DA_Test;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
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
    /// Interaction logic for StartPage.xaml
    /// </summary>
    /// 
    public partial class StartPage : Page
    {
        string _checkProp;
        public delegate void ChangedItems(KeyValuePair<string, string> itemDict);
        public event ChangedItems ItemsChanged;
        Dictionary<string, string> _opcItemDict;
        public Dictionary<string, string> opcItemDict
        {   get { return _opcItemDict; }
            set
            {
                _opcItemDict = value;
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    SetOPCElements(_opcItemDict);
                }));
                
            }
        }
        OrderedDictionary _itemDict;
        public OrderedDictionary ItemDict
        {
            get { return _itemDict; }
            set
            {
                _itemDict = value;
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    SetElements(_itemDict);
                }));

            }
        }

        private void SetOPCElements(Dictionary<string, string> opcItemDict)
        {
            oPCItemsList.ItemsSource = opcItemDict;
            foreach (KeyValuePair<string, string> item in opcItemDict)
            {
                SetValueToTextBox(textBox, "PLC1:.rSetPressure", item);
            }
        }
        private void SetElements(OrderedDictionary itemDict)
        {
            string[] vs = new string[itemDict.Count];
            IDictionaryEnumerator myEnumerator = itemDict.GetEnumerator();
            String[] myKeys = new String[itemDict.Count];
            String[] myValues = new String[itemDict.Count];
            ICollection keyCollection = itemDict.Keys;
            ICollection valueCollection = itemDict.Values;
            keyCollection.CopyTo(myKeys, 0);
            valueCollection.CopyTo(myValues, 0);
            for (int i = 0; i < itemDict.Count; i++)
            {
                vs[i] = myKeys[i] + "," + myValues[i];
            }
                //while (myEnumerator.MoveNext())
                //{
                //    int i = 0;
                //    vs[i] = myEnumerator.Key.ToString() +"," + myEnumerator.Value.ToString();
                //}
                //for (int i = 0; i < itemDict.Count; i++)
                //{
                //    vs[i, i] = itemDict[i];
                //}
                dataFromFile.ItemsSource = vs;
        }

        public StartPage()
        {
            InitializeComponent();

            //oPC_DA_Client.ItemsChanged += OPC_DA_Client_ItemsChanged;
            textBox.LostKeyboardFocus += TextBox_LostKeyboardFocus;
        }

        private void TextBox_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            KeyValuePair<string, string> item = new KeyValuePair<string, string>("PLC1:.rSetPressure", textBox.Text);          
            ItemsChanged?.Invoke(item);
        }

        private void SetValueToTextBox(System.Windows.Controls.TextBox textBox, string key, KeyValuePair<string, string> keyValuePair)
        {
            if (keyValuePair.Key == key)
            {
                textBox.Text = keyValuePair.Value;
            }
        }
    }
}
