using CommonFunctions.Common;
using CommonFunctionsPlugin.Helper;
using System;

namespace CommonFunctionsPlugin.Plugins
{
    internal class RruleOperationsPlugin : PluginBase
    {
        public override void Execute(ContextBase _context)
        {
            RruleHelper helper = new RruleHelper();
            string RRule;
            DateTime startSearch = DateTime.Now;
            DateTime endSearch = DateTime.Now;
            string ocuurences = string.Empty;

            if (_context.Context.InputParameters["RRule"] != null 
                && !string.IsNullOrEmpty(_context.Context.InputParameters["RRule"].ToString())
                && _context.Context.InputParameters["StartSearch"]!=null
                && _context.Context.InputParameters["EndSearch"] != null)
            {

                RRule = _context.Context.InputParameters["RRule"].ToString();
                startSearch =(DateTime) _context.Context.InputParameters["StartSearch"];
                endSearch = (DateTime)_context.Context.InputParameters["EndSearch"];
                ocuurences=helper.GetReccurences(RRule, startSearch, endSearch,_context);

                _context.Context.OutputParameters["Occurences"] = ocuurences;
            }
        }
    }
}
