using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace widget3.Services.Abstract
{
    public interface IMapperService<TFrom, TInto>
    {
        TInto Map(TFrom from);
        TInto Map(TFrom from, Action<TFrom, TInto> customCast);
    }
}
