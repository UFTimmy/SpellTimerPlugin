using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using GeniePlugin.Interfaces;
using System.Windows.Forms;

namespace SpellTimerPlugin
{
    sealed class Genie
    {
        private static Mutex _mutex = new Mutex();
        private static IHost _host;
        private static Genie _instance;

        public static Genie Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Genie();
                }
                return _instance;
            }
        }

        public Form ParentForm
        {
            get
            {
#if DEBUG
                return new Form();
#else
                return _host.ParentForm;
#endif
            }
        }

        private Genie()
        {

        }


        public void Initialize(ref IHost host)
        {
            _host = host;
        }

        public string get_Variable(string variableName)
        {
            string variable = "";
#if !DEBUG
            variable =_host.get_Variable(variableName);
#endif
            return variable;
        }

        public void set_Variable(string variableName, string variableValue)
        {
#if !DEBUG
            _host.set_Variable(variableName, variableValue);
#endif
        }
        public void SetVariable(string variableName, string variableValue)
        {
#if !DEBUG
            _host.SendText("#var " + variableName + " " + variableValue);
#endif
        }

        public void EchoText(string echo)
        {
#if !DEBUG
            _host.EchoText(echo);
#endif
        }
        public void SendText(string text)
        {
#if !DEBUG
            _host.SendText(text);
#endif
        }
    }
}
