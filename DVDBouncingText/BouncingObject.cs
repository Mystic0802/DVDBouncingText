namespace DVDBouncingText
{
    internal class BouncingObject
    {
        private static Random _random = new();

        public event EventHandler CornerBounceEventHandler;

        #region [ Coordinates ]

        /// <summary>
        /// Object's previous X position, based on the buffer column index.
        /// </summary>
        public float PreviousX { get; set; } = 0;
        /// <summary>
        /// Object's previous Y position, based on the buffer row index.
        /// </summary>
        public float PreviousY { get; set; } = 0;

        /// <summary>
        /// Object's current X position, based on the buffer column index.
        /// </summary>
        public float X { get; set; }
        /// <summary>
        /// Object's current Y position, based on the buffer row index.
        /// </summary>
        public float Y { get; set; }

        #endregion


        /// <summary>
        /// Object's directional speed.
        /// </summary>
        public (float velX, float velY) Velocity { get; set; }

        #region [ Appearance ]

        /// <summary>
        /// Object's drawable colour.
        /// </summary>
        public ConsoleColor Colour { get; set; }

        public int Height { get; private set; }
        public int Width { get; private set; }
        public string Text { get; set; }

        /// <summary>
        /// Available colours for the object to chose from when randomly changing it's colour.
        /// </summary>
        public enum Colours
        {
            Red = ConsoleColor.Red,
            Green = ConsoleColor.Green,
            Blue = ConsoleColor.Blue,
            Magenta = ConsoleColor.Magenta,
            Yellow = ConsoleColor.Yellow,
            Cyan = ConsoleColor.Cyan,
            White = ConsoleColor.White,
        }

        #endregion
        
        public BouncingObject(float x, float y, (float velX, float velY) velocity, int height = 1, int width = 1, string text = "")
        {
            X = x;
            Y = y;
            Velocity = velocity;
            Colour = GetRandomConsoleColour();
            Height = height;
            Width = width;
            Text = text;
        }

        /// <summary>
        /// Moves the object based on it's velocity. 
        /// </summary>
        public void Move()
        {
            float nextX = X + Velocity.velX;
            float nextY = Y + Velocity.velY;

            /// The boundary collision logic
            /// Flips horizontal direction.
            if(nextX < 0 || nextX > Console.WindowWidth - Width)
            {
                Velocity = (Velocity.velX * -1, Velocity.velY);
                Colour = GetRandomConsoleColour();
            }
            /// Flips vertical direction.
            if (nextY < 0 || nextY > Console.WindowHeight - Height)
            {
                Velocity = (Velocity.velX, Velocity.velY * -1);
                Colour = GetRandomConsoleColour();
            }

            PreviousX = X;
            PreviousY = Y;
            X = nextX;
            Y = nextY;
        }

        public void Draw()
        {
            if ((int)Math.Round(X) == (int)Math.Round(PreviousX) && (int)Math.Round(Y) == (int)Math.Round(PreviousY))
                return;
            if (((int)Math.Round(X) == 0 || (int)Math.Round(X) == Console.WindowWidth - Width) && ((int)Math.Round(Y) == 0 || (int)Math.Round(Y) == Console.WindowHeight - Height))
                CornerBounceEventHandler?.Invoke(this, new EventArgs());
            ClearPrevious();
            Console.ForegroundColor = Colour;

            for (int row = 0; row < Width; row++)
            {
                for (int col = 0; col < Height; col++)
                {
                    Console.SetCursorPosition((int)Math.Round(X) + row, (int)Math.Round(Y) + col);
                    Console.Write("▓");
                }
            }
        }

        private void ClearPrevious()
        {
            Console.ForegroundColor = ConsoleColor.Black;
            for (int row = 0; row < Width; row++)
            {
                for (int col = 0; col < Height; col++)
                {
                    Console.SetCursorPosition((int)Math.Round(PreviousX) + row, (int)Math.Round(PreviousY) + col);
                    Console.Write(" ");
                }
            }
        }

        private static ConsoleColor GetRandomConsoleColour()
        {
            Array consoleColors = Colours.GetValues(typeof(ConsoleColor));
            ConsoleColor nextColour = (ConsoleColor)consoleColors.GetValue(_random.Next(consoleColors.Length));
            return nextColour == ConsoleColor.Black ? ConsoleColor.Red : nextColour;
        }

    }
}