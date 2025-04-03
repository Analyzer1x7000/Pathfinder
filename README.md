
<p align="center"><img src="https://github.com/user-attachments/assets/701b52e6-620e-45d4-a4e3-1301744360b2" width="300" height="225"></p>

# Pathfinder - Hunt Builder

Pathfinder is a cross-platform tool designed for Digital Forensics and Incident Response (DFIR) and threat hunting professionals. It converts Indicators of Compromise (IOCs) into search queries for multiple Endpoint Detection and Response (EDR) platforms, including SentinelOne, CrowdStrike, Microsoft Defender, Carbon Black Response, and Carbon Black Cloud. 

Built with .NET 8 and Avalonia UI, Pathfinder offers a sleek, user-friendly interface for building and copying EDR-specific queries.

<p align="center"><img src="https://github.com/user-attachments/assets/601ec317-7f24-44f6-8764-317a6b56e503"></p>

## Features
- **Multi-EDR Support**: Generate queries for SentinelOne, CrowdStrike, Defender, CB Response, and CB Cloud.
- **IOC Types**: Supports domains, IPs, MD5/SHA1/SHA256 hashes, file names, and command lines.
- **Tabbed Interface**: View and copy queries in separate tabs for each EDR platform.
- **Word Wrap**: Output text wraps for easy reading, with line and character counts.
- **Cross-Platform**: Runs on Windows and Linux with standalone executables.

## Downloads
Download the latest standalone executables from the [Releases page](https://github.com/Analyzer1x7000/Pathfinder/releases):

| Platform       | Download Link                                                                                       | File Size (Approx.) |
|----------------|----------------------------------------------------------------------------------------------------|---------------------|
| Windows (x64)  | [Pathfinder.exe](https://github.com/Analyzer1x7000/Pathfinder/releases/download/v1.0.0/Pathfinder_W64.exe) | ~80 MB             |
| Linux (x64)    | [Pathfinder](https://github.com/Analyzer1x7000/Pathfinder/releases/download/v1.0.0/Pathfinder_L64)         | ~80 MB             |

### Notes
- **Windows**: Run `Pathfinder.exe`—no .NET installation required.
- **Linux**: Make executable with `chmod +x Pathfinder`, then run `./Pathfinder`.

## Installation
1. Download the appropriate executable from the table above.
2. For Linux, ensure it’s executable:
   ```bash
   chmod +x Pathfinder
