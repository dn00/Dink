using System;
using System.Collections.Generic;
using System.Text;

namespace Dink.States.Enum
{
    public enum BotState
    {
        L2R_DEAD = 0,
        MAIN_INGAME_SCREEN = 1,
        GOTO_ELITE = 2,
        GOTO_FIELD = 3,
        RUN_ELITE = 4,
        RUN_FIELD = 5,
        RUN_SUBQUESTS = 6,
        RUN_WEEKLIES = 7,
        RUN_RESPAWN_ELITE = 8,
        RUN_RESPAWN_FIELD = 9
    }
}
