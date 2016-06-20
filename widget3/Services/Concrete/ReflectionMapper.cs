using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using widget3.Services.Abstract;

namespace widget3.Services.Concrete
{
    public class ReflectionMapper : IMapperService
    {
        public TInto Map<TFrom, TInto>(TFrom from)
        {
            if (from == null)
            {
                throw new ArgumentNullException("from");
            }

            Type intoType = typeof(TInto);
            Type fromType = from.GetType();
            object into = Activator.CreateInstance(intoType);
            var properties = fromType.GetProperties();
            foreach (var property in properties)
            {
                var intoProperty = intoType.GetProperty(property.Name);
                if (intoProperty != null)
                {
                    if (property.PropertyType.IsAssignableFrom(intoProperty.PropertyType))
                    {
                        intoProperty.SetValue(into, property.GetValue(from));
                    }
                }
            }
            return (TInto)into;
        }

        public TInto Map<TFrom, TInto>(TFrom from, Action<TFrom, TInto> customCast)
        {
            if (customCast == null)
            {
                throw new ArgumentNullException("customCast");
            }

            TInto into = Map<TFrom, TInto>(from);
            customCast(from, into);
            return into;
        }
    }
}
