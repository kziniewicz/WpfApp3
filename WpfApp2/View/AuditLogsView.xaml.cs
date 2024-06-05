using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using LiveCharts;
using LiveCharts.Wpf;
using WpfApp2.Model;

namespace WpfApp2
{
    public partial class AuditLogsView : Window
    {
        public ObservableCollection<AuditLog> AuditLogs { get; set; }
        public SeriesCollection AuditLogPieSeries { get; set; }

        public AuditLogsView(ObservableCollection<AuditLog> auditLogs)
        {
            InitializeComponent();
            AuditLogs = auditLogs;
            AuditLogDataGrid.ItemsSource = AuditLogs;

            AuditLogPieSeries = new SeriesCollection();

            var auditLogGroups = AuditLogs.GroupBy(a => a.Name);
            
            foreach (var group in auditLogGroups)
            {
                AuditLogPieSeries.Add(new PieSeries
                {
                    Title = group.Key,
                    Values = new ChartValues<int> { group.Count() },
                    DataLabels = true
                });
            }
            
            AuditLogPieChart.Series = AuditLogPieSeries;
        }
    }
}