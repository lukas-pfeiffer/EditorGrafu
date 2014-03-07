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
    /// Interaction logic for OknoNapoveda.xaml
    /// </summary>
    public partial class OknoNapoveda : Window
    {
        public OknoNapoveda()
        {
            InitializeComponent();
            nacteniNapovedy();
        }

        private void nacteniNapovedy()
        {
            string s;

            using (StreamReader sr = new StreamReader(@"napoveda.txt", Encoding.Default))
            {
                s = sr.ReadToEnd();
                textBlock1.Text = s;
            }
        }
    }
}
