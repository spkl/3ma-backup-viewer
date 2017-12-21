using System.Windows.Controls;

namespace LateNightStupidities.IIImaBackupViewer.View
{
    internal class ScrollingListBox : ListBox
    {
        public ScrollingListBox()
        {
            this.SelectionChanged += this.OnSelectionChanged;
        }

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs selectionChangedEventArgs)
        {
            if (this.SelectedItem != null)
            {
                this.ScrollIntoView(this.SelectedItem);
            }
        }
    }
}