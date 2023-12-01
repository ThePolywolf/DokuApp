using DokuApp.Model.Data;

namespace DokuApp.Model.Solver
{
    internal class HiddenPairStrategy : HiddenSetStrategy
    {
        public HiddenPairStrategy()
        {
            _multi = 2;
        }

        protected override string GetName()
        {
            return "Hidden Pairs";
        }
    }
}
