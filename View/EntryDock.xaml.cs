using System;
using System.Windows.Controls;

namespace DokuApp.View
{
    public partial class EntryDock : UserControl
    {
        private readonly EntryButton[] _buttons;

        public event EventHandler<int>? NumberRecieved;
        public event EventHandler<bool>? NewEntryMode;

        public EntryDock()
        {
            InitializeComponent();

            _buttons = new EntryButton[9] { b1, b2, b3, b4, b5, b6, b7, b8, b9 };

            foreach (EntryButton button in _buttons)
            {
                button.NumberButtonPressed += SendReceivedNumber;
            }

            Slider.NewEntryMode += SendNewEntryMode;
        }

        public void SetActivities(bool[] isActive)
        {
            for (int i = 0; i < 9; i++)
            {
                try
                {
                    _buttons[i].SetActivity(isActive[i]);
                }
                catch
                {
                    _buttons[i].SetActivity(true);
                }
            }
        }

        private void SendReceivedNumber(object? sender, int number)
        {
            NumberRecieved?.Invoke(this, number);
        }

        private void SendNewEntryMode(object? sender, bool newMode)
        {
            NewEntryMode?.Invoke(this, newMode);
        }

        public void Shift(bool doShift)
        {
            foreach (EntryButton button in _buttons)
            {
                button.Shift(doShift);
            }
        }
    }
}
