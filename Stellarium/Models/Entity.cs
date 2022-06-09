namespace Stellarium.Models
{
    public class Entity
    {
        public static string OverDay(DateTime dateTime)
        {
            if ((DateTime.Now - dateTime).TotalDays > 7)
            {
                return dateTime.ToString("HH:mm d MMM yyyy");
            }
            else
            {
                var daysAgo = Math.Round((DateTime.Now - dateTime).TotalDays, MidpointRounding.ToZero);
                switch (daysAgo)
                {
                    case 0: return TodayTime(dateTime); break;
                    case 1: return daysAgo + " день назад"; break;
                    case 2: case 3: case 4: return daysAgo + " дня назад"; break;
                    case 5: case 6: case 7: return daysAgo + " дней назад"; break;
                }
                return dateTime.ToString();
            }
        }

        public static string TodayTime(DateTime dateTime)
        {
            var hoursAgo = Math.Round((DateTime.Now - dateTime).TotalHours, MidpointRounding.ToEven);
            var minutesAgo = Math.Round((DateTime.Now - dateTime).TotalMinutes, MidpointRounding.ToEven);
            if (minutesAgo > 59)
            {
                switch (hoursAgo.ToString().Last())
                {
                    case '1': return hoursAgo + " час назад"; break;
                    case '2': case '3': case '4': return hoursAgo + " часa назад"; break;
                    case '5': case '6': case '7': case '8': case '9': case '0': return hoursAgo + " часов назад"; break;
                }
                if (hoursAgo.ToString() == "11" || hoursAgo.ToString() == "12" || hoursAgo.ToString() == "13" || hoursAgo.ToString() == "14")
                {
                    return hoursAgo + " часов назад";
                }
                return hoursAgo + "";
            }
            else
            {
                if (minutesAgo.ToString() == "0")
                {
                    return "Только что"; 
                }
                switch (minutesAgo.ToString().Last())
                {
                    case '1': return minutesAgo + " минуту назад"; break;
                    case '2': case '3': case '4': return minutesAgo + " минуты назад"; break;
                    case '5': case '6': case '7': case '8': case '9': case '0': return minutesAgo + " минут назад"; break;
                }
                if (minutesAgo.ToString() == "11" || minutesAgo.ToString() == "12" || minutesAgo.ToString() == "13" || minutesAgo.ToString() == "14")
                {
                    return minutesAgo + " минут назад";
                }
                return minutesAgo + "";
            }
        }
    }
}
