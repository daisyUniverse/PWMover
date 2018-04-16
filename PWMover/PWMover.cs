using SharpDX.XInput;
using System;
using System.Windows.Forms.Integration;
using WindowsInput;
using WindowsInput.Native;

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
            public System.Windows.Point leftThumb, rightThumb = new System.Windows.Point(0, 0);
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

            [STAThread]
            static void Main(string[] args)
            {

                var wpfwindow = new WpfApplication.MainWindow();
                InputSimulator sim = new InputSimulator();
                ElementHost.EnableModelessKeyboardInterop(wpfwindow);
                wpfwindow.Show();

            while (true)
                {

                    XInputController controller = new XInputController();

                    controller.Update();

                    var CLY = controller.leftThumb.Y;
                    var CLX = controller.leftThumb.X;
                    var CRY = controller.rightThumb.Y;
                    var CRX = controller.rightThumb.X;

                    int LY = Convert.ToInt32(CLY);
                    int LX = Convert.ToInt32(CLX);
                    int RY = Convert.ToInt32(CRY);
                    int RX = Convert.ToInt32(CRX);

                    Wait(1);
                    int TimeUnit = 100;

                    int YP = (int)Math.Round((double)(100 * LY) / TimeUnit);
                    wpfwindow.LYPlus.Value = YP;
                    wpfwindow.LYMinus.Value = YP * -1;

                    int XP = (int)Math.Round((double)(100 * LX) / TimeUnit);
                    wpfwindow.LXPlus.Value = XP;
                    wpfwindow.LXMinus.Value = XP * -1;

                    int RYP = (int)Math.Round((double)(100 * RY) / TimeUnit);
                    wpfwindow.RYPlus.Value = RYP;
                    wpfwindow.RYMinus.Value = RYP * -1;

                    int RXP = (int)Math.Round((double)(100 * RX) / TimeUnit);
                    wpfwindow.RXPlus.Value = RXP;
                    wpfwindow.RXMinus.Value = RXP * -1;

                if (wpfwindow.LeftDisable.IsChecked == false)
                {

                    // Left Thumbstick Loop

                    if (YP > 1)
                    {
                        sim.Keyboard.KeyDown(VirtualKeyCode.VK_W);
                        Wait(YP);
                        sim.Keyboard.KeyUp(VirtualKeyCode.VK_W);
                        Wait(TimeUnit - YP);
                    }
                    if (YP < -1)
                    {
                        sim.Keyboard.KeyDown(VirtualKeyCode.VK_S);
                        Wait(YP * -1);
                        sim.Keyboard.KeyUp(VirtualKeyCode.VK_S);
                        Wait(TimeUnit - (YP * -1));
                    }

                    if (XP > 1)
                    {
                        sim.Keyboard.KeyDown(VirtualKeyCode.VK_D);
                        Wait(XP);
                        sim.Keyboard.KeyUp(VirtualKeyCode.VK_D);
                        Wait(TimeUnit - XP);
                    }
                    if (XP < -1)
                    {
                        sim.Keyboard.KeyDown(VirtualKeyCode.VK_A);
                        Wait(XP * -1);
                        sim.Keyboard.KeyUp(VirtualKeyCode.VK_A);
                        Wait(TimeUnit - XP * -1);
                    }
                }

                if (wpfwindow.RightDisable.IsChecked == false)
                {

                    // Right Thumbstick Loop

                    if (RXP > 1)
                    {
                        sim.Keyboard.KeyDown(VirtualKeyCode.UP);
                        Wait(RXP);
                        sim.Keyboard.KeyUp(VirtualKeyCode.UP);
                        Wait(TimeUnit - RXP);
                    }
                    if (RXP < -1)
                    {
                        sim.Keyboard.KeyDown(VirtualKeyCode.DOWN);
                        Wait(RXP * -1);
                        sim.Keyboard.KeyUp(VirtualKeyCode.DOWN);
                        Wait(TimeUnit - (RXP * -1));
                    }

                    if (RYP > 1)
                    {
                        sim.Keyboard.KeyDown(VirtualKeyCode.RIGHT);
                        Wait(RYP);
                        sim.Keyboard.KeyUp(VirtualKeyCode.RIGHT);
                        Wait(TimeUnit - RYP);
                    }
                    if (RYP < -1)
                    {
                        sim.Keyboard.KeyDown(VirtualKeyCode.LEFT);
                        Wait(RYP * -1);
                        sim.Keyboard.KeyUp(VirtualKeyCode.LEFT);
                        Wait(TimeUnit - RYP * -1);
                    }
                }

                if ( wpfwindow.IsLoaded == false ) { break; }

                };
                void Wait(int ms)
                {
                    DateTime start = DateTime.Now;
                    while ((DateTime.Now - start).TotalMilliseconds < ms)
                    System.Windows.Forms.Application.DoEvents();
                }
            }
        
    }
}