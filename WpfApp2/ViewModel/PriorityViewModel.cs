using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using WpfApp2.Model;
using WpfApp2.Data;

namespace WpfApp2.ViewModels
{
    public class PriorityViewModel : INotifyPropertyChanged
    {
        
        private readonly ApplicationDbContext _context;
        private List<Priority> _priorities;

        public List<Priority> Priorities
        {
            get { return _priorities; }
            set { SetField(ref _priorities, value); }
        }


        public PriorityViewModel(ApplicationDbContext context)
        {
            _context = context;
            _priorities = new List<Priority>
            {
                new Priority { Name = "High" },
                new Priority { Name = "Medium" },
                new Priority { Name = "Low" }
            };
        }

        public List<Priority> GetAllPriorities()
        {
            try
            {
                var priorities = _context.Priority.ToList(); 
                return priorities;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while retrieving priorities: {ex.Message}");
                return new List<Priority>();
            }
        }
        

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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