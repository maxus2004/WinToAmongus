using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestHack
{
    public partial class MyCopyWindow : Form
    {
        public MyCopyWindow()
        {
            InitializeComponent();
            TransparencyKey = Color.FromArgb(1, 1, 1);
            leftFolder.Parent = pictureBox1;
            rightFolder.Parent = pictureBox1;

            Closed += OnClosed;
            pictureBox1.MouseDown += OnMouseDown;
            pictureBox1.MouseMove += OnMouseMove;
            pictureBox1.MouseUp += OnMouseUp;

            new Thread(()=>
            {
                Thread.Sleep(1000);
                while (true)
                {
                    try
                    {
                        BeginInvoke((MethodInvoker) delegate
                        {
                            leftFolder.Image = Properties.Resources.dataTransfer_folderOpen0002;
                            rightFolder.Image = Properties.Resources.dataTransfer_folderOpen0003;
                        });
                        Thread.Sleep(50);
                        BeginInvoke((MethodInvoker) delegate
                        {
                            leftFolder.Image = Properties.Resources.dataTransfer_folderOpen0003;
                            rightFolder.Image = Properties.Resources.dataTransfer_folderOpen0002;
                        });
                        Thread.Sleep(50);
                        BeginInvoke((MethodInvoker) delegate
                        {
                            leftFolder.Image = Properties.Resources.dataTransfer_folderOpen0004;
                            rightFolder.Image = Properties.Resources.dataTransfer_folderOpen0001;
                        });
                        Thread.Sleep(1500);
                        BeginInvoke((MethodInvoker) delegate
                        {
                            leftFolder.Image = Properties.Resources.dataTransfer_folderOpen0003;
                            rightFolder.Image = Properties.Resources.dataTransfer_folderOpen0002;
                        });
                        Thread.Sleep(50);
                        BeginInvoke((MethodInvoker) delegate
                        {
                            leftFolder.Image = Properties.Resources.dataTransfer_folderOpen0002;
                            rightFolder.Image = Properties.Resources.dataTransfer_folderOpen0003;
                        });
                        Thread.Sleep(50);
                        BeginInvoke((MethodInvoker) delegate
                        {
                            leftFolder.Image = Properties.Resources.dataTransfer_folderOpen0001;
                            rightFolder.Image = Properties.Resources.dataTransfer_folderOpen0004;
                        });
                        Thread.Sleep(1500);
                    }
                    catch
                    {
                        break;
                    }
                }
            }).Start();
        }

        public void UpdateText(string text)
        {
            if (InvokeRequired)
            {
                BeginInvoke((MethodInvoker)delegate
               {
                   InfoLabel.Text = text;
                   int percentage;
                   if (int.TryParse(Regex.Match(text, @"\d+").Value, out percentage))
                   {
                       pictureBox3.Width = (int)(390 * (percentage / 100.0));
                   }
                   else
                   {
                       pictureBox3.Width = 390;
                   }

               });
            }
            else
            {
                InfoLabel.Text = text;
            }
        }
        public void Stop()
        {
            if (InvokeRequired)
            {
                BeginInvoke((MethodInvoker)Close);
            }
            else
            {
                Close();
            }
        }

        //private void OnKeyUp(object sender, KeyEventArgs e)
        //{
        //    if (e.KeyCode == Keys.Space)
        //    {
        //       Program.PauseCopy();
        //    }
        //}

        private void OnClosed(object sender, EventArgs e)
        {
            Program.CancelCopy();
        }

        private bool mouseDown;
        private Point lastLocation;

        void OnMouseDown(object sender, MouseEventArgs e)
        {
            mouseDown = true;
            lastLocation = e.Location;
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                this.Location = new Point(
                    (this.Location.X - lastLocation.X) + e.X, (this.Location.Y - lastLocation.Y) + e.Y);

                this.Update();
            }
        }

        private void OnMouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Program.PauseCopy();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
