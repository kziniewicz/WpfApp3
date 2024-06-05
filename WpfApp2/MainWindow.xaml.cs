using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using WpfApp2.Data;
using WpfApp2.Model;
using WpfApp2.View;
using WpfApp2.ViewModels;

namespace WpfApp2
{
    public partial class MainWindow : Window
    {
        private IServiceProvider serviceProvider;
        public string selectedSearchType;

        public MainWindow()
        {
            InitializeComponent();

            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = configuration.GetConnectionString("Database");

            serviceProvider = new ServiceCollection()
                .AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connectionString))
                .BuildServiceProvider();

            var context = serviceProvider.GetService<ApplicationDbContext>();
            var viewModel = new TasksViewModel(context);
            DataContext = viewModel;

        }

        private void AddNewTask_Click(object sender, RoutedEventArgs e)
        {
            var context = serviceProvider.GetService<ApplicationDbContext>();
            var addTaskWindow = new AddTask(context);
            if (addTaskWindow.ShowDialog() == true)
            {
                var viewModel = (TasksViewModel)DataContext;
                viewModel.Tasks = viewModel.GetAllTasks();
            }
        }

        private void SortTasks_Click(object sender, RoutedEventArgs e)
        {
            var sortTasksWindow = new SortTasks();
            if (sortTasksWindow.ShowDialog() == true)
            {
                var sortCriteria = sortTasksWindow.sortCriteria;
                var viewModel = (TasksViewModel)DataContext;
                var tasks = viewModel.SortTasks(sortCriteria)?.ToList();
                TasksDataGrid.ItemsSource = tasks;
            }
        }


        private void ManageCategories_Click(object sender, RoutedEventArgs e)
        {
            var context = serviceProvider.GetService<ApplicationDbContext>();
            var categorySettingsWindow = new CategorySettings(context);
            categorySettingsWindow.Show();
        }

        private void AuditLogs_Click(object sender, RoutedEventArgs e)
        {
            var context = serviceProvider.GetService<ApplicationDbContext>();
            var auditLogs = new ObservableCollection<AuditLog>(context.AuditLogs.ToList());
            var auditLogsWindow = new AuditLogsView(auditLogs);
            auditLogsWindow.Show();
        }

        private void AddSubtask_Click(object sender, RoutedEventArgs e)
        {
            var context = serviceProvider.GetService<ApplicationDbContext>();
            var addSubTask = new AddSubTask(context);
            addSubTask.Show();
        }


        private void TaskDetails_Click(object sender, MouseButtonEventArgs e)
        {
            var textBlock = (TextBlock)sender;
            var task = (Tasks)textBlock.DataContext;
            var taskDetailsWindow = new TaskDetails(task);
            taskDetailsWindow.Show();
        }


        private void DeleteTask_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var task = (Tasks)button.DataContext;

            var deleteTaskWindow = new DeleteTask();
            if (deleteTaskWindow.ShowDialog() == true)
            {
                var viewModel = (TasksViewModel)DataContext;
                viewModel.DeleteTask(task);
                viewModel.Tasks = viewModel.GetAllTasks();
            }
        }
        
        private void EditTaskButton_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var task = (Tasks)button.DataContext;
            
            var context = serviceProvider.GetService<ApplicationDbContext>();
            var viewModel = (TasksViewModel)DataContext;
            var subTaskViewModel = new SubTasksViewModel(context);
            var editTaskWindow = new EditTask(task, context, viewModel, subTaskViewModel); 
            editTaskWindow.Show();
        }

        public void TaskCompleted_Checked(object sender, RoutedEventArgs e)
        {
            var checkBox = sender as CheckBox;
            if (checkBox != null)
            {
                var task = checkBox.DataContext as Tasks;
                if (task != null)
                {
                    task.Completed = checkBox.IsChecked ?? false;
                    var viewModel = (TasksViewModel)DataContext;
                    viewModel.UpdateTask(task);
                }
            }
        }

        private void ShowAllTasks_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = (TasksViewModel)DataContext;
            TasksDataGrid.ItemsSource = viewModel.GetAllTasks();
        }

        private void ShowCompletedTasks_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = (TasksViewModel)DataContext;
            TasksDataGrid.ItemsSource = viewModel.GetCompletedTasks();
        }

        private void ShowUncompletedTasks_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = (TasksViewModel)DataContext;
            TasksDataGrid.ItemsSource = viewModel.GetUncompletedTasks();
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            var viewModel = (TasksViewModel)DataContext;
            var textBox = sender as TextBox;
            if (textBox != null)
            {
                TasksDataGrid.ItemsSource = viewModel.SearchTasks(textBox.Text);
            }
        }

        private void Priority_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            var viewModel = (TasksViewModel)DataContext;

            if (comboBox != null && comboBox.SelectedItem != null)
            {
                var selectedItem = comboBox.SelectedItem as string;

                TasksDataGrid.ItemsSource = viewModel.SearchByPriorites(selectedItem);
            }
        }

        private void Category_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            var viewModel = (TasksViewModel)DataContext;

            if (comboBox != null && comboBox.SelectedItem != null)
            {
                var selectedItem = comboBox.SelectedItem as string;

                TasksDataGrid.ItemsSource = viewModel.SearchByCategories(selectedItem);
            }
        }

        private void TypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var context = serviceProvider.GetService<ApplicationDbContext>();

            if (TypeComboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                selectedSearchType = selectedItem?.Content?.ToString();
                switch (selectedSearchType)
                {
                    case "Name":
                        InputControl.Content = new ContentControl
                        {
                            ContentTemplate = this.Resources["NameTemplate"] as DataTemplate,
                            DataContext = string.Empty
                        };
                        break;
                    case "Priority":
                        InputControl.Content = new ContentControl
                        {
                            ContentTemplate = this.Resources["PriorityTemplate"] as DataTemplate,
                            DataContext = string.Empty
                        };
                        break;
                    case "Category":
                        InputControl.Content = new ContentControl
                        {
                            ContentTemplate = this.Resources["CategoryTemplate"] as DataTemplate,
                            DataContext = string.Empty
                        };
                        break;
                }
            }
        }
    }
}