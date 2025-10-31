using System.Windows.Controls;

namespace Journal
{
    internal class AddTask
    {
        DataGrid DataGrid { get; set; } = new DataGrid();
        public AddTask(DataGrid dataGrid) => DataGrid = dataGrid;
        public void Add(object sender, EventArgs e)
        {
            try
            {
                Lesson lesson = (Lesson)this.DataGrid.SelectedItem;
                Task task = new Task(lesson.GetId());
                task.Show();
            }
            catch (NullReferenceException) { }
        }
    }
}