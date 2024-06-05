using System.Windows;
using WpfApp2.Data;
using WpfApp2.Model;
using WpfApp2.ViewModels;

namespace WpfApp2.View
{
    public partial class CategorySettings : Window
    {
        private CategoryViewModel _categoryViewModel;

        public CategorySettings(ApplicationDbContext context)
        {
            InitializeComponent();
            _categoryViewModel = new CategoryViewModel(context);
            this.DataContext = _categoryViewModel;
        }

        private void AddCategory_Click(object sender, RoutedEventArgs e)
        {
            CategoriesGrid.Visibility = Visibility.Collapsed;
            AddCategoryGrid.Visibility = Visibility.Visible;
            txtNewCategory.Text = string.Empty; 
        }

        private void SaveCategory_Click(object sender, RoutedEventArgs e)
        {
            var newCategory = new Category { Name = txtNewCategory.Text };
            _categoryViewModel.AddCategory(newCategory);

            _categoryViewModel.Categories = _categoryViewModel.GetAllCategories();

            CategoriesGrid.Visibility = Visibility.Visible;
            AddCategoryGrid.Visibility = Visibility.Collapsed;

            txtNewCategory.Text = string.Empty; 
            
            
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            CategoriesGrid.Visibility = Visibility.Visible;
            AddCategoryGrid.Visibility = Visibility.Collapsed;
            txtNewCategory.Text = string.Empty; 
        }

        private void DeleteCategory_Click(object sender, RoutedEventArgs e)
        {
            var selectedCategory = (Category)CategoryList.SelectedItem;

            _categoryViewModel.DeleteCategory(selectedCategory);

            _categoryViewModel.Categories = _categoryViewModel.GetAllCategories();

            var mainWindow = Application.Current.MainWindow as MainWindow;
            if (mainWindow != null)
            {
                var viewModel = (TasksViewModel)mainWindow.DataContext;
                viewModel.Tasks = viewModel.GetAllTasks();
            }
        }
        
    }
}
