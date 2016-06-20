using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace widget3.Services.Abstract
{
    public interface IMapperService
    {
        TInto Map<TFrom, TInto>(TFrom from);
        TInto Map<TFrom, TInto>(TFrom from, Action<TFrom, TInto> customCast);
    }
}
