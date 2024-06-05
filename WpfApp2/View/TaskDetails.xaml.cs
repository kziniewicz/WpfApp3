using System.Windows;
using WpfApp2.Model;

namespace WpfApp2.View
{
    public partial class TaskDetails : Window
    {
        public TaskDetails(Tasks task)
        {
            InitializeComponent();
            DataContext = task;
        }
    }
}