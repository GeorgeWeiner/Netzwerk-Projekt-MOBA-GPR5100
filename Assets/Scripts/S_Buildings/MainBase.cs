using Mirror;
using S_Manager;

namespace S_Buildings
{
    public class MainBase : Building
    {
        public override void OnStartServer()
        {
            health.ServerOnDie += ServerHandleDie;
        }

        public override void OnStopServer()
        {
            health.ServerOnDie -= ServerHandleDie;
        }

        [Server]
        protected override void ServerHandleDie()
        {
            GameOverManager.HandleGameOver();
            base.ServerHandleDie();
        }
    }
}