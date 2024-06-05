using System.Windows;
using WpfApp2.Data;
using WpfApp2.ViewModels;
using WpfApp2.Model;

namespace WpfApp2.View
{
    public partial class AddSubTask : Window
    {
        private TasksViewModel _taskViewModel;
        private SubTasksViewModel _subTasksViewModel;
        private AuditLogsViewModel _auditLogsViewModel;
        private ApplicationDbContext _context;

        public AddSubTask(ApplicationDbContext context)
        {
            InitializeComponent();

            _context = context;
            _taskViewModel = new TasksViewModel(_context);
            _subTasksViewModel = new SubTasksViewModel(_context);
            _auditLogsViewModel = new AuditLogsViewModel(_context);

            TaskComboBox.ItemsSource = _taskViewModel.GetAllTasks();
            TaskComboBox.DisplayMemberPath = "Description";
        }

        private void AddSubTaskButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var selectedTask = (WpfApp2.Model.Tasks)TaskComboBox.SelectedItem;
                var description = DescriptionTextBox.Text;

                var newSubTask = new Subtasks
                {
                    TaskId = selectedTask.TaskId,
                    Description = description,
                    Completed = false,
                };

                _subTasksViewModel.AddSubTask(newSubTask);

                _taskViewModel.UpdateTask(selectedTask);
                _auditLogsViewModel.AddAuditLog("Insert", "Task", newSubTask.Id);

                var mainWindow = Application.Current.MainWindow as MainWindow;
                if (mainWindow != null)
                {
                    var viewModel = (TasksViewModel)mainWindow.DataContext;
                    viewModel.Tasks = _taskViewModel.GetAllTasks();
                    viewModel.SubTasks = _subTasksViewModel.GetAllSubTasks();
                    viewModel.LoadData(selectedTask);
                }
                this.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while adding a subtask: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }
            }
        }
    
    
    }
}