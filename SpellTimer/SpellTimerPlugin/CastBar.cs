using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.IO;
using GeniePlugin.Interfaces;


namespace SpellTimerPlugin
{
    public class CastBar
    {
        #region Properties
        static Regex _castpattern = new Regex("<castTime value=\'(.*)\'/>$");
        public static Regex CASTPATTERN
        {
            get
            {
                return _castpattern;
            }
        } 

        #region Settings
        private bool _castbar = true;
        private bool _countdown = true;
        private bool _spinners = true;
        private bool _ready = true;
        private bool _enabled = true;

        private string _triquarterColor = "";
        private string _halfColor = "";
        private string _quarterColor = "";
        private string _readyColor = "";
        #endregion

        private IHost _host;
        private Thread _timer;
        private int _spinnerIndex;
        private int CastTime { get; set; }
        private Thread Timer
        {
            get
            {
                if(_timer == null)
                {
                    _timer = new Thread(CountdownTimer);
                }
                return _timer;
            }
            set
            {
                _timer = value;
            }
        }

        #endregion

#region Constructors
        public CastBar(IHost host)
        {
            _host = host;
            CastTime = 0;
            LoadCountdownSettings();
        }
        private void LoadCountdownSettings()
        {
            _enabled = !(_host.get_Variable("SpellTimer.EnableCastBar") == "0");
            if (!_enabled) return;

            _countdown          = !(_host.get_Variable("SpellTimer.countdown") == "0");
            _castbar            = !(_host.get_Variable("SpellTimer.progress") == "0");
            _spinners           = !(_host.get_Variable("SpellTimer.spinners") == "0");
            _ready              = !(_host.get_Variable("SpellTimer.prepared") == "0");

            _triquarterColor    = _host.get_Variable("SpellTimer.triquarter");
            _halfColor          = _host.get_Variable("SpellTimer.half");
            _quarterColor       = _host.get_Variable("SpellTimer.quarter");
            _readyColor         = _host.get_Variable("SpellTimer.ready");
        }
        #endregion

        #region Public Methods
        public void RunTimer(int GameTimePrepWillFinish)
        {
            try
            {
                LoadCountdownSettings();
                if (!_enabled)
                {
                    return;
                }
                CastTime = GameTimePrepWillFinish;
                if (Timer.IsAlive)
                {
                    try
                    {
                        Timer.Abort();
                    }
                    catch (Exception ex)
                    {
                        //Aborting Thread can throw an error that the thread is being aborted.
                        //Ignore that, we intended to abort.
                    }
                }
                Timer = new Thread(CountdownTimer);
                //this._host.SendText("#var CastTime " + CastTime);
                //int.TryParse(_host.get_Variable("CastTime"), out CastTime;
                Timer.Start();
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
            }

        }

        public void ParseInput(string input)
        {
            input = input.ToLower().Trim();
            if(input == "/castbar")
            {
                _host.EchoText("Castbar by Jingo");
                _host.EchoText(" ");
                _host.EchoText("Commands:");
                _host.EchoText("/castbar {on|off}\tToggles Castbar Functionality.");
                _host.EchoText("/castbar progress {on|off}\tToggles display of the progress bar.");
                _host.EchoText("/castbar countdown {on|off}\tToggles display of the numeric countdown.");
                _host.EchoText("/castbar prepared {on|off}\tToggles display of the word 'Ready' when prepped.");
                _host.EchoText("/castbar spinners {on|off}\tToggles display of spinning 'graphic' when prepped.");
                _host.EchoText(" ");
                _host.EchoText("Color Options");
                _host.EchoText("You can recolor the Castbar based on the remaining percentage to be cast - 75, 50, 25, and Ready.");
                _host.EchoText("Valid colors are those recognized by Genie's Echo command - named or RGB Hex. You can test using #ECHO [color] [text].");
                _host.EchoText("Color Option Commands:");
                _host.EchoText("/castbar triquarter {clear|color}");
                _host.EchoText("/castbar half {clear|color}");
                _host.EchoText("/castbar quarter {clear|color}");
                _host.EchoText("/castbar ready {clear|color}");
            }
            else if (input == "/castbar on")
            {
                _host.SendText("#var SpellTimer.EnableCastBar 1");
            }
            else if (input == "/castbar off")
            {
                _host.SendText("#var SpellTimer.EnableCastBar 0");
            }
            else {
                Match command = new Regex("/castbar (progress|countdown|prepared|spinners|triquarter|half|quarter|ready) (.*)").Match(input);
                if (command.Success)
                {
                    string update = "#var SpellTimer." + command.Groups[1].Value + " ";
                    switch (command.Groups[2].Value)
                    {
                        case "on":
                            update += "1";
                            break;
                        case "off":
                            update += "0";
                            break;
                        case "clear":
                            update += "\"\"";
                            break;
                        default:
                            update += command.Groups[2].Value;
                            break;
                    }
                    _host.SendText(update);
                    _host.SendText("#save variable");
                }else
                {
                    _host.EchoText("Command not recognized.");
                }
            }
        }

        #endregion
        private void CountdownTimer()
        {
            try
            {
                int gametime = 0;
                if (int.TryParse(_host.get_Variable("gametime"), out gametime))
                {
                    int preptime = CastTime - gametime;
                    int countdown = preptime;
                    this._host.SendText("#var CastTimeRemaining " + countdown.ToString());

                    while (_host.get_Variable("preparedspell") != "None")
                    {
                        _host.SendText("#clear Casting;#echo >Casting " + CreateDisplayString(countdown, preptime));
                        Thread.Sleep(countdown > 0 ? 1000 : 250);
                        if (countdown > 0)
                        {
                            countdown--;
                            this._host.SendText("#var CastTimeRemaining " + countdown.ToString());
                        }
                    }
                }
                _host.SendText("#clear Casting");
            }
            catch(Exception ex)
            {
                if(!(ex.Message == "Thread was being aborted.")) ErrorLog.Write(ex);

            }
        }

        private string CreateDisplayString(int countdown, int preptime)
        {
            string display = Color(countdown, preptime) + " ";
            if(countdown > 0)
            {
                if (_countdown)
                {
                    display += countdown;
                }

                if (_castbar)
                {
                    display += " " + CreateBar(countdown);
                    this._host.SendText("#var CastBar " + display);
                }
                
            }
            else if(countdown <= 0)
            {
                string nextspinner = Spinner();
                if (_spinners)
                {
                    display += nextspinner;
                }
                if (_ready)
                {
                    display += "Ready";
                    if (_spinners) display += nextspinner;
                }
            }
            return display;
        }
        private string CreateBar(int countdown)
        {
            string countdownBar = "";
            for (int i = 0; i < countdown; i++)
            {
                countdownBar += "|";
            }
            return countdownBar;
        }

        private string Spinner()
        {
            if (!_spinners) return "";
            char[] spinnerArray = new char[] { '-', '\\', '|', '/' };
            _spinnerIndex++;
            if (_spinnerIndex >= spinnerArray.Length) _spinnerIndex = 0;
            return spinnerArray[_spinnerIndex].ToString();

        }

        private string Color(int remainingTime, int preptime)
        {
            if (remainingTime <= 0) return _readyColor;
            if (remainingTime <= preptime / 4) return _quarterColor;
            if (remainingTime <= preptime / 2) return _halfColor;
            if (remainingTime <= preptime - (preptime / 4)) return _triquarterColor;
            return "";
        }

    }
}
