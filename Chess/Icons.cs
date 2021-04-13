using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Chess
{
    static class Icons
    {
        private static readonly BitmapImage[] images = new BitmapImage[12];
        private static readonly List<Action> listeneres = new List<Action>();

        public static void LoadImages(string folderName)
        {
            images[0] = new BitmapImage(new Uri($@"{Environment.CurrentDirectory}\Resources\{folderName}\WKing.png"));
            images[1] = new BitmapImage(new Uri($@"{Environment.CurrentDirectory}\Resources\{folderName}\WQueen.png"));
            images[2] = new BitmapImage(new Uri($@"{Environment.CurrentDirectory}\Resources\{folderName}\WRook.png"));
            images[3] = new BitmapImage(new Uri($@"{Environment.CurrentDirectory}\Resources\{folderName}\WKnight.png"));
            images[4] = new BitmapImage(new Uri($@"{Environment.CurrentDirectory}\Resources\{folderName}\WBishop.png"));
            images[5] = new BitmapImage(new Uri($@"{Environment.CurrentDirectory}\Resources\{folderName}\WPawn.png"));
            images[6] = new BitmapImage(new Uri($@"{Environment.CurrentDirectory}\Resources\{folderName}\BKing.png"));
            images[7] = new BitmapImage(new Uri($@"{Environment.CurrentDirectory}\Resources\{folderName}\BQueen.png"));
            images[8] = new BitmapImage(new Uri($@"{Environment.CurrentDirectory}\Resources\{folderName}\BRook.png"));
            images[9] = new BitmapImage(new Uri($@"{Environment.CurrentDirectory}\Resources\{folderName}\BKnight.png"));
            images[10] = new BitmapImage(new Uri($@"{Environment.CurrentDirectory}\Resources\{folderName}\BBishop.png"));
            images[11] = new BitmapImage(new Uri($@"{Environment.CurrentDirectory}\Resources\{folderName}\BPawn.png"));

            foreach (Action action in listeneres)
                action();
        }

        public static BitmapImage GetImage(Type type, bool colour) => images[(int)type + (colour ? 0 : 6)];

        public static void RegisterListener(Action action)
        {
            listeneres.Add(action);
        }
    }
}
