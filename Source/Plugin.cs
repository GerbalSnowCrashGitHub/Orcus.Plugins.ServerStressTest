using System.Windows.Controls;
using Orcus.Administration.Plugins;

namespace ServerStressTest
{
    public class Plugin : IAdministrationPlugin
    {
        private IAdministrationControl _administrationControl;
        private MenuItem _menuItem;
        private MainViewModel _viewModel;

        public void Initialize(IUiModifier uiModifier, IAdministrationControl administrationControl)
        {
            uiModifier.AddMainMenuItem(_menuItem = new MenuItem {Header = "Server Stress Test"});
            uiModifier.MenuItemClicked += UiModifier_MenuItemClicked;
            _administrationControl = administrationControl;
        }

        private void UiModifier_MenuItemClicked(object sender, MenuItemClickedEventArgs e)
        {
            if (e.MenuItem != _menuItem)
                return;

            var window = new StressTestWindow
            {
                Owner = e.MainWindow,
                DataContext = _viewModel ?? (_viewModel = new MainViewModel(_administrationControl))
            };
            window.ShowDialog();
        }
    }
}