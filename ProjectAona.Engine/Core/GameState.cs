using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectAona.Engine.Core
{
    public static class GameState
    {
        public static GameStateType State = GameStateType.PLAYING;
    }

    public enum GameStateType
    {
        LOADING,
        PLAYING, // Idle?
        SELECTING,
        PAUSING,
        SAVING
    }
}
