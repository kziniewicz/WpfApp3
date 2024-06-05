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
    public class CategoryViewModel : INotifyPropertyChanged
    {
        private readonly ApplicationDbContext _context;
        private List<Category> _categories;

        public List<Category> Categories
        {
            get { return _categories; }
            set { SetField(ref _categories, value); }
        }

        public CategoryViewModel(ApplicationDbContext context)
        {
            _context = context;
            Categories = GetAllCategories();
            Console.WriteLine("CategoryViewModel initialized. AllCategories count: " + Categories.Count);


        }

        public List<Category> GetAllCategories()
        {
            try
            {
                var categories = _context.Category.ToList();
                return categories;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while retrieving categories: {ex.Message}");
                return new List<Category>();
            }
        }

        public void AddCategory(Category newCategory)
        {
            try
            {
                _context.Category.Add(newCategory);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while adding a new category: {ex.Message}");
            }
        }

        public void UpdateCategory(Category updatedCategory)
        {
            try
            {
                _context.Category.Update(updatedCategory);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while updating a category: {ex.Message}");
            }
        }

        public void DeleteCategory(Category categoryToDelete)
        {
            try
            {
                _context.Category.Remove(categoryToDelete);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while deleting a category: {ex.Message}");
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