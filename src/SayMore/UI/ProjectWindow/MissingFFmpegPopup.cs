using System;
using System.Windows.Forms;

namespace SayMore.Utilities.ProjectWindow
{
	public partial class MissingFFmpegPopup : Form
	{
		private Timer _goUpTimer;
		private Timer _goDownTimer;
		private Timer _pauseTimer;
		private int startPosX;
		private int startPosY;

		public MissingFFmpegPopup()
		{
			InitializeComponent();
			// We want our window to be the top most
			TopMost = true;
			// Pop doesn't need to be shown in task bar
			ShowInTaskbar = false;
			// Create and run timer for animation
			_goUpTimer = new Timer();
			_goUpTimer.Interval = 50;
			_goUpTimer.Tick += GoUpTimerTick;
			_goDownTimer = new Timer();
			_goDownTimer.Interval = 50;
			_goDownTimer.Tick += GoDownTimerTick;
			_pauseTimer = new Timer();
			_pauseTimer.Interval = 10000;
			_pauseTimer.Tick += PauseTimerTick;
		}

		private void PauseTimerTick(object sender, EventArgs e)
		{
			_pauseTimer.Stop();
			_goDownTimer.Start();
		}


		protected override void OnLoad(EventArgs e)
		{
			// Move window out of screen
			startPosX = Screen.PrimaryScreen.WorkingArea.Width - Width;
			startPosY = Screen.PrimaryScreen.WorkingArea.Height;
			SetDesktopLocation(startPosX, startPosY);
			base.OnLoad(e);
			// Begin animation
			_goUpTimer.Start();
		}

		void GoUpTimerTick(object sender, EventArgs e)
		{
			//Lift window by 5 pixels
			startPosY -= 5;
			//If window is fully visible stop the timer
			if (startPosY < Screen.PrimaryScreen.WorkingArea.Height - Height)
			{
				_goUpTimer.Stop();
				_pauseTimer.Start();
			}
			else
				SetDesktopLocation(startPosX, startPosY);
		}

		private void GoDownTimerTick(object sender, EventArgs e)
		{
			//Lower window by 5 pixels
			startPosY += 5;
			//If window is fully visible stop the timer
			if (startPosY > Screen.PrimaryScreen.WorkingArea.Height)
			{
				_goDownTimer.Stop();
				Close();
			}
			else
				SetDesktopLocation(startPosX, startPosY);

		}

		private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			System.Diagnostics.Process.Start("http://saymore.palaso.org/download");
		}
	}
}
