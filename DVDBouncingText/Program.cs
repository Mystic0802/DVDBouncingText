namespace DVDBouncingText
{
    internal class Program
    {
        const int FrameRate = 10;
        private static List<BouncingObject> bouncingObjects = new();

        static void Main(string[] args)
        {
            Console.CursorVisible = false;
            //bouncingObjects.Add(new BouncingObject());
            //bouncingObjects.Add(new BouncingObject((0.6, 0.15), 1f, 5));
            //bouncingObjects.Add(new BouncingObject((-1, 0.1), 2f, 1, 5));
            //bouncingObjects.Add(new BouncingObject((0.1, -1), 1f, 3, 3));
            bouncingObjects.Add(new BouncingObject(1f, 6,6));

            while (true)
            {
                for (int i = 0; i < bouncingObjects.Count; i++)
                {
                    bouncingObjects[i].Move();
                    bouncingObjects[i].Draw();
                }
                Thread.Sleep(1000/FrameRate);
            }
        }

        public static void AddBoucy(BouncingObject bouncy)
        {
            bouncingObjects.Add(bouncy);
        }
    }

    internal class BouncingObject
    {
        private static Random _random = new();
        public double PreviousX { get; set; } = 0;
        public double PreviousY { get; set; } = 0;

        // Objects coordinates
        public double X { get; set; }
        public double Y { get; set; }

        // Vector pointing direction
        public (double x, double y) Direction { get; set; }

        // speed the object will move
        public float Velocity { get; set; }

        public ConsoleColor CurrentColor { get; set; }

        public int VerticalSize { get; private set; }
        public int HorizontalSize { get; private set; }

        public int Text { get; set; }

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

        #region [ Constructors ]

        public BouncingObject()
        {
            X = Console.WindowWidth / 2;
            Y = Console.WindowHeight / 2;
            Direction = (_random.NextDouble(), _random.NextDouble());
            Velocity = 1f;
            CurrentColor = GetRandomConsoleColour();
            Draw();
            VerticalSize = 1;
            HorizontalSize = 1;
        }

        public BouncingObject(float velocity, int verticalSize, int horizontalSize)
        {
            X = Console.WindowWidth / 2;
            Y = Console.WindowHeight / 2;
            Direction = (_random.NextDouble(), _random.NextDouble());
            Velocity = velocity;
            CurrentColor = GetRandomConsoleColour();
            VerticalSize = verticalSize;
            HorizontalSize = horizontalSize;
        }

        public BouncingObject(double x, double y, float velocity = 1, int verticalSize = 1, int horizontalSize = 1)
        {
            X = x;
            Y = y;
            Direction = (_random.NextDouble(), _random.NextDouble());
            Velocity = velocity;
            CurrentColor = GetRandomConsoleColour();
            VerticalSize = verticalSize;
            HorizontalSize = horizontalSize;
        }

        public BouncingObject((double x, double y) startDirection, float velocity = 1, int verticalSize = 1, int horizontalSize = 1)
        {
            X = Console.WindowWidth / 2;
            Y = Console.WindowHeight / 2;
            Direction = startDirection;
            Velocity = velocity;
            CurrentColor = GetRandomConsoleColour();
            VerticalSize = verticalSize;
            HorizontalSize = horizontalSize;
        }
        public BouncingObject(double x, double y, (double x, double y) startDirection, float velocity = 1, int verticalSize = 1, int horizontalSize = 1)
        {
            X = x;
            Y = y;
            Direction = startDirection;
            Velocity = velocity;
            CurrentColor = GetRandomConsoleColour();
            VerticalSize = verticalSize;
            HorizontalSize = horizontalSize;
        }

        #endregion

        public void Move()
        {
            double nextX = X + (Direction.x * Velocity);
            double nextY = Y + (Direction.y * Velocity);

            // Collision logic
            if(nextX <= 0 || nextX >= Console.WindowWidth - HorizontalSize)
            {
                if (nextX <= 0)
                    nextX = 0;
                else
                    nextX = Console.WindowWidth - (HorizontalSize);
                Direction = (Direction.x * -1, Direction.y);
                CurrentColor = GetRandomConsoleColour();
            }
            if (nextY <= 0 || nextY >= Console.WindowHeight - (VerticalSize-1))
            {
                if (nextY <= 0)
                    nextY = 0;
                else
                    nextY = Console.WindowHeight - (VerticalSize - 1);
                Direction = (Direction.x, Direction.y * -1);
                CurrentColor = GetRandomConsoleColour();

                if (X + HorizontalSize - 1 == Console.WindowWidth - 1 || X == 0)
                    Program.AddBoucy(new BouncingObject(X, Y, Velocity, VerticalSize - 1 > 0 ? VerticalSize - 1 : 1, HorizontalSize - 1 > 0 ? HorizontalSize - 1 : 1));
            }


            PreviousX = X;
            PreviousY = Y;
            X = nextX;
            Y = nextY;
        }

        public void Draw()
        {
            ClearPrevious();
            Console.ForegroundColor = CurrentColor;

            for (int row = 0; row < HorizontalSize; row++)
            {
                for (int col = 0; col < VerticalSize; col++)
                {
                    Console.SetCursorPosition((int)X + row, (int)Y + col);
                    Console.Write("▓");
                }
            }
        }

        private void ClearPrevious()
        {
            Console.ForegroundColor = ConsoleColor.Black;
            for (int row = 0; row < HorizontalSize; row++)
            {
                for (int col = 0; col < VerticalSize; col++)
                {
                    Console.SetCursorPosition((int)PreviousX + row, (int)PreviousY + col);
                    Console.Write(" ");
                }
            }
        }

        private static ConsoleColor GetRandomConsoleColour()
        {
            var consoleColors = Enum.GetValues(typeof(ConsoleColor));
            var nextColour = (ConsoleColor)consoleColors.GetValue(_random.Next(consoleColors.Length));
            return nextColour == ConsoleColor.Black ? ConsoleColor.Red : nextColour;
        }

    }
}