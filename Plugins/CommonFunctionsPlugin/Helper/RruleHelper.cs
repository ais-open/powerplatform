using CommonFunctions.Common;
using Ical.Net.CalendarComponents;
using Ical.Net.DataTypes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TimeZoneConverter;

namespace CommonFunctionsPlugin.Helper
{
    public class RruleHelper
    {
        public string GetReccurences(string RRule, DateTime startDatetime, DateTime EndDateTime, ContextBase context)
        {
            try
            {
                List<DateTime> occurences = new List<DateTime>(); 
                string pattern = @"TZID=([^:;]+)";
                string tzid = string.Empty;
                RecurrencePattern recurrenceRule = new RecurrencePattern(RRule);
                Match match = Regex.Match(RRule, pattern);
             
                if (match.Success)
                {
                    tzid = match.Groups[1].Value;
                    tzid = TZConvert.IanaToWindows(tzid);
                    context.Trace($"Dataverse:Windows time zone string:{tzid}");
                    TimeZoneInfo timezone = TimeZoneInfo.FindSystemTimeZoneById(tzid);

                    var scheduleOriginalStartDate = TimeZoneInfo.ConvertTimeFromUtc(startDatetime, timezone);
                    var endDateTime = TimeZoneInfo.ConvertTimeFromUtc(EndDateTime, timezone);

                    var startSearch = new CalDateTime(scheduleOriginalStartDate);

                    var endSearch = new CalDateTime(EndDateTime);

                    recurrenceRule.Until = endDateTime;

                    var vEvent = new CalendarEvent
                    {
                        DtStart = new CalDateTime(scheduleOriginalStartDate),
                        RecurrenceRules = new List<RecurrencePattern> { recurrenceRule },
                    };

                    var calendar = new Ical.Net.Calendar();

                    calendar.Events.Add(vEvent);

                    HashSet<Occurrence> occurrences = calendar.GetOccurrences(startSearch, endSearch);                   

                    foreach (var item in occurrences)
                    {
                        var occurenceDateTime = TimeZoneInfo.ConvertTimeToUtc(item.Period.StartTime.Value, timezone);

                        occurences.Add(occurenceDateTime);
                    }

                    return JsonConvert.SerializeObject(occurences);
                }
                else
                {
                    throw new Exception("The timezone details are missing for the Schedule");
                }
            }
            catch (Exception ex)
            {
                context.Trace($"The Error occured is {ex.Message}");
            }

            return string.Empty;
        }
    }
}
