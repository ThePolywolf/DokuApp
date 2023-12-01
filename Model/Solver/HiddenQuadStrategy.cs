﻿namespace DokuApp.Model.Solver
{
    internal class HiddenQuadStrategy : HiddenSetStrategy
    {
        public HiddenQuadStrategy()
        {
            _multi = 4;
        }

        protected override string GetName()
        {
            return "Hidden Quads";
        }
    }
}
