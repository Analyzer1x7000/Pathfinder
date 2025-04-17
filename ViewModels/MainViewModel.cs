using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Threading;
using ReactiveUI;

namespace Pathfinder.ViewModels
{
    public class Theme : ReactiveObject
    {
        private string _name;
        private ISolidColorBrush _panelBackground;
        private ISolidColorBrush _tabBackground;
        private ISolidColorBrush _textBoxBackground;
        private ISolidColorBrush _textForeground;
        private ISolidColorBrush _buttonBackground;
        private ISolidColorBrush _buttonForeground;
        private string _inputFontFamily;

        public Theme(string name, string panelBackground, string tabBackground, string textBoxBackground, string textForeground, string buttonBackground, string buttonForeground, string inputFontFamily)
        {
            _name = name;
            _panelBackground = new SolidColorBrush(Color.Parse(panelBackground));
            _tabBackground = new SolidColorBrush(Color.Parse(tabBackground));
            _textBoxBackground = new SolidColorBrush(Color.Parse(textBoxBackground));
            _textForeground = new SolidColorBrush(Color.Parse(textForeground));
            _buttonBackground = new SolidColorBrush(Color.Parse(buttonBackground));
            _buttonForeground = new SolidColorBrush(Color.Parse(buttonForeground));
            _inputFontFamily = inputFontFamily;
        }

        public string Name
        {
            get => _name;
            set => this.RaiseAndSetIfChanged(ref _name, value);
        }

        public ISolidColorBrush PanelBackground
        {
            get => _panelBackground;
            set => this.RaiseAndSetIfChanged(ref _panelBackground, value);
        }

        public ISolidColorBrush TabBackground
        {
            get => _tabBackground;
            set => this.RaiseAndSetIfChanged(ref _tabBackground, value);
        }

        public ISolidColorBrush TextBoxBackground
        {
            get => _textBoxBackground;
            set => this.RaiseAndSetIfChanged(ref _textBoxBackground, value);
        }

        public ISolidColorBrush TextForeground
        {
            get => _textForeground;
            set => this.RaiseAndSetIfChanged(ref _textForeground, value);
        }

        public ISolidColorBrush ButtonBackground
        {
            get => _buttonBackground;
            set => this.RaiseAndSetIfChanged(ref _buttonBackground, value);
        }

        public ISolidColorBrush ButtonForeground
        {
            get => _buttonForeground;
            set => this.RaiseAndSetIfChanged(ref _buttonForeground, value);
        }

        public string InputFontFamily
        {
            get => _inputFontFamily;
            set => this.RaiseAndSetIfChanged(ref _inputFontFamily, value);
        }
    }

    public class MainViewModel : ReactiveObject
    {
        private readonly Window _window;

        private ObservableCollection<string> _domains = new();
        private ObservableCollection<string> _ips = new();
        private ObservableCollection<string> _md5Hashes = new();
        private ObservableCollection<string> _sha1Hashes = new();
        private ObservableCollection<string> _sha256Hashes = new();
        private ObservableCollection<string> _fileNames = new();
        private ObservableCollection<string> _commands = new();
        private ObservableCollection<string> _processNames = new();
        private ObservableCollection<string> _filePaths = new();
        private ObservableCollection<string> _loadedDLLs = new();
        private ObservableCollection<string> _scheduledTasks = new();
        private ObservableCollection<string> _services = new();
        private ObservableCollection<string> _registryModifications = new();
        private ObservableCollection<string> _logins = new();
        private string _hostname = "";

        private Theme? _currentTheme;

        private bool _isDomainsCollapsed = true;
        private bool _isIPsCollapsed = true;
        private bool _isMD5Collapsed = true;
        private bool _isSHA1Collapsed = true;
        private bool _isSHA256Collapsed = true;
        private bool _isFileNamesCollapsed = true;
        private bool _isCommandsCollapsed = true;
        private bool _isProcessNamesCollapsed = true;
        private bool _isFilePathsCollapsed = true;
        private bool _isLoadedDLLsCollapsed = true;
        private bool _isScheduledTasksCollapsed = true;
        private bool _isServicesCollapsed = true;
        private bool _isRegistryModificationsCollapsed = true;
        private bool _isLoginsCollapsed = true;

        public ObservableCollection<string> Domains
        {
            get => _domains;
            set => this.RaiseAndSetIfChanged(ref _domains, new ObservableCollection<string>(
                value.Select(d => d.Replace("[", "").Replace("]", "").Replace("hxxps://", "https://").Replace("hxxp://", "http://"))));
        }
        public ObservableCollection<string> IPs
        {
            get => _ips;
            set => this.RaiseAndSetIfChanged(ref _ips, new ObservableCollection<string>(value.Select(ip => ip.Replace("[", "").Replace("]", ""))));
        }
        public ObservableCollection<string> MD5Hashes { get => _md5Hashes; set => this.RaiseAndSetIfChanged(ref _md5Hashes, value); }
        public ObservableCollection<string> SHA1Hashes { get => _sha1Hashes; set => this.RaiseAndSetIfChanged(ref _sha1Hashes, value); }
        public ObservableCollection<string> SHA256Hashes { get => _sha256Hashes; set => this.RaiseAndSetIfChanged(ref _sha256Hashes, value); }
        public ObservableCollection<string> FileNames { get => _fileNames; set => this.RaiseAndSetIfChanged(ref _fileNames, value); }
        public ObservableCollection<string> Commands { get => _commands; set => this.RaiseAndSetIfChanged(ref _commands, value); }
        public ObservableCollection<string> ProcessNames { get => _processNames; set => this.RaiseAndSetIfChanged(ref _processNames, value); }
        public ObservableCollection<string> FilePaths { get => _filePaths; set => this.RaiseAndSetIfChanged(ref _filePaths, value); }
        public ObservableCollection<string> LoadedDLLs { get => _loadedDLLs; set => this.RaiseAndSetIfChanged(ref _loadedDLLs, value); }
        public ObservableCollection<string> ScheduledTasks { get => _scheduledTasks; set => this.RaiseAndSetIfChanged(ref _scheduledTasks, value); }
        public ObservableCollection<string> Services { get => _services; set => this.RaiseAndSetIfChanged(ref _services, value); }
        public ObservableCollection<string> RegistryModifications { get => _registryModifications; set => this.RaiseAndSetIfChanged(ref _registryModifications, value); }
        public ObservableCollection<string> Logins { get => _logins; set => this.RaiseAndSetIfChanged(ref _logins, value); }
        public string Username
        {
            get => _username;
            set => this.RaiseAndSetIfChanged(ref _username, value);
        }
        private string _username = "";
        public string Hostname
        {
            get => _hostname;
            set => this.RaiseAndSetIfChanged(ref _hostname, value);
        }
        private string _registryModificationsReminder = "";
        public string RegistryModificationsReminder
        {
            get => _registryModificationsReminder;
            set => this.RaiseAndSetIfChanged(ref _registryModificationsReminder, value);
        }

