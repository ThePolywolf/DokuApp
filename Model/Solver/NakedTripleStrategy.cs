namespace DokuApp.Model.Solver
{
    internal class NakedTripleStrategy : NakedSetStrategy
    {
        public NakedTripleStrategy()
        {
            _multi = 3;
        }

        protected override string GetName()
        {
            return "Naked Triple";
        }
    }
}
