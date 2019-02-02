using System;

namespace VoronoiLib.Structures
{
    interface FortuneEvent : IComparable<FortuneEvent>
    {
        float X { get; }
        float Y { get; }
    }
}