        public string SentinelOneQuery { get => _sentinelOneQuery; set => this.RaiseAndSetIfChanged(ref _sentinelOneQuery, value); }
        public string CrowdStrikeQuery { get => _crowdStrikeQuery; set => this.RaiseAndSetIfChanged(ref _crowdStrikeQuery, value); }
        public string DefenderQuery { get => _defenderQuery; set => this.RaiseAndSetIfChanged(ref _defenderQuery, value); }
        public string CBResponseQuery { get => _cbResponseQuery; set => this.RaiseAndSetIfChanged(ref _cbResponseQuery, value); }
        public string CBCloudQuery { get => _cbCloudQuery; set => this.RaiseAndSetIfChanged(ref _cbCloudQuery, value); }
        public string SentinelOneReminder { get => _sentinelOneReminder; set => this.RaiseAndSetIfChanged(ref _sentinelOneReminder, value); }
        public string CrowdStrikeReminder { get => _crowdStrikeReminder; set => this.RaiseAndSetIfChanged(ref _crowdStrikeReminder, value); }
        public string DefenderReminder { get => _defenderReminder; set => this.RaiseAndSetIfChanged(ref _defenderReminder, value); }
        public string CBResponseReminder { get => _cbResponseReminder; set => this.RaiseAndSetIfChanged(ref _cbResponseReminder, value); }
        public string CBCloudReminder { get => _cbCloudReminder; set => this.RaiseAndSetIfChanged(ref _cbCloudReminder, value); }
        public string FilePathsReminder { get => _filePathsReminder; set => this.RaiseAndSetIfChanged(ref _filePathsReminder, value); }

        private string _sentinelOneQuery = "";
        private string _crowdStrikeQuery = "";
        private string _defenderQuery = "";
        private string _cbResponseQuery = "";
        private string _cbCloudQuery = "";
        private string _sentinelOneReminder = "";
        private string _crowdStrikeReminder = "";
        private string _defenderReminder = "";
        private string _cbResponseReminder = "";
        private string _cbCloudReminder = "";
        private string _filePathsReminder = "";

        private bool _isCopiedSentinelOne;
        private bool _isCopiedCrowdStrike;
        private bool _isCopiedDefender;
        private bool _isCopiedCBResponse;
        private bool _isCopiedCBCloud;

        public bool IsCopiedSentinelOne { get => _isCopiedSentinelOne; set => this.RaiseAndSetIfChanged(ref _isCopiedSentinelOne, value); }
        public bool IsCopiedCrowdStrike { get => _isCopiedCrowdStrike; set => this.RaiseAndSetIfChanged(ref _isCopiedCrowdStrike, value); }
        public bool IsCopiedDefender { get => _isCopiedDefender; set => this.RaiseAndSetIfChanged(ref _isCopiedDefender, value); }
        public bool IsCopiedCBResponse { get => _isCopiedCBResponse; set => this.RaiseAndSetIfChanged(ref _isCopiedCBResponse, value); }
        public bool IsCopiedCBCloud { get => _isCopiedCBCloud; set => this.RaiseAndSetIfChanged(ref _isCopiedCBCloud, value); }

        public bool IsDomainsCollapsed
        {
            get => _isDomainsCollapsed;
            set => this.RaiseAndSetIfChanged(ref _isDomainsCollapsed, value);
        }

        public bool IsIPsCollapsed
        {
            get => _isIPsCollapsed;
            set => this.RaiseAndSetIfChanged(ref _isIPsCollapsed, value);
        }

        public bool IsMD5Collapsed
        {
            get => _isMD5Collapsed;
            set => this.RaiseAndSetIfChanged(ref _isMD5Collapsed, value);
        }

        public bool IsSHA1Collapsed
        {
            get => _isSHA1Collapsed;
            set => this.RaiseAndSetIfChanged(ref _isSHA1Collapsed, value);
        }

        public bool IsSHA256Collapsed
        {
            get => _isSHA256Collapsed;
            set => this.RaiseAndSetIfChanged(ref _isSHA256Collapsed, value);
        }

        public bool IsFileNamesCollapsed
        {
            get => _isFileNamesCollapsed;
            set => this.RaiseAndSetIfChanged(ref _isFileNamesCollapsed, value);
        }

        public bool IsCommandsCollapsed
        {
            get => _isCommandsCollapsed;
            set => this.RaiseAndSetIfChanged(ref _isCommandsCollapsed, value);
        }

        public bool IsProcessNamesCollapsed
        {
            get => _isProcessNamesCollapsed;
            set => this.RaiseAndSetIfChanged(ref _isProcessNamesCollapsed, value);
        }

        public bool IsFilePathsCollapsed
        {
            get => _isFilePathsCollapsed;
            set => this.RaiseAndSetIfChanged(ref _isFilePathsCollapsed, value);
        }

        public bool IsLoadedDLLsCollapsed
        {
            get => _isLoadedDLLsCollapsed;
            set => this.RaiseAndSetIfChanged(ref _isLoadedDLLsCollapsed, value);
        }
        
        public bool IsScheduledTasksCollapsed
        {
            get => _isScheduledTasksCollapsed;
            set => this.RaiseAndSetIfChanged(ref _isScheduledTasksCollapsed, value);
        }

        public bool IsServicesCollapsed
        {
            get => _isServicesCollapsed;
            set => this.RaiseAndSetIfChanged(ref _isServicesCollapsed, value);
        }

        public bool IsRegistryModificationsCollapsed
        {
            get => _isRegistryModificationsCollapsed;
            set => this.RaiseAndSetIfChanged(ref _isRegistryModificationsCollapsed, value);
        }

