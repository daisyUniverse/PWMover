using System;
using WindowsInput;
using WindowsInput.Native;
using SharpDX.XInput;
using Windows.Foundation;

namespace PWMover
{

    class PWMove
    {

        class XInputController
        {
            Controller controller;
            Gamepad gamepad;
            public bool connected = false;
            public int deadband = 2500;
            public Point leftThumb, rightThumb = new Point(0, 0);
            public float leftTrigger, rightTrigger;

            public XInputController()
            {
                controller = new Controller(UserIndex.One);
                connected = controller.IsConnected;
            }

            public void Update()
            {
                if (!connected)
                    return;

                gamepad = controller.GetState().Gamepad;

                leftThumb.X = (Math.Abs((float)gamepad.LeftThumbX) < deadband) ? 0 : (float)gamepad.LeftThumbX / short.MinValue * -100;
                leftThumb.Y = (Math.Abs((float)gamepad.LeftThumbY) < deadband) ? 0 : (float)gamepad.LeftThumbY / short.MaxValue * 100;
                rightThumb.Y = (Math.Abs((float)gamepad.RightThumbX) < deadband) ? 0 : (float)gamepad.RightThumbX / short.MaxValue * 100;
                rightThumb.X = (Math.Abs((float)gamepad.RightThumbY) < deadband) ? 0 : (float)gamepad.RightThumbY / short.MaxValue * 100;

                leftTrigger = gamepad.LeftTrigger;
                rightTrigger = gamepad.RightTrigger;

            }
        }

        static void Main(string[] args)
        {
            InputSimulator sim = new InputSimulator();
            Console.Title = "PWMove by Elisha Shaddock";
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Clear();
            Console.WriteLine();


            while (true)
            {

                XInputController controller = new XInputController();
                controller.Update();

                var CLY = controller.leftThumb.Y;
                var CLX = controller.leftThumb.X;
                int LY = Convert.ToInt32(CLY);
                int LX = Convert.ToInt32(CLX);
                int TimeUnit = 100;
                int YP = (int)Math.Round((double)(100 * LY) / TimeUnit);
                int XP = (int)Math.Round((double)(100 * LX) / TimeUnit);

                if (YP > 1)
                {
                    sim.Keyboard.KeyDown(VirtualKeyCode.VK_W);
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine(" Y Axis Ontime Percentage:  " + YP);
                    System.Threading.Thread.Sleep(YP);

                    Console.WriteLine(" Y Axis Offtime Percentage: " + (TimeUnit - YP));
                    sim.Keyboard.KeyUp(VirtualKeyCode.VK_W);
                    System.Threading.Thread.Sleep(TimeUnit - YP);
                }
                if (YP < 0)
                {
                    sim.Keyboard.KeyDown(VirtualKeyCode.VK_S);
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("-Y Axis Ontime Percentage:  " + (YP * -1));
                    System.Threading.Thread.Sleep(YP * -1);

                    Console.WriteLine("-Y Axis Offtime Percentage: " + (TimeUnit - (YP * -1)));
                    sim.Keyboard.KeyUp(VirtualKeyCode.VK_S);
                    System.Threading.Thread.Sleep(TimeUnit - (YP * -1));
                }

                if (XP > 0)
                {
                    sim.Keyboard.KeyDown(VirtualKeyCode.VK_D);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(" X Axis Ontime Percentage:  " + XP);
                    System.Threading.Thread.Sleep(XP);

                    Console.WriteLine(" X Axis Offtime Percentage: " + (TimeUnit - XP));
                    sim.Keyboard.KeyUp(VirtualKeyCode.VK_D);
                    System.Threading.Thread.Sleep(TimeUnit - XP);
                }
                if (XP < 0)
                {
                    sim.Keyboard.KeyDown(VirtualKeyCode.VK_A);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("-X Axis Ontime Percentage:  " + XP * -1);
                    System.Threading.Thread.Sleep(XP * -1);

                    Console.WriteLine("-X Axis Offtime Percentage: " + (TimeUnit - (XP * -1)));
                    sim.Keyboard.KeyUp(VirtualKeyCode.VK_A);
                    System.Threading.Thread.Sleep(TimeUnit - XP * -1);
                }
            };
        }
    }
}