using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Controls;
using Avalonia.Threading;
using ReactiveUI;

namespace Pathfinder.ViewModels
{
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

        public ObservableCollection<string> Domains { get => _domains; set => this.RaiseAndSetIfChanged(ref _domains, value); }
        public ObservableCollection<string> IPs { get => _ips; set => this.RaiseAndSetIfChanged(ref _ips, value); }
        public ObservableCollection<string> MD5Hashes { get => _md5Hashes; set => this.RaiseAndSetIfChanged(ref _md5Hashes, value); }
        public ObservableCollection<string> SHA1Hashes { get => _sha1Hashes; set => this.RaiseAndSetIfChanged(ref _sha1Hashes, value); }
        public ObservableCollection<string> SHA256Hashes { get => _sha256Hashes; set => this.RaiseAndSetIfChanged(ref _sha256Hashes, value); }
        public ObservableCollection<string> FileNames { get => _fileNames; set => this.RaiseAndSetIfChanged(ref _fileNames, value); }
        public ObservableCollection<string> Commands { get => _commands; set => this.RaiseAndSetIfChanged(ref _commands, value); }

        public string SentinelOneQuery { get => _sentinelOneQuery; set => this.RaiseAndSetIfChanged(ref _sentinelOneQuery, value); }
        public string CrowdStrikeQuery { get => _crowdStrikeQuery; set => this.RaiseAndSetIfChanged(ref _crowdStrikeQuery, value); }
        public string DefenderQuery { get => _defenderQuery; set => this.RaiseAndSetIfChanged(ref _defenderQuery, value); }
        public string CBResponseQuery { get => _cbResponseQuery; set => this.RaiseAndSetIfChanged(ref _cbResponseQuery, value); }
        public string CBCloudQuery { get => _cbCloudQuery; set => this.RaiseAndSetIfChanged(ref _cbCloudQuery, value); }
        public string SentinelOneReminder { get => _sentinelOneReminder; set => this.RaiseAndSetIfChanged(ref _sentinelOneReminder, value); }
        public string CBResponseReminder { get => _cbResponseReminder; set => this.RaiseAndSetIfChanged(ref _cbResponseReminder, value); }
        public string CBCloudReminder { get => _cbCloudReminder; set => this.RaiseAndSetIfChanged(ref _cbCloudReminder, value); }

        private string _sentinelOneQuery = "";
        private string _crowdStrikeQuery = "";
        private string _defenderQuery = "";
        private string _cbResponseQuery = "";
        private string _cbCloudQuery = "";
        private string _sentinelOneReminder = "";
        private string _cbResponseReminder = "";
        private string _cbCloudReminder = "";

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

        public ICommand CopySentinelOneCommand { get; }
        public ICommand CopyCrowdStrikeCommand { get; }
        public ICommand CopyDefenderCommand { get; }
        public ICommand CopyCBResponseCommand { get; }
        public ICommand CopyCBCloudCommand { get; }

        public MainViewModel(Window window)
        {
            _window = window ?? throw new ArgumentNullException(nameof(window));

            this.WhenAnyValue(
                x => x.Domains,
                x => x.IPs,
                x => x.MD5Hashes,
                x => x.SHA1Hashes,
                x => x.SHA256Hashes,
                x => x.FileNames,
                x => x.Commands)
                .Throttle(TimeSpan.FromMilliseconds(100))
                .Subscribe(_ => UpdateQueries());

            CopySentinelOneCommand = new RelayCommand(CopySentinelOneQuery);
            CopyCrowdStrikeCommand = new RelayCommand(CopyCrowdStrikeQuery);
            CopyDefenderCommand = new RelayCommand(CopyDefenderQuery);
            CopyCBResponseCommand = new RelayCommand(CopyCBResponseQuery);
            CopyCBCloudCommand = new RelayCommand(CopyCBCloudQuery);
        }

        private void UpdateQueries()
        {
            SentinelOneQuery = BuildSentinelOneQuery();
            CrowdStrikeQuery = BuildCrowdStrikeQuery();
            DefenderQuery = BuildDefenderQuery();
            CBResponseQuery = BuildCBResponseQuery();
            CBCloudQuery = BuildCBCloudQuery();
        }

