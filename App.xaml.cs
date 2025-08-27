using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.Windows.AppLifecycle;
using Microsoft.Windows.AppNotifications;

using MusicFileOrganizer.Services;

using System;
using System.ComponentModel;
using System.ComponentModel.Design;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MusicFileOrganizer
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : Application
    {
        public static IServiceProvider Services { get; private set; } = null!;

        private Window? _window;

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            InitializeComponent();

            // 싱글 인스턴스 제어
            var keyInstance = AppInstance.FindOrRegisterForKey("main");
            if (!keyInstance.IsCurrent)
            {
                // 이미 실행 중인 인스턴스가 있으면 해당 인스턴스를 활성화
                var appInstance = keyInstance.RedirectActivationToAsync(AppInstance.GetCurrent().GetActivatedEventArgs());
                Environment.Exit(0);
            }

            var services = new ServiceCollection();
            services.AddSingleton<FileMetaRepository>();
            services.AddSingleton<DirectoryManager>();
            services.AddSingleton<Organizer>();

            Services = services.BuildServiceProvider();

        }

        /// <summary>
        /// Invoked when the application is launched.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            _window = new MainWindow();
            _window.SetTitleBar(null);
            _window.Activate();
        }
    }
}
