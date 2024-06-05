using System.Windows;

namespace WpfApp2.View
{
    public partial class DeleteTask : Window
    {
        public DeleteTask()
        {
            InitializeComponent();
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
        

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}