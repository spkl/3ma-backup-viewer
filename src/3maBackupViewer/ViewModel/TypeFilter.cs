namespace LateNightStupidities.IIImaBackupViewer.ViewModel
{
    public class TypeFilter
    {
        public string Type { get; set; }

        public string DisplayText { get; set; }

        public TypeFilter(string type, string displayText)
        {
            this.Type = type;
            this.DisplayText = displayText;
        }

        public override string ToString() => this.DisplayText;
    }
}