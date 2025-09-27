# GitHub Copilot Development Guide

This guide helps developers effectively use GitHub Copilot with the Power Platform repository.

## Getting Started

### Prerequisites
1. Install [Visual Studio Code](https://code.visualstudio.com/)
2. Install the [GitHub Copilot extension](https://marketplace.visualstudio.com/items?itemName=GitHub.copilot)
3. Install the [GitHub Copilot Chat extension](https://marketplace.visualstudio.com/items?itemName=GitHub.copilot-chat)
4. Open the workspace using `powerplatform.code-workspace`

### Initial Setup
1. Open the repository in VS Code
2. Install recommended extensions when prompted
3. Verify GitHub Copilot is activated (check status bar)
4. Review `.vscode/settings.json` for Copilot configuration

## Using Copilot with PCF Controls

### Creating a New PCF Control
Use Copilot Chat to generate boilerplate:
```
@workspace Create a new PCF control called "DateRangePicker" that allows users to select a start and end date. Include proper TypeScript types and follow the existing patterns in this repository.
```

### Adding Properties to Existing Controls
Example prompt:
```
Add a new boolean property called "allowClearSelection" to the LookupToCombobox control that allows users to clear their selection. Update the manifest and implementation.
```

### Implementing Dataverse Operations
```
@workspace Show me how to retrieve records using context.webAPI in a PCF control, following the patterns used in the existing controls in this repository.
```

## Working with C# Plugins

### Creating Plugin Classes
```
@workspace Create a new plugin class that runs on the Update message of the Contact entity. Include proper error handling and logging using the patterns in the CommonFunctionsPlugin.
```

### Plugin Registration
```
Help me create the plugin registration steps for a plugin that should run on Pre-Operation Update of the Account entity.
```

## Best Practices for Copilot Prompts

### Be Specific About Context
- ❌ "Create a component"
- ✅ "Create a PCF control component for Power Platform that displays data in a table format"

### Reference Existing Patterns
- ❌ "Add error handling"
- ✅ "Add error handling following the pattern used in LookupToComboboxControl"

### Include Technical Requirements
- ❌ "Make it accessible"
- ✅ "Make it accessible following WCAG 2.1 AA guidelines and Power Platform accessibility requirements"

## Effective Copilot Chat Commands

### Repository Analysis
```
@workspace What is the structure of this Power Platform repository and what types of components does it contain?
```

### Code Review
```
@workspace Review this PCF control implementation for Power Platform best practices and suggest improvements
```

### Debugging
```
@workspace This PCF control is not updating when the bound property changes. Help me debug the issue based on the existing control patterns.
```

### Documentation
```
@workspace Generate JSDoc comments for this PCF control class following the existing documentation patterns in the repository
```

## Code Generation Templates

### PCF Control Template
Ask Copilot to generate using this structure:
```typescript
import { IInputs, IOutputs } from "./generated/ManifestTypes";

export class YourControl implements ComponentFramework.StandardControl<IInputs, IOutputs> {
    private _container: HTMLDivElement;
    private _context: ComponentFramework.Context<IInputs>;
    private _notifyOutputChanged: () => void;

    public init(context: ComponentFramework.Context<IInputs>, notifyOutputChanged: () => void, state: ComponentFramework.Dictionary, container: HTMLDivElement): void {
        // Implementation
    }

    public updateView(context: ComponentFramework.Context<IInputs>): void {
        // Implementation
    }

    public getOutputs(): IOutputs {
        // Implementation
    }

    public destroy(): void {
        // Cleanup
    }
}
```

### Plugin Template
```csharp
using Microsoft.Xrm.Sdk;
using System;

namespace YourPluginNamespace
{
    public class YourPlugin : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            // Implementation following repository patterns
        }
    }
}
```

## Troubleshooting

### Copilot Not Providing Power Platform Suggestions
1. Ensure you're working in the correct file context
2. Add Power Platform-specific comments to guide Copilot
3. Reference existing controls in your prompts

### TypeScript Errors with Generated Code
1. Check that all required imports are included
2. Verify ComponentFramework types are available
3. Run `npm run refreshTypes` to update PCF types

### ESLint Issues
1. Use `npm run lint:fix` to auto-fix issues
2. Check ESLint configuration in individual control folders
3. Copilot can help fix specific ESLint rule violations

## Advanced Usage

### Multi-file Refactoring
```
@workspace Refactor the data access logic from LookupToComboboxControl into a shared utility class that can be used by other controls
```

### Testing Generation
```
@workspace Generate unit tests for the ReactQueryBuilder control following TypeScript testing best practices
```

### Performance Optimization
```
@workspace Analyze the ReactQueryBuilder control for performance issues and suggest optimizations
```

## Tips for Maximum Productivity

1. **Context is Key**: Always provide context about Power Platform, PCF, or Dataverse
2. **Reference Existing Code**: Point Copilot to similar implementations in the repository
3. **Iterate Incrementally**: Break large tasks into smaller, specific requests
4. **Verify Suggestions**: Always review and test Copilot's suggestions
5. **Use Chat for Complex Tasks**: Use Copilot Chat for architecture discussions and complex refactoring
6. **Leverage Workspace Context**: Use `@workspace` to include repository-wide context

## Resources

- [Power Platform Component Framework Documentation](https://docs.microsoft.com/en-us/powerapps/developer/component-framework/)
- [GitHub Copilot Documentation](https://docs.github.com/en/copilot)
- [VS Code Copilot Extension](https://marketplace.visualstudio.com/items?itemName=GitHub.copilot)

Remember: GitHub Copilot is a tool to enhance your development workflow. Always review, test, and validate the generated code before using it in production.