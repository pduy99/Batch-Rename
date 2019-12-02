using System;
using System.Collections.Generic;
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

namespace Main_Windows
{
    /// <summary>
    /// Interaction logic for PresetControl.xaml
    /// </summary>
    public partial class PresetControl : Window
    {
        public static string presetName;
        public PresetControl()
        {
            InitializeComponent();
            
        }

        private void btnOk(object sender, RoutedEventArgs e)
        {
            presetName = txtName.Text;
            DialogResult = true;
            Close();
        }
    }
}
