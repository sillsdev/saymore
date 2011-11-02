using System;
using Palaso.Progress.LogBox;
using Palaso.Reporting;

namespace SayMore.UI
{
	/// <summary>
	/// This simple class (which I then didn't use) was written with the idea that we could collect up warnings/errors when loading a project, then display the accumulated ones at the end
	/// after loading all the files.
	/// </summary>
	public class ErrorCollector
	{
		public ErrorCollector()
		{
			Progress = new StringBuilderProgress();
		}
		public IProgress Progress { get; set; }
		public bool ShowDialogIfErrorsRecorded(string introErrorMessage)
		{
			if(Progress.ErrorEncountered)
			{
				ErrorReport.NotifyUserOfProblem(introErrorMessage + Environment.NewLine+ Environment.NewLine+ Progress.ToString());
				return true;
			}
			return false;
		}
	}
}
