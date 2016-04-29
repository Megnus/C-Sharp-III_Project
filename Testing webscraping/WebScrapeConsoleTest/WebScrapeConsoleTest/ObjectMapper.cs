using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WebScrapeConsoleTest
{
    /*
    http://stackoverflow.com/questions/16118085/best-practices-for-mapping-one-object-to-another
    http://stackoverflow.com/questions/737151/how-to-get-the-list-of-properties-of-a-class
    http://stackoverflow.com/questions/619767/set-object-property-using-reflection
    http://stackoverflow.com/questions/1089123/setting-a-property-by-reflection-with-a-string-value
    http://stackoverflow.com/questions/10264308/c-sharp-error-an-object-reference-is-required-for-the-non-static-field-method
    */
      
    public class ObjectMapper<T>
    {
        Dictionary<string, PropertyInfo> properties;

        public ObjectMapper()
        {
            //var t = typeof(T);
           // properties = t.GetProperties().ToDictionary(p => p.Name, p => p);
        }

        public static T Map(object obj)
        {
            //PropertyInfo[] objProperties = objGetProperties();
            Dictionary<string, PropertyInfo> prop = obj.GetType().GetProperties().ToDictionary(p => p.Name, p => p);
            
            var instance = Activator.CreateInstance(typeof(T));
            PropertyInfo[] instanceProp = instance.GetType().GetProperties();

            //PropertyInfo[] objProperties = obj.GetType().GetProperties();
            //PropertyInfo[] propertiesx = typeof(T).GetProperties();

            foreach (var p in instanceProp)
            {
                //  PropertyInfo prop = obj.GetType().GetProperty("Name", BindingFlags.Public | BindingFlags.Instance);
                if (null != p && p.CanWrite)
                {
                    p.SetValue(obj, prop[p.Name].GetValue(typeof(string), new object[]{}), null);
                }
            }
            return (T)instance;

               //var instance = Activator.CreateInstance(typeof(T));
               //foreach(var p in properties){
               //  p.SetValue(instance, Convert.ChangeType(p.pro, dto.Items[Array.IndexOf(dto.ItemsNames, p.Name)]);
               //  propertyInfo.SetValue(ship, Convert.ChangeType(value, propertyInfo.PropertyType), null);
               //}
               // return instance;
        }

        public static void GetProperites(object obj)
        {
            PropertyInfo[] properties = obj.GetType().GetProperties();
            properties.ToList().ForEach(x => Console.WriteLine(x.ToString()));

            foreach (var p in properties)
            {
                //  PropertyInfo prop = obj.GetType().GetProperty("Name", BindingFlags.Public | BindingFlags.Instance);
                if (null != p && p.CanWrite)
                {
                    p.SetValue(obj, "Magnus Sundström", null);
                }
            }

        }
    }
}
