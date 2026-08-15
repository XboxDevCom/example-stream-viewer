using System;
using System.Collections.Generic;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using StreamViewer.Models;

namespace StreamViewer
{
    /// <summary>
    /// Hauptseite der Stream-Viewer-Anwendung.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private List<StreamPlatform> _platforms;
        private StreamPlatform _selectedPlatform;

        /// <summary>
        /// Initialisiert eine neue Instanz der MainPage-Klasse.
        /// </summary>
        public MainPage()
        {
            this.InitializeComponent();

            _platforms = StreamPlatform.GetPlatforms();

            foreach (var platform in _platforms)
            {
                PlatformSelector.Items.Add(platform.DisplayName);
            }

            if (PlatformSelector.Items.Count > 0)
            {
                PlatformSelector.SelectedIndex = 0;
            }

            CreatePresetChannelButtons();

            this.Loaded += MainPage_Loaded;
        }

        /// <summary>
        /// Wird aufgerufen, wenn die Seite geladen wurde. Setzt den anfänglichen Fokus.
        /// </summary>
        /// <param name="sender">Die Quelle des Ereignisses.</param>
        /// <param name="e">Ereignisdaten.</param>
        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            PlatformSelector.Focus(FocusState.Programmatic);
        }

        /// <summary>
        /// Erstellt die Schaltflächen für beliebte Kanäle dynamisch.
        /// </summary>
        private void CreatePresetChannelButtons()
        {
            string[] presets = { "shroud", "xQc", "pokimane", "summit1g", "caseoh" };
            Button previousButton = null;

            for (int i = 0; i < presets.Length; i++)
            {
                var button = new Button
                {
                    Content = presets[i],
                    MinHeight = 48,
                    MinWidth = 120,
                    Margin = new Thickness(6, 0, 6, 0),
                    Tag = presets[i]
                };

                button.Click += PresetChannel_Click;

                if (previousButton != null)
                {
                    previousButton.XYFocusRight = button;
                    button.XYFocusLeft = previousButton;
                }

                PresetChannels.Children.Add(button);
                previousButton = button;
            }
        }

        /// <summary>
        /// Wird aufgerufen, wenn sich die Plattformauswahl ändert.
        /// </summary>
        /// <param name="sender">Die Quelle des Ereignisses.</param>
        /// <param name="e">Ereignisdaten zur Auswahländerung.</param>
        private void PlatformSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (PlatformSelector.SelectedIndex >= 0 && PlatformSelector.SelectedIndex < _platforms.Count)
            {
                _selectedPlatform = _platforms[PlatformSelector.SelectedIndex];
                CurrentPlatformText.Text = "Plattform: " + _selectedPlatform.DisplayName;
            }
        }

        /// <summary>
        /// Wird aufgerufen, wenn auf die Ansehen-Schaltfläche geklickt wird.
        /// </summary>
        /// <param name="sender">Die Quelle des Ereignisses.</param>
        /// <param name="e">Ereignisdaten.</param>
        private void WatchButton_Click(object sender, RoutedEventArgs e)
        {
            WatchStream();
        }

        /// <summary>
        /// Wird aufgerufen, wenn in das Kanalname-Eingabefeld eine Taste gedrückt wird.
        /// Löst das Ansehen des Streams aus, wenn die Eingabetaste gedrückt wird.
        /// </summary>
        /// <param name="sender">Die Quelle des Ereignisses.</param>
        /// <param name="e">Ereignisdaten zur Tasteneingabe.</param>
        private void ChannelInput_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter)
            {
                WatchStream();
                e.Handled = true;
            }
        }

        /// <summary>
        /// Wird aufgerufen, wenn auf eine Kanal-Voreinstellungsschaltfläche geklickt wird.
        /// Setzt den Kanalnamen und startet die Wiedergabe.
        /// </summary>
        /// <param name="sender">Die Quelle des Ereignisses.</param>
        /// <param name="e">Ereignisdaten.</param>
        private void PresetChannel_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is string channelName)
            {
                ChannelInput.Text = channelName;
                WatchStream();
            }
        }

        /// <summary>
        /// Wird aufgerufen, wenn die WebView-Navigation abgeschlossen wurde.
        /// </summary>
        /// <param name="sender">Die Quelle des Ereignisses.</param>
        /// <param name="args">Ereignisdaten zur Navigation.</param>
        private void StreamWebView_NavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            if (!args.IsSuccess)
            {
                CurrentChannelText.Text = "Fehler beim Laden des Streams";
            }
        }

        /// <summary>
        /// Startet die Wiedergabe des Streams mit der ausgewählten Plattform und dem eingegebenen Kanalnamen.
        /// </summary>
        private void WatchStream()
        {
            string channelName = ChannelInput.Text?.Trim();

            if (string.IsNullOrEmpty(channelName))
            {
                CurrentChannelText.Text = "Bitte gib einen Kanalnamen ein";
                return;
            }

            if (_selectedPlatform == null)
            {
                return;
            }

            string embedUrl = _selectedPlatform.GetEmbedUrl(channelName);

            try
            {
                StreamWebView.Source = new Uri(embedUrl);

                EmptyState.Visibility = Visibility.Collapsed;
                StreamWebViewBorder.Visibility = Visibility.Visible;

                CurrentChannelText.Text = "Kanal: " + channelName;
            }
            catch (Exception)
            {
                CurrentChannelText.Text = "Ungültige URL für den Kanal";
            }
        }
    }
}
