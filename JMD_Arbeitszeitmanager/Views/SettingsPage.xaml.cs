using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

using JMD_Arbeitszeitmanager.Contracts.Services;
using JMD_Arbeitszeitmanager.Contracts.Views;
using JMD_Arbeitszeitmanager.Models;
using JMD_Arbeitszeitmanager.Services;
using Microsoft.Extensions.Options;
using Microsoft.Win32;

namespace JMD_Arbeitszeitmanager.Views
{
    public partial class SettingsPage : Page, INotifyPropertyChanged, INavigationAware
    {
        private readonly AppConfig _appConfig;
        private readonly IThemeSelectorService _themeSelectorService;
        private readonly ISystemService _systemService;
        private readonly IApplicationInfoService _applicationInfoService;
        private readonly IDatabaseConnector _databaseConnector;
        private bool _isInitialized;
        private AppTheme _theme;
        private string _versionDescription;

        private ISecretManager _secretManager;

        public AppTheme Theme
        {
            get { return _theme; }
            set { Set(ref _theme, value); }
        }

        public string VersionDescription
        {
            get { return _versionDescription; }
            set { Set(ref _versionDescription, value); }
        }

        public SettingsPage(IOptions<AppConfig> appConfig, IThemeSelectorService themeSelectorService, ISystemService systemService, IApplicationInfoService applicationInfoService, ISecretManager secretManager, IDatabaseConnector databaseConnector)
        {
            _appConfig = appConfig.Value;
            _themeSelectorService = themeSelectorService;
            _systemService = systemService;
            _applicationInfoService = applicationInfoService;
            InitializeComponent();
            DataContext = this;
            _secretManager = secretManager;
            _databaseConnector = databaseConnector;
        }

        public void OnNavigatedTo(object parameter)
        {
            VersionDescription = $"JMD_Arbeitszeitmanager - {_applicationInfoService.GetVersion()}";
            Theme = _themeSelectorService.GetCurrentTheme();
            _isInitialized = true;

            initDatabaseParameters();
            LoadHiddenCostumers();
        }

        public void OnNavigatedFrom()
        {
        }

        private void OnLightChecked(object sender, RoutedEventArgs e)
        {
            if (_isInitialized)
            {
                _themeSelectorService.SetTheme(AppTheme.Light);
            }
        }

        private void OnDarkChecked(object sender, RoutedEventArgs e)
        {
            if (_isInitialized)
            {
                _themeSelectorService.SetTheme(AppTheme.Dark);
            }
        }

        private void OnPrivacyStatementClick(object sender, RoutedEventArgs e)
            => _systemService.OpenInWebBrowser(_appConfig.PrivacyStatement);

        public event PropertyChangedEventHandler PropertyChanged;

        private void Set<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(storage, value))
            {
                return;
            }

            storage = value;
            OnPropertyChanged(propertyName);
        }

        private void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private void initDatabaseParameters()
        {
            tb_user.Text = SettingsService.ReadSetting(Properties.Resources.DBUser);
            tb_url.Text = SettingsService.ReadSetting(Properties.Resources.ServerAddress);

            tb_port.Text = SettingsService.ReadSetting(Properties.Resources.Port);
            tb_db.Text = SettingsService.ReadSetting(Properties.Resources.DatabaseName);

            tb_pw.Password = _secretManager.getDecryptedPasswordToDB();

            tb_sslCa.Text = SettingsService.ReadSetting(Properties.Resources.SSLCaPath);
            tb_sslKey.Text = SettingsService.ReadSetting(Properties.Resources.SSLKeyPath);
            tb_sslCert.Text = SettingsService.ReadSetting(Properties.Resources.SSLCertPath);
        }

        private void btn_save_Click(object sender, RoutedEventArgs e)
        {
            string user, url, port, db;
            string pw;
            string sslCaPath, sslKeyPath, sslCertPath;

            user = tb_user.Text ?? null;
            url = tb_url.Text ?? null;
            port = tb_port.Text ?? null;
            db = tb_db.Text ?? null;
            pw = tb_pw.Password ?? null;
            sslCaPath = tb_sslCa.Text ?? null;
            sslKeyPath = tb_sslKey.Text ?? null;
            sslCertPath = tb_sslCert.Text ?? null;

            if (user != null && url != null && port != null && db != null && pw != null && pw!= "" && sslCaPath != null && sslCertPath != null && sslKeyPath != null)
            {
                _secretManager.saveDatabaseConnectionParameters(user, port, pw, url, db, sslCaPath, sslKeyPath, sslCertPath);
                _databaseConnector.setNewConnectionString();
                MessageBox.Show("Erfolgreich gespeichert");
            }
            else
            {
                MessageBox.Show("Bitte alle Felder ausfüllen!");
            }
        }

        private void chooseFile(TextBox tb)
        {

            if (_isInitialized)
            {
                String filePath;

                OpenFileDialog choofdlog = new OpenFileDialog();
                choofdlog.Filter = "Alle Dateien (*.*)|*.*";
                choofdlog.FilterIndex = 1;
                choofdlog.Multiselect = false;
                choofdlog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

                if (choofdlog.ShowDialog() == true)
                {
                    filePath = choofdlog.FileName;
                    tb.Text = filePath;
                }
            }
        }

        private void btn_cancel_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn_chooseFileSslCA_Click(object sender, RoutedEventArgs e)
        {
            chooseFile(tb_sslCa);
        }

        private void btn_chooseFileSslKey_Click(object sender, RoutedEventArgs e)
        {
            chooseFile(tb_sslKey);
        }

        private void btn_chooseFileSslCert_Click(object sender, RoutedEventArgs e)
        {
            chooseFile(tb_sslCert);
        }

        private void LoadHiddenCostumers()
        {
            lb_hiddenCostumers.Items.Clear();
            var hiddenCostumers = SettingsService.GetHiddenCostumers();
            foreach (var costumer in hiddenCostumers)
            {
                lb_hiddenCostumers.Items.Add(costumer);
            }
        }

        private void btn_addHiddenCostumer_Click(object sender, RoutedEventArgs e)
        {
            var name = tb_hiddenCostumerName.Text?.Trim();
            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Bitte einen Kundennamen eingeben.");
                return;
            }

            var hiddenCostumers = SettingsService.GetHiddenCostumers();
            if (hiddenCostumers.Any(h => h.Equals(name, StringComparison.OrdinalIgnoreCase)))
            {
                MessageBox.Show($"Der Kunde \"{name}\" ist bereits ausgeblendet.");
                return;
            }

            hiddenCostumers.Add(name);
            SettingsService.SaveHiddenCostumers(hiddenCostumers);
            LoadHiddenCostumers();
            tb_hiddenCostumerName.Text = string.Empty;
        }

        private void btn_removeHiddenCostumer_Click(object sender, RoutedEventArgs e)
        {
            if (lb_hiddenCostumers.SelectedItem == null)
            {
                MessageBox.Show("Bitte einen Kunden aus der Liste auswählen.");
                return;
            }

            var selected = lb_hiddenCostumers.SelectedItem.ToString();
            var hiddenCostumers = SettingsService.GetHiddenCostumers();
            hiddenCostumers.RemoveAll(c => c.Equals(selected, StringComparison.OrdinalIgnoreCase));
            SettingsService.SaveHiddenCostumers(hiddenCostumers);
            LoadHiddenCostumers();
        }
    }
}
