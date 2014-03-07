using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Shapes;

namespace KombinatorickeAlgoritmy
{
    /// <summary>
    /// Interaction logic for OknoMatice.xaml
    /// </summary>
    public partial class OknoMatice : Window
    {
        public OknoMatice()
        {
            InitializeComponent();
            nacteniMatice();
        }

        private void nacteniMatice()
        {
            string s;

            using (StreamReader sr = new StreamReader(@"matice.txt", Encoding.Default))
            {
                s = sr.ReadToEnd();
                textBlock1.Text = s;
            }
        }
    }
}
