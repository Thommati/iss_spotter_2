using System;
using System.Collections.Generic;

namespace iss_spotter
{
    public class ISSPassTimes
    {
        public List<PassOverTime> Response { get; set; } = new List<PassOverTime>();
        public string ErrorMessage { get; set; }

        public void PrintPassTimes()
        {
            foreach (var passTime in Response)
            {
                DateTime dateTime = DateTimeOffset.FromUnixTimeSeconds(passTime.Risetime).DateTime;
                TimeSpan duration = TimeSpan.FromSeconds(passTime.Duration);

                string reportString = $"Next pass at {dateTime.ToLocalTime():dddd MMMM dd, yyyy \"at\" HHH:m:ss} ({TimeZoneInfo.Local.Id}) for {duration.Minutes} minutes.";
                Console.WriteLine(reportString);
            }
        }
    }

    public class PassOverTime
    {
        public int Duration { get; set; }
        public long Risetime { get; set; }
    }
}