        public bool IsLoginsCollapsed
        {
            get => _isLoginsCollapsed;
            set => this.RaiseAndSetIfChanged(ref _isLoginsCollapsed, value);
        }

        public Theme? CurrentTheme
        {
            get => _currentTheme;
            set => this.RaiseAndSetIfChanged(ref _currentTheme, value);
        }

        public ICommand CopySentinelOneCommand { get; }
        public ICommand CopyCrowdStrikeCommand { get; }
        public ICommand CopyDefenderCommand { get; }
        public ICommand CopyCBResponseCommand { get; }
        public ICommand CopyCBCloudCommand { get; }

        public ICommand ClearDomainsCommand { get; }
        public ICommand ClearIPsCommand { get; }
        public ICommand ClearMD5HashesCommand { get; }
        public ICommand ClearSHA1HashesCommand { get; }
        public ICommand ClearSHA256HashesCommand { get; }
        public ICommand ClearFileNamesCommand { get; }
        public ICommand ClearCommandsCommand { get; }
        public ICommand ClearProcessNamesCommand { get; }
        public ICommand ClearFilePathsCommand { get; }
        public ICommand ClearLoadedDLLsCommand { get; }
        public ICommand ClearScheduledTasksCommand { get; }
        public ICommand ClearServicesCommand { get; }
        public ICommand ClearRegistryModificationsCommand { get; }
        public ICommand ClearLoginsCommand { get; }
        public ICommand ClearHostnameCommand { get; }
        public ICommand ClearUsernameCommand { get; }
        public ICommand ClearAllCommand { get; }
        public ICommand CollapseAllCommand { get; }

        public ICommand SetNightShiftThemeCommand { get; }
        public ICommand SetMatrixThemeCommand { get; }
        public ICommand SetStrawberryMilkshakeThemeCommand { get; }
        public ICommand SetWindows95ThemeCommand { get; }
        public ICommand SetDayShiftThemeCommand { get; }

        public MainViewModel(Window window)
        {
            _window = window ?? throw new ArgumentNullException(nameof(window));

            CurrentTheme = new Theme("Night Shift", "#1A1A1A", "#333333", "#222222", "White", "#333333", "#D3D3D3", "JetBrains Mono, Consolas, Ubuntu Mono");

            this.WhenAnyValue(
                    x => x.Domains,
                    x => x.FilePaths,
                    x => x.Hostname,
                    x => x.Username,
                    x => x.ScheduledTasks,
                    x => x.Services,
                    x => x.RegistryModifications)
                    .Throttle(TimeSpan.FromMilliseconds(100))
                    .Subscribe(_ => UpdateQueries());

                this.WhenAnyValue(
                    x => x.IPs,
                    x => x.MD5Hashes,
                    x => x.SHA1Hashes,
                    x => x.SHA256Hashes,
                    x => x.LoadedDLLs)
                    .Throttle(TimeSpan.FromMilliseconds(100))
                    .Subscribe(_ => UpdateQueries());

                this.WhenAnyValue(
                    x => x.Commands,
                    x => x.ProcessNames,
                    x => x.FileNames,
                    x => x.Logins)
                    .Throttle(TimeSpan.FromMilliseconds(100))
                    .Subscribe(_ => UpdateQueries());

                this.WhenAnyValue(x => x.Logins)
                .Subscribe(logins =>
                {
                    if (logins.Any() && !string.IsNullOrEmpty(Username))
                    {
                        Username = "";
                        this.RaisePropertyChanged(nameof(Username));
                    }
                });

            CopySentinelOneCommand = new RelayCommand(CopySentinelOneQuery);
            CopyCrowdStrikeCommand = new RelayCommand(CopyCrowdStrikeQuery);
            CopyDefenderCommand = new RelayCommand(CopyDefenderQuery);
            CopyCBResponseCommand = new RelayCommand(CopyCBResponseQuery);
            CopyCBCloudCommand = new RelayCommand(CopyCBCloudQuery);

            ClearDomainsCommand = new RelayCommand(() => { Domains.Clear(); this.RaisePropertyChanged(nameof(Domains)); });
            ClearIPsCommand = new RelayCommand(() => { IPs.Clear(); this.RaisePropertyChanged(nameof(IPs)); });
            ClearMD5HashesCommand = new RelayCommand(() => { MD5Hashes.Clear(); this.RaisePropertyChanged(nameof(MD5Hashes)); });
            ClearSHA1HashesCommand = new RelayCommand(() => { SHA1Hashes.Clear(); this.RaisePropertyChanged(nameof(SHA1Hashes)); });
            ClearSHA256HashesCommand = new RelayCommand(() => { SHA256Hashes.Clear(); this.RaisePropertyChanged(nameof(SHA256Hashes)); });
            ClearFileNamesCommand = new RelayCommand(() => { FileNames.Clear(); this.RaisePropertyChanged(nameof(FileNames)); });
            ClearCommandsCommand = new RelayCommand(() => { Commands.Clear(); this.RaisePropertyChanged(nameof(Commands)); });
            ClearProcessNamesCommand = new RelayCommand(() => { ProcessNames.Clear(); this.RaisePropertyChanged(nameof(ProcessNames)); });
            ClearFilePathsCommand = new RelayCommand(() => { FilePaths.Clear(); this.RaisePropertyChanged(nameof(FilePaths)); });
            ClearLoadedDLLsCommand = new RelayCommand(() => { LoadedDLLs.Clear(); this.RaisePropertyChanged(nameof(LoadedDLLs)); });
            ClearScheduledTasksCommand = new RelayCommand(() => { ScheduledTasks.Clear(); this.RaisePropertyChanged(nameof(ScheduledTasks)); });
            ClearServicesCommand = new RelayCommand(() => { Services.Clear(); this.RaisePropertyChanged(nameof(Services)); });
            ClearRegistryModificationsCommand = new RelayCommand(() => { RegistryModifications.Clear(); this.RaisePropertyChanged(nameof(RegistryModifications)); });
            ClearLoginsCommand = new RelayCommand(() => { Logins.Clear(); this.RaisePropertyChanged(nameof(Logins)); });
            ClearHostnameCommand = new RelayCommand(() => { Hostname = ""; this.RaisePropertyChanged(nameof(Hostname)); });
            ClearUsernameCommand = new RelayCommand(() => { Username = ""; this.RaisePropertyChanged(nameof(Username)); });
            ClearAllCommand = new RelayCommand(ClearAll);
            CollapseAllCommand = new RelayCommand(CollapseAll);

            SetNightShiftThemeCommand = new RelayCommand(() => CurrentTheme = new Theme("Night Shift", "#1A1A1A", "#333333", "#222222", "White", "#333333", "#D3D3D3", "JetBrains Mono, Consolas, Ubuntu Mono"));
            SetMatrixThemeCommand = new RelayCommand(() => CurrentTheme = new Theme("Matrix", "#000000", "#1C2526", "#0A0A0A", "#00FF00", "#1C2526", "#00FF00", "IBM Plex Mono, Consolas, Ubuntu Mono"));
            SetStrawberryMilkshakeThemeCommand = new RelayCommand(() => CurrentTheme = new Theme("Strawberry Milkshake", "#FF9999", "#FF6666", "#FFCCCC", "#5E2A2A", "#FF6666", "#FFFFFF", "Source Code Pro, Consolas, Ubuntu Mono"));
            SetWindows95ThemeCommand = new RelayCommand(() => CurrentTheme = new Theme("Windows 95", "#C0C0C0", "#008080", "#FFFFFF", "#000080", "#C0C0C0", "#FFFFFF", "Courier New, Consolas, Ubuntu Mono"));
            SetDayShiftThemeCommand = new RelayCommand(() => CurrentTheme = new Theme("Day Shift", "#F0F0F0", "#FFFFFF", "#E8ECEF", "#000000", "#F5F5F5", "#FFFFFF", "JetBrains Mono, Consolas, Ubuntu Mono"));
        }

