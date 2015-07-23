using Loria.Dal;
using Loria.Dal.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Speech.Recognition;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Loria.Channels.VoiceChannel.In
{
    class Program
    {
        static void Main(string[] args)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                if (db.Channels.Any(x => x.Id == VoiceChannel.Install.Program.VoiceChannel.Id))
                {
                    Trigger sentenceRecognizedTrigger = db.Triggers.Include(x => x.TriggerItems)
                                                                   .Include(x => x.TriggerItems.Select(t => t.ReceipeIns))
                                                                   .FirstOrDefault(x => x.Id == VoiceChannel.Install.Program.SentenceRecognizedTriggerGuid);

                    if (sentenceRecognizedTrigger != null)
                    {
                        using (SpeechRecognitionEngine recognitionEngine = new SpeechRecognitionEngine(new CultureInfo("fr-FR")))
                        {
                            recognitionEngine.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(SpeechRecognized);
                            recognitionEngine.SetInputToDefaultAudioDevice();

                            Choices choices = new Choices();
                            choices.Add(sentenceRecognizedTrigger.TriggerItems.First().ReceipeIns.Select(x => x.Value).ToArray());

                            GrammarBuilder grammarBuilder = new GrammarBuilder();
                            grammarBuilder.Culture = new CultureInfo("fr-FR");
                            grammarBuilder.Append(choices);

                            recognitionEngine.UnloadAllGrammars();
                            recognitionEngine.LoadGrammar(new Grammar(grammarBuilder));

                            recognitionEngine.RecognizeAsync(RecognizeMode.Multiple);

                            while (Console.ReadLine() != "stop")
                            {
                                Thread.Sleep(1000);
                            }
                        }
                    }
                }
            }
        }

        static void SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            if (e.Result.Confidence >= 0.5f)
            {
                Console.WriteLine(e.Result.Text);
            }
        }
    }
}
