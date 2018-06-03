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

namespace GMS_Explorer
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

        private void Test(object sender, RoutedEventArgs e)
        {
            FileStream fs = new FileStream("./ror_data.win", FileMode.Open, FileAccess.Read);

            using (BinaryReader br = new BinaryReader(fs))
            {
                GEN8.Load(br);
                GEN8 g = GEN8.Instance;
                Console.WriteLine(string.Format("Filename: {0}\nName: {1}\nDisplayName: {2}\nSteamAppID: {3}", g.Filename, g.Name, g.DisplayName, g.SteamAppID));

                TXTR.Load(br);
                TXTR.Instance.GetBitmap(0).Save("sheet.png");

                SPRT.Load(br);
                Sprite spr = SPRT.Instance.GetSprite(477);
                string name = spr.Name;
                Bitmap[] bitmaps = spr.GetFrames();

                for (int i = 0; i < bitmaps.Length; i++)
                {
                    string outDir = "./" + name;
                    Directory.CreateDirectory(outDir);
                    bitmaps[i].Save(string.Format("{0}/{1}_{2}.png", outDir, name, i));
                }
            }

            fs.Close();
        }
    }
}
