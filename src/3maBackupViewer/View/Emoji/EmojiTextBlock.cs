using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace LateNightStupidities.IIImaBackupViewer.View.Emoji
{
    public class EmojiTextBlock
    {
        public static readonly DependencyProperty EmojiTextProperty = DependencyProperty.RegisterAttached(
            "EmojiText", 
            typeof(string),
            typeof(EmojiTextBlock), 
            new PropertyMetadata(PropertyChangedCallback));

        public static bool Initialized { get; private set; }

        public static List<Emoji> EmojiDictionary { get; } = new List<Emoji>();

        public static string GetEmojiText(TextBlock target)
        {
            return (string)target.GetValue(EmojiTextProperty);
        }

        public static void SetEmojiText(TextBlock target, string value)
        {
            target.SetValue(EmojiTextProperty, value);
        }

        private static void PropertyChangedCallback(DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            if (!Initialized)
            {
                Initialize();
            }

            TextBlock textBlock = (TextBlock) dependencyObject;
            string newValue = (string) dependencyPropertyChangedEventArgs.NewValue;

            textBlock.Inlines.Clear();
            new EmojiString(newValue).AddInlines(textBlock.Inlines);
        }

        private static void Initialize()
        {
            string assemblyDir = Path.GetDirectoryName(typeof(EmojiTextBlock).Assembly.Location);
            string emojiDir = Path.Combine(assemblyDir, "Emoji");

            if (!Directory.Exists(emojiDir))
            {
                // No emojis installed. Skip initialization and don't use emojis.
                Initialized = true;
                return;
            }

            foreach (string file in Directory.GetFiles(emojiDir, "*.png"))
            {
                string searchStringInHex = Path.GetFileNameWithoutExtension(file);
                string searchString = string.Join(string.Empty,
                    searchStringInHex.Split('-').Select(hex => char.ConvertFromUtf32(Convert.ToInt32(hex, 16))));
                EmojiDictionary.Add(new Emoji {Code = searchString, File = file});
            }

            // Add all emojis that end with fe0f also without the fe0f part.
            // Add them at the end of the list, so the complete code emojis (with fe0f oder other modifiers)
            // will be found first (before these ones without fe0f).
            foreach (string file in Directory.GetFiles(emojiDir, "*-fe0f.png"))
            {
                string fileNameWithoutExt = Path.GetFileNameWithoutExtension(file);
                string searchStringInHex = fileNameWithoutExt.Substring(0, fileNameWithoutExt.Length - 5);
                string searchString = string.Join(string.Empty,
                    searchStringInHex.Split('-').Select(hex => char.ConvertFromUtf32(Convert.ToInt32(hex, 16))));
                EmojiDictionary.Add(new Emoji { Code = searchString, File = file });
            }

            Initialized = true;
        }
    }
}