        private string BuildSentinelOneQuery()
        {
            var parts = new System.Collections.Generic.List<string>();

            if (MD5Hashes.Any() || SHA256Hashes.Any())
            {
                SentinelOneReminder = "MD5(s) or SHA256(s) detected -- S1 Visibility searches only accept SHA1. Only SHA1 hashes will be included below.";
            }
            else
            {
                SentinelOneReminder = "";
            }

            if (Domains.Any())
            {
                parts.Add($"(event.category = 'dns' and event.dns.request in ('{string.Join("', '", Domains)}'))");
            }

            if (IPs.Any())
            {
                parts.Add($"(event.category = 'ip' and dst.ip.address in ('{string.Join("', '", IPs)}'))");
            }

            if (SHA1Hashes.Any())
            {
                var hashes = $"'{string.Join("', '", SHA1Hashes)}'";
                parts.Add($"(tgt.file.sha1 in ({hashes}) OR src.process.image.sha1 in ({hashes}) OR src.process.parent.image.sha1 in ({hashes}))");
            }

            if (FileNames.Any())
            {
                parts.Add($"({string.Join(" OR ", FileNames.Select(f => $"tgt.file.path contains '{f}'"))})");
            }

            if (Commands.Any())
            {
                parts.Add($"({string.Join(" OR ", Commands.Select(c => $"tgt.file.path contains '{c}'"))})");
            }

            return parts.Any() ? string.Join(" OR ", parts) : "No IOCs entered";
        }

