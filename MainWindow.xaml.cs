using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BeatMaker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string filename;
        NAudio.Wave.AudioFileReader reader;
        NAudio.Wave.WaveOut waveOut;
        Timer t;
        DateTime start;
        List<string> outputs;
        bool down = false;

        public MainWindow()
        {
            InitializeComponent();
            t = new Timer();
            t.Interval = 5000;
            t.Tick += T_Tick;
        }

        private void T_Tick(object sender, EventArgs e)
        {
            t.Stop();
            this.Dispatcher.Invoke(() =>
            {
                start = DateTime.Now;
                waveOut.Play();
            });
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(reader != null)
            {
                t.Start();
                outputs = new List<string>() { "Starting song in 5 seconds." };
                output.Text = String.Join(System.Environment.NewLine, outputs);
                input.Focus();
            }
        }

        private void find_Click(object sender, RoutedEventArgs e)
        {
            var ofd = new OpenFileDialog();
            if(ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                filename = ofd.FileName;
                reader = new NAudio.Wave.AudioFileReader(filename);
                waveOut = new NAudio.Wave.WaveOut(); // or WaveOutEvent()
                waveOut.Init(reader);
                locationField.Text = ofd.FileName;
            }
        }

        void HandleKey(String key, bool pressed)
        {
            if (reader != null && waveOut != null)
            {
                if (waveOut.PlaybackState == NAudio.Wave.PlaybackState.Playing)
                {
                    var elapsed = (DateTime.Now - start).TotalSeconds;
                    string next = elapsed + " - " + key + (pressed ? " onset" : " offset");
                    outputs.Add(next);
                    output.Text = String.Join(System.Environment.NewLine, outputs);
                }
            }
        }

        private void input_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if(!down)
            {
                HandleKey(e.Key.ToString(), true);
                input.Text = "";
                down = true;
            }
        }

        private void input_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            HandleKey(e.Key.ToString(), false);
            input.Text = "";
            down = false;
        }
    }
}
