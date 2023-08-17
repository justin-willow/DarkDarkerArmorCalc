using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DarkDarkerArmorCalc.Repositories;

internal interface IRepository<T>
{
    IEnumerable<T> Get();
}
