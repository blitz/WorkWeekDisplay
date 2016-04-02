using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tasks
{
    public sealed class WorkWeek
    {
        public static int Current()
        {
            var ci = new CultureInfo("en-US");
            var cal = ci.Calendar;

            return cal.GetWeekOfYear(
                DateTime.Now,
                CalendarWeekRule.FirstFourDayWeek,
                ci.DateTimeFormat.FirstDayOfWeek);
        }
    }
}
