namespace DokuApp.Model.Solver
{
    internal class NakedQuadStrategy : NakedSetStrategy
    {
        public NakedQuadStrategy()
        {
            _multi = 4;
        }

        protected override string GetName()
        {
            return "Naked Quad";
        }
    }
}
