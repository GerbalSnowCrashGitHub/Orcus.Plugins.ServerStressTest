using System.Collections.Generic;
using System.Threading.Tasks;
using Orcus.Administration.Plugins;
using Sorzus.Wpf.Toolkit;

namespace ServerStressTest
{
    public class MainViewModel : PropertyChangedBase
    {
        private readonly IAdministrationControl _administrationControl;
        private List<Client> _clients;
        private int _connectedClients;
        private bool _isCanceled;
        private bool _isConnecting;

        private bool _isRunning;

        private RelayCommand _startCommand;

        private RelayCommand _stopCommand;

        public MainViewModel(IAdministrationControl administrationControl)
        {
            _administrationControl = administrationControl;
        }

        public bool IsRunning
        {
            get { return _isRunning; }
            set { SetProperty(value, ref _isRunning); }
        }

        public int ClientsToConnect { get; set; } = 100;

        public int ConnectedClients
        {
            get { return _connectedClients; }
            set { SetProperty(value, ref _connectedClients); }
        }

        public RelayCommand StartCommand
        {
            get
            {
                return _startCommand ?? (_startCommand = new RelayCommand(async parameter =>
                {
                    _clients = new List<Client>();
                    IsRunning = true;
                    _isConnecting = true;
                    _isCanceled = false;
                    for (int i = 0; i < ClientsToConnect; i++)
                    {
                        if (_isCanceled)
                        {
                            StopOperation();
                            break;
                        }
                        var client = new Client();
                        var succeed =
                            await
                                Task.Run(() => client.Connect(_administrationControl.AdministrationConnectionManager.Ip,
                                    _administrationControl.AdministrationConnectionManager.Port));
                        _clients.Add(client);
                        if (succeed)
                            ConnectedClients++;
                    }

                    _isConnecting = false;
                }));
            }
        }

        public RelayCommand StopCommand
        {
            get
            {
                return _stopCommand ?? (_stopCommand = new RelayCommand(parameter =>
                {
                    if (!IsRunning)
                        return;

                    if (_isConnecting)
                    {
                        _isCanceled = true;
                        return;
                    }

                    StopOperation();
                }));
            }
        }

        private void StopOperation()
        {
            foreach (var client in _clients)
                client.Connection?.Dispose();

            ConnectedClients = 0;
            IsRunning = false;
        }
    }
}