        private string BuildCrowdStrikeQuery()
        {
            var parts = new System.Collections.Generic.List<string>();

            if (Domains.Any())
            {
                var domainConditions = string.Join(" OR ", Domains.Select(d => $"DomainName like \"{d}\" OR DnsRequest like \"{d}\""));
                parts.Add($"(#event_simpleName = \"DnsRequest\" AND ({domainConditions}))");
            }

            if (IPs.Any())
            {
                parts.Add($"({string.Join(" OR ", IPs.Select(ip => $"RemoteIP = \"{ip}\""))})");
            }

            if (MD5Hashes.Any())
            {
                parts.Add($"({string.Join(" OR ", MD5Hashes.Select(md5 => $"MD5HashData like \"{md5}\""))})");
            }

            if (SHA1Hashes.Any())
            {
                parts.Add($"({string.Join(" OR ", SHA1Hashes.Select(sha1 => $"Sha1HashData like \"{sha1}\""))})");
            }

            if (SHA256Hashes.Any())
            {
                parts.Add($"({string.Join(" OR ", SHA256Hashes.Select(sha256 => $"Sha256HashData like \"{sha256}\""))})");
            }

            if (FileNames.Any())
            {
                var fileConditions = string.Join(" OR ", FileNames.Select(f => $"FilePath like \"{f}\""));
                parts.Add($"(#event_simpleName = \"File Writ*\" AND ({fileConditions}))");
            }

            if (Commands.Any())
            {
                var cmdConditions = string.Join(" OR ", Commands.Select(c => $"CommandLine like \"{c}\""));
                parts.Add($"(#event_simpleName = ProcessRollup2 AND ({cmdConditions}))");
            }

            return parts.Any() ? string.Join(" OR ", parts) : "No IOCs entered";
        }

        private string BuildDefenderQuery()
        {
            var parts = new System.Collections.Generic.List<string>();

            if (Domains.Any())
            {
                parts.Add($"(DeviceNetworkEvents\n| where RemoteURL in (\"{string.Join("\", \"", Domains)}\"))");
            }

            if (IPs.Any())
            {
                parts.Add($"(DeviceNetworkEvents\n| where RemoteIP in (\"{string.Join("\", \"", IPs)}\"))");
            }

            if (MD5Hashes.Any())
            {
                parts.Add($"(DeviceFileEvents\n| where MD5 in (\"{string.Join("\", \"", MD5Hashes)}\"))");
            }

            if (SHA1Hashes.Any())
            {
                parts.Add($"(DeviceFileEvents\n| where SHA1 in (\"{string.Join("\", \"", SHA1Hashes)}\"))");
            }

            if (SHA256Hashes.Any())
            {
                parts.Add($"(DeviceFileEvents\n| where SHA256 in (\"{string.Join("\", \"", SHA256Hashes)}\"))");
            }

            if (FileNames.Any())
            {
                parts.Add($"(DeviceFileEvents\n| where FileName in (\"{string.Join("\", \"", FileNames)}\"))");
            }

            if (Commands.Any())
            {
                parts.Add($"(DeviceProcessEvents\n| where ProcessCommandLine in (\"{string.Join("\", \"", Commands)}\"))");
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
                CBResponseReminder = "SHA1(s) detected -- Carbon Black Response Process Search only supports MD5 and SHA256 hashes. Only MD5 and SHA256 hashes will be included below.";
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
                parts.Add($"({string.Join(" OR ", IPs.Select(ip => $"ipaddr:\"{ip}\""))})");
            }

            if (MD5Hashes.Any())
            {
                parts.Add($"({string.Join(" OR ", MD5Hashes.Select(md5 => $"md5:\"{md5}\""))})");
            }

            if (SHA256Hashes.Any())
            {
                parts.Add($"({string.Join(" OR ", SHA256Hashes.Select(sha256 => $"sha256:\"{sha256}\""))})");
            }

            if (FileNames.Any())
            {
                var fileConditions = string.Join(" OR ", FileNames.Select(f => $"filemod:\"{f}\" OR path:\"{f}\""));
                parts.Add($"({fileConditions})");
            }

            if (Commands.Any())
            {
                parts.Add($"({string.Join(" OR ", Commands.Select(c => $"cmdline:\"{c}\""))})");
            }

            return parts.Any() ? string.Join(" OR ", parts) : "No IOCs entered";
        }

        private string BuildCBCloudQuery()
        {
            var parts = new System.Collections.Generic.List<string>();

            // Check for SHA1 hashes and set warning
            if (SHA1Hashes.Any())
            {
                CBCloudReminder = "SHA1(s) detected -- Carbon Black Cloud Investigate Search only supports MD5 and SHA256 hashes. Only MD5 and SHA256 hashes will be included below.";
            }
            else
            {
                CBCloudReminder = "";
            }

            // Domains
            if (Domains.Any())
            {
                parts.Add($"({string.Join(" OR ", Domains.Select(d => $"netconn_domain:\"{d}\""))})");
            }

            // IPs
            if (IPs.Any())
            {
                parts.Add($"({string.Join(" OR ", IPs.Select(ip => $"netconn_ipv4:\"{ip}\""))})");
            }

            // MD5 Hashes
            if (MD5Hashes.Any())
            {
                var md5Conditions = string.Join(" OR ", MD5Hashes.Select(md5 => $"hash:\"{md5}\" OR filemod_hash:\"{md5}\" OR parent_hash:\"{md5}\" OR process_hash:\"{md5}\" OR childproc_hash:\"{md5}\""));
                parts.Add($"({md5Conditions})");
            }

            // SHA256 Hashes (excluding SHA1 as per requirement)
            if (SHA256Hashes.Any())
            {
                var sha256Conditions = string.Join(" OR ", SHA256Hashes.Select(sha256 => $"hash:\"{sha256}\" OR filemod_hash:\"{sha256}\" OR parent_hash:\"{sha256}\" OR process_hash:\"{sha256}\""));
                parts.Add($"({sha256Conditions})");
            }

            // File Names
            if (FileNames.Any())
            {
                var fileConditions = string.Join(" OR ", FileNames.Select(f => $"process_original_filename:\"{f}\" OR childproc_name:\"{f}\" OR filemod_name:\"{f}\" OR crossproc_name:\"{f}\""));
                parts.Add($"({fileConditions})");
            }

            // Commands
            if (Commands.Any())
            {
                var cmdConditions = string.Join(" OR ", Commands.Select(c => $"process_cmdline:\"{c}\" OR parent_cmdline:\"{c}\" OR childproc_cmdline:\"{c}\""));
                parts.Add($"({cmdConditions})");
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
                    Console.WriteLine($"Copying query: {query}");
                    await _window.Clipboard.SetTextAsync(query);
                    Console.WriteLine("Query copied to clipboard");
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