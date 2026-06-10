using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Win32;
using WorkeaseAdmin_WPF.Models;
using WorkeaseAdmin_WPF.Services;

namespace WorkeaseAdmin_WPF.Pages
{
    public partial class NarrateObservationPage : Page
    {
        private readonly Center _selectedCenter;
        private readonly ReportService _reportService = new ReportService();
        private readonly SessionManager _session;

        // This constructor MUST accept the Center object from the ReportsPage
        public NarrateObservationPage(Center center)
        {
            InitializeComponent();
            _selectedCenter = center;
            _session = App.Services.GetRequiredService<SessionManager>();

            // Assign the center name to the TextBlock from your XAML
            TxtCenterName.Text = _selectedCenter.CenterName;
        }

        // Handles the Click event from your 'Generate Word Report' button
        private async void GenerateNarrative_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtObservation.Text))
            {
                MessageBox.Show("Please enter observations before generating.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                Mouse.OverrideCursor = Cursors.Wait;

                var request = new GenerateNarrativeDto
                {
                    CenterId = _selectedCenter.CenterId,
                    Month = DateTime.Now.Month,
                    Year = DateTime.Now.Year,
                    CycleInfo = "13th CYCLE IMPLEMENTATION",
                    SchoolYear = "2026-2027",
                    PreparedBy = _session.GetUserName(),
                    NotedBy = "PAMELA G. VALENZUELA",
                    Observations = TxtObservation.Text
                };

                var reportDto = await _reportService.GenerateNarrativeAsync(request);
                byte[] fileData = await _reportService.DownloadReportAsync(reportDto.ReportId);

                var sfd = new SaveFileDialog
                {
                    FileName = $"Narrative_{_selectedCenter.CenterName}_{DateTime.Now:yyyyMMdd}",
                    DefaultExt = ".docx",
                    Filter = "Word Document (.docx)|*.docx"
                };

                if (sfd.ShowDialog() == true)
                {
                    File.WriteAllBytes(sfd.FileName, fileData);
                    MessageBox.Show("Report downloaded successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    NavigationService.GoBack();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                Mouse.OverrideCursor = null;
            }
        }

        // Handles the Click event from your 'Cancel' button
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService.CanGoBack)
            {
                NavigationService.GoBack();
            }
        }
    }
}