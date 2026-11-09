namespace RaceDay.MVC.Services
{
    public static class UiHelper
    {
        public static readonly string[] EnrolmentStatuses = { "Pending", "Paid", "Confirmed" };

        public static string StatusClass(string? status) => status switch
        {
            "Confirmed" or "Paid" => "bg-success",
            "Pending" => "bg-warning text-dark",
            "Cancelled" => "bg-danger",
            "Upcoming" => "bg-primary",
            "Completed" => "bg-secondary",
            _ => "bg-secondary"
        };

        public static string FormatFinishTime(decimal? minutes)
        {
            if (minutes == null) return "-";
            var totalSeconds = (int)Math.Round(minutes.Value * 60m);
            var time = TimeSpan.FromSeconds(totalSeconds);
            return $"{(int)time.TotalHours}:{time.Minutes:00}:{time.Seconds:00}";
        }

        public static bool TryParseFinishTime(string? text, out decimal minutes)
        {
            minutes = 0;
            if (string.IsNullOrWhiteSpace(text)) return false;

            var parts = text.Trim().Split(':');
            if (parts.Length != 3) return false;
            if (!int.TryParse(parts[0], out var h) || !int.TryParse(parts[1], out var m) || !int.TryParse(parts[2], out var s)) return false;
            if (h < 0 || h > 99 || m < 0 || m > 59 || s < 0 || s > 59) return false;

            minutes = Math.Round((h * 3600m + m * 60m + s) / 60m, 2);
            return minutes > 0;
        }

        public static string Ordinal(int number)
        {
            var lastTwo = number % 100;
            if (lastTwo is >= 11 and <= 13) return number + "th";
            return (number % 10) switch
            {
                1 => number + "st",
                2 => number + "nd",
                3 => number + "rd",
                _ => number + "th"
            };
        }
    }
}