        private void UpdateQueries()
        {
            SentinelOneQuery = BuildSentinelOneQuery();
            CrowdStrikeQuery = BuildCrowdStrikeQuery();
            DefenderQuery = BuildDefenderQuery();
            CBResponseQuery = BuildCBResponseQuery();
            CBCloudQuery = BuildCBCloudQuery();
        }

        private void ClearAll()
        {
            Domains.Clear();
            IPs.Clear();
            MD5Hashes.Clear();
            SHA1Hashes.Clear();
            SHA256Hashes.Clear();
            FileNames.Clear();
            Commands.Clear();
            ProcessNames.Clear();
            FilePaths.Clear();
            LoadedDLLs.Clear();
            ScheduledTasks.Clear();
            Services.Clear();
            RegistryModifications.Clear();
            Logins.Clear();
            Hostname = "";
            Username = "";
            this.RaisePropertyChanged(nameof(Domains));
            this.RaisePropertyChanged(nameof(IPs));
            this.RaisePropertyChanged(nameof(MD5Hashes));
            this.RaisePropertyChanged(nameof(SHA1Hashes));
            this.RaisePropertyChanged(nameof(SHA256Hashes));
            this.RaisePropertyChanged(nameof(FileNames));
            this.RaisePropertyChanged(nameof(Commands));
            this.RaisePropertyChanged(nameof(ProcessNames));
            this.RaisePropertyChanged(nameof(FilePaths));
            this.RaisePropertyChanged(nameof(LoadedDLLs));
            this.RaisePropertyChanged(nameof(ScheduledTasks));
            this.RaisePropertyChanged(nameof(Services));
            this.RaisePropertyChanged(nameof(RegistryModifications));
            this.RaisePropertyChanged(nameof(Logins));
            this.RaisePropertyChanged(nameof(Hostname));
            this.RaisePropertyChanged(nameof(Username));
        }

        private void CollapseAll()
        {
            IsDomainsCollapsed = false;
            IsIPsCollapsed = false;
            IsMD5Collapsed = false;
            IsSHA1Collapsed = false;
            IsSHA256Collapsed = false;
            IsFileNamesCollapsed = false;
            IsCommandsCollapsed = false;
            IsProcessNamesCollapsed = false;
            IsFilePathsCollapsed = false;
            IsLoadedDLLsCollapsed = false;
            IsScheduledTasksCollapsed = false;
            IsServicesCollapsed = false;
            IsRegistryModificationsCollapsed = false;
            IsLoginsCollapsed = false;
        }

