using Loria.Core.Debug;
using System;
using System.Speech.Synthesis;

namespace Loria.Text
{
    public class LoriaVocalizer : IDisposable
    {
        private ILoggable LogManager;
        private SpeechSynthesizer Synthesizer;

        public LoriaVocalizer(ILoggable logManager = null)
        {
            LogManager = logManager;

            if (LogManager != null) logManager.WriteLog(LogType.INFO, "Start SpeechSynthesizer to default output audio device.");
            Synthesizer = new SpeechSynthesizer();
            Synthesizer.SetOutputToDefaultAudioDevice();
            Synthesizer.Rate = 0;

            // User have to install this voice in order to use it
            if (LogManager != null) LogManager.WriteLog(LogType.INFO, "Set SpeechSynthesizer voice to Virginie.");
            Synthesizer.SelectVoice("ScanSoft Virginie_Dri40_16kHz");
        }

        public void Dispose()
        {
            if (LogManager != null) LogManager.WriteLog(LogType.INFO, "Dispose SpeechSynthesizer.");
            Synthesizer.Dispose();
        }

        public void Speech(string textToSpeech)
        {
            if (LogManager != null) LogManager.WriteLog(LogType.INFO, "Speech '{0}'.", textToSpeech);
            Synthesizer.Speak(textToSpeech);
        }
    }
}
