namespace BuffSystem
{
    public abstract class Buff
    {
        public readonly string BuffName; 

        protected PlayerScripts.Player Player;

        public Buff(string name, float duration)
        {
            this.BuffName = name;
            this.Player = PlayerScripts.Player.Instance;
        }
    
        public abstract void Apply();
    
        public abstract void Remove();
    }
}