        private string BuildSentinelOneQuery()
        {
            var parts = new System.Collections.Generic.List<string>();

            if (MD5Hashes.Any())
            {
                SentinelOneReminder = "SentinelOne Visibility currently only supports SHA1 and SHA256 hashes. Pathfinder will exclude MD5 hashes.";
            }
            else
            {
                SentinelOneReminder = "";
            }

            if (FilePaths.Any())
            {
                bool hasMultipleSlashes = FilePaths.Any(p =>
                {
                    var segments = p.Split(new[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);
                    for (int i = 0; i < segments.Length - 1; i++)
                    {
                        int slashes = p.IndexOf(segments[i + 1], p.IndexOf(segments[i]) + segments[i].Length) - (p.IndexOf(segments[i]) + segments[i].Length);
                        if (slashes > 1) return true;
                    }
                    return false;
                });

                FilePathsReminder = hasMultipleSlashes
                    ? "Multiple consecutive slashes detected - Pathfinder expects single slashes and adjusts output accordingly."
                    : "";
            }
            else
            {
                FilePathsReminder = "";
            }

            if (Domains.Any())
            {
                var domainConditions = string.Join(" or ", Domains.Select(d => $"event.dns.request matches '{d}'"));
                parts.Add($"(event.category = 'dns' and ({domainConditions}))");
            }

            if (IPs.Any())
            {
                parts.Add($"(event.category = 'ip' and dst.ip.address in ('{string.Join("', '", IPs)}'))");
            }

            if (SHA1Hashes.Any())
            {
                var hashes = $"'{string.Join("', '", SHA1Hashes.Select(h => h.ToLower()))}'";
                parts.Add($"(tgt.file.sha1 in ({hashes}) OR src.process.image.sha1 in ({hashes}) OR src.process.parent.image.sha1 in ({hashes}))");
            }

            if (SHA256Hashes.Any())
            {
                var hashes = $"'{string.Join("', '", SHA256Hashes.Select(h => h.ToLower()))}'";
                parts.Add($"(tgt.file.sha256 in ({hashes}) OR src.process.image.sha256 in ({hashes}) OR src.process.parent.image.sha256 in ({hashes}))");
            }

            if (FileNames.Any())
            {
                parts.Add($"({string.Join(" OR ", FileNames.Select(f => $"tgt.file.path contains '{f}'"))})");
            }

            if (Commands.Any())
            {
                var cmdConditions = string.Join(" OR ", Commands.Select(c => $"(src.process.cmdline matches '{c}' OR src.process.parent.cmdline matches '{c}')"));
                parts.Add($"({cmdConditions})");
            }

            if (ProcessNames.Any())
            {
                var processConditions = string.Join(" OR ", ProcessNames.Select(p => $"(src.process.displayName matches '{p}' OR src.process.parent.displayName matches '{p}')"));
                parts.Add($"({processConditions})");
            }

            if (FilePaths.Any())
            {
                var pathConditions = string.Join(" OR ", FilePaths.Select(p =>
                {
                    string escapedPath = p.Replace(@"\", @"\\\\");
                    return $"(tgt.file.path matches '{escapedPath}' OR tgt.file.oldPath matches '{escapedPath}')";
                }));
                parts.Add($"({pathConditions})");
            }

            if (LoadedDLLs.Any())
            {
                var dllConditions = string.Join(" or ", LoadedDLLs.Select(dll => $"module.path matches '{dll}'"));
                parts.Add($"(event.type='Module Load' and ({dllConditions}))");
            }

            if (ScheduledTasks.Any())
            {
                var taskConditions = string.Join(" or ", ScheduledTasks.Select(t => $"(task.path matches '{t}' or task.name matches '{t}')"));
                parts.Add($"(event.category='scheduled_task' and ({taskConditions}))");
            }

            if (Services.Any())
            {
                var serviceConditions = string.Join(" or ", Services.Select(s => $"src.process.cmdline matches \"{s}\""));
                parts.Add($"((event.type = 'Process Creation') and (src.process.displayName contains 'services.exe' or src.process.cmdline contains 'sc create' or src.process.cmdline contains 'sc start') and ({serviceConditions}))");
            }

            if (RegistryModifications.Any())
            {
                bool hasMultipleSlashes = RegistryModifications.Any(r => r.Contains("\\\\"));
                RegistryModificationsReminder = hasMultipleSlashes
                    ? "Multiple consecutive slashes detected - Pathfinder expects single slashes and adjusts output accordingly."
                    : "";
                
                var registryConditions = string.Join(" or ", RegistryModifications.Select(r => 
                    $"registry.keyPath matches \"{r.Replace(@"\", @"\\\\")}\"")); // Quadruple slashes
                parts.Add($"(event.category = 'registry' AND ({registryConditions}))");
            } else
            {
                RegistryModificationsReminder = "";
            }

            if (Logins.Any())
            {
                var loginConditions = string.Join(" OR ", Logins.Select(l => $"event.login.userName = \"{l}\""));
                parts.Add($"(((event.type = 'Login') AND (event.login.type != \"UNLOCK\" AND event.login.type != \"INTERACTIVE\" AND event.login.type != \"CACHED_INTERACTIVE\") AND src.endpoint.ip.address != \"127.0.0.1\") AND ({loginConditions}))");
            }

            var userHostParts = new System.Collections.Generic.List<string>();
            if (!string.IsNullOrEmpty(Hostname))
            {
                userHostParts.Add($"(endpoint.name matches \"{Hostname}\")");
            }
            if (!string.IsNullOrEmpty(Username))
            {
                userHostParts.Add($"(event.login.userName matches '{Username}' OR src.process.user matches '{Username}' OR tgt.process.user matches '{Username}' OR src.process.parent.user matches '{Username}')");
            }

            if (userHostParts.Any())
            {
                var userHostCondition = userHostParts.Count > 1 ? $"({string.Join(" AND ", userHostParts)})" : userHostParts[0];
                if (parts.Any())
                {
                    return $"({userHostCondition}) AND (({string.Join(" OR ", parts)}))"; 
                }
                return userHostParts.Count > 1 ? userHostCondition : (string.IsNullOrEmpty(Hostname) ? userHostParts[0] : $"endpoint.name = \"{Hostname}\"");
            }

            return parts.Any() ? string.Join(" OR ", parts) : "No IOCs entered";
        }

        private string BuildCrowdStrikeQuery()
        {
            var parts = new System.Collections.Generic.List<string>();
            CrowdStrikeReminder = "";

            if (FilePaths.Any())
            {
                bool hasMultipleSlashes = FilePaths.Any(p => p.Contains("\\\\"));
                if (hasMultipleSlashes)
                {
                    CrowdStrikeReminder = "Multiple consecutive slashes detected - Pathfinder expects single slashes and adjusts output accordingly.";
                }

                var pathConditions = string.Join(" OR ", FilePaths.Select(p => 
                    $"FilePath like \"{p.Replace(@"\", @"\\")}\""));
                parts.Add($"({pathConditions})");
            }

            if (Domains.Any())
            {
                var domainConditions = string.Join(" OR ", Domains.Select(d => $"DomainName like \"{d}\" OR DnsRequest like \"{d}\""));
                parts.Add($"(#event_simpleName = \"DnsRequest\" AND ({domainConditions}))");
            }

            if (IPs.Any())
            {
                var ipConditions = string.Join(" OR ", IPs.Select(ip => $"RemoteIP = \"{ip}\" OR RemoteIP4 = \"{ip}\""));
                parts.Add($"({ipConditions})");
            }

            if (MD5Hashes.Any())
            {
                parts.Add($"({string.Join(" OR ", MD5Hashes.Select(md5 => $"MD5HashData like \"{md5.ToLower()}\""))})");
            }

