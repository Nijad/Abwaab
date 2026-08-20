namespace Abwaab.Application.Features.Properties.Queries.GetPropertyForUpdate
{
    public class WeekDay
    {
        public int DayIndex { get; set; }
        public string DayName { get; set; }

        public static List<WeekDay> GetWeekDaysList()
        {
            var list = new List<WeekDay>();
            list.Add(new() { 
                DayIndex = (int)DayOfWeek.Sunday, 
                DayName = DayOfWeek.Sunday.ToString() 
            });
            list.Add(new() { 
                DayIndex = (int)DayOfWeek.Monday, 
                DayName = DayOfWeek.Monday.ToString() 
            });
            list.Add(new() { 
                DayIndex = (int)DayOfWeek.Tuesday, 
                DayName = DayOfWeek.Tuesday.ToString() 
            });
            list.Add(new() { 
                DayIndex = (int)DayOfWeek.Wednesday, 
                DayName = DayOfWeek.Wednesday.ToString() 
            });
            list.Add(new() { 
                DayIndex = (int)DayOfWeek.Thursday, 
                DayName = DayOfWeek.Thursday.ToString() 
            });
            list.Add(new() { 
                DayIndex = (int)DayOfWeek.Friday, 
                DayName = DayOfWeek.Friday.ToString() 
            });
            list.Add(new() { 
                DayIndex = (int)DayOfWeek.Saturday, 
                DayName = DayOfWeek.Saturday.ToString() 
            });

            return list;
        }
    }
}
