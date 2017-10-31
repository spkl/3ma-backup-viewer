using System.ComponentModel;
using System.Runtime.CompilerServices;
using LateNightStupidities.IIImaBackupReader;

namespace LateNightStupidities.IIImaBackupViewer.ViewModel
{
    public class ContactViewModel : INotifyPropertyChanged
    {
        private Contact Contact { get; }

        public string Identity => this.Contact.Identity.ToString();

        public string DisplayName => this.Contact.DisplayName;

        public string CustomDisplayName
        {
            get => this.Contact.CustomDisplayName;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    value = null;
                }

                this.Contact.CustomDisplayName = value;
                this.OnPropertyChanged();
                this.OnPropertyChanged(nameof(this.DisplayName));
            }
        }

        public string Verification => this.Contact.Verification;

        public ContactViewModel(Contact contact)
        {
            this.Contact = contact;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}