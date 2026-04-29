using System;
using System.Windows;
using System.Windows.Controls;

namespace WorkeaseAdmin_WPF.Pages
{
    public partial class ReportsPage : Page
    {
        public ReportsPage()
        {
            InitializeComponent();
        }

        private void MasterList_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Generating Master List...", "Report System", MessageBoxButton.OK, MessageBoxImage.Information);
            // TODO: Add logic to show Master List
        }

        private void PDFGenerate_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Opening PDF Generator...", "Report System", MessageBoxButton.OK, MessageBoxImage.Information);
            // TODO: Add logic for PDF export
        }

        private void ReportFee_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Loading Fee Reports...", "Report System", MessageBoxButton.OK, MessageBoxImage.Information);
            // TODO: Add logic for financial reports
        }

        private void ListOfReports_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Showing Archive of Reports...", "Report System", MessageBoxButton.OK, MessageBoxImage.Information);
            // TODO: Add logic to view historical reports
        }
    }
}