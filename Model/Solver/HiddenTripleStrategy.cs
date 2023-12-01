using DokuApp.Model.Data;

namespace DokuApp.Model.Solver
{
    internal class HiddenTripleStrategy : HiddenSetStrategy
    {
        public HiddenTripleStrategy() 
        {
            _multi = 3;
        }

        protected override string GetName()
        {
            return "Hidden Triples";
        }
    }
}