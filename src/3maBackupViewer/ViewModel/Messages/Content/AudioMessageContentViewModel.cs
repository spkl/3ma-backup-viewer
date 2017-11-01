using System;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using LateNightStupidities.IIImaBackupReader.Messages;

namespace LateNightStupidities.IIImaBackupViewer.ViewModel.Messages.Content
{
    public class AudioMessageContentViewModel : MessageContentViewModel<AudioMessage>
    {
        public DefaultCommand PlayCommand { get; }

        public DefaultCommand PauseCommand { get; }

        public string Position
        {
            get
            {
                if (this.mediaPlayer == null)
                {
                    return "00:00 / 00:00";
                }

                return $"{this.mediaPlayer.Position:mm\\:ss} / {this.mediaPlayer.NaturalDuration.TimeSpan:mm\\:ss}";
            }
        }

        private MediaPlayer mediaPlayer;

        private DispatcherTimer timer;

        private string tempFile;

        public AudioMessageContentViewModel(AudioMessage message, MessageViewModel parent) 
            : base(message, parent)
        {
            this.PlayCommand = new DefaultCommand(o => this.Play(), true);
            this.PauseCommand = new DefaultCommand(o => this.Pause(), false);
        }

        private void Play()
        {
            if (this.mediaPlayer == null)
            {
                this.tempFile = Path.Combine(App.TempPath, $"audio{Guid.NewGuid()}.m4a");
                Directory.CreateDirectory(App.TempPath);
                File.Copy(this.Message.FilePath, tempFile, true);

                this.mediaPlayer = new MediaPlayer();
                this.mediaPlayer.MediaEnded += this.OnMediaEnded;
                this.mediaPlayer.MediaFailed += this.OnMediaFailed;
                this.mediaPlayer.Open(new Uri(tempFile));
                this.mediaPlayer.Play();

                this.PlayCommand.SetCanExecute(false);
                this.PauseCommand.SetCanExecute(true);

                this.timer = new DispatcherTimer();
                this.timer.Interval = TimeSpan.FromMilliseconds(400);
                this.timer.Tick += this.OnTick;
                this.timer.Start();
            }
            else
            {
                this.mediaPlayer.Play();

                this.PlayCommand.SetCanExecute(false);
                this.PauseCommand.SetCanExecute(true);
            }
        }

        private void Pause()
        {
            this.mediaPlayer.Pause();
            this.PlayCommand.SetCanExecute(true);
            this.PauseCommand.SetCanExecute(false);
        }

        private void OnTick(object sender, EventArgs eventArgs)
        {
            this.OnPropertyChanged(nameof(this.Position));
        }

        private void OnMediaFailed(object sender, ExceptionEventArgs exceptionEventArgs)
        {
            MessageBox.Show(
                $"Audio playback failed: {exceptionEventArgs.ErrorException.Message}", 
                "Error",
                MessageBoxButton.OK, 
                MessageBoxImage.Error);

            this.EndPlayback();
        }

        private void OnMediaEnded(object sender, EventArgs eventArgs)
        {
            this.EndPlayback();
        }

        private void EndPlayback()
        {
            this.timer.Stop();
            this.OnPropertyChanged(nameof(this.Position));
            this.mediaPlayer.Close();
            this.mediaPlayer = null;
            this.PlayCommand.SetCanExecute(true);
            this.PauseCommand.SetCanExecute(false);
            if (File.Exists(this.tempFile))
            {
                File.Delete(this.tempFile);
            }
        }
    }
}