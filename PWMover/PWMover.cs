using MahApps.Metro;
using SharpDX.XInput;
using System;
using System.Diagnostics;
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
                    var CRT = controller.rightTrigger;
                    var CLT = controller.leftTrigger;

                    int LY = Convert.ToInt32(CLY);
                    int LX = Convert.ToInt32(CLX);
                    int RY = Convert.ToInt32(CRY);
                    int RX = Convert.ToInt32(CRX);
                    int LT = Convert.ToInt32(CLT);
                    int RT = Convert.ToInt32(CRT);

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

                    int RTP = (int)Math.Round((double)(39 * RT) / TimeUnit);
                    wpfwindow.RTProg.Value = RTP;    

                    int LTP = (int)Math.Round((double)(39 * LT) / TimeUnit);
                    wpfwindow.LTProg.Value = LTP;

                    wpfwindow.DZLP.Value = wpfwindow.DZL.Value;
                    wpfwindow.DZRP.Value = wpfwindow.DZR.Value;

                    wpfwindow.LBP.Value = wpfwindow.LBPS.Value;
                    wpfwindow.RBP.Value = wpfwindow.RBPS.Value;

                    wpfwindow.TBP.Value = wpfwindow.TBS.Value;
                    wpfwindow.TDZP.Value = wpfwindow.DZT.Value;

                if (wpfwindow.LUP_Button.IsPressed)
                {
                    
                }

                if (wpfwindow.TrigDisable.IsChecked == false)
                {

                    //Trigger Loop

                    if (RTP > (100 + (-wpfwindow.TBP.Value))) { sim.Keyboard.KeyDown(VirtualKeyCode.RCONTROL); };

                    if (LTP > (100 + (-wpfwindow.TBP.Value))) { sim.Keyboard.KeyDown(VirtualKeyCode.RCONTROL); };

                    if (RTP > wpfwindow.TDZP.Value)
                    {
                        Debug.WriteLine(RTP);
                        sim.Keyboard.KeyDown(VirtualKeyCode.VK_R);
                        Wait(RTP);
                        sim.Keyboard.KeyUp(VirtualKeyCode.VK_R);
                        Wait(TimeUnit - RTP);
                    }

                    if (LTP > wpfwindow.TDZP.Value)
                    {
                        sim.Keyboard.KeyDown(VirtualKeyCode.VK_L);
                        Wait(LTP);
                        sim.Keyboard.KeyUp(VirtualKeyCode.VK_L);
                        Wait(TimeUnit - LTP);
                    }

                    sim.Keyboard.KeyUp(VirtualKeyCode.RCONTROL);

                }

                if (wpfwindow.LeftDisable.IsChecked == false)
                {

                    // Left Thumbstick Loop

                    if (YP > (100 + (-wpfwindow.LBP.Value))) { sim.Keyboard.KeyDown(VirtualKeyCode.SHIFT); };
                    if (-YP > (100 + (-wpfwindow.LBP.Value))) { sim.Keyboard.KeyDown(VirtualKeyCode.SHIFT); };
                    if (XP > (100 + (-wpfwindow.LBP.Value))) { sim.Keyboard.KeyDown(VirtualKeyCode.SHIFT); };
                    if (-XP > (100 + (-wpfwindow.LBP.Value))) { sim.Keyboard.KeyDown(VirtualKeyCode.SHIFT); };

                    if (YP > wpfwindow.DZL.Value)
                    {
                        sim.Keyboard.KeyDown(VirtualKeyCode.VK_W);
                        Wait(YP);
                        sim.Keyboard.KeyUp(VirtualKeyCode.VK_W);
                        Wait(TimeUnit - YP);
                    }
                    if (YP < -wpfwindow.DZL.Value)
                    {
                        sim.Keyboard.KeyDown(VirtualKeyCode.VK_S);
                        Wait(YP * -1);
                        sim.Keyboard.KeyUp(VirtualKeyCode.VK_S);
                        Wait(TimeUnit - (YP * -1));
                    }

                    if (XP > wpfwindow.DZL.Value)
                    {
                        sim.Keyboard.KeyDown(VirtualKeyCode.VK_D);
                        Wait(XP);
                        sim.Keyboard.KeyUp(VirtualKeyCode.VK_D);
                        Wait(TimeUnit - XP);
                    }
                    if (XP < -wpfwindow.DZL.Value)
                    {
                        sim.Keyboard.KeyDown(VirtualKeyCode.VK_A);
                        Wait(XP * -1);
                        sim.Keyboard.KeyUp(VirtualKeyCode.VK_A);
                        Wait(TimeUnit - XP * -1);
                    }

                    sim.Keyboard.KeyUp(VirtualKeyCode.SHIFT);

                }

                if (wpfwindow.RightDisable.IsChecked == false)
                {

                    // Right Thumbstick Loop

                    if (RYP > (100 + (-wpfwindow.RBP.Value))) { sim.Keyboard.KeyDown(VirtualKeyCode.CONTROL); };
                    if (-RYP > (100 + (-wpfwindow.RBP.Value))) { sim.Keyboard.KeyDown(VirtualKeyCode.CONTROL); };
                    if (RXP > (100 + (-wpfwindow.RBP.Value))) { sim.Keyboard.KeyDown(VirtualKeyCode.CONTROL); };
                    if (-RXP > (100 + (-wpfwindow.RBP.Value))) { sim.Keyboard.KeyDown(VirtualKeyCode.CONTROL); };

                    if (RXP > wpfwindow.DZR.Value)
                    {
                        sim.Keyboard.KeyDown(VirtualKeyCode.UP);
                        Wait(RXP);
                        sim.Keyboard.KeyUp(VirtualKeyCode.UP);
                        Wait(TimeUnit - RXP);
                    }
                    if (RXP < -wpfwindow.DZR.Value)
                    {
                        sim.Keyboard.KeyDown(VirtualKeyCode.DOWN);
                        Wait(RXP * -1);
                        sim.Keyboard.KeyUp(VirtualKeyCode.DOWN);
                        Wait(TimeUnit - (RXP * -1));
                    }

                    if (RYP > wpfwindow.DZR.Value)
                    {
                        sim.Keyboard.KeyDown(VirtualKeyCode.RIGHT);
                        Wait(RYP);
                        sim.Keyboard.KeyUp(VirtualKeyCode.RIGHT);
                        Wait(TimeUnit - RYP);
                    }
                    if (RYP < -wpfwindow.DZR.Value)
                    {
                        sim.Keyboard.KeyDown(VirtualKeyCode.LEFT);
                        Wait(RYP * -1);
                        sim.Keyboard.KeyUp(VirtualKeyCode.LEFT);
                        Wait(TimeUnit - RYP * -1);
                    }

                    sim.Keyboard.KeyUp(VirtualKeyCode.CONTROL);

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