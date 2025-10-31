using System.Windows;

namespace Journal
{
    /// <summary>
    /// Логика взаимодействия для Work.xaml
    /// </summary>
    public partial class Work : Window
    {
        public Work(Homework homework)
        {
            InitializeComponent();
            complWork.Text = homework.GetCont();
        }
    }
}