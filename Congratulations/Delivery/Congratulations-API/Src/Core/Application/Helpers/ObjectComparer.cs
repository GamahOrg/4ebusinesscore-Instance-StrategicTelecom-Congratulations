

namespace Congrats.Application.Helpers
{
    public static class ObjectComparer
    {
        public static string CompareObjects<T>(T dbObject, T requestObject)
        {
            if (dbObject == null || requestObject == null)
                return "One of the objects is null.";

            Type type = typeof(T);
            PropertyInfo[] properties = type.GetProperties();
            List<string> differences = new List<string>();

            foreach (var property in properties)
            {
                if (typeof(ICollection).IsAssignableFrom(property.PropertyType))
                {
                    // احصل على القيم من الخصائص
                    var dbCollection = property.GetValue(dbObject) as ICollection;
                    var requestCollection = property.GetValue(requestObject) as ICollection;

                    if (dbCollection != null && requestCollection != null)
                    {
                        if (dbCollection.Count != requestCollection.Count)
                        {

                            differences.Add($"Property '{property.Name}': Collection sizes differ. Database Count = {dbCollection.Count}, Request Count = {requestCollection.Count}");
                        }
                        else
                        {
                            // يمكنك إضافة منطق أكثر تفصيلاً لمقارنة عناصر المجموعات هنا
                        }
                    }
                    else if (dbCollection == null || requestCollection == null)
                    {
                        differences.Add($"Property '{property.Name}': One of the collections is null.");
                    }
                }
                else
                {
                    object dbValue = property.GetValue(dbObject);
                    object requestValue = property.GetValue(requestObject);

                    if ((dbValue == null && requestValue != null) || (dbValue != null && !dbValue.Equals(requestValue)))
                    {
                        differences.Add($"Property '{property.Name}': Database Value = {dbValue ?? "null"}, Request Value = {requestValue ?? "null"}");
                    }
                }
            }

            return differences.Any() ? string.Join("\n", differences) : "No differences found.";
        }

    }
}
