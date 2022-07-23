using Interfaces;

namespace S_Combat
{
    public class Mana : Stat, ICharacterStat
    {
        public override void OnStartClient()
        {
            GameManager.Instance.OnPlayerDie += OnRevive;
            GameManager.Instance.OnRoundWon += OnRoundReset;
        }
        public void UseMana(int amount, out bool canUse)
        {
            canUse = currentValue - amount > 0;
            if (!canUse) return;
            
            currentValue -= amount;
        }

        private void OnDestroy()
        {
            GameManager.Instance.OnPlayerDie -= OnRevive;
            GameManager.Instance.OnRoundWon -= OnRoundReset;
        }

        private void OnRevive(MobaPlayerData player)
        {
            currentValue = MaxValue;
        }

        private void OnRoundReset()
        {
            currentValue = MaxValue;
        }
    }
}