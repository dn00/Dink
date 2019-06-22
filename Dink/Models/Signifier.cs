using System;
using System.Collections.Generic;
using System.Text;

namespace Dink.States
{
    /// <summary>
    /// We use this class for data partaining to the current state of the game.
    /// </summary>
    public class Signifier
    {
        public String Color {get; set;}
        public ushort X { get; set; }
        public ushort Y { get; set; }
        //public Signifier(String color, ushort _x, ushort _y)
        //{
        //    Color = color;
        //    X = _x;
        //    Y = _y;
        //}
    }
}
