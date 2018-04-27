using UnityEngine;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace x600d1dea.stubs 
{
	public class Cmd
	{
		static string cwd = Path.GetDirectoryName(Application.dataPath);

		public static string ChangeWorkingDir(string dir)
		{
			UnityEngine.Debug.Log("Change Working Dir " + dir);
			var old = cwd;
			cwd = dir;
			return old;
		}

		public static void Execute(string cmdLine, bool threaded = false)
		{
			cmdLine = cmdLine.Replace('/', '\\');
			if (threaded)
			{
				var th = new Thread(() => Command(cmdLine));
				th.Start();
			}
			else
			{
				Command(cmdLine);
			}
		}

		static void Command(string cmdLine)
		{
			CustomEditorApp.AddTask(() => UnityEngine.Debug.LogFormat("executing {0}", cmdLine));
			var proc = new Process();
			var processInfo = new ProcessStartInfo("cmd.exe", "/c " + cmdLine);
			processInfo.CreateNoWindow = true;
			processInfo.UseShellExecute = false;
			processInfo.WorkingDirectory = cwd;
			processInfo.RedirectStandardOutput = true;
			processInfo.RedirectStandardError = true;

			proc.StartInfo = processInfo;
			proc.OutputDataReceived += (s, e) =>
			{
				if (!string.IsNullOrEmpty(e.Data))
				{
					CustomEditorApp.AddTask(() => UnityEngine.Debug.Log(e.Data));
				}
			};
			proc.ErrorDataReceived += (s, e) =>
			{
				if (!string.IsNullOrEmpty(e.Data))
					CustomEditorApp.AddTask(() => UnityEngine.Debug.LogError(e.Data));
			};

			proc.Start();
			proc.BeginOutputReadLine();
			proc.BeginErrorReadLine();
			proc.WaitForExit();
			var exitCode = proc.ExitCode;
			CustomEditorApp.AddTask(() => UnityEngine.Debug.LogFormat("{0} end with {1}", cmdLine, exitCode));
			proc.Close();
		}

	}
}
