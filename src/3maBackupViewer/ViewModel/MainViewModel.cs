using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using Ionic.Zip;
using LateNightStupidities.IIImaBackupReader;
using LateNightStupidities.IIImaBackupViewer.Properties;
using Microsoft.Win32;

namespace LateNightStupidities.IIImaBackupViewer.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private string statusText;
        private bool isProgressIndeterminate;
        private double progressValue;
        private bool backupOpen;
        private ConversationViewModel selectedConversation;

        public string StatusText
        {
            get => this.statusText;
            set
            {
                this.statusText = value;
                this.OnPropertyChanged();
            }
        }

        public bool IsProgressIndeterminate
        {
            get => this.isProgressIndeterminate;
            set
            {
                this.isProgressIndeterminate = value;
                this.OnPropertyChanged();
            }
        }

        public double ProgressValue
        {
            get => this.progressValue;
            set
            {
                this.progressValue = value;
                this.OnPropertyChanged();
            }
        }

        public bool BackupOpen
        {
            get => this.backupOpen;
            set
            {
                this.backupOpen = value;
                this.OnPropertyChanged();
            }
        }

        public IIImaBackup CurrentBackup { get; set; }

        public ObservableCollection<ContactViewModel> Contacts { get; } = new ObservableCollection<ContactViewModel>();

        public ObservableCollection<ConversationViewModel> Conversations { get; } = new ObservableCollection<ConversationViewModel>();

        public ConversationViewModel SelectedConversation
        {
            get => this.selectedConversation;
            set
            {
                this.selectedConversation = value;
                this.OnPropertyChanged();
            }
        }

        #region Commands

        public ICommand OpenZipFileCommand => new RelayCommand(o => this.OpenZipFile());

        private void OpenZipFile()
        {
            OpenFileDialog ofd = new OpenFileDialog
            {
                Title = "Select Backup ZIP File",
                DefaultExt = "*.zip",
                Filter = "All Files|*.*|ZIP Files|*.zip",
                FilterIndex = 2,
                CheckFileExists = true,
                Multiselect = false
            };

            if (ofd.ShowDialog(Application.Current.MainWindow).GetValueOrDefault())
            {
                Task.Run(() => this.OpenZipFile(ofd.FileName));
            }
        }
        
        private void OpenZipFile(string fileName)
        {
            try
            {
                this.StatusText = "Checking ZIP file...";
                this.IsProgressIndeterminate = true;
                if (!ZipFile.CheckZip(fileName))
                {
                    MessageBox.Show("The ZIP archive is corrupted and cannot be opened.", "Error", MessageBoxButton.OK,
                        MessageBoxImage.Error);
                    this.StatusText = "";
                    this.IsProgressIndeterminate = false;
                    this.ProgressValue = 0;
                    return;
                }

                this.StatusText = "Enter password...";
                string password = Microsoft.VisualBasic.Interaction.InputBox("Enter the backup password:", "Enter Password");

                this.StatusText = "Extracting backup data...";
                string tempDir = Path.Combine(App.TempPath, Guid.NewGuid().ToString());
                ZipFile zipFile = ZipFile.Read(fileName);
                zipFile.Password = password;
                zipFile.ExtractProgress += this.ZipFileOnExtractProgress;
                zipFile.ExtractAll(tempDir, ExtractExistingFileAction.OverwriteSilently);

                string identity = null;
                string archiveName = Path.GetFileNameWithoutExtension(Path.GetFileName(fileName));
                if (archiveName.Contains("_") && archiveName.Length > archiveName.IndexOf('_') + 9)
                {
                    identity = archiveName.Substring(archiveName.IndexOf('_') + 1, 8);
                }

                this.OpenFolder(tempDir, identity, true);

                this.StatusText = $"Opened backup from ZIP file {Path.GetFileName(fileName)}.";
                this.IsProgressIndeterminate = false;
                this.ProgressValue = 0;
            }
            catch (ZipException e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                this.StatusText = "";
                this.IsProgressIndeterminate = false;
                this.ProgressValue = 0;
            }
        }

        private void ZipFileOnExtractProgress(object sender, ExtractProgressEventArgs extractProgressEventArgs)
        {
            if (extractProgressEventArgs.EventType == ZipProgressEventType.Extracting_BeforeExtractEntry)
            {
                this.IsProgressIndeterminate = false;
                this.ProgressValue = ((double)extractProgressEventArgs.EntriesExtracted / extractProgressEventArgs.EntriesTotal) * 100;
            }
        }

        public ICommand OpenFolderCommand => new RelayCommand(o => this.OpenFolder());

        private void OpenFolder()
        {
            OpenFileDialog ofd = new OpenFileDialog
            {
                Title = "Select Backup Identity File",
                Filter = "Identity File|identity",
                FilterIndex = 1,
                CheckFileExists = true,
                Multiselect = false
            };

            if (ofd.ShowDialog(Application.Current.MainWindow).GetValueOrDefault())
            {
                Task.Run(() => this.OpenFolder(Path.GetDirectoryName(ofd.FileName)));
            }
        }

        private void OpenFolder(string folder, string identity = null, bool isTempFolder = false)
        {
            try
            {
                this.IsProgressIndeterminate = true;
                this.StatusText = "Reading backup files...";
                identity = Microsoft.VisualBasic.Interaction.InputBox("Enter the 3ma ID:", "Enter ID", identity);
                this.CurrentBackup = new IIImaBackup(folder, identity);
                this.CurrentBackup.Read();
                this.BackupOpen = true;

                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    this.Contacts.Clear();
                    foreach (Contact contact in this.CurrentBackup.Contacts.Values)
                    {
                        this.Contacts.Add(new ContactViewModel(contact));
                    }

                    this.Conversations.Clear();
                    foreach (Conversation conversation in this.CurrentBackup.Conversations)
                    {
                        this.Conversations.Add(new ConversationViewModel(conversation));
                    }
                }));
            }
            finally
            {
                if (!isTempFolder)
                {
                    this.StatusText = $"Opened backup from folder {folder}.";
                }
                this.IsProgressIndeterminate = false;
                this.ProgressValue = 0;
            }
        }

        public ICommand CloseCommand => new RelayCommand(o => this.CloseBackup(), o => this.CanCloseBackup());

        private bool CanCloseBackup()
        {
            return this.CurrentBackup != null;
        }

        private void CloseBackup()
        {
            this.BackupOpen = false;
            this.CurrentBackup = null;
            this.Contacts.Clear();
            this.Conversations.Clear();
            this.StatusText = "Closed backup.";
        }

        public ICommand PrintCommand => new RelayCommand(o => this.Print(), o => this.CanPrint());

        private void Print()
        {
            Task.Run(() => this.ShowDialogAndSendToPrinter());
        }

        private void ShowDialogAndSendToPrinter()
        {
            bool ok = false;
            try
            {
                this.StatusText = "Preparing to print...";
                this.IsProgressIndeterminate = true;

                PrintDialog printDialog = null;
                Application.Current.Dispatcher.Invoke(() =>
                {
                    printDialog = new PrintDialog
                    {
                        CurrentPageEnabled = false,
                        SelectedPagesEnabled = false,
                        UserPageRangeEnabled = false
                    };

                    ok = printDialog.ShowDialog().GetValueOrDefault();
                });

                if (!ok)
                {
                    return;
                }

                ConversationViewModel conversationVm = this.SelectedConversation;

                ResourceDictionary messageTemplates =
                    new ResourceDictionary
                    {
                        Source = new Uri("View/MessageTemplates.xaml", UriKind.Relative)
                    };

                FlowDocument doc = null;
                Application.Current.Dispatcher.Invoke(() =>
                {
                    doc = new FlowDocument();
                    doc.PageWidth = printDialog.PrintableAreaWidth;
                    doc.PageHeight = printDialog.PrintableAreaHeight;
                    doc.FontFamily = new FontFamily("Calibri");
                });

                this.IsProgressIndeterminate = false;
                int item = 0;
                int count = conversationVm.Messages.Cast<object>().Count();
                foreach (object message in conversationVm.Messages)
                {
                    this.ProgressValue = (double)item / count * 100;

                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        ContentPresenter cp = new ContentPresenter();
                        cp.HorizontalAlignment = HorizontalAlignment.Stretch;
                        cp.Resources = messageTemplates;
                        cp.Content = message;

                        Block block = new BlockUIContainer(cp);
                        doc.Blocks.Add(block);
                    });

                    item++;
                }

                this.StatusText = "Sending to printer...";
                this.IsProgressIndeterminate = true;
                Thread.Sleep(TimeSpan.FromSeconds(0.1));

                Application.Current.Dispatcher.Invoke(() =>
                {
                    IDocumentPaginatorSource pageSource = doc;
                    printDialog.PrintDocument(pageSource.DocumentPaginator, $"3ma Conversation with {conversationVm.DisplayName}");
                });
            }
            finally
            {
                this.IsProgressIndeterminate = false;
                this.ProgressValue = 0;
                this.StatusText = ok ? "Sent job to printer." : "Canceled printing.";
            }
        }

        private bool CanPrint()
        {
            return this.SelectedConversation != null && !this.SelectedConversation.Messages.IsEmpty;
        }

        public ICommand ExitCommand => new RelayCommand(o => this.Exit());

        private void Exit()
        {
            Application.Current.Shutdown();
        }

        public ICommand AboutCommand => new RelayCommand(o => this.ShowAbout());

        private void ShowAbout()
        {
            MessageBox.Show(Resources.AboutText, "About", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        #endregion

        public MainViewModel()
        {
            this.StatusText = "Hello";
            this.IsProgressIndeterminate = false;
            this.ProgressValue = 0;
        }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}