using System;
using System.Speech.Recognition;

namespace Loria.Action.Ask
{
    public interface IAskAction : IDisposable
    {
        Choices GetChoices();
        string Ask(string choice);
    }
}
