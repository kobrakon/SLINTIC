using SLINTIC;

namespace SLINTIC
{
    public class ActiveScript
    {
        private protected Interval I;
        private protected string instance { get; private set; }

        // instance constructor
        protected ActiveScript()
        {
            this.OnLoad();
            this.I = new Interval(() => this.Update(), 5);
            this.instance = this.GetType().Name;
            this.PostLoad();
        }

        #pragma warning disable CS0626 // I don't need your suggestions
        protected extern void OnLoad();
        protected extern void PostLoad();
        protected extern void Update();
        protected void SetUpdate(int nint) => this.I.interval = nint;
    }
}