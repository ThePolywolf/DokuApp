namespace DokuApp.Model.Solver
{
    internal class NakedPairStrategy : NakedSetStrategy
    {
        public NakedPairStrategy()
        {
            _multi = 2;
        }

        protected override string GetName()
        {
            return "Naked Pair";
        }
    }
}
