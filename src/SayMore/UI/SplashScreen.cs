// --------------------------------------------------------------------------------------------
#region // Copyright (c) 2014, SIL International. All Rights Reserved.
// <copyright from='2012' to='2014' company='SIL International'>
//		Copyright (c) 2014, SIL International. All Rights Reserved.
//
//		Distributable under the terms of the MIT License (http://sil.mit-license.org/)
// </copyright>
#endregion
//
// File: SplashScreen.cs
// --------------------------------------------------------------------------------------------
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;

namespace SayMore.UI
{
	#region ISplashScreen interface
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Public interface (exported with COM wrapper) for the splash screen
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public interface ISplashScreen
	{
		/// ----------------------------------------------------------------------------------------
		void Show();

		/// ----------------------------------------------------------------------------------------
		void Show(bool showBuildDate, bool isBetaVersion);

		/// ----------------------------------------------------------------------------------------
		void ShowWithoutFade();

		/// ----------------------------------------------------------------------------------------
		/// <summary>
		/// Activates (brings back to the top) the splash screen (assuming it is already visible
		/// and the application showing it is the active application).
		/// </summary>
		/// ----------------------------------------------------------------------------------------
		void Activate();

		/// ----------------------------------------------------------------------------------------
		void Close();

		/// ----------------------------------------------------------------------------------------
		void Refresh();

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// The copyright info to display on the splash screen
		/// </summary>
		/// <remarks>
		/// .Net clients should not set this. It will be ignored.
		/// </remarks>
		/// ------------------------------------------------------------------------------------
		string Copyright { set;}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// The message to display to indicate startup activity on the splash screen
		/// </summary>
		/// ------------------------------------------------------------------------------------
		string Message { set;}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a value indicating whether or not the ISplashScreen's underlying form is
		/// still available (i.e. non null).
		/// </summary>
		/// ------------------------------------------------------------------------------------
		bool StillAlive { get;}
	}

	#endregion

	#region SplashScreen implementation
	/// ----------------------------------------------------------------------------------------
	public class SplashScreen : ISplashScreen
	{
		#region Data members
		private delegate void MethodWithStringDelegate(string value);

		private bool m_useFading = true;
		private Thread m_thread;
		private SplashScreenForm _splashScreenForm;
		internal EventWaitHandle m_waitHandle;
		#endregion

