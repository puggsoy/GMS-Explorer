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
using System.Windows.Shapes;
using System.IO;
using System.Drawing;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing.Imaging;

namespace GMS_Explorer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<AssetItem> ItemList { get; set; }

        public MainWindow()
        {
            ItemList = new ObservableCollection<AssetItem>();

            InitializeComponent();

            Test();
        }

        private void Test()
        {
            FileStream fs = new FileStream("./ror_data.win", FileMode.Open, FileAccess.Read);

            using (BinaryReader br = new BinaryReader(fs))
            {
                GEN8.Load(br);
                GEN8 g = GEN8.Instance;
                Console.WriteLine(string.Format("Filename: {0}\nName: {1}\nDisplayName: {2}\nSteamAppID: {3}", g.Filename, g.Name, g.DisplayName, g.SteamAppID));

                TXTR.Load(br);

                SPRT.Load(br);
                List<Sprite> sprites = SPRT.Instance.Contents;
                ItemList.Clear();

                for (int i = 0; i < sprites.Count; i++)
                {
                    AssetItem ai = new AssetItem(sprites[i].Name, sprites[i]);
                    ItemList.Add(ai);
                }
            }

            fs.Close();
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AssetItem ai = (AssetItem)e.AddedItems[0];
            Console.WriteLine(ai.Name);

            Sprite sprite = (Sprite)ai.Asset;
            Bitmap bmp = sprite.GetFrames()[0];

            using (MemoryStream mem = new MemoryStream())
            {
                bmp.Save(mem, ImageFormat.Bmp);
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
