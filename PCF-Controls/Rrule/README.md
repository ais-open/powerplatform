---
title: Rrule PCF
author: Nick Gill
date: 7/01/2024
---
# Rrule PCF

This control enables the use of the rrule.js library inside of a Canvas app or Custom page. The control is not meant to render items in the UI; rather, it accepts inputs to produce a date/time series or an Rrule string. This simplifies the handling of recurrence rules for calendar dates.

The following video provides a basic walkthrough of the PCF control:
https://youtu.be/irVfGyw_-r8

This control is based off the rrule.js library which can be found here:
https://github.com/jkbrzt/rrule

| Canvas apps | Custom pages | Model-driven apps | Portals |
| ----------- | ------------ | ----------------- | ------- |
| ✅           | ✅            | ⬜                 | ⬜       |

## Project artifacts

This project contains the following artifacts:

| Artifacts | Relative Folder location | Description |
| -------- | -------------- | ---------- |
| RruleControl | ./RruleControl | The pcfproj for the actual PCF control  |
| RruleSolution | ./RruleCSolution | Solution containing the PCF control for deployment  |
| DemoSolution | ./DemoSolution | A sample Canvas app solution that demonstrates the use of the Rrule PCF control |
| RrulePackage | ./RrulePackage | A package deployment project that enables easier deployment of both the DemoSolution and RruleSolution in one deployment but still keeping as individual solutions. |

## Control Properties

### Input Properties

| Property name | Display name | Type  | Description |
| -------- | ----------- | -------------- | ---------- |
| `freq` (required) | Frequency | Options (Enum) | One of the following: Yearly, Monthly, Weekly, Daily, Hourly, Minutely, Secondly |
| `interval` | Interval | Whole number | The interval between each freq iteration. For example, when using Yearly, an interval of 2 means once every two years, but with Hourly, it means once every two hours. The default interval is 1.|
| `count` | Count | Whole number | How many occurrences will be generated |
| `bySetPos` | By Set Pos | Whole number | If given, it must be an integer, positive or negative. The given integer will specify an occurrence number, corresponding to the nth occurrence of the rule inside the frequency period. For example, a bysetpos of -1 if combined with a Monthly frequency, and a byweekday of (RRule.MO, RRule.TU, RRule.WE, RRule.TH, RRule.FR), will result in the last work day of every month. |
| `startDate` | Start Date | Date/Time | The recurrence start. |
| `endDate` | End Date | Date/Time |  The last or until date |
| `byWeekDay` | By Weekday Day | String | A day or a multiple days separated by a comma (example: "Monday,Tuesday,Wednesday,Thursday,Friday,Saturday,Sunday"). When given, these variables will define the weekdays where the recurrence will be applied.|
| `byMonth` | By Month | String | If given, it must be either an integer, or an array of integers, meaning the months to apply the recurrence to. With January == 1, February == 2, and so on.|
| `byMonthDay` | By Month Day | String |  If given, it must be either an integer, or an array of integers, meaning the month days to apply the recurrence to. |
| `byYearDay` | By Year Day | String |  If given, it must be either an integer, or an array of integers, meaning the year days to apply the recurrence to. |


### Output Properties

| Property name | Display name | Type | Description |
| -------- | ----------- | -------------- | ---------- |
| `stringDateOutput` | Dates | String | The generated dates as a json string. |
| `rruleStringOutput` | RRULE String | String | The generated rrule string |
| `rruleTextOutput` | RRULE Text Output | String | Returns a natural language representation of the Rrule |

### Notes

- The following options are available in the rrule.js library but have not been implemented as properties in the control: tzid, byweekno, byhour, byminute, bysecond, byeaster, wkst.
- Wkst is set internally to Sunday to align with Power App defaults.
- The generated dates are returned as a string. The ParseJSON function should be used to retrieve the values into a table of dates/time values. For example, "ClearCollect(colDates, ForAll(ParseJSON(rrulepcf1.stringDateOutput), {Date: DateTimeValue(ThisRecord)}));"