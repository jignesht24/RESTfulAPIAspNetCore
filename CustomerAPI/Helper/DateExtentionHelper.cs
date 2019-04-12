using System;

namespace CustomerAPI
{
    public static class DateExtentionHelper
    {
        public static Int32 GetRelationAge(this DateTime createdDate)
        {
            var today = DateTime.Now;
            int age = today.Year - createdDate.Year;
            if (today < createdDate.AddYears(age))
            {
                age--;
            }
            return age;
        }
    }
}
