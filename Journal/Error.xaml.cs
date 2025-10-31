using System.Windows;

namespace Journal
{
    /// <summary>
    /// Логика взаимодействия для Error.xaml
    /// </summary>
    public partial class Error : Window
    {
        public Error(string text)
        {
            InitializeComponent();
            message.Text = text;
        }
    }
}