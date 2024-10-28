using CommonFunctions.Common;
using TimeZoneConverter;

namespace CommonFunctions.Plugins
{
    public class TimeZoneConverterPlugin : PluginBase
    {
        string tzid = string.Empty;

        string windowsTimezone = string.Empty;

        public override void Execute(ContextBase _context)
        {
            if (_context.Context.InputParameters["IANAName"] != null && !string.IsNullOrEmpty(_context.Context.InputParameters["IANAName"].ToString()))
            {
                tzid = _context.Context.InputParameters["IANAName"].ToString();

                windowsTimezone = TZConvert.IanaToWindows(tzid);

                _context.Context.OutputParameters["WindowsTimeZoneName"] = windowsTimezone;
            }
        }
    }
}
