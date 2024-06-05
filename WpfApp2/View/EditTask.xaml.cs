using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using WpfApp2.Data;
using WpfApp2.Model;
using WpfApp2.ViewModels;

namespace WpfApp2.View
{
    public partial class EditTask : Window
    {
        private Tasks _task;
        private ApplicationDbContext _context;
        private TasksViewModel _tasksViewModel;
        private SubTasksViewModel _subTaskViewModel;
        private TextBox _lastFocusedTextBox;
        private MainViewModel _mainViewModel;


        public EditTask(Tasks task, ApplicationDbContext context, TasksViewModel tasksViewModel,
            SubTasksViewModel subTaskViewModel)
        {
            InitializeComponent();

            _task = task;
            _context = context;
            _tasksViewModel = tasksViewModel;
            _subTaskViewModel = subTaskViewModel;

            this.DataContext = _task;

            txtCategory.ItemsSource = _context.Category.ToList();
            txtCategory.SelectedItem = _task.Category;

            var availablePriorities = new ObservableCollection<string>(new List<string> { "High", "Medium", "Low" });
            if (!availablePriorities.Contains(_task.Priority.Name))
            {
                availablePriorities.Add(_task.Priority.Name);
            }
            priority.ItemsSource = availablePriorities;
            priority.SelectedItem = _task.Priority.Name;
        }
        
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedPriorityName = (string)priority.SelectedItem;
            var selectedPriority = _context.Priority.FirstOrDefault(p => p.Name == selectedPriorityName);

            if (selectedPriority == null)
            {
                selectedPriority = new Priority { Name = selectedPriorityName };
                _context.Priority.Add(selectedPriority);
                _context.SaveChanges();
            }

            _task.Priority = selectedPriority;

            _tasksViewModel.UpdateTask(_task);

            if (_lastFocusedTextBox != null)
            {
                var subtask = (Subtasks)_lastFocusedTextBox.DataContext;
                _subTaskViewModel.UpdateSubTask(subtask);
            }

            var mainWindow = Application.Current.MainWindow as MainWindow;
            if (mainWindow != null)
            {
                var viewModel = (TasksViewModel)mainWindow.DataContext;
                viewModel.Tasks = viewModel.GetAllTasks();
                viewModel.SubTasks = _subTaskViewModel.GetAllSubTasks();
            }

            this.Close();
        }
        
        
       
        
    }
}