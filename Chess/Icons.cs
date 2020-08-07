using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Chess
{
    struct ImagePool
    {
        public BitmapImage BKing;
        public BitmapImage BQueen;
        public BitmapImage BBishop;
        public BitmapImage BRook;
        public BitmapImage BKnight;
        public BitmapImage BPawn;
        public BitmapImage WKing;
        public BitmapImage WQueen;
        public BitmapImage WBishop;
        public BitmapImage WRook;
        public BitmapImage WKnight;
        public BitmapImage WPawn;
    }

    static class Icons
    {
        private static ImagePool imagePool;
        private static BitmapImage[] images = new BitmapImage[12];
        private static List<Action> listeneres = new List<Action>();

        public static void LoadImages(string folderName)
        {
            imagePool = new ImagePool()
            {
                BKing = new BitmapImage(new Uri($@"{Environment.CurrentDirectory}\Resources\{folderName}\BKing.png")),
                BQueen = new BitmapImage(new Uri($@"{Environment.CurrentDirectory}\Resources\{folderName}\BQueen.png")),
                BBishop = new BitmapImage(new Uri($@"{Environment.CurrentDirectory}\Resources\{folderName}\BBishop.png")),
                BRook = new BitmapImage(new Uri($@"{Environment.CurrentDirectory}\Resources\{folderName}\BRook.png")),
                BKnight = new BitmapImage(new Uri($@"{Environment.CurrentDirectory}\Resources\{folderName}\BKnight.png")),
                BPawn = new BitmapImage(new Uri($@"{Environment.CurrentDirectory}\Resources\{folderName}\BPawn.png")),
                WKing = new BitmapImage(new Uri($@"{Environment.CurrentDirectory}\Resources\{folderName}\WKing.png")),
                WQueen = new BitmapImage(new Uri($@"{Environment.CurrentDirectory}\Resources\{folderName}\WQueen.png")),
                WBishop = new BitmapImage(new Uri($@"{Environment.CurrentDirectory}\Resources\{folderName}\WBishop.png")),
                WRook = new BitmapImage(new Uri($@"{Environment.CurrentDirectory}\Resources\{folderName}\WRook.png")),
                WKnight = new BitmapImage(new Uri($@"{Environment.CurrentDirectory}\Resources\{folderName}\WKnight.png")),
                WPawn = new BitmapImage(new Uri($@"{Environment.CurrentDirectory}\Resources\{folderName}\WPawn.png")),
            };

            foreach (Action action in listeneres)
            {
                action.Invoke();
            }
        }

        public static ImagePool GetImagePool() => imagePool;

        public static BitmapImage GetImage(Piece piece) => images[(int)piece.GetType() + (piece.GetColour()? 0 : 6)];

        public static void RegisterListener(Action action)
        {
            listeneres.Add(action);
        }
    }
}