		#region Public Methods
		/// ------------------------------------------------------------------------------------
		void ISplashScreen.Show(bool showBuildDate, bool isBetaVersion)
		{
			InternalShow();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Shows the splash screen
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void ISplashScreen.Show()
		{
			InternalShow();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Does the work of showing.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void InternalShow()
		{
			if (m_thread != null)
				return;

			m_waitHandle = new EventWaitHandle(false, EventResetMode.AutoReset);

			if (Thread.CurrentThread.Name == null)
				Thread.CurrentThread.Name = "Main";

			// For some reason we have to specify a stack size, otherwise we get a stack overflow.
			// The default stack size of 1MB works on WinXP. Needs to be 2MB on Win2K.
			// Don't know what value it's using if we don't specify it.
			m_thread = new Thread(StartSplashScreen, 0x200000);
			m_thread.CurrentUICulture = Thread.CurrentThread.CurrentUICulture;
			m_thread.IsBackground = true;
			m_thread.SetApartmentState(ApartmentState.STA);
			m_thread.Name = "SplashScreen";
			m_thread.Start();
			m_waitHandle.WaitOne();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Shows the splash screen
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void ISplashScreen.ShowWithoutFade()
		{
			m_useFading = false;
			StartSplashScreen();

			// Wait until the splash screen is actually up
			while (_splashScreenForm == null || !_splashScreenForm.Visible)
				Thread.Sleep(50);
		}

		/// ----------------------------------------------------------------------------------------
		/// <summary>
		/// Activates (brings back to the top) the splash screen (assuming it is already visible
		/// and the application showing it is the active application).
		/// </summary>
		/// ----------------------------------------------------------------------------------------
		void ISplashScreen.Activate()
		{
			if (!m_useFading)
			{
				_splashScreenForm.Activate();
				Application.DoEvents();
				return;
			}

			Debug.Assert(_splashScreenForm != null);
			lock (_splashScreenForm)
			{
				_splashScreenForm.Invoke(new MethodInvoker(_splashScreenForm.Activate));
			}
		}

		/// ----------------------------------------------------------------------------------------
		/// <summary>
		/// Closes the splash screen
		/// </summary>
		/// ----------------------------------------------------------------------------------------
		void ISplashScreen.Close()
		{
			if (_splashScreenForm.Opacity < 1.0)
			{
				lock (_splashScreenForm)
				{
					_splashScreenForm.Invoke(new MethodInvoker(_splashScreenForm.MakeFullyOpaque));
				}
			}

			if (_splashScreenForm == null)
				return;

			if (!m_useFading)
			{
				_splashScreenForm.Hide();
				_splashScreenForm = null;
				return;
			}

			lock (_splashScreenForm)
			{
				try
				{
					if (!_splashScreenForm.IsDisposed)
						_splashScreenForm.Invoke(new MethodInvoker(_splashScreenForm.RealClose));
				}
				catch { }
			}

			try
			{
				m_thread.Join();
				lock (_splashScreenForm)
				{
					_splashScreenForm.Dispose();
				}
			}
			catch { }

			_splashScreenForm = null;
			m_thread = null;
		}

		/// ----------------------------------------------------------------------------------------
		/// <summary>
		/// Refreshes the display of the splash screen
		/// </summary>
		/// ----------------------------------------------------------------------------------------
		void ISplashScreen.Refresh()
		{
			if (!m_useFading)
			{
				_splashScreenForm.Refresh();
				Application.DoEvents();
				return;
			}

			Debug.Assert(_splashScreenForm != null);
			lock (_splashScreenForm)
			{
				_splashScreenForm.Invoke(new MethodInvoker(_splashScreenForm.Refresh));
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a value indicating whether or not the ISplashScreen's underlying form is
		/// still available (i.e. non null).
		/// </summary>
		/// ------------------------------------------------------------------------------------
		bool ISplashScreen.StillAlive
		{
			get {return _splashScreenForm != null; }
		}

		#endregion

		#region Public Properties needed for all clients
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// The message to display to indicate startup activity on the splash screen
		/// </summary>
		/// ------------------------------------------------------------------------------------
		string ISplashScreen.Message
		{
			set
			{
				Debug.Assert(_splashScreenForm != null);
				lock (_splashScreenForm)
				{
					_splashScreenForm.Invoke(new MethodWithStringDelegate(_splashScreenForm.SetMessage), value);
				}
			}
		}

		#endregion

		#region Public properties set automatically in constructor for .Net apps
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// The copyright info which appears in the Copyright label on the splash screen
		/// </summary>
		/// <remarks>
		/// .Net clients should not set this. It will be ignored. They should set the
		/// AssemblyCopyrightAttribute attribute in AssemblyInfo.cs of the executable.
		/// </remarks>
		/// ------------------------------------------------------------------------------------
		string ISplashScreen.Copyright
		{
			set
			{
				Debug.Assert(_splashScreenForm != null);
				lock (_splashScreenForm)
				{
					_splashScreenForm.Invoke(new MethodWithStringDelegate(_splashScreenForm.SetCopyright), value);
				}
			}
		}
		#endregion

		#region private/protected methods
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Starts the splash screen.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected virtual void StartSplashScreen()
		{
			_splashScreenForm = GetSplashScreenForm();
			_splashScreenForm.RealShow(m_waitHandle, m_useFading);
			if (m_useFading)
				_splashScreenForm.ShowDialog();
		}

		/// ------------------------------------------------------------------------------------
		protected virtual SplashScreenForm GetSplashScreenForm()
		{
			_splashScreenForm = new SplashScreenForm();
			return _splashScreenForm;
		}

		#endregion
	}

	#endregion
}
