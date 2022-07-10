using System;

namespace Interfaces
{
    public interface ICharacterStat
    {
        public event Action<int, int> ClientOnStatUpdated;
    }
}