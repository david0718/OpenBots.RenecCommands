using OpenBots.Core.Attributes.PropertyAttributes;
using OpenBots.Core.Command;
using OpenBots.Core.Enums;
using OpenBots.Core.Infrastructure;
using OpenBots.Core.Properties;
using OpenBots.Core.Utilities.CommandUtilities;
using OpenBots.Core.Utilities.CommonUtilities;

using System;
using System.Net;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Windows.Forms;
using Tasks = System.Threading.Tasks;


namespace RenecCommands
{
	[Serializable]
	[Category("Renec Commands")]
	[Description("This command download and save file.")]
	public class DownloadCommand : ScriptCommand
	{

		

		[Required]
		[DisplayName("Download Link")]
		[Description("Enter URI of the file to download.")]
		[SampleUsage("\"http://rpachallenge.com/assets/ \" || vLink")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(string) })]
		public string v_DownloadLink { get; set; }


		[Required]
		[DisplayName("Save Full Path")]
		[Description("Enter full path ( with name) for save downloaded file.")]
		[SampleUsage("\"D:\\text.txt \" || vSaveFullPath")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(string) })]
		public string v_SaveFullPath { get; set; }



		[Required]
		[Editable(false)]
		[DisplayName("Output Result Variable")]
		[Description("Create a new variable or select a variable from the list.")]
		[SampleUsage("vResult")]
		[Remarks("New variables/arguments may be instantiated by utilizing the Ctrl+K/Ctrl+J shortcuts.")]
		[CompatibleTypes(new Type[] { typeof(string) })]
		public string v_Result { get; set; }

		public DownloadCommand()
		{
			CommandName = "DownloadCommand";
			SelectionName = "Download Command";
			CommandEnabled = true;
			CommandIcon = Resources.command_string;

			
		}

		public async override Tasks.Task RunCommand(object sender)
		{
			var engine = (IAutomationEngineInstance)sender;

			var downloadLink = (string)await v_DownloadLink.EvaluateCode(engine);
			var saveFullPath = (string)await v_SaveFullPath.EvaluateCode(engine);
			string resultData = "START";
			try
            {
				resultData = "OK";
				var wCli = new WebClient();
				wCli.DownloadFile(downloadLink, saveFullPath);
			}
			catch (WebException webEx)
			{
				var err = webEx.ToString();
				resultData = err;
			}
		

			resultData.SetVariableValue(engine, v_Result);
		}

		public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
		{
			base.Render(editor, commandControls);

	
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_DownloadLink", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_SaveFullPath", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultOutputGroupFor("v_Result", this, editor));

			return RenderedControls;
		}

		public override string GetDisplayValue()
		{
			return base.GetDisplayValue() + $" [Get '{v_DownloadLink}' - Store File in '{v_SaveFullPath}']";
		}
	}
}