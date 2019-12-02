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
    /// Interaction logic for NewCaseMethod.xaml
    /// </summary>
    public partial class NewCaseMethodControl : Window
    {
        NewCaseArgs newArgs;
        public NewCaseMethodControl(StringArgs args)
        {
            InitializeComponent();
            newArgs = args as NewCaseArgs;
            NewCaseStyleCombobox.SelectedIndex = newArgs.style - 1;
           
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int style = NewCaseStyleCombobox.SelectedIndex + 1;
            newArgs.style = style;
            DialogResult = true;
            Close();
        }
    }
}
