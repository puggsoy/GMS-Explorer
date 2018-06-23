using System;
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
using System.IO;
using System.Drawing;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing.Imaging;
using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace GMS_Explorer
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public ObservableCollection<AssetItem> SpriteList { get; set; }
		public ObservableCollection<AssetItem> BackgroundList { get; set; }

		private AssetItem currentSelection = null;
		private int currentSpriteFrame = 0;

		public MainWindow()
		{
			SpriteList = new ObservableCollection<AssetItem>();
			BackgroundList = new ObservableCollection<AssetItem>();

			InitializeComponent();
		}

		private void OpenFile(object sender, RoutedEventArgs e)
		{
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
				fs = dialog.OpenFile();
			else
				return;

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
		}

		private void SpriteListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			currentSpriteFrame = 0;

			RefreshSelection();
		}

		private void BackgroundListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			RefreshSelection();
		}

		private void ListTabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			RefreshSelection();
		}

		private void RefreshSelection()
		{
			switch (ListTabControl.SelectedIndex)
			{
				case 0:
					currentSelection = (AssetItem)SpriteListBox.SelectedItem;
					PopulateSprite();
					break;
				case 1:
					currentSelection = (AssetItem)BackgroundListBox.SelectedItem;
					break;
			}
		}

		private void PrevBtn_Click(object sender, RoutedEventArgs e)
		{
			ChangeFrame(-1);
		}

		private void NextBtn_Click(object sender, RoutedEventArgs e)
		{
			ChangeFrame(1);
		}

		private void ChangeFrame(int amount)
		{
			if (currentSelection != null && currentSelection.Asset.GetType() == typeof(Sprite))
			{
				Sprite sprite = (Sprite)currentSelection.Asset;
				currentSpriteFrame += amount;

				while (currentSpriteFrame < 0)
				{
					currentSpriteFrame += sprite.FrameNum;
				}

				currentSpriteFrame %= sprite.FrameNum;

				PopulateSprite();
			}
		}

		private void PopulateSprite()
		{
			if (currentSelection == null)
				return;

			Sprite sprite = (Sprite)currentSelection.Asset;
			ApplyBmp(sprite.GetFrame(currentSpriteFrame));

			ControlPanel.Visibility = Visibility.Visible;
			FrameLabel.Content = string.Format("Frame {0}/{1}", currentSpriteFrame + 1, sprite.FrameNum);
		}

		private void ApplyBmp(Bitmap bmp)
		{
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

		private void ExportBtn_Click(object sender, RoutedEventArgs e)
		{
			if (currentSelection == null)
				return;

			CommonOpenFileDialog dialog = new CommonOpenFileDialog
			{
				Title = "Select where to save extracted files",
				IsFolderPicker = true,
				EnsurePathExists = true
			};

			string outDir = null;
			if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
				outDir = dialog.FileName;
			else
				return;

			Sprite sprite = (Sprite)currentSelection.Asset;

			outDir = Path.Combine(outDir, sprite.Name);
			Directory.CreateDirectory(outDir);
			Bitmap[] bmps = sprite.GetFrames();

			for (int i = 0; i < bmps.Length; i++)
			{
				string outPath = Path.Combine(outDir, sprite.Name);
				if (bmps.Length > 1)
					outPath += "_" + (i + 1);

				outPath = Path.ChangeExtension(outPath, "png");
				bmps[i].Save(outPath, ImageFormat.Png);
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
