using EverNoteApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Speech.Recognition;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using EverNoteApp.View;
using EverNoteLatest;
using System.IO;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using System.Net.Http;
using EverNoteLatest.Properties;

namespace EverNoteApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SpeechRecognitionEngine engine;
        private MainViewModel mvm;
        public MainWindow()
        {
            InitializeComponent();

            //Start:required for speech recognition
            engine = new SpeechRecognitionEngine();
            engine.SpeechRecognized += speechRecognizedHandler;
            engine.SetInputToDefaultAudioDevice();
            GrammarBuilder gmbuilder = new GrammarBuilder();
            gmbuilder.AppendDictation();
            engine.LoadGrammar(new Grammar(gmbuilder));
            //End:required for speech recognition
            //initialize font families
            var fontfamilies = Fonts.SystemFontFamilies.OrderBy(f => f.Source);
            fontFamilyBox.ItemsSource = fontfamilies;

            var fontsizes = new List<double>() { 8, 10, 12, 14, 16, 20, 24 };
            fontSizeBox.ItemsSource = fontsizes;
            mvm = this.Resources["vm"] as MainViewModel;
            everNoteMainConatainer.DataContext = mvm;
            MainViewModel.SelectedNoteEvent += MainViewModel_SelectedNoteEvent;
        }


        protected  override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
            if (string.IsNullOrEmpty(App.UserId))
            {
                LoginWindow loginWindow = new LoginWindow();
                loginWindow.ShowDialog();
            }
        }
        private void RichTextBoxContent_TextChanged(object sender, TextChangedEventArgs e)
        {
            //Console.WriteLine("From Code Behind");
            int wordsTyped = new TextRange(richTextBoxContent.Document.ContentStart, richTextBoxContent.Document.ContentEnd).Text.Length;
            //Console.WriteLine(wordsTyped);
            statusBar.Text = "No of Words are " + wordsTyped;
        }

        private void Bold_Button_Click(object sender, RoutedEventArgs e)
        {
            //below is done because speechButton.IsChecked can be null or boolean.
            //in case speechButton.IsChecked is null, set it to false.
            var boldButtonChecked = bold_Button.IsChecked ?? false;
            if (boldButtonChecked)
            {
                richTextBoxContent.Selection.ApplyPropertyValue(Inline.FontWeightProperty, FontWeights.Bold);
            }
            else
            {
                richTextBoxContent.Selection.ApplyPropertyValue(Inline.FontWeightProperty, FontWeights.Normal);
            }
        }

        private void SpeechButton_Click(object sender, RoutedEventArgs e)
        {
            //below is done because speechButton.IsChecked can be null or boolean.
            //in case speechButton.IsChecked is null, set it to false.
            bool isSppechButtonChecked = speechButton.IsChecked ?? false;
            if (isSppechButtonChecked)
            {
                engine.RecognizeAsync(RecognizeMode.Multiple);
            }
            else
            {
                engine.RecognizeAsyncStop();
            }
        }
        private void speechRecognizedHandler(object sender, SpeechRecognizedEventArgs  e)
        {
            string speechResult=e.Result.Text;
            //richTextBoxContent.Document.Blocks.Add(new Paragraph(new Run(speechResult)));
            FlowDocument flowDoc = new FlowDocument(new Paragraph(new Run(speechResult)));
            richTextBoxContent.Document = flowDoc;
        }

        private void RichTextBoxContent_SelectionChanged(object sender, RoutedEventArgs e)
        {
            //here we built in the checks to ensure if the selection of rich content text box 
            //changes, then the button is in the appropriate state.
            //for example if button is BOLD to start with and selection is changed to unbold
            //text, then the button should be UNBOLDED to reflect the correct state of the text.

            //DependencyProperty.UnsetValue just checks the property is not null.
            var selectedWeight = richTextBoxContent.Selection.GetPropertyValue(Inline.FontWeightProperty);
            bold_Button.IsChecked = (selectedWeight != DependencyProperty.UnsetValue) && selectedWeight.Equals(FontWeights.Bold);

            var selectedStyle = richTextBoxContent.Selection.GetPropertyValue(Inline.FontStyleProperty);
            italic_Button.IsChecked = (selectedStyle!=DependencyProperty.UnsetValue) && (selectedStyle.Equals(FontStyles.Italic));

            var selectedUnderline = richTextBoxContent.Selection.GetPropertyValue(Inline.TextDecorationsProperty);
            underline_Button.IsChecked = (selectedUnderline!=DependencyProperty.UnsetValue) && selectedUnderline.Equals(TextDecorations.Underline);

            fontFamilyBox.SelectedItem = richTextBoxContent.Selection.GetPropertyValue(Inline.FontFamilyProperty);
            //fontSizeBox.Text = richTextBoxContent.Selection.GetPropertyValue(Inline.FontSizeProperty) as string;

        }

        private void Italic_Button_Click(object sender, RoutedEventArgs e)
        {
            var isItalicButtonClicked = italic_Button.IsChecked ?? false;
            if (isItalicButtonClicked)
            {
                richTextBoxContent.Selection.ApplyPropertyValue(Inline.FontStyleProperty, FontStyles.Italic);
            }
            else
            {
                richTextBoxContent.Selection.ApplyPropertyValue(Inline.FontStyleProperty, FontStyles.Normal);
            }
        }

        private void Underline_Button_Click(object sender, RoutedEventArgs e)
        {
            var underlineButtonChecking = underline_Button.IsChecked ?? false;
            if (underlineButtonChecking)
            {
                richTextBoxContent.Selection.ApplyPropertyValue(Inline.TextDecorationsProperty, TextDecorations.Underline);
            }
            else
            {
                //text decorations property is acollection of property for an element
                //to remove underline property,  we need to remove that from the collection
                //hence below first underline is removed from the collection
                //and then that collection is added to the the property.

                TextDecorationCollection decoCollections;
                (richTextBoxContent.Selection.GetPropertyValue(Inline.TextDecorationsProperty) as TextDecorationCollection).TryRemove(TextDecorations.Underline,out decoCollections);
                richTextBoxContent.Selection.ApplyPropertyValue(Inline.TextDecorationsProperty, decoCollections);
            }

        }

        private void FontFamilyBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            richTextBoxContent.Selection.ApplyPropertyValue(Inline.FontFamilyProperty, fontFamilyBox.SelectedItem);
        }

        private void FontSizeBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Console.WriteLine("font size should be "+fontSizeBox.SelectedItem+" "+ fontSizeBox.Text);
            Console.WriteLine("richTextBoxContent.Selection.ToString()" + richTextBoxContent.Selection.ToString());
            if (!string.IsNullOrEmpty(fontSizeBox.Text))
            {
                richTextBoxContent.Selection.ApplyPropertyValue(Inline.FontSizeProperty, fontSizeBox.Text);
            }
        }

        private async void MainViewModel_SelectedNoteEvent(object sender, EventArgs e)
        {
            richTextBoxContent.Document.Blocks.Clear();
            if (mvm.SelectedNote != null && !string.IsNullOrEmpty(mvm.SelectedNote.FileLocation))
            {
                string filepath = Path.Combine(Environment.CurrentDirectory, mvm.SelectedNote.FileLocation);
                using (HttpClient client = new HttpClient())
                {
                    var response = await client.GetAsync(mvm.SelectedNote.FileLocation);
                    var dataFromCloud = await response.Content.ReadAsStreamAsync();
                    TextRange range = new TextRange(richTextBoxContent.Document.ContentStart, richTextBoxContent.Document.ContentEnd);
                    range.Load(dataFromCloud, DataFormats.Rtf);
                }
            }
        }
        /*
         * Process to upload file in cloud--
         * 1. Create a container in storage account, for this app container name is notes
         * 2. Upload the file from local to the container created in step 1.
         * 2.a Note data in container is uploaded as blob, with name as per the filename
         * 3. Post uploading data in container, get the url of the blob data from blob.
         * 4. This url is updated int he table as file location.
         * 5. For fetching data from the cloud file location, use httpclient.
         * 6. Get data as ReadStreamAsync and use it as you wish.
         */

        private async void SaveNotes_Click(object sender, RoutedEventArgs e)
        {
            if (mvm.SelectedNote != null)
            {
                string filename = $"{mvm.SelectedNote.Id}.rtf";
                string filepath = Path.Combine(Environment.CurrentDirectory,filename);

                using (FileStream fs = new FileStream(filepath, FileMode.Create))
                {
                    TextRange range = new TextRange(richTextBoxContent.Document.ContentStart, richTextBoxContent.Document.ContentEnd);
                    range.Save(fs, DataFormats.Rtf);
                }
                string filecloudurl = await uploadFileInCloud(filename, filepath);
                //here the notes table is updated with cloud file location.
                mvm.SelectedNote.FileLocation = filecloudurl;
                mvm.updateNote(mvm.SelectedNote);
            }
        }


        //this method uploads the file from local to cloud
        //file is uploaded as blob in the container for each note with the appropriate filename
        private async Task<string> uploadFileInCloud(string filename,string filepath)
        {
            string fileUrl = string.Empty;
            var account = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=evernotestorage;AccountKey=09xG/XI19z2MAjJ2+ftit/wOluuQRdLyGsHa3lkgh+9MZ/4gLr2Mr+nZRfJkie6CC3LDfHn6q5ffwCj/oM7QGw==;EndpointSuffix=core.windows.net");
            var client = account.CreateCloudBlobClient();
            var container = client.GetContainerReference("notes");
            var blob = container.GetBlockBlobReference(filename);
            using(FileStream fs=new FileStream(filepath,FileMode.Open))
            {
                await blob.UploadFromStreamAsync(fs);
                //returning the url of the file stored in cloud
                fileUrl = blob.Uri.OriginalString;

            }
            return fileUrl;
        }
    }
}
