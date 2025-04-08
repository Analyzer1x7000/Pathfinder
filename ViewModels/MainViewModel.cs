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

        private Theme? _currentTheme;

        // Individual collapse states for each section
        private bool _isDomainsCollapsed = true;
        private bool _isIPsCollapsed = true;
        private bool _isMD5Collapsed = true;
        private bool _isSHA1Collapsed = true;
        private bool _isSHA256Collapsed = true;
        private bool _isFileNamesCollapsed = true;
        private bool _isCommandsCollapsed = true;
        private bool _isProcessNamesCollapsed = true;
        private bool _isFilePathsCollapsed = true;

        public ObservableCollection<string> Domains
        {
            get => _domains;
            set => this.RaiseAndSetIfChanged(ref _domains, new ObservableCollection<string>(
                value.Select(d => d.Replace("[", "").Replace("]", "")
                                .Replace("hxxps://", "https://")
                                .Replace("hxxp://", "http://"))));
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
                x => x.IPs,
                x => x.MD5Hashes)
                .Throttle(TimeSpan.FromMilliseconds(100))
                .Subscribe(_ => UpdateQueries());

            this.WhenAnyValue(
                x => x.SHA1Hashes,
                x => x.SHA256Hashes,
                x => x.FileNames)
                .Throttle(TimeSpan.FromMilliseconds(100))
                .Subscribe(_ => UpdateQueries());

            this.WhenAnyValue(
                x => x.Commands,
                x => x.ProcessNames,
                x => x.FilePaths)
                .Throttle(TimeSpan.FromMilliseconds(100))
                .Subscribe(_ => UpdateQueries());

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
            this.RaisePropertyChanged(nameof(Domains));
            this.RaisePropertyChanged(nameof(IPs));
            this.RaisePropertyChanged(nameof(MD5Hashes));
            this.RaisePropertyChanged(nameof(SHA1Hashes));
            this.RaisePropertyChanged(nameof(SHA256Hashes));
            this.RaisePropertyChanged(nameof(FileNames));
            this.RaisePropertyChanged(nameof(Commands));
            this.RaisePropertyChanged(nameof(ProcessNames));
            this.RaisePropertyChanged(nameof(FilePaths));
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
                    ? "Possible multiple slashes detected in file path input. Please double check your input. The `matches` keyword in S1 Visibility requires double escaped slashes in file paths."
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

            return parts.Any() ? string.Join(" OR ", parts) : "No IOCs entered";
        }

        private string BuildCrowdStrikeQuery()
        {
            var parts = new System.Collections.Generic.List<string>();

            if (ProcessNames.Any())
            {
                CrowdStrikeReminder = "CrowdStrike Falcon Advanced Event Search currently supports processes by ID. Process names will be excluded from this output.";
            }
            else
            {
                CrowdStrikeReminder = "";
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
                parts.Add($"(#event_simpleName = \"*ileWrite*\" AND ({fileConditions}))");
            }

            if (Commands.Any())
            {
                var cmdConditions = string.Join(" OR ", Commands.Select(c => $"CommandLine like \"{c}\""));
                parts.Add($"(#event_simpleName = ProcessRollup2 AND ({cmdConditions}))");
            }

            if (FilePaths.Any())
            {
                var pathConditions = string.Join(" OR ", FilePaths.Select(p => $"FilePath like \"{(p)}\"")); 
                parts.Add($"({pathConditions})");
            }

            return parts.Any() ? string.Join(" OR ", parts) : "No IOCs entered";
        }

        private string BuildDefenderQuery()
        {
            var parts = new System.Collections.Generic.List<string>();

            if (Domains.Any())
            {
                var domainConditions = string.Join(" or ", Domains.Select(d => $"RemoteUrl like \"{d}\""));
                parts.Add($"(DeviceNetworkEvents\n| where {domainConditions})");
            }

            if (IPs.Any())
            {
                parts.Add($"(DeviceNetworkEvents\n| where RemoteIP in (\"{string.Join("\", \"", IPs)}\"))");
            }

            if (MD5Hashes.Any())
            {
                parts.Add($"(DeviceFileEvents\n| where MD5 in (\"{string.Join("\", \"", MD5Hashes.Select(h => h.ToLower()))}\"))");
            }

            if (SHA1Hashes.Any())
            {
                parts.Add($"(DeviceFileEvents\n| where SHA1 in (\"{string.Join("\", \"", SHA1Hashes.Select(h => h.ToLower()))}\"))");
            }

            if (SHA256Hashes.Any())
            {
                parts.Add($"(DeviceFileEvents\n| where SHA256 in (\"{string.Join("\", \"", SHA256Hashes.Select(h => h.ToLower()))}\"))");
            }

            if (FileNames.Any())
            {
                parts.Add($"(DeviceFileEvents\n| where FileName in (\"{string.Join("\", \"", FileNames)}\"))");
            }

            if (Commands.Any())
            {
                var cmdConditions = string.Join(" or ", Commands.Select(c => $"(ProcessCommandLine like \"{c}\" or InitiatingProcessCommandLine like \"{c}\")"));
                parts.Add($"(DeviceProcessEvents\n| where {cmdConditions})");
            }

            if (ProcessNames.Any())
            {
                var processConditions = string.Join(" or ", ProcessNames.Select(p => $"FileName like \"{p}\""));
                parts.Add($"(DeviceProcessEvents\n| where ({processConditions}))");
            }

            if (FilePaths.Any())
            {
                var pathConditions = string.Join(" or ", FilePaths.Select(p => $"FolderPath contains tolower(\"{p}\")"));
                parts.Add($"(DeviceFileEvents\n| where ({pathConditions}))");
            }

            if (parts.Count == 0)
            {
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
                var pathConditions = string.Join(" OR ", FilePaths.Select(p => $"(filemod:\"{p}\" OR modload:\"{p}\" OR path:\"{p}\")"));
                parts.Add($"({pathConditions})");
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