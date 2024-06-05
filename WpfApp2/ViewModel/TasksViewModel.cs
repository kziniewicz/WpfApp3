using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using WpfApp2.Model;
using WpfApp2.Data;
using WpfApp2.View;

namespace WpfApp2.ViewModels
{
    public class TasksViewModel : INotifyPropertyChanged
    {
        private readonly ApplicationDbContext _context;
        private List<Tasks> _tasks;
        public ObservableCollection<string> AvailablePriorities { get; set; }
        public ObservableCollection<string> AvailableCategories { get; set; }
        
        public ICollection<Subtasks> SubTasks { get; set; } = new List<Subtasks>();


        public List<Tasks> Tasks
        {
            get { return _tasks; }
            set { SetField(ref _tasks, value); }
        }

        public TasksViewModel(ApplicationDbContext context)
        {
            _context = context;
            CheckDatabaseConnection();
            Tasks = GetAllTasks();
            LoadData();
        }

        public void LoadData(Tasks selectedTask = null)
        {
            AvailablePriorities = new ObservableCollection<string>(new List<string> { "High", "Medium", "Low" });

            if (selectedTask != null && !AvailablePriorities.Contains(selectedTask.Priority.Name))
            {
                AvailablePriorities.Add(selectedTask.Priority.Name);
            }

            AvailableCategories = new ObservableCollection<string>(_context.Category.ToList().Select(x => x.Name));
        }

        public List<Tasks> GetAllTasks()
        {
            try
            {
                var tasks = _context.Tasks.Include(t => t.Category).Include(t => t.Priority).Include(t => t.SubTasks).ToList();
                return tasks;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while retrieving tasks: {ex.Message}");
                return new List<Tasks>();
            }
        }
        
        public List<Tasks> GetCompletedTasks()
        {
            return _context.Tasks.Where(t => t.Completed).ToList();
        }
        
        public List<Tasks> GetUncompletedTasks()
        {
            return _context.Tasks.Where(t => !t.Completed).ToList();
        }

        public void AddTask(Tasks newTask)
        {
            try
            {
                _context.Tasks.Add(newTask);
                _context.SaveChanges();
                

            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateException ex)
            {
                Console.WriteLine($"An error occurred while adding a new task: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }
        }
        public void UpdateTask(Tasks taskToUpdate)
        {
            try
            {
                _context.Tasks.Update(taskToUpdate);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while updating a task: {ex.Message}");
            }
        }
       

        public void DeleteTask(Tasks taskToDelete)
        {
            try
            {
                _context.Tasks.Remove(taskToDelete);
                _context.AuditLogs.Add(new AuditLog
                {
                    ModifiedId = taskToDelete.TaskId,
                    ModifiedTable = "Tasks",
                    Name = "Delete",
                    Date = DateTime.UtcNow
                });
                _context.SaveChanges();
                GetAllTasks();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while deleting a task: {ex.Message}");
            }
        }
        
        public List<Tasks> SearchTasks(string searchText)
        {
            return _context.Tasks.Where(t => t.Description.Contains(searchText)).ToList();
        }
        
        public List<Tasks> SearchByPriorites(string priority)
        {
            return _context.Tasks.Where(t => t.Priority.Name == priority).ToList();
        }
        
        public List<Tasks> SearchByCategories(string category)
        {
            return _context.Tasks.Where(t => t.Category.Name == category).ToList();
        }

        public void CheckDatabaseConnection()
        {
            try
            {
                var taskCount = _context.Tasks.Count();
                Console.WriteLine($"Successfully retrieved {taskCount} tasks from the database.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to connect to the database: {ex.Message}");
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        
        private int GetPriorityValue(string priority)
        {
            switch (priority)
            {
                case "High":
                    return 1;
                case "Medium":
                    return 2;
                case "Low":
                    return 3;
                default:
                    return int.MaxValue;
            }
        }

        
        public IEnumerable<Tasks> SortTasks(SortTasks.SortCriteria criteria)
        {
            var tasks = _context.Tasks; 
            
                if (criteria.PropertyName == "Name")
                {
                    if (criteria.SortOrder)
                    {
                        return tasks.OrderBy(x => x.Description);
                    }
                    
                    return tasks.OrderByDescending(x => x.Description);
                }
                else if (criteria.PropertyName == "Priority")
                {
                    if (criteria.SortOrder)
                    {
                        return tasks.ToList().OrderBy(x => GetPriorityValue(x.Priority.Name));
                    }
                    
                    return tasks.ToList().OrderByDescending(x => GetPriorityValue(x.Priority.Name));
                }
                else if (criteria.PropertyName == "Category")
                {
                    if (criteria.SortOrder)
                    {
                        return tasks.OrderBy(x => x.Category);
                    }

                    return tasks.OrderByDescending(x => x.Category);
                }
                else if (criteria.PropertyName == "Start Date")
                {
                    if (criteria.SortOrder)
                    {
                        return tasks.OrderBy(x => x.StartDate);
                    }
                    
                    return tasks.OrderByDescending(x => x.StartDate);
                }
                else if (criteria.PropertyName == "End Date")
                {
                    if (criteria.SortOrder)
                    {
                        return tasks.OrderBy(x => x.EndDate);
                    }
                    
                    return tasks.OrderByDescending(x => x.EndDate);
                }

                else
                {
                    return tasks.OrderBy(x => x.Description);
                }
        }
        
        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
        
        
    }
}