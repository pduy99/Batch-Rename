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
    /// Interaction logic for MoveMethodControl.xaml
    /// </summary>
    public partial class MoveMethodControl : Window
    {
        MoveArgs newArgs;
        public MoveMethodControl(StringArgs args)
        {
            InitializeComponent();
            newArgs = args as MoveArgs;
            int style = (newArgs.FrontToEnd == true) ? style = 0 : style = 1;
            StyleCombobox.SelectedIndex = style;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(StyleCombobox.SelectedIndex == 0)
            {
                newArgs.FrontToEnd = true;
            }
            else
            {
                newArgs.FrontToEnd = false;
            }

            DialogResult = true;
            Close();
            
        }
    }
}
