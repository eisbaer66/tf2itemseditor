using System;
using System.Collections;
using System.Diagnostics;
using System.Threading.Tasks;
using log4net;

namespace icebear
{
    public static class ProcessUtil
    {
        public static async Task<ProcessResult> StartBackgroundProcess(string exePath, string args, ILog log)
        {
            using (NDC.Push(string.Format("[running {0}]", exePath)))
            {
                Process process = new Process
                                  {
                                      StartInfo = new ProcessStartInfo(exePath, args)
                                                  {
                                                      CreateNoWindow = true
                                                  },
                                  };
                return await process.StartWithLoggingAsync(log);
            }
        }

        public static Process InitializeLogging(this Process process)
        {
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.UseShellExecute = false;

            return process;
        }
        public static ProcessResult CaptureLogging(this ProcessResult result, ILog log)
        {

            if (!String.IsNullOrEmpty(result.StandardOutput))
                log.Debug(result.StandardOutput);
            if (!String.IsNullOrEmpty(result.ErrorOutput))
                log.Error(result.ErrorOutput);

            string exitMessage = "exited with ExitCode " + result.ExitCode;
            if (result.ExitCode == 0)
                log.Debug(exitMessage);
            else
                log.Error(exitMessage);

            return result;
        }

        public static async Task<ProcessResult> StartWithLoggingAsync(this Process process, ILog log)
        {
            ProcessResult result = await process
                .InitializeLogging()
                .StartAsync();
            return result
                .CaptureLogging(log);
        }
        public static Task<ProcessResult> StartAsync(this Process process)
        {
            TaskCompletionSource<ProcessResult> taskCompletionSource = new TaskCompletionSource<ProcessResult>();
            Stack ndcStack = NDC.CloneStack();

            process.EnableRaisingEvents = true;
            process.Exited += (s, a) =>
                              {
                                  NDC.Inherit(ndcStack);
                                  ProcessResult result = new ProcessResult
                                                         {
                                                             ExitCode = process.ExitCode,
                                                             StandardOutput = process.StandardOutput.ReadToEnd(),
                                                             ErrorOutput = process.StandardError.ReadToEnd(),
                                                         };

                                  taskCompletionSource.SetResult(result);
                                  process.Dispose();
                              };

            process.Start();

            return taskCompletionSource.Task;
        }
    }
}