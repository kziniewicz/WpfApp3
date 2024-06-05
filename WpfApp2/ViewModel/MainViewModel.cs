using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using WpfApp2.Data;
using WpfApp2.Model;
using WpfApp2.ViewModels;

public class MainViewModel : INotifyPropertyChanged
{
    private ApplicationDbContext _context;
    public ObservableCollection<Category> AllCategories { get; set; }
    public ObservableCollection<Priority> AllPriorities { get; set; }

    public MainViewModel(ApplicationDbContext context)
    {
        _context = context;
        LoadData();
    }
    public event PropertyChangedEventHandler? PropertyChanged;

    private void LoadData()
    {
        AllCategories = new ObservableCollection<Category>(_context.Category.ToList());
        AllPriorities = new ObservableCollection<Priority>([
            new Priority { Name = "High" },
            new Priority { Name = "Medium" },
            new Priority { Name = "Low" }
        ]);
    }
    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}