namespace UIFlow
{
    public abstract class Transition
    {
        protected ViewController Current { get; private set; }
        protected ViewController Previous { get; private set; }
        protected float Duration { get; private set; }

        // Contructors

        public Transition(ViewController current, ViewController previous, float duration)
        {
            Current = current;
            Previous = previous;
            Duration = duration;
        }

        // Methods

        public abstract void Animate();
    }
}