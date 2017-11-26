using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace LateNightStupidities.IIImaBackupViewer.View.Emoji
{
    internal class EmojiString
    {
        private class EmojiStringPart
        {
        }

        private class TextPart : EmojiStringPart
        {
            public string Content { get; set; }
        }

        private class EmojiPart : EmojiStringPart
        {
            public string FileName { get; set; }
        }

        private readonly string source;

        private List<EmojiStringPart> Parts { get; set; }

        public EmojiString(string text)
        {
            this.source = text;
        }

        private void CreateParts()
        {
            this.Parts = new List<EmojiStringPart>();
            string workingSource = this.source;

            bool replacedEmoji = true;
            Emoji foundEmoji = null;
            int foundEmojiIndex = -1;
            while (replacedEmoji && workingSource.Length > 0)
            {
                replacedEmoji = false;

                // Find emoji with lowest index in string.
                foreach (Emoji emoji in EmojiTextBlock.EmojiDictionary)
                {
                    int emojiIndex = workingSource.IndexOf(emoji.Code, StringComparison.Ordinal);
                    if (emojiIndex == -1)
                    {
                        continue;
                    }

                    if (foundEmojiIndex == -1 || emojiIndex < foundEmojiIndex)
                    {
                        foundEmoji = emoji;
                        foundEmojiIndex = emojiIndex;
                    }
                }

                // Replace emoji with lowest index in string.
                if (foundEmoji != null)
                {
                    if (foundEmojiIndex != 0)
                    {
                        this.Parts.Add(new TextPart { Content = workingSource.Substring(0, foundEmojiIndex) });
                    }

                    this.Parts.Add(new EmojiPart { FileName = foundEmoji.File });
                    workingSource = workingSource.Substring(foundEmojiIndex + foundEmoji.Code.Length);

                    // Look for another emoji.
                    replacedEmoji = true;
                    foundEmoji = null;
                    foundEmojiIndex = -1;
                }
            }

            // No more emojis found, add the rest of the string as plain text.
            if (!string.IsNullOrEmpty(workingSource))
            {
                this.Parts.Add(new TextPart { Content = workingSource });
            }
        }

        public void AddInlines(InlineCollection inlines)
        {
            if (this.Parts == null)
            {
                this.CreateParts();
            }

            foreach (EmojiStringPart part in this.Parts)
            {
                switch (part)
                {
                    case EmojiPart emoji:
                        Image image = new Image
                        {
                            Source = new BitmapImage(new Uri(emoji.FileName)),
                            Height = 16,
                            Width = 16,
                            Margin = new Thickness(0)
                        };
                        RenderOptions.SetBitmapScalingMode(image, BitmapScalingMode.HighQuality);
                        inlines.Add(image);
                        break;
                    case TextPart text:
                        inlines.Add(new Run(text.Content));
                        break;
                }
            }
        }
    }
}