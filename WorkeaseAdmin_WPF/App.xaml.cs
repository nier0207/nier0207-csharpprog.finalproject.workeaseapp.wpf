// App.xaml.cs
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;
using WorkeaseAdmin_WPF.Pages;
using WorkeaseAdmin_WPF.Services;

namespace WorkeaseAdmin_WPF
{
    public partial class App : Application
    {
        public static IServiceProvider Services { get; private set; } = null!;
        public SessionManager Session { get; } = new SessionManager();

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var services = new ServiceCollection();

            // Registered Singleton Services
            services.AddSingleton<SessionManager>();
            services.AddSingleton<ApiService>();
            services.AddSingleton<AuthService>();
            services.AddSingleton<CenterService>();
            services.AddSingleton<HealthService>();
            services.AddSingleton<DashboardService>();
            services.AddSingleton<UserService>();
            services.AddSingleton<ChildService>();
            services.AddSingleton<FeeService>();
            services.AddSingleton<AutoFeeService>();
            services.AddSingleton<AttendanceService>();
            services.AddSingleton<ReportService>();

            // Registered Pages
            RegisterPages(services);

            Services = services.BuildServiceProvider();

            var login = new LoginWindow();
            login.Show();
        }

        private void RegisterPages(IServiceCollection services)
        {
            // Main Windows
            services.AddSingleton<MainWindow>();
            services.AddTransient<LoginWindow>();

            // Main List Pages
            services.AddTransient<DashboardPage>();
            services.AddTransient<CentersPage>();
            services.AddTransient<ChildrenPage>();
            services.AddTransient<WorkersPage>();
            services.AddTransient<AttendancePage>();
            services.AddTransient<HealthPage>();
            services.AddTransient<FeesPage>();
            services.AddTransient<ReportsPage>();

            // Add Pages
            services.AddTransient<AddCenterPage>();
            services.AddTransient<AddChildrenPage>();
            services.AddTransient<AddWorkerPage>();
            services.AddTransient<AddHealthPage>();
            services.AddTransient<AddFeePage>();
            services.AddTransient<AddAttendancePage>();

            // Edit Pages
            services.AddTransient<EditCenterPage>();
            services.AddTransient<EditChildPage>();
            services.AddTransient<EditWorkerPage>();
            services.AddTransient<EditHealthPage>();
            services.AddTransient<EditFeePage>();
            services.AddTransient<EditAttendancePage>();

            // Delete Pages
            services.AddTransient<DeleteCenterPage>();
            services.AddTransient<DeleteChildPage>();
            services.AddTransient<DeleteWorkerPage>();
            services.AddTransient<DeleteHealthPage>();
            services.AddTransient<DeleteFeePage>();
            services.AddTransient<DeleteAttendancePage>();

            services.AddTransient<NarrateObservationPage>();

        }
    }
}