using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Speech.Recognition;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Loria.Core.Senses
{
    public class HearingSense : ISense
    {
        private const string SenseCulture = "fr-FR";
        private SpeechRecognitionEngine RecognitionEngine;
        private List<string> Stimulus;
        private bool IsRunning;

        public HearingSense()
        {
            RecognitionEngine = new SpeechRecognitionEngine(new CultureInfo(SenseCulture));
            RecognitionEngine.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(SpeechRecognized);
            RecognitionEngine.SetInputToDefaultAudioDevice();

            Stimulus = new List<string>();
        }

        public void AddStimulus(params string[] stimulus)
        {
            Stimulus.AddRange(stimulus);

            Choices choices = new Choices();
            choices.Add(Stimulus.ToArray());

            GrammarBuilder grammarBuilder = new GrammarBuilder();
            grammarBuilder.Culture = new CultureInfo(SenseCulture);
            grammarBuilder.Append(choices);

            RecognitionEngine.UnloadAllGrammars();
            RecognitionEngine.LoadGrammar(new Grammar(grammarBuilder));
        }

        public void StartSensing()
        {
            RecognitionEngine.RecognizeAsync(RecognizeMode.Multiple);
        }

        public void StopSensing()
        {
            RecognitionEngine.RecognizeAsyncCancel();
        }

        private void SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            if (e.Result.Confidence >= 0.5f)
            {
                if (StimulusRecognized != null) StimulusRecognized(e.Result.Text);
            }
        }

        public event StimulusRecognizedEventHandler StimulusRecognized;
    }
}
