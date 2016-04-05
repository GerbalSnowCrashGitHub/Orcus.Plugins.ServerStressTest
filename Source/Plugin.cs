using System.Windows.Controls;
using Orcus.Administration.Plugins;

namespace ServerStressTest
{
    public class Plugin : IAdministrationPlugin
    {
        private IAdministrationControl _administrationControl;
        private MainViewModel _viewModel;

        public void Initialize(IUiModifier uiModifier, IAdministrationControl administrationControl)
        {
            uiModifier.AddMainMenuItem(new MenuItem {Header = "Server Stress Test"}, MenuEventHandler);
            _administrationControl = administrationControl;
        }

        private void MenuEventHandler(object sender, MenuItemClickedEventArgs e)
        {
            var window = new StressTestWindow
            {
                Owner = e.MainWindow,
                DataContext = _viewModel ?? (_viewModel = new MainViewModel(_administrationControl))
            };
            window.ShowDialog();
        }
    }
}