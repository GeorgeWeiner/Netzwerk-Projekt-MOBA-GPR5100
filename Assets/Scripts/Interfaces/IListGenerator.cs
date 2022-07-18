using System.Collections.Generic;
using S_Combat;

namespace Interfaces
{
    public interface IListGenerator
    {
        public List<Health> GetList<T>();
    }
}