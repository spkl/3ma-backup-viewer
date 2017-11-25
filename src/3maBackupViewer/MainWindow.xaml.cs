using System;
using System.Windows;
using System.Windows.Controls;

namespace LateNightStupidities.IIImaBackupViewer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MessagesListBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 1)
            {
                this.MessagesListBox.ScrollIntoView(e.AddedItems[0]);
            }
        }
    }
}
