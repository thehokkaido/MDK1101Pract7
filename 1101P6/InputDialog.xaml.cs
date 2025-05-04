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

namespace _1101P6
{
    public partial class InputDialog : Window
    {
        public string LabelText { get; set; }
        public string UserInput { get; private set; }

        public InputDialog(string labelText, string defaultValue = "")
        {
            InitializeComponent();
            DataContext = this;
            LabelText = labelText;
            InputTextBox.Text = defaultValue;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            UserInput = InputTextBox.Text;
            DialogResult = true;
            Close();
        }
    }
}
