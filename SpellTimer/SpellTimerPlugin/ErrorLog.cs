using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using GeniePlugin.Interfaces;

namespace SpellTimerPlugin
{
    class ErrorLog
    {
        public static void Write(Exception Log)
        {
            try
            {
                using (StreamWriter w = File.AppendText(Genie.Instance.get_Variable("PluginPath") + "\\SpellTImer\\errors.txt"))
                {
                    w.Write("\r\nLog Entry : ");
                    w.WriteLine($"{DateTime.Now.ToLongTimeString()} {DateTime.Now.ToLongDateString()}");
                    w.WriteLine("  :");
                    w.WriteLine($"  :{Log.Message}");
                    w.WriteLine("----------StackTrace-----------");
                    w.WriteLine($"  :{Log.StackTrace}");
                    w.WriteLine("-------------------------------");
                }
            }
            catch (Exception ex)
            {
                Genie.Instance.EchoText("An Error occurred while trying to log the following error." +
                   Environment.NewLine +
                   Log.Message +
                   Environment.NewLine +
                   Log.StackTrace +
                   Environment.NewLine +
                   "The error writing to the log was:" +
                   Environment.NewLine +
                   ex.Message
                   );
            }
        }
    }
}
