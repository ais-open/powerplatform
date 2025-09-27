# Power Platform Repository

Inventions and creations in Power Platform by the AIS team.

## Overview

This repository contains Power Platform components including:
- **PCF Controls**: TypeScript/React-based Power Component Framework controls
- **Plugins**: C# Dataverse plugins for Power Platform
- **Solutions**: Power Platform solution files

## GitHub Copilot Integration

This repository is configured for optimal GitHub Copilot experience:

### Quick Start with Copilot
1. Open the repository in VS Code
2. Install recommended extensions (GitHub Copilot, GitHub Copilot Chat)
3. Open the workspace using `powerplatform.code-workspace`
4. Follow the [Copilot Development Guide](docs/COPILOT-GUIDE.md)

### Key Features
- Pre-configured VS Code settings for Power Platform development
- Copilot-optimized workspace structure
- Power Platform-specific code generation templates
- Context-aware development assistance

## Getting Started

### Prerequisites
- Visual Studio Code
- Node.js (for PCF controls)
- .NET Framework 4.6.2+ (for plugins)
- Power Platform CLI
- GitHub Copilot extension

### Development Setup
1. Clone the repository
2. Open `powerplatform.code-workspace` in VS Code
3. Install recommended extensions when prompted
4. Navigate to specific PCF control folders to work on individual components

## Repository Structure

```
├── .vscode/                    # VS Code configuration
├── docs/                       # Documentation
├── PCF-Controls/              # Power Component Framework controls
│   ├── LookupToCombobox/      # Lookup to ComboBox control
│   ├── LookupToDropdown/      # Lookup to Dropdown control
│   ├── ReactQueryBuilder/     # React Query Builder control
│   └── Rrule/                 # Recurrence Rule control
├── Plugins/                   # C# Dataverse plugins
└── powerplatform.code-workspace # VS Code workspace configuration
```

## Contributing

Please refer to the [Copilot Development Guide](docs/COPILOT-GUIDE.md) for best practices when contributing to this repository with GitHub Copilot assistance.
