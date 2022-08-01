namespace DVDBouncingText
{
    internal class Program
    {
        const int RefreshRate = 30;
        private static List<BouncingObject> bouncingObjects = new();

        static void Main(string[] args)
        {
            Console.CursorVisible = false;
            bouncingObjects.Add(new BouncingObject(Console.WindowWidth / 2, Console.WindowHeight / 2, (0.05f / RefreshRate, 0.05f / RefreshRate)));
            //bouncingObjects[0].CornerBounceEventHandler += Program_CornerBounceEventHandler;

            Task.Run(() =>
            {
                while (true)
                {
                    for (int i = 0; i < bouncingObjects.Count; i++)
                    {
                        MoveAndDrawObject(i);
                    }
                    Task.Delay(1000 / RefreshRate);
                }
            });
            Console.ReadKey();
        }

        /// <summary>
        /// For when an object hits the corner. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void Program_CornerBounceEventHandler(object? sender, EventArgs e)
        {
            if (sender == null)
                return;
            var parent = sender as BouncingObject;
            if (bouncingObjects.Count <= 10)
                bouncingObjects.Add(new BouncingObject(0,0, (parent.Velocity.velX + 0.5f, parent.Velocity.velY + 0.5f), (int)Math.Round((float)parent.Height / 2), (int)Math.Round((float)parent.Width / 2)));
        }

        static void MoveAndDrawObject(int index)
        {
            bouncingObjects[index].Move();
            bouncingObjects[index].Draw();
        }
    }
}