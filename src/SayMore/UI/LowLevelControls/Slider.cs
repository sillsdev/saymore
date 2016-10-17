using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace SayMore.UI.LowLevelControls
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// This control is like a TrackBar except it looks more like the slider control in
	/// Windows Media Player.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class Slider : UserControl
	{
		public event EventHandler ValueChanged;
		public event EventHandler AfterUserMovingThumb;

		public delegate void BeforeUserMovingThumbHandler(object sender, float newValue);
		public event BeforeUserMovingThumbHandler BeforeUserMovingThumb;

		private Orientation _orientation = Orientation.Horizontal;
		private float _value;
		private float _max = 100;
		private int _trackThickness = 5;
		private int _margin;
		private bool _mouseDown;
		private Rectangle _thumbRect;
		private Rectangle _prevThumbRect = Rectangle.Empty;
		private Image _thumb;
		private Point _prevTooltipLocation;
		private readonly ToolTip _tooltip;

		/// ------------------------------------------------------------------------------------
		public Slider() : this(Orientation.Horizontal)
		{
		}

		/// ------------------------------------------------------------------------------------
		public Slider(Orientation orientation)
		{
			ShowTooltip = true;
			DoubleBuffered = true;
			Cursor = Cursors.Hand;
			_orientation = orientation;
			SetupThumb();
			SetValueWithoutEvent(0);

			_tooltip = new ToolTip();
			TooltipFormat = "{0:F2} / {1:F2}";
		}

		#region Properties
		/// ------------------------------------------------------------------------------------
		[DefaultValue(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		public bool ShowTooltip { get; set; }

		/// ------------------------------------------------------------------------------------
		[DefaultValue("{0:F2} / {1:F2}")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		public string TooltipFormat { get; set; }

		/// ------------------------------------------------------------------------------------
		[DefaultValue(100f)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		public float Maximum
		{
			get { return _max; }
			set
			{
				_max = value;
				Invalidate();
			}
		}

		/// ------------------------------------------------------------------------------------
		[DefaultValue(0f)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		public float Value
		{
			get { return _value; }
			set
			{
				if (SetValueWithoutEvent(value) && ValueChanged != null)
					ValueChanged(this, EventArgs.Empty);
			}
		}

		/// ------------------------------------------------------------------------------------
		[DefaultValue(5)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		public int TrackThickness
		{
			get { return _trackThickness; }
			set
			{
				if (value != _trackThickness)
				{
					var maxThickness = (_orientation == Orientation.Horizontal ? Height : Width);
					_trackThickness = (value <= 0 ? 1 : Math.Min(maxThickness, value));
					Invalidate();
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		[DefaultValue(Orientation.Horizontal)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		public Orientation Orientation
		{
			get { return _orientation; }
			set
			{
				if (_orientation != value)
				{
					_orientation = value;
					SetupThumb();
					CalculateThumbRectangle();
					Invalidate();
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the pixel value where the slider's value is at zero.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private int MinPoint
		{
			get
			{
				if (_orientation == Orientation.Horizontal)
					return 0;

				return (ClientSize.Height - _margin);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the pixel value where the slider will be at its maximum value.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private int MaxPoint
		{
			get
			{
				if (_orientation == Orientation.Horizontal)
					return ClientSize.Width - _margin;

				return 0;
			}
		}

		/// ------------------------------------------------------------------------------------
		public bool UserIsMoving { get; private set; }

		#endregion

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Sets the slider's value without generating a ValueChanged event.
		/// </summary>
		/// <returns>true if the value changed. Otherwise, false.</returns>
		/// ------------------------------------------------------------------------------------
		public bool SetValueWithoutEvent(float value)
		{
			value = (float)Math.Round(value, 4);

			if (value < 0 || value > Maximum)
				throw new ArgumentOutOfRangeException("value");

			if (value == _value && IsHandleCreated)
				return false;

			_value = value;
			Invalidate(_prevThumbRect);
			CalculateThumbRectangle();
			Invalidate(_thumbRect);

			return true;
		}

		/// ------------------------------------------------------------------------------------
		private void SetupThumb()
		{
			if (Enabled)
			{
				_thumb = (_orientation == Orientation.Horizontal ?
					ResourceImageCache.HSliderThumb : ResourceImageCache.VSliderThumb);

				_margin = (_orientation == Orientation.Horizontal ? _thumb.Width : _thumb.Height);
			}
			else
			{
				_thumb = (_orientation == Orientation.Horizontal ?
					ResourceImageCache.HSliderThumbDisabled : ResourceImageCache.VSliderThumbDisabled);

				_margin = (_orientation == Orientation.Horizontal ? _thumb.Width : _thumb.Height);
			}

			CalculateThumbRectangle();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This is the width or height of the control between which values are counted. It
		/// extends from half the width of the thumb on the left/bottom edge of the control
		/// to half the width of the thumb from the right/top edge of the control. This is
		/// so the actual slider value can be in the center of the thumb, so when the value
		/// is at zero, the center of the thumb is really at several pixels to the
		/// right/above of the left/bottom edge, and likewise for the max. value.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private int SlidersValueDistance
		{
			get
			{
				return ((_orientation == Orientation.Horizontal ?
					ClientSize.Width : ClientSize.Height) - _margin);
			}
		}

		/// ------------------------------------------------------------------------------------
		private float GetValueFromPoint(int pt)
		{
			if (_orientation == Orientation.Horizontal)
			{
				if (pt < MinPoint)
					return 0;

				if (pt > MaxPoint)
					return Maximum;
			}
			else
			{
				if (pt > MinPoint)
					return 0;

				if (pt < MaxPoint)
					return Maximum;

				pt = (MinPoint - pt);

				if (pt > MinPoint)
					return 0;

				if (pt < MaxPoint)
					return Maximum;
			}

			return pt / GetPixelsPerValue();
		}

		/// ------------------------------------------------------------------------------------
		private float GetPixelsPerValue()
		{
			return SlidersValueDistance / Maximum;
		}

		/// ------------------------------------------------------------------------------------
		private void CalculateThumbRectangle()
		{
			_thumbRect = new Rectangle(new Point(0, 0), _thumb.Size);

			if (_orientation == Orientation.Horizontal)
				_thumbRect.X = (int)(Value * GetPixelsPerValue());
			else
			{
				var y = (int)(Value * GetPixelsPerValue());
				_thumbRect.Y = (MinPoint - y);
			}
		}

		#region Overridden methods
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Force the height of the control to be the same height as the thumb image.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
		{
			if (_orientation == Orientation.Horizontal)
				height = _thumb.Height;
			else
				width = _thumb.Width;

			base.SetBoundsCore(x, y, width, height, specified);
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnEnabledChanged(EventArgs e)
		{
			SetupThumb();
			Invalidate();
			base.OnEnabledChanged(e);
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnSizeChanged(EventArgs e)
		{
			base.OnSizeChanged(e);
			SetupThumb();
			Invalidate();
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			Rectangle rc;

			if (_orientation == Orientation.Horizontal)
			{
				rc = new Rectangle(1, 0, ClientSize.Width - 2, _trackThickness);
				rc.Y = (ClientSize.Height - _trackThickness) / 2;
			}
			else
			{
				rc = new Rectangle(0, 1, _trackThickness, ClientSize.Height - 2);
				rc.X = (ClientSize.Width - _trackThickness) / 2;
			}

			Image img = _thumb;
			var angle = (_orientation == Orientation.Horizontal ? 90f : 0f);

			if (Enabled)
			{
				if (_mouseDown)
				{
					img = (_orientation == Orientation.Horizontal ?
						ResourceImageCache.HSliderThumbMousePressed : ResourceImageCache.VSliderThumbPressed);
				}

				using (var br = new LinearGradientBrush(rc, Color.SlateGray, Color.LightSteelBlue, angle))
					e.Graphics.FillRectangle(br, rc);
			}
			else
			{
				using (var br = new LinearGradientBrush(rc, SystemColors.ControlDark, SystemColors.ControlLight, angle))
					e.Graphics.FillRectangle(br, rc);
			}

			//Color.Turquoise;
			//Color.SlateGray

			e.Graphics.DrawImage(img, _thumbRect);
			_prevThumbRect = _thumbRect;
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnMouseDown(MouseEventArgs e)
		{
			if (e.Button != MouseButtons.Left)
				return;

			var newValue = GetValueFromPoint(GetAdjustedMousePoint(e.Location));

			if (BeforeUserMovingThumb != null)
				BeforeUserMovingThumb(this, newValue);

			base.OnMouseDown(e);

			if (Enabled)
			{
				Value = newValue;
				_mouseDown = true;
			}
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);

			var rcHot = ClientRectangle;

			if (_orientation == Orientation.Horizontal)
				rcHot.Inflate(0, 5);
			else
				rcHot.Inflate(5, 0);

			if (Enabled)
			{
				if (_mouseDown && rcHot.Contains(e.Location))
				{
					Value = GetValueFromPoint(GetAdjustedMousePoint(e.Location));
					UserIsMoving = true;
				}

				if (ShowTooltip)
					ManageTooltip(e.Location);
			}
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnMouseUp(MouseEventArgs e)
		{
			UserIsMoving = false;
			_mouseDown = false;
			base.OnMouseUp(e);
			Invalidate(_thumbRect);
			_tooltip.Hide(this);

			if (AfterUserMovingThumb != null)
				AfterUserMovingThumb(this, EventArgs.Empty);
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave(e);
			_tooltip.Hide(this);
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		private void ManageTooltip(Point pt)
		{
			if (!_mouseDown && !_thumbRect.Contains(pt))
				_tooltip.Hide(this);
			else
			{
				if (pt != _prevTooltipLocation)
				{
					_prevTooltipLocation = pt;
					pt.X += 10;
					pt.Y += 10;

					if (TooltipFormat.Contains("{1"))
						_tooltip.Show(string.Format(TooltipFormat, Value, Maximum), this, pt);
					else
						_tooltip.Show(string.Format(TooltipFormat, Value), this, pt);
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// When the user clicks on the slider, the location on the slider where the mouse
		/// is treated as being clicked is offset by half the width (or height) of the thumb so
		/// that when the thumb is relocated, the mouse point is right over the middle of it.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private int GetAdjustedMousePoint(Point mouseLocation)
		{
			int pt;

			if (_orientation == Orientation.Horizontal)
			{
				pt = mouseLocation.X - (_margin / 2);
				if (pt < MinPoint)
					pt = MinPoint;

				if (pt > MaxPoint)
					pt = MaxPoint;
			}
			else
			{
				pt = mouseLocation.Y - (_margin / 2);
				if (pt > MinPoint)
					pt = MinPoint;

				if (pt < MaxPoint)
					pt = MaxPoint;
			}

			return pt;
		}
	}
}
