using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using WpfApp2.ViewModels;

namespace WpfApp2.View;

public partial class SortTasks : Window
{
    public SortCriteria sortCriteria;
    private string sortField;
    public SortTasks()
    {
        InitializeComponent();
    }
    
    private void RadioButtonChecked(object sender, RoutedEventArgs e) {
        var radioButton = sender as RadioButton;
        if (radioButton?.Content == null)
            return;

        sortField = radioButton.Content.ToString();
    }

    private void SortButton_Click(object sender, RoutedEventArgs e)
    {

        AddSortCriteria(sortField, sortOrder, "Name");

        this.DialogResult = true;
        this.Close();
    }
    
    private void AddSortCriteria(string sortField, ComboBox comboBox, string propertyName)
    {
        var sortOrder = (ComboBoxItem)comboBox.SelectedItem;
        var order = sortOrder.Content.ToString();
        
        sortCriteria = new SortCriteria();
        sortCriteria.PropertyName = sortField;
        sortCriteria.SortOrder = GetSortDirection(order);
    }

    private bool GetSortDirection(string direction)
    {
        if (!string.IsNullOrEmpty(direction))
        {
            if (direction == "Ascending")
            {
                return true;
            }

            return false;
        }

        return false;
    }
    
    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        this.DialogResult = false;
        this.Close();
    }
    
    public class SortCriteria
    {
        public string PropertyName { get; set; }
        public bool SortOrder { get; set; }
    }

}