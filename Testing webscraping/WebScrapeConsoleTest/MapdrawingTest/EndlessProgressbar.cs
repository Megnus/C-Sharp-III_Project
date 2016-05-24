using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace HomeMade.Gui.Controls
{
    /// <summary>
    /// An endless progress bar.
    /// </summary>
    public class EndlessProgressBar : System.Windows.Forms.UserControl
    {
        private System.ComponentModel.IContainer components;

        /// <summary>
        /// Endless progress bar
        /// </summary>
        public EndlessProgressBar()
        {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();

            this.ResizeRedraw = true;
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);

            this.Paint += new PaintEventHandler(this.PaintHandler);
            this.Resize += new EventHandler(this.ResizeHandler);
            this.tmrAutoProgress.Tick += new EventHandler(this.TimerHandler);
        }

        /// <summary>
        /// Disposes all used resources.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                    components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.tmrAutoProgress = new System.Windows.Forms.Timer(this.components);
            // 
            // OSProgress
            // 
            this.Name = "OSProgress";
            this.Size = new System.Drawing.Size(226, 30);

        }
        #endregion

        private System.Windows.Forms.Timer tmrAutoProgress;

        private byte speedMultiPlier = 2;
        private bool requireClear = false;
        private bool increasing = true;

        #region Properties
        private ProgressType progressType = ProgressType.Box;

        /// <summary>
        /// Gets/Sets the type of progress bar.
        /// </summary>
        [Description("Determines the type of progress bar"), DefaultValue(ProgressType.Box)]
        public ProgressType ProgressType
        {
            get { return progressType; }
            set
            {
                progressType = value;
                this.Invalidate();
            }
        }

        private Image normalImage;

        /// <summary>
        /// Gets/Sets the background graphic.
        /// </summary>
        [Description("Gets/Sets the background graphic")]
        public Image NormalImage
        {
            get { return normalImage; }
            set
            {
                normalImage = value;
                this.Invalidate();
            }

        }

        private Image pointImage;

        /// <summary>
        /// Gets/Sets the point graphic.
        /// </summary>
        [Description("Gets/Sets the point graphic")]
        public Image PointImage
        {
            get { return pointImage; }
            set
            {
                pointImage = value;
                this.Invalidate();
            }
        }

        private bool showBorder = true;

        /// <summary>
        /// Gets/Sets if the border is shown.
        /// </summary>
        [Description("Determines if the border is shown"), DefaultValue(true)]
        public bool ShowBorder
        {
            get { return showBorder; }
            set
            {
                showBorder = value;
                this.Invalidate();
            }
        }

        private int numPoints;

        /// <summary>
        /// Gets the number of points in the progressbar.
        /// </summary>
        [Browsable(false)]
        public int NumPoints
        {
            get { return numPoints; }
        }

        private int position;

        /// <summary>
        /// Gets/Sets the position of the progress indicator.
        /// </summary>
        [Browsable(false)]
        public int Position
        {
            get { return position; }
            set
            {
                position = value;
                this.Invalidate();
            }
        }

        private Color indicatorColor = Color.Red;

        /// <summary>
        /// Gets/Sets the color of the indicator.
        /// </summary>
        [Description("Color of the indicator")]
        public Color IndicatorColor
        {
            get { return indicatorColor; }
            set
            {
                indicatorColor = value;
                this.Invalidate();
            }
        }
        private ProgressStyle progressStyle = ProgressStyle.LeftToRight;

        /// <summary>
        /// Gets/Sets the progress indicator rotation style.
        /// </summary>
        [Description("Indicates the progress indicator rotation style"),
         DefaultValue(ProgressStyle.LeftToRight)]
        public ProgressStyle ProgressStyle
        {
            get { return progressStyle; }
            set
            {
                progressStyle = value;
                this.Invalidate();
            }
        }

        private bool autoProgress = false;

        /// <summary>
        /// Gets/Sets whether auto-progress is enabled.
        /// </summary>
        [Description("Indicates whether auto-progress is enabled"), DefaultValue(false)]
        public bool AutoProgress
        {
            get { return autoProgress; }
            set
            {
                if (!this.DesignMode)
                {
                    this.tmrAutoProgress.Interval = (255 - this.autoProgressSpeed) * this.speedMultiPlier;
                    if (value)
                    {
                        this.tmrAutoProgress.Start();
                    }
                    else
                    {
                        this.tmrAutoProgress.Stop();
                    }
                }
                autoProgress = value;
            }
        }

        private int autoProgressSpeed = 100;

        /// <summary>
        /// Gets/Sets the speed of the progress indicator (1 [slower] to 254 [faster]).
        /// </summary>
        [Description("Indicates the speed of the progress indicator (1 [slower] to 254 [faster])"),
         DefaultValue(100)]
        public int AutoProgressSpeed
        {
            get { return autoProgressSpeed; }
            set
            {
                if (value < 1)
                {
                    value = 1;
                }
                else if (value > 254)
                {
                    value = 254;
                }
                tmrAutoProgress.Stop();
                tmrAutoProgress.Interval = (255 - value) * speedMultiPlier;
                tmrAutoProgress.Enabled = this.autoProgress;
                autoProgressSpeed = value;
            }
        }

        private ProgressBoxStyle progressBoxStyle = ProgressBoxStyle.SolidSameSize;

        /// <summary>
        /// Gets/Sets the style of the progress box.
        /// </summary>
        [Description("Gets/Sets the style of the progress box."),
         DefaultValue(ProgressBoxStyle.SolidSameSize)]
        public ProgressBoxStyle ProgressBoxStyle
        {
            get
            {
                return progressBoxStyle;
            }
            set
            {
                progressBoxStyle = value;
                this.Invalidate();
            }
        }
        #endregion

        #region Methods
        private void ResizeHandler(object sender, System.EventArgs e)
        {
            this.requireClear = true;
            this.position = 0;
            this.Invalidate();
        }

        private void TimerHandler(object sender, System.EventArgs e)
        {
            if (this.position == this.numPoints - 1)
            {
                if (this.progressStyle == ProgressStyle.LeftToRight)
                {
                    this.position = 0;
                }
                else
                {
                    this.position -= 1;
                    this.increasing = false;
                }
            }
            else if ((this.position == 0) && (!this.increasing))
            {
                this.position += 1;
                this.increasing = true;
            }
            else
            {
                if (this.increasing)
                {
                    this.position += 1;
                }
                else
                {
                    this.position -= 1;
                }
            }
            this.requireClear = false;
            this.Invalidate();
        }

        private void PaintHandler(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.HighSpeed;
            if (this.requireClear)
            {
                g.Clear(this.BackColor);
            }
            DrawBackground(g);
        }

        private void DrawBackground(Graphics g)
        {
            this.numPoints = 0;
            if (this.Width > 0 && this.Height > 0)
            {
                if (this.showBorder)
                {
                    Pen p = new Pen(SystemColors.ActiveBorder);
                    g.DrawRectangle(p, 0, 0, this.Width - 1, this.Height - 1);
                    p.Dispose();
                }
                int iBoxSize = checked((int)(this.Height * 0.75));
                int iBoxLeft = iBoxSize / 2;
                if (iBoxSize > 3)
                {
                    do
                    {
                        Rectangle r = new Rectangle(iBoxLeft, 0, this.Height - 1, this.Height - 1);
                        if (r.Left + r.Width > this.Width)
                        {
                            break;
                        }
                        if (this.numPoints == this.position)
                        {
                            PositionIndicator(r, g);
                        }
                        else
                        {
                            Rectangle r2 = new Rectangle(r.Left + 3, r.Top + 3, r.Width - 6, r.Height - 6);
                            if ((this.normalImage != null) && (this.progressType == ProgressType.Graphic))
                            {
                                g.DrawImage(this.normalImage, r2);
                            }
                            else
                            {
                                Brush b = new SolidBrush(this.ForeColor);
                                g.FillRectangle(b, r2);
                                b.Dispose();
                            }
                        }
                        iBoxLeft += checked((int)(iBoxSize * 1.5));
                        this.numPoints += 1;
                    }
                    while (true);
                }
            }
        }

        private void PositionIndicator(Rectangle Rect, Graphics g)
        {
            if ((this.pointImage != null) && (this.progressType == ProgressType.Graphic))
            {
                g.DrawImage(this.pointImage, Rect);
            }
            else
            {
                Brush b = new SolidBrush(indicatorColor);
                if (this.ProgressBoxStyle == ProgressBoxStyle.SolidSameSize)
                {
                    g.FillRectangle(b, Rect.Left + 3, Rect.Top + 3, Rect.Width - 5, Rect.Height - 5);
                }
                else if (this.ProgressBoxStyle == ProgressBoxStyle.BoxAround)
                {
                    Pen p = new Pen(indicatorColor);
                    g.DrawRectangle(p, Rect);
                    p.Dispose();
                    g.FillRectangle(b, Rect.Left + 3, Rect.Top + 3, Rect.Width - 5, Rect.Height - 5);
                }
                else if (this.ProgressBoxStyle == ProgressBoxStyle.SolidBigger)
                {
                    g.FillRectangle(b, Rect);
                }
                else if (this.ProgressBoxStyle == ProgressBoxStyle.SolidSmaller)
                {
                    g.FillRectangle(b, Rect.Left + 5, Rect.Top + 5, Rect.Width - 9, Rect.Height - 9);
                }
                b.Dispose();
            }
        }
        #endregion
    }

    /// <summary>
    /// The style of moving of the progress indicator.
    /// </summary>
    public enum ProgressStyle
    {
        /// <summary>
        /// The progress indicator moves from left to right.
        /// </summary>
        LeftToRight,
        /// <summary>
        /// The progress indicator moves left and right.
        /// </summary>
        LeftAndRight
    }

    /// <summary>
    /// The type of the progress indicator.
    /// </summary>
    public enum ProgressType
    {
        /// <summary>
        /// Colored rectangle box as progress indicator.
        /// </summary>
        Box,
        /// <summary>
        /// Graphical progress indicator.
        /// </summary>
        Graphic
    }

    /// <summary>
    /// The style of the active progress indicator (in box mode).
    /// </summary>
    public enum ProgressBoxStyle
    {
        /// <summary>
        /// Active box has same size as other boxes.
        /// </summary>
        SolidSameSize,
        /// <summary>
        /// Active box has same size as other boxes but a rectangle
        /// around it.
        /// </summary>
        BoxAround,
        /// <summary>
        /// Active box is bigger than the other boxes.
        /// </summary>
        SolidBigger,
        /// <summary>
        /// Active box is smaller than the other boxes.
        /// </summary>
        SolidSmaller,
    }
}
