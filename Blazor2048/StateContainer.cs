using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blazor2048
{
    public class StateContainer
    {
        public Game2048 Game { get; set; } = new Game2048();

        public event Action? OnChange;

        public void SetProperty(Game2048 value)
        {
            Game = value;
            NotifyStateChanged();
        }

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
