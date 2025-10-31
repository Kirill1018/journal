using System.Windows;

namespace Journal
{
    /// <summary>
    /// Логика взаимодействия для Target.xaml
    /// </summary>
    public partial class Target : Window
    {
        public Target(Homework homework)
        {
            InitializeComponent();
            purpose.Text = homework.GetTask();
        }
    }
}