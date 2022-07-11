using System;

namespace Interfaces
{
    public interface ICharacterStat
    {
        public event Action<StatType, int,int> ClientOnStatUpdated;
    }
}