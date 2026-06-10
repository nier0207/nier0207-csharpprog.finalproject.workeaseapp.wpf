using Microsoft.Extensions.DependencyInjection;
using Microsoft.Win32;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WorkeaseAdmin_WPF.Models;
using WorkeaseAdmin_WPF.Services;

namespace WorkeaseAdmin_WPF.Pages
{
    public partial class ReportsPage : Page
    {
        private readonly ReportService _reportService = new ReportService();
        private readonly SessionManager _session;

        public ReportsPage()
        {
            InitializeComponent();
            _session = App.Services.GetRequiredService<SessionManager>();
        }

        // 1. MASTER LIST - Needs Center Selection
        private async void MasterList_Click(object sender, RoutedEventArgs e)
        {
            await RunReportWithCenter("MasterList", ".xlsx", "Excel Workbook|*.xlsx", async (centerId) =>
            {
                return await _reportService.GenerateMasterListAsync(new GenerateMasterListDto
                {
                    CenterId = centerId,
                    CycleInfo = "13th CYCLE IMPLEMENTATION",
                    SchoolYear = "2026-2027",
                    PreparedBy = _session.GetUserName(),
                    NotedBy = "PAMELA G. VALENZUELA"
                });
            });
        }

        // 2. PDF SUMMARY - Needs Center Selection
        private async void PDFGenerate_Click(object sender, RoutedEventArgs e)
        {
            await RunReportWithCenter("Summary_Report", ".pdf", "PDF Document|*.pdf", async (centerId) =>
            {
                return await _reportService.GeneratePdfSummaryAsync(new GeneratePdfSummaryDto
                {
                    CenterId = centerId,
                    CycleInfo = "13th CYCLE IMPLEMENTATION",
                    SchoolYear = "2026-2027"
                });
            });
        }

        // 3. REPORT FEE - Automatic (No Dialog)
        private async void ReportFee_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Mouse.OverrideCursor = Cursors.Wait;

                var request = new GenerateReportFeeDto
                {
                    Month = DateTime.Now.Month,
                    Year = DateTime.Now.Year,
                    CycleInfo = "13th CYCLE IMPLEMENTATION",
                    SchoolYear = "2026-2027",
                    PreparedBy = _session.GetUserName(),
                    NotedBy = "PAMELA G. VALENZUELA"
                };

                var reportDto = await _reportService.GenerateReportFeeAsync(request);
                await DownloadAndSave(reportDto.ReportId, "Fee_Report", ".xlsx", "Excel Workbook|*.xlsx");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error generating Fee Report: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                Mouse.OverrideCursor = null;
            }
        }

        // --- HELPERS ---

        private async Task RunReportWithCenter(string prefix, string ext, string filter, Func<int, Task<ReportListDto>> genFunc)
        {
            var dialog = new Windows.CenterSelectionWindow();
            dialog.Owner = Window.GetWindow(this);

            if (dialog.ShowDialog() == true)
            {
                try
                {
                    Mouse.OverrideCursor = Cursors.Wait;
                    var reportDto = await genFunc(dialog.SelectedCenter.CenterId);
                    await DownloadAndSave(reportDto.ReportId, $"{prefix}_{dialog.SelectedCenter.CenterName}", ext, filter);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                finally { Mouse.OverrideCursor = null; }
            }
        }

        private async Task DownloadAndSave(int id, string defaultName, string ext, string filter)
        {
            byte[] fileData = await _reportService.DownloadReportAsync(id);
            var sfd = new SaveFileDialog
            {
                FileName = $"{defaultName}_{DateTime.Now:yyyyMMdd}",
                DefaultExt = ext,
                Filter = filter
            };

            if (sfd.ShowDialog() == true)
            {
                File.WriteAllBytes(sfd.FileName, fileData);
                MessageBox.Show("Download Complete!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void NarrativeReview_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Windows.CenterSelectionWindow();
            dialog.Owner = Window.GetWindow(this);

            if (dialog.ShowDialog() == true)
            {
                var observationPage = new NarrateObservationPage(dialog.SelectedCenter);
                this.NavigationService?.Navigate(observationPage);
            }
        }
    }
}