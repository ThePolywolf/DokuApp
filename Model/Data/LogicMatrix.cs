namespace DokuApp.Model.Data
{
    class LogicMatrix
    {
        private bool[,] _truths;

        public bool[,] Truths { get { return  _truths; } }

        public LogicMatrix()
        {
            _truths = new bool[9, 9];
        }

        public LogicMatrix(bool[,] truths)
        {
            _truths = new bool[9, 9];

            SetLogic(truths);
        }

        public void SetLogic(bool[,] truths)
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    try
                    {
                        _truths[i, j] = truths[i, j];
                    }
                    catch
                    {
                        _truths[i, j] = false;
                    }
                }
            }
        }

        public bool Add(LogicMatrix matrix)
        {
            bool changed = false;

            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    bool newTruth = _truths[i, j] || matrix.Truths[i, j];

                    if (!changed && _truths[i, j] != newTruth)
                    {
                        changed = true;
                    }

                    _truths[i, j] = newTruth;
                }
            }

            return changed;
        }

        public bool Subtract(LogicMatrix matrix)
        {
            bool changed = false;

            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    bool newTruth = _truths[i, j] && !matrix.Truths[i, j];

                    if (!changed && _truths[i, j] != newTruth)
                    {
                        changed = true;
                    }

                    _truths[i, j] = newTruth;
                }
            }

            return changed;
        }

        public void Flip(LogicMatrix matrix)
        {
            if (Add(matrix))
            {
                return;
            }

            Subtract(matrix);
            return;
        }

        public LogicMatrix Inverted()
        {
            bool[,] truths = new bool[9, 9];

            for (int i = 0;i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    truths[i, j] = !_truths[i, j];
                }
            }

            return new LogicMatrix(truths);
        }
    }
}
