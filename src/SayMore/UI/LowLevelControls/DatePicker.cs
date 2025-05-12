using System;
using System.Linq;
using System.Windows.Forms;
using SIL.Extensions;
using SIL.Reporting;
using SIL.Windows.Forms.Extensions;

namespace SayMore.UI.LowLevelControls
{
	/// ----------------------------------------------------------------------------------------
	/// A DateTimePicker that allows null values and displays an empty picker when the value
	/// is null. When the control has focus and Del is pressed, the value will be set to null.
	/// ----------------------------------------------------------------------------------------
	public class DatePicker : DateTimePicker
	{
		private bool _isBlank;
		private bool _ignoreEvents;

		/// ------------------------------------------------------------------------------------
		public override string Text
		{
			get => base.Text;
			set => SetValue(value);
		}

		/// ------------------------------------------------------------------------------------
		public string GetISO8601DateValueOrNull()
		{
			return _isBlank ? null : Value.ToISO8601TimeFormatDateOnlyString();
		}

		/// ------------------------------------------------------------------------------------
		public void SetValue(string value)
		{
			_isBlank = string.IsNullOrWhiteSpace(value);

			if (_isBlank)
				SetBlankFormat();
			else
				SetDefaultFormat();

			_ignoreEvents = true;

			try
			{
				if (_isBlank)
				{
					base.Text = null;
					return;
				}

				DateTime parsedDate;
				try
				{
					parsedDate = DateTimeExtensions.ParseISO8601DateTime(value);
				}
				catch (ApplicationException)
				{
					parsedDate = DateTimeExtensions.ParseDateTimePermissivelyWithException(value);
					if (parsedDate.Day <= 12 && parsedDate.Day != parsedDate.Month && !(value.Replace("AM", string.Empty).Replace("PM", string.Empty).Any(char.IsLetter)))
					{
						//The date was not in the ISO8601 format and cannot be unambiguously parsed.

						// Before we despair, in SayMore we do not expect future dates, so if the
						// date is future and switching the month and day puts it into the past,
						// then that's presumably the correct thing to do.
						if (parsedDate.Date > DateTime.Today && 
						    new DateTime(parsedDate.Year, parsedDate.Day, parsedDate.Month) is DateTime flipped &&
						    flipped <= DateTime.Today)
						{
							parsedDate = flipped;
						}
						else
						{
							// Still ambiguous. We need to alert the caller.
							try
							{
								Value = parsedDate;
							}
							catch (ArgumentOutOfRangeException e)
							{
								Logger.WriteError(e);
							}
							throw new AmbiguousDateException(value);
						}
					}
				}

				Value = parsedDate;
			}
			finally
			{
				_ignoreEvents = false;
			}
		}

		/// ------------------------------------------------------------------------------------
		private void SetBlankFormat()
		{
			_ignoreEvents = true;
			this.SetWindowRedraw(false);
			Format = DateTimePickerFormat.Custom;
			CustomFormat = @" ";
			this.SetWindowRedraw(true);
			_ignoreEvents = false;
		}

		/// ------------------------------------------------------------------------------------
		private void SetDefaultFormat()
		{
			_ignoreEvents = true;
			this.SetWindowRedraw(false);
			Format = DateTimePickerFormat.Short;
			CustomFormat = string.Empty;
			this.SetWindowRedraw(true);
			_ignoreEvents = false;
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnEnter(EventArgs e)
		{
			base.OnEnter(e);

			if (_ignoreEvents)
				return;

			SetDefaultFormat();

			if (!_isBlank)
				return;

			// SP-807: Date field will not keep today's date
			//_ignoreEvents = true;
			base.Text = DateTime.Today.ToShortDateString();
			//_ignoreEvents = false;
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnLeave(EventArgs e)
		{
			base.OnLeave(e);

			if (_ignoreEvents)
				return;

			if (_isBlank)
				SetBlankFormat();
			else
				SetDefaultFormat();
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown(e);

			if (e.KeyCode != Keys.Delete || _isBlank)
				return;

			SetValue(null);

			// The effects of setting the value to null cannot be seen unless the selected
			// control changes. Therefore, select the next control. REVIEW: Will the user
			// find this annoying? Inquiring minds want to know.
			// I can't get SelectNextControl to work, so I'll try it this way.
			var nextCtrl = Parent.GetNextControl(this, true);
			while (nextCtrl != null && !nextCtrl.CanSelect)
				nextCtrl = Parent.GetNextControl(nextCtrl, true);

			if (nextCtrl != null)
				nextCtrl.Focus();
			else
				SelectNextControl(this, true, true, true, true);
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnValueChanged(EventArgs e)
		{
			// Setting the Text property will trigger this event, and sometimes
			// we don't want to set _value when that happens.
			if (!_ignoreEvents)
				_isBlank = false;

			base.OnValueChanged(e);
		}
	}

	/// ------------------------------------------------------------------------------------
	public class AmbiguousDateException : Exception
	{
		public AmbiguousDateException(string msg) : base(msg)
		{
		}
	}
}