            if (SHA1Hashes.Any())
            {
                parts.Add($"({string.Join(" OR ", SHA1Hashes.Select(sha1 => $"SHA1HashData like \"{sha1.ToLower()}\""))})");
            }

            if (SHA256Hashes.Any())
            {
                parts.Add($"({string.Join(" OR ", SHA256Hashes.Select(sha256 => $"SHA256HashData like \"{sha256.ToLower()}\""))})");
            }

            if (FileNames.Any())
            {
                var fileConditions = string.Join(" OR ", FileNames.Select(f => $"FileName like \"{f}\" OR TargetFileName like \"{f}\""));
                parts.Add($"(#event_simpleName = \"File Writ*\" AND ({fileConditions}))");
            }

            if (Commands.Any())
            {
                var cmdConditions = string.Join(" OR ", Commands.Select(c => $"CommandLine like \"{c}\""));
                parts.Add($"(#event_simpleName = ProcessRollup2 AND ({cmdConditions}))");
            }

            if (ProcessNames.Any())
            {
                var processConditions = string.Join(" OR ", ProcessNames.Select(p => $"(FileName like \"{p}\" OR ParentBaseFileName like \"{p}\" OR GrandparentBaseFileName like \"{p}\" OR ImageFileName like \"{p}\")"));
                parts.Add($"(#event_simpleName = \"ProcessRollup2\" AND ({processConditions}))");
            }

            if (LoadedDLLs.Any())
            {
                var dllConditions = string.Join(" OR ", LoadedDLLs.Select(dll => $"FileName like \"{dll}\""));
                parts.Add($"(#event_simpleName = \"*oduleLoa*\" AND ({dllConditions}))");
            }

            if (ScheduledTasks.Any())
            {
                var taskConditions = string.Join(" OR ", ScheduledTasks.Select(t => $"(TaskName like \"{t}\" OR TaskExecCommand like \"{t}\")"));
                parts.Add($"(#event_simpleName = \"*chedul*\" AND ({taskConditions}))");
            }

            if (Services.Any())
            {
                var serviceConditions = string.Join(" OR ", Services.Select(s => $"(ServiceImagePath like \"{s}\" OR CommandLine like \"{s}\")"));
                parts.Add($"(#event_simpleName=\"*ervic*\" AND ({serviceConditions}))");
            }

            if (RegistryModifications.Any())
            {
                var registryConditions = string.Join(" OR ", RegistryModifications.Select(r => 
                    $"RegObjectName like \"{r.Replace(@"\", @"\\")}\"")); // Double slashes
                parts.Add($"(#event_simpleName like \"Reg\" AND ({registryConditions}))");
            }

            if (Logins.Any())
            {
                var loginConditions = string.Join(" OR ", Logins.Select(l => $"UserName = \"{l}\""));
                parts.Add($"((#event_simpleName = \"UserLogon\" AND (LogonType=10 OR LogonType=3)) AND ({loginConditions}))");
            }

            var userHostParts = new System.Collections.Generic.List<string>();
            if (!string.IsNullOrEmpty(Hostname))
            {
                userHostParts.Add($"(ComputerName = \"{Hostname}\")");
            }
            if (!string.IsNullOrEmpty(Username))
            {
                userHostParts.Add($"(UserName = \"{Username}\")");
            }

            if (userHostParts.Any())
            {
                var userHostCondition = userHostParts.Count > 1 ? $"({string.Join(" AND ", userHostParts)})" : userHostParts[0];
                if (parts.Any())
                {
                    return $"({userHostCondition}) AND (({string.Join(" OR ", parts)}))"; // Add extra parentheses around IOC parts
                }
                return userHostParts.Count > 1 ? userHostCondition : (string.IsNullOrEmpty(Hostname) ? $"UserName = \"{Username}\"" : $"ComputerName contains \"{Hostname}\"");
            }

            return parts.Any() ? string.Join(" OR ", parts) : "No IOCs entered";
        }

        private string BuildDefenderQuery()
        {
            var parts = new System.Collections.Generic.List<string>();
            var hostFilter = !string.IsNullOrEmpty(Hostname) ? $"DeviceName == \"{Hostname}\"" : "";
            var userFilter = !string.IsNullOrEmpty(Username) ? $"AccountName == \"{Username}\"" : "";
            var combinedFilter = "";
            if (!string.IsNullOrEmpty(hostFilter) && !string.IsNullOrEmpty(userFilter))
            {
                combinedFilter = $"\n| where {hostFilter} and {userFilter}";
            }
            else if (!string.IsNullOrEmpty(hostFilter))
            {
                combinedFilter = $"\n| where {hostFilter}";
            }
            else if (!string.IsNullOrEmpty(userFilter))
            {
                combinedFilter = $"\n| where {userFilter}";
            }

            if (FilePaths.Any())
            {
                bool hasMultipleSlashes = FilePaths.Any(p => p.Contains("\\\\"));
                if (hasMultipleSlashes)
                {
                    DefenderReminder = "Multiple consecutive slashes detected - Pathfinder expects single slashes and adjusts output accordingly.";
                }
                else
                {
                    DefenderReminder = "";
                }

                var pathConditions = string.Join(" or ", FilePaths.Select(p => 
                    $"FolderPath contains tolower(\"{p.Replace(@"\", @"\\")}\")"));
                parts.Add($"(DeviceFileEvents\n| where ({pathConditions}){combinedFilter})");
            }

            if (Domains.Any())
            {
                var domainConditions = string.Join(" or ", Domains.Select(d => $"RemoteUrl like \"{d}\""));
                parts.Add($"(DeviceNetworkEvents\n| where {domainConditions}{combinedFilter})");
            }

            if (IPs.Any())
            {
                parts.Add($"(DeviceNetworkEvents\n| where RemoteIP in (\"{string.Join("\", \"", IPs)}\"){combinedFilter})");
            }

            if (MD5Hashes.Any())
            {
                parts.Add($"(DeviceFileEvents\n| where MD5 in (\"{string.Join("\", \"", MD5Hashes.Select(h => h.ToLower()))}\"){combinedFilter})");
            }

            if (SHA1Hashes.Any())
            {
                parts.Add($"(DeviceFileEvents\n| where SHA1 in (\"{string.Join("\", \"", SHA1Hashes.Select(h => h.ToLower()))}\"){combinedFilter})");
            }

            if (SHA256Hashes.Any())
            {
                parts.Add($"(DeviceFileEvents\n| where SHA256 in (\"{string.Join("\", \"", SHA256Hashes.Select(h => h.ToLower()))}\"){combinedFilter})");
            }

            if (FileNames.Any())
            {
                parts.Add($"(DeviceFileEvents\n| where FileName in (\"{string.Join("\", \"", FileNames)}\"){combinedFilter})");
            }

            if (Commands.Any())
            {
                var cmdConditions = string.Join(" or ", Commands.Select(c => $"(ProcessCommandLine like \"{c}\" or InitiatingProcessCommandLine like \"{c}\")"));
                parts.Add($"(DeviceProcessEvents\n| where {cmdConditions}{combinedFilter})");
            }

            if (ProcessNames.Any())
            {
                var processConditions = string.Join(" or ", ProcessNames.Select(p => $"FileName like \"{p}\""));
                parts.Add($"(DeviceProcessEvents\n| where ({processConditions}){combinedFilter})");
            }

            if (LoadedDLLs.Any())
            {
                var dllConditions = string.Join(" or ", LoadedDLLs.Select(dll => $"FileName like \"{dll}\""));
                parts.Add($"(DeviceImageLoadEvents\n| where {dllConditions}{combinedFilter})");
            }

            if (ScheduledTasks.Any())
            {
                var taskConditions = string.Join(" or ", ScheduledTasks.Select(t => $"(ProcessCommandLine like \"{t}\")"));
                parts.Add($"(DeviceProcessEvents\n| where FileName endswith \"schtasks.exe\" and (ProcessCommandLine has \"/create\" or ProcessCommandLine has \"/run\") and {taskConditions}{combinedFilter})");
            }

            if (Services.Any())
            {
                var serviceConditions = string.Join(" or ", Services.Select(s => $"(FileName like \"{s}\" or ProcessCommandLine like \"{s}\")"));
                parts.Add($"(DeviceProcessEvents\n| where FileName has \"services.exe\" or ProcessCommandLine contains \"sc create\" or ProcessCommandLine contains \"sc start\"\n| where {serviceConditions}{combinedFilter})");
            }

            if (RegistryModifications.Any())
            {
                var registryConditions = string.Join(" or ", RegistryModifications.Select(r => 
                    $"RegistryKey like \"{r.Replace(@"\", @"\\")}\"")); // Double slashes
                parts.Add($"(DeviceRegistryEvents\n| where ActionType in (\"RegistryValueSet\", \"RegistryKeyCreate\", \"RegistryKeyDelete\", \"RegistryValueDelete\", \"RegistryValueRename\", \"RegistryKeyRename\")\n| where {registryConditions}{combinedFilter})");
            }

            if (Logins.Any())
            {
                var loginConditions = string.Join(" OR ", Logins.Select(l => $"AccountName has \"{l}\""));
                parts.Add($"(DeviceLogonEvents\n| where LogonType != \"Unknown\" AND LogonType != \"Interactive\"\n| where {loginConditions}{combinedFilter})");
            }

            if (parts.Count == 0)
            {
                if (!string.IsNullOrEmpty(Hostname) || !string.IsNullOrEmpty(Username))
                {
                    var filters = new System.Collections.Generic.List<string>();
                    if (!string.IsNullOrEmpty(Hostname)) filters.Add($"DeviceName == \"{Hostname}\"");
                    if (!string.IsNullOrEmpty(Username)) filters.Add($"AccountName == \"{Username}\"");
                    return $"DeviceEvents\n| where {string.Join(" and ", filters)}";
                }
                return "No IOCs entered";
            }
            else if (parts.Count == 1)
            {
                return parts[0];
            }
            else
            {
                return "union\n" + string.Join(",\n", parts);
            }
        }

        private string BuildCBResponseQuery()
        {
            var parts = new System.Collections.Generic.List<string>();

            if (SHA1Hashes.Any())
            {
                CBResponseReminder = "SHA1(s) detected -- Carbon Black Response Process Search only supports MD5 and SHA256 hashes.";
            }
            else
            {
                CBResponseReminder = "";
            }

            if (Domains.Any())
            {
                parts.Add($"({string.Join(" OR ", Domains.Select(d => $"domain:\"{d}\""))})");
            }

            if (IPs.Any())
            {
                parts.Add($"({string.Join(" OR ", IPs.Select(ip => $"ipaddr:{ip}"))})");
            }

            if (MD5Hashes.Any())
            {
                parts.Add($"({string.Join(" OR ", MD5Hashes.Select(md5 => $"md5:\"{md5.ToUpper()}\""))})");
            }

            if (SHA256Hashes.Any())
            {
                parts.Add($"({string.Join(" OR ", SHA256Hashes.Select(sha256 => $"sha256:\"{sha256.ToUpper()}\""))})");
            }

            if (FileNames.Any())
            {
                var fileConditions = string.Join(" OR ", FileNames.Select(f => $"filemod:\"{f}\""));
                parts.Add($"({fileConditions})");
            }

            if (Commands.Any())
            {
                parts.Add($"({string.Join(" OR ", Commands.Select(c => $"cmdline:\"{c}\""))})");
            }

            if (ProcessNames.Any())
            {
                var processConditions = string.Join(" OR ", ProcessNames.Select(p => $"(process_name:\"{p}\" OR parent_name:\"{p}\" OR childproc_name:\"{p}\")"));
                parts.Add($"({processConditions})");
            }

            if (FilePaths.Any())
            {
                var pathConditions = string.Join(" OR ", FilePaths.Select(p => $"(filemod:\"{p.Replace("\\", "/")}\" OR modload:\"{p.Replace("\\", "/")}\" OR path:\"{p.Replace("\\", "/")}\")"));
                parts.Add($"({pathConditions})");
            }

            if (LoadedDLLs.Any())
            {
                var dllConditions = string.Join(" OR ", LoadedDLLs.Select(dll => $"modload:\"{dll}\""));
                parts.Add($"({dllConditions})");
            }

            if (ScheduledTasks.Any())
            {
                CBResponseReminder = "Pathfinder does not yet support Scheduled Tasks for this EDR. Scheduled Task output will be omitted.";
            }

            if (Services.Any())
            {
                CBResponseReminder = "Pathfinder does not yet support Services for this EDR. Services output will be omitted.";
            }

            if (RegistryModifications.Any())
            {
                CBResponseReminder = "Pathfinder does not yet support Registry Modifications for this EDR. Registry Modifications output will be omitted.";
            }

            if (Logins.Any())
            {
                CBResponseReminder = "Logins are not yet supported for this EDR.";
            }

            var userHostParts = new System.Collections.Generic.List<string>();
            if (!string.IsNullOrEmpty(Hostname))
            {
                userHostParts.Add($"(hostname:{Hostname})");
            }
            if (!string.IsNullOrEmpty(Username))
            {
                userHostParts.Add($"(username:{Username})");
            }

            if (userHostParts.Any())
            {
                var userHostCondition = userHostParts.Count > 1 ? $"({string.Join(" AND ", userHostParts)})" : userHostParts[0];
                if (parts.Any())
                {
                    return $"{userHostCondition} AND ({string.Join(" OR ", parts)})";
                }
                return userHostParts.Count > 1 ? userHostCondition : (string.IsNullOrEmpty(Hostname) ? $"username:{Username}" : $"hostname:{Hostname}");
            }

            return parts.Any() ? string.Join(" OR ", parts) : "No IOCs entered";
        }

        private string BuildCBCloudQuery()
        {
            var parts = new System.Collections.Generic.List<string>();

            if (SHA1Hashes.Any())
            {
                CBCloudReminder = "SHA1(s) detected -- Carbon Black Cloud Investigate Search only supports MD5 and SHA256 hashes.";
            }
            else
            {
                CBCloudReminder = "";
            }

            if (Domains.Any())
            {
                parts.Add($"({string.Join(" OR ", Domains.Select(d => $"netconn_domain:\"{d}\""))})");
            }

            if (IPs.Any())
            {
                parts.Add($"({string.Join(" OR ", IPs.Select(ip => $"netconn_ipv4:\"{ip}\""))})");
            }

            if (MD5Hashes.Any())
            {
                parts.Add($"({string.Join(" OR ", MD5Hashes.Select(md5 => $"hash:\"{md5.ToLower()}\" OR filemod_hash:\"{md5.ToLower()}\" OR parent_hash:\"{md5.ToLower()}\" OR process_hash:\"{md5.ToLower()}\" OR childproc_hash:\"{md5.ToLower()}\""))})");
            }

            if (SHA256Hashes.Any())
            {
                var sha256Conditions = string.Join(" OR ", SHA256Hashes.Select(sha256 => $"hash:\"{sha256.ToLower()}\" OR filemod_hash:\"{sha256.ToLower()}\" OR parent_hash:\"{sha256.ToLower()}\" OR process_hash:\"{sha256.ToLower()}\""));
                parts.Add($"({sha256Conditions})");
            }

            if (FileNames.Any())
            {
                var fileConditions = string.Join(" OR ", FileNames.Select(f => $"filemod_name:\"{f}\""));
                parts.Add($"({fileConditions})");
            }

            if (Commands.Any())
            {
                var cmdConditions = string.Join(" OR ", Commands.Select(c => $"process_cmdline:\"{c}\" OR parent_cmdline:\"{c}\" OR childproc_cmdline:\"{c}\""));
                parts.Add($"({cmdConditions})");
            }

            if (ProcessNames.Any())
            {
                var processConditions = string.Join(" OR ", ProcessNames.Select(p => $"(process_name:\"{p}\" OR parent_name:\"{p}\" OR childproc_name:\"{p}\")"));
                parts.Add($"({processConditions})");
            }

            if (FilePaths.Any())
            {
                var pathConditions = string.Join(" OR ", FilePaths.Select(p => $"(filemod_name:\"{p.Replace("\\", "/")}\" OR modload_name:\"{p.Replace("\\", "/")}\")"));
                parts.Add($"({pathConditions})");
            }

            if (LoadedDLLs.Any())
            {
                var dllConditions = string.Join(" OR ", LoadedDLLs.Select(dll => $"modload_name:\"{dll}\""));
                parts.Add($"({dllConditions})");
            }

            if (ScheduledTasks.Any())
            {
                CBCloudReminder = "Pathfinder does not yet support Scheduled Tasks for this EDR. Scheduled Task output will be omitted.";
            }

             if (Services.Any())
            {
                CBCloudReminder = "Pathfinder does not yet support Services for this EDR. Services output will be omitted.";
            }

            if (RegistryModifications.Any())
            {
                CBCloudReminder = "Pathfinder does not yet support Registry Modifications for this EDR. Registry Modifications output will be omitted.";
            }

            if (Logins.Any())
            {
                CBCloudReminder = "Logins are not yet supported for this EDR.";
            }

            var userHostParts = new System.Collections.Generic.List<string>();
            if (!string.IsNullOrEmpty(Hostname))
            {
                userHostParts.Add($"(device_name:{Hostname})");
            }
            if (!string.IsNullOrEmpty(Username))
            {
                userHostParts.Add($"(process_username:{Username})");
            }

            if (userHostParts.Any())
            {
                var userHostCondition = userHostParts.Count > 1 ? $"({string.Join(" AND ", userHostParts)})" : userHostParts[0];
                if (parts.Any())
                {
                    return $"{userHostCondition} AND ({string.Join(" OR ", parts)})";
                }
                return userHostParts.Count > 1 ? userHostCondition : (string.IsNullOrEmpty(Hostname) ? $"process_username:{Username}" : $"device_name:{Hostname}");
            }

            return parts.Any() ? string.Join(" OR ", parts) : "No IOCs entered";
        }

        private void CopySentinelOneQuery() => ExecuteCopy(SentinelOneQuery, () => IsCopiedSentinelOne = true, () => IsCopiedSentinelOne = false);
        private void CopyCrowdStrikeQuery() => ExecuteCopy(CrowdStrikeQuery, () => IsCopiedCrowdStrike = true, () => IsCopiedCrowdStrike = false);
        private void CopyDefenderQuery() => ExecuteCopy(DefenderQuery, () => IsCopiedDefender = true, () => IsCopiedDefender = false);
        private void CopyCBResponseQuery() => ExecuteCopy(CBResponseQuery, () => IsCopiedCBResponse = true, () => IsCopiedCBResponse = false);
        private void CopyCBCloudQuery() => ExecuteCopy(CBCloudQuery, () => IsCopiedCBCloud = true, () => IsCopiedCBCloud = false);

        private void ExecuteCopy(string query, Action setCopiedTrue, Action setCopiedFalse)
        {
            if (!string.IsNullOrEmpty(query) && _window.Clipboard != null)
            {
                Dispatcher.UIThread.Post(async () =>
                {
                    await _window.Clipboard.SetTextAsync(query);
                    setCopiedTrue();
                    await Task.Delay(3000);
                    setCopiedFalse();
                });
            }
        }

        private class RelayCommand : ICommand
        {
            private readonly Action _execute;
            public event EventHandler? CanExecuteChanged { add { } remove { } }

            public RelayCommand(Action execute)
            {
                _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            }

            public bool CanExecute(object? parameter) => true;
            public void Execute(object? parameter) => _execute();
        }
    }
}
