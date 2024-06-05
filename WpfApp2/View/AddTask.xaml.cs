using System.Windows;
using WpfApp2.Data;
using WpfApp2.Model;
using WpfApp2.ViewModels;

namespace WpfApp2.View
{
    public partial class AddTask : Window
    {
        private MainViewModel _mainViewModel;
        private TasksViewModel _tasksViewModel;
        private AuditLogsViewModel _auditLogsViewModel;

        public AddTask(ApplicationDbContext context)
        {
            InitializeComponent();
            _mainViewModel = new MainViewModel(context);
            _tasksViewModel = new TasksViewModel(context);
            _auditLogsViewModel = new AuditLogsViewModel(context);
            this.DataContext = _mainViewModel;
        }

        private void AddTaskButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var newTask = new Tasks
                {
                    Description = txtTaskName.Text,
                    Category = (Category)cmbCategory.SelectedItem,
                    Priority = (Priority)priority.SelectedItem,
                    StartDate = startDate.SelectedDate.HasValue ? startDate.SelectedDate.Value.ToUniversalTime() : (DateTime?)null,
                    EndDate = endDate.SelectedDate.HasValue ? endDate.SelectedDate.Value.ToUniversalTime() : (DateTime?)null,
                    SubTasks = new List<Subtasks>() 

                };

                if (string.IsNullOrEmpty(newTask.Description) || newTask.Category == null || newTask.Priority == null)
                {
                    MessageBox.Show("Please select a category, priority and enter a description of the task.");
                    return;
                }

                _tasksViewModel.AddTask(newTask);
                _auditLogsViewModel.AddAuditLog("Insert", "Task", newTask.TaskId);
                this.IsVisibleChanged += AddTask_IsVisibleChanged;

                this.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException.Message);
            }
        }
        
        private void AddTask_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (!this.IsVisible)
            {
                var mainWindow = Application.Current.MainWindow as MainWindow;
                if (mainWindow != null)
                {
                    var viewModel = (TasksViewModel)mainWindow.DataContext;
                    viewModel.Tasks = viewModel.GetAllTasks();
                }
            }
        }
        
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        
        }
    }
