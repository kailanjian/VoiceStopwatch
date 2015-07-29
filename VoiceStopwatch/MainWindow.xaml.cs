using System;
using System.Windows;
using System.Speech.Recognition;
using System.Windows.Threading;
using System.Diagnostics;
using System.Globalization;

namespace SpeechRecognition
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static Stopwatch stopwatch = new Stopwatch();
        static SpeechRecognitionEngine voiceEngine;
        public MainWindow()
        {
            InitializeComponent();


            voiceEngine = new SpeechRecognitionEngine(new CultureInfo("en-US"));
            Choices choices = new Choices(new string[] { "time", "reset", "stop", "start" });
            Grammar grammar = new Grammar(new GrammarBuilder(choices));
            voiceEngine.LoadGrammar(grammar);

            voiceEngine.SetInputToDefaultAudioDevice();
            voiceEngine.RecognizeAsync(RecognizeMode.Multiple);

            DispatcherTimer timer = new DispatcherTimer();
            timer.Start();

            // Create Events
            timer.Tick += new EventHandler(timer_Tick);
            voiceEngine.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(voiceEngine_SpeechRecognized);
            Closed += new EventHandler(this_Closed);
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            timerLabel.Content = String.Format("{0:00}:{1:00}",stopwatch.Elapsed.Minutes,stopwatch.Elapsed.Seconds);
        }

        private void this_Closed(object sender, EventArgs e)
        {
            voiceEngine.Dispose();
        }

        private static void voiceEngine_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            if (e.Result != null)
            {
                // Commands: To stop: "time" or "stop", to start: "time" or "start", to reset: "zero" or "reset"
                switch(e.Result.Text)
                {
                    case "stop":
                        stopwatch.Stop();
                        break;
                    case "start":
                        stopwatch.Start();
                        break;
                    case "reset":
                    case "zero":
                        stopwatch.Reset();
                        break;
                    case "time":
                        {
                            if (stopwatch.IsRunning)
                                stopwatch.Stop();
                            else
                                stopwatch.Start();
                        }
                        break;
                }
            }
            else
                MessageBox.Show("No result");

        }
    }
}
