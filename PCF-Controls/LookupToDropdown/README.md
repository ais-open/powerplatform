---
title: Lookup To DropDown
author: Joe Davis
date: 2/11/2022
---

# LookupToDropDown

This project is largely inspired by initial work done by Andrw Butenko, who did a great step-by-step video tutorial on putting together this control.  That said, it was quickly discovered that it was largely a proof-of-concept and didn't support functionality needed for the users.  Specifically, the original control used the default view for the bound lookup field and assumed the "name" field would be the displayed value desired.  

## Calling by View

In many cases e.g., State and Country fields we might prefer to show the abbreviation or long name of a row.  Additionally, the original control was a glorified container for an ODATA *$select* call against the entity to which the control was bound.  However, the more intuitive (and arguably more flexible) approach is to leverage the exposure of the ```getViewId()``` method on the lookup bound field and use it to make the query.  The control still calls ```retreiveMultipleRecords``` but does so using the fetchXml version of the method vs. passing in the ODATA query directly.

So, the control uses the view specified in the form designer (customization) and since we're only given the ID of the view as a property from the lookup parameter, we end up making a call to ```retrieveRecord``` against the **savedquery** entity, pulling back the *fetchxml* property.  Once we have that in hand, we then pass it to the ```retrieveMultipleRecords``` call to return the data that will be used to populate the dropdown control.

## Display Name

It is important to note that the control exposes an additional property -- *displayNameField* -- to allow the customer to provide an alternate field (other than the *name* field) to be displayed in the dropdown selection area.  If no value is supplied in that property, the *name* field is assumed.

## Testing

Better test might be to use this:
[Scott Durow's PCF React Base](https://github.com/scottdurow/pcf-react)

## Getting Started

### Prerequisites

- Visual Studio Code

### Installation

1. Clone the repo

```powershell
git clone https://github.com/ais-open/powerplatform.git
```

2. Install NPM packages

```powershell
npm install
```

3. Change directory to Control: powerplatform\PCF-Controls\LookupToCombobox\LookupToDropdownControl
4. Build the Control

```powershell
npm run build
```

5. Change directory to Solution: powerplatform\PCF-Controls\LookupToDropdown
6. Build the Solution

```powershell
dotnet build /t:build /restore  
```

7. [Import the solution](https://learn.microsoft.com/en-us/power-apps/maker/data-platform/import-update-export-solutions) into Power Platform

### Configuration

1. [Add the PCF Control to a field](https://learn.microsoft.com/en-us/power-apps/developer/component-framework/add-custom-controls-to-a-field-or-entity)
1. Provide the **LookupViewGuid** - this  is the Global Unique Identifier (GUID) of the desired view that should be used.
1. Provide the **DisplayFieldName** - the field from the Lookup View that will be displayed in the control.
