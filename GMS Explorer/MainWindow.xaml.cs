﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Drawing;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing.Imaging;
using Microsoft.Win32;

namespace GMS_Explorer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<AssetItem> SpriteList { get; set; }
        public ObservableCollection<AssetItem> BackgroundList { get; set; }

        public MainWindow()
        {
            SpriteList = new ObservableCollection<AssetItem>();
            BackgroundList = new ObservableCollection<AssetItem>();

            InitializeComponent();
        }

        private void OpenFile(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("FOO");

            OpenFileDialog dialog = new OpenFileDialog
            {
                Title = "Select data.win to open",
                CheckPathExists = true,
                CheckFileExists = true,
                Filter = "Game Maker .win files|*.win",
                FilterIndex = 0,
                Multiselect = false
            };

            Stream fs = null;

            if (dialog.ShowDialog() == true)
            {
                fs = dialog.OpenFile();
            }

            using (BinaryReader br = new BinaryReader(fs))
            {
                GEN8.Load(br);
                //GEN8 g = GEN8.Instance;
                //Console.WriteLine(string.Format("Filename: {0}\nName: {1}\nDisplayName: {2}\nSteamAppID: {3}", g.Filename, g.Name, g.DisplayName, g.SteamAppID));

                TXTR.Load(br);
                SPRT.Load(br);
                BGND.Load(br);
            }

            List<Sprite> sprites = SPRT.Instance.Contents;
            SpriteList.Clear();

            for (int i = 0; i < sprites.Count; i++)
            {
                Sprite spr = sprites[i];
                if (spr.GetFrames().Length < 1)
                    continue;

                AssetItem ai = new AssetItem(spr.Name, spr);
                SpriteList.Add(ai);
            }

            List<Background> backgrounds = BGND.Instance.Contents;
            BackgroundList.Clear();

            for (int i = 0; i < backgrounds.Count; i++)
            {
                AssetItem ai = new AssetItem(backgrounds[i].Name, backgrounds[i]);
                BackgroundList.Add(ai);
            }

            GEN8 g = GEN8.Instance;
            GameInfoText.Inlines.Clear();
            GameInfoText.Inlines.Add(new Bold(new Run("Name: ")));
            GameInfoText.Inlines.Add(new Run(g.DisplayName + "\n"));
            GameInfoText.Inlines.Add(new Bold(new Run("Development Name: ")));
            GameInfoText.Inlines.Add(new Run(g.Name + "\n"));
            GameInfoText.Inlines.Add(new Bold(new Run("Version: ")));
            GameInfoText.Inlines.Add(new Run(string.Format("{0}.{1}\n", g.MajorVersion, g.MinorVersion)));
            GameInfoText.Inlines.Add(new Bold(new Run("Build Version: ")));
            GameInfoText.Inlines.Add(new Run(g.BuildVersion + "\n"));
            GameInfoText.Inlines.Add(new Bold(new Run("Release Version: ")));
            GameInfoText.Inlines.Add(new Run(g.ReleaseVersion + "\n"));

            fs.Close();
        }

        private void SpriteListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RefreshSelection();
        }

        private void BackgroundListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RefreshSelection();
        }

        private void RefreshSelection()
        {
            AssetItem ai = null;
            Bitmap bmp = null;

            switch(ListTabControl.SelectedIndex)
            {
                case 0:
                    ai = (AssetItem)SpriteListBox.SelectedItem;
                    Sprite sprite = (Sprite)ai.Asset;
                    bmp = sprite.GetFrames()[0];
                    break;
                case 1:
                    ai = (AssetItem)BackgroundListBox.SelectedItem;
                    Background background = (Background)ai.Asset;
                    bmp = background.GetBitmap();
                    break;
            }

            using (MemoryStream mem = new MemoryStream())
            {
                bmp.Save(mem, ImageFormat.Png);
                mem.Position = 0;
                BitmapImage bmpImage = new BitmapImage();
                bmpImage.BeginInit();
                bmpImage.StreamSource = mem;
                bmpImage.CacheOption = BitmapCacheOption.OnLoad;
                bmpImage.EndInit();

                MainImage.Source = bmpImage;
                MainImage.Width = bmpImage.Width;
                MainImage.Height = bmpImage.Height;
            }
        }
    }

    public class AssetItem
    {
        public string Name { get; set; }

        public IChunkItem Asset { get; set; }

        public AssetItem(string name, IChunkItem asset)
        {
            Name = name;
            Asset = asset;
        }
    }
}
