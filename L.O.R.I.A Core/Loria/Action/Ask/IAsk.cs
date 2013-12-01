using System;
using System.Speech.Recognition;

namespace Loria.Action.Ask
{
    public interface IAsk : IDisposable
    {
        Choices GetChoices();
        string Ask(string choice);
    }
}
