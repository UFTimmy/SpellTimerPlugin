using System;
using System.IO;
using GeniePlugin.Interfaces;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using System.Windows.Forms;

namespace SpellTimerPlugin
{
    public class SpellTimer : IPlugin
    {
        //## Value class for holding spell info
        public class Spell
        {
            public string name;
            public bool active = false;
            public int duration = 0;
        }

        private bool _enabled = true;
        private string _filePath;

        //## List of all spells we know about.
        private List<Spell> spells;
        //## Spells in the last clear/popStream
        private List<Spell> poppedSpells = new List<Spell>();
        private CastBar _castbar;
        private SpellTimerForm _form;
        static void Main(string[] args)
        {
        }

        private Genie _host => Genie.Instance;

        public void Initialize(IHost host)
        {
            Genie.Instance.Initialize(ref host);
            string _pluginPath = _host.get_Variable("PluginPath");
            if (!_pluginPath.EndsWith("\\")) _pluginPath += "\\";
            _filePath = _pluginPath + "SpellTimer\\";
            _castbar = new CastBar(host);

            try
            {
                Directory.CreateDirectory(this._filePath);
            }
            catch (IOException ex)
            {
                _host.EchoText("Error reading SpellTimer settings file: " + ex.Message);
            }

            this.readSpellList();
        }

        public string ParseInput(string text)
        {
            if (text.StartsWith("/spelltimer"))
            {
                this._host.EchoText("Active:");
                foreach (Spell spell in this.spells)
                {
                    if (spell.active)
                    {
                        this._host.EchoText(spell.name + " (" + spell.duration + " roisaen)");
                    }
                }
                this._host.EchoText("Inactive:");
                foreach (Spell spell in this.spells)
                {
                    if (spell.active == false)
                    {
                        this._host.EchoText(spell.name + " (" + spell.duration + " roisaen)");
                    }
                }

                return "";
            }
            else if (text.StartsWith("/castbar"))
            {
                _castbar.ParseInput(text);
                return "";
            }
            else if (text.ToLower().Equals("quit") || text.ToLower().Equals("exit"))
            {
                this.serializeSpellList();
                return text;
            }
            else
            {
                return text;
            }
        }

        public string ParseText(string text, string window)
        {
            if (text.StartsWith("All Rights Reserved"))
            {
                this.readSpellList();
            }
            if (text.StartsWith("Connection closed."))
            {
                this.serializeSpellList();
            }

            if (window.Equals("percwindow"))
            {
                Regex pattern = new Regex("(.+?)\\s+\\((.+)\\)");
                Match match = pattern.Match(text);

                string spellName = match.Success ? match.Groups[1].Value.Trim() : text.Trim();
                int duration = 0;
                bool active = true;
                bool success = false;
                if (match.Success)
                {
                    success = true;
                    if (spellName.Equals("Osrel Meraud"))
                    {
                        Regex omPattern = new Regex("(\\d+)%");
                        Match omMatch = omPattern.Match(match.Groups[2].Value);

                        if (omMatch.Success)
                        {
                            duration = Convert.ToInt32(omMatch.Groups[1].Value);
                        }
                    }
                    else
                    {
                        Regex durationPattern = new Regex("(\\d+) roisaen");
                        Match durationMatch = durationPattern.Match(match.Groups[2].Value);

                        if (durationMatch.Success)
                        {
                            duration = Convert.ToInt32(durationMatch.Groups[1].Value);
                        }
                        else if (match.Groups[2].Value.Equals("OM")
                            || match.Groups[2].Value.Equals("Indefinite"))
                        {
                            duration = 999;
                        }
                    }
                }
                else if (spellName.EndsWith("small orbiting slivers of lunar magic"))
                {
                    if (spellName.StartsWith("Many"))
                    {
                        duration = 2;
                    }
                    else if (spellName.StartsWith("No"))
                    {
                        duration = 0;
                        active = false;
                    }
                    else
                    {
                        duration = 1;
                    }

                    spellName = "Moonblade Slivers";
                    success = true;
                }

                if (success)
                {
                    Spell poppedSpell = new Spell { name = spellName, active = active, duration = duration };

                    //this._host.EchoText("Update for " + poppedSpell.name + " - Duration: " + poppedSpell.duration.ToString());

                    bool spellInList = false;
                    foreach (Spell spell in this.spells)
                    {
                        if (spell.name.Equals(spellName))
                        {
                            spellInList = true;

                            spell.active = active;
                            spell.duration = duration;
                            break;
                        }
                    }

                    if (!spellInList)
                    {
                        this.spells.Add(poppedSpell);
                        //this._host.EchoText("SpellTimer found a new spell: " + poppedSpell.name);
                    }

                    //## Add this spell to the list of spells in the last pop.
                    this.poppedSpells.Add(poppedSpell);

                    this.setGenieVariables();
                }
            }

            return text;
        }

        public void ParseXML(string xml)
        {
            if (this._enabled == false)
                return;

            if (xml.Equals("<clearStream id=\"percWindow\"/>"))
            {
                //## Starting a new XML steam.
                //this._host.EchoText("clearStream start");
                this.poppedSpells = new List<Spell>();

                this.finishPop();
            }
            Match castMatch = CastBar.CASTPATTERN.Match(xml);
            if (castMatch.Success)
            {
                int casttime;
                if (int.TryParse(castMatch.Groups[1].Value, out casttime))
                {
                    _castbar.RunTimer(casttime);
                }

            }
        }

        private void finishPop()
        {
            //this._host.EchoText("finishPop");
            try
            {
                foreach (Spell spell in this.spells)
                {
                    bool wasPopped = false;
                    foreach (Spell poppedSpell in this.poppedSpells)
                    {
                        if (poppedSpell.name.Equals(spell.name))
                        {
                            wasPopped = true;
                            break;
                        }
                    }

                    //## Disable any spells we know about that were not popped.
                    if (!wasPopped)
                    {
                        spell.active = false;
                        spell.duration = 0;

                        //this._host.EchoText(spell.name + " is not on the list, set to inactive.");
                    }
                }

                this.setGenieVariables();
            }
            catch (Exception ex)
            {
                _host.SendText("#echo >DebugLog An error occured in the finishPop Method.");
                _host.SendText("#echo >DebugLog " + ex.Message);
            }
        }
        private void setGenieVariables()
        {
            //this._host.EchoText("We have " + this.spells.Count + " spells in the list.");

            //## Set Genie variables
            try
            {
                foreach (Spell spell in this.spells)
                {
                    string activeVar = "SpellTimer." + this.spellNameToVariableName(spell.name) + ".active";
                    string activeValue = (spell.active ? "1" : "0");
                    if (!this._host.get_Variable(activeVar).Equals(activeValue))
                    {
                        //this._host.EchoText("Updating " + spell.name + " active to " + activeValue);
                        this._host.SendText("#var " + activeVar + " " + activeValue);
                    }

                    string durationVar = "SpellTimer." + this.spellNameToVariableName(spell.name) + ".duration";
                    string durationValue = spell.duration.ToString();
                    if (!this._host.get_Variable(durationVar).Equals(durationValue))
                    {
                        //this._host.EchoText("Updating " + spell.name + " duration to " + durationValue);
                        this._host.SendText("#var " + durationVar + " " + durationValue);
                    }
                    //this._host.SendText("#var SpellTimer." + this.spellNameToVariableName(spell.name) + ".active " + (spell.active ? "1" : "0"));
                    //this._host.SendText("#var SpellTimer." + this.spellNameToVariableName(spell.name) + ".duration " + spell.duration.ToString());
                    //this._host.SendText("#save vars");

                    //this._host.EchoText("Setting " + spellVarName + " to " + (spell.active ? "1" : "0"));
                }
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
            }
        }

        private void serializeSpellList()
        {
            string characterName = this._host.get_Variable("charactername");
            if (characterName.Length == 0)
                return;

            try
            {
                FileStream writer = new FileStream(this.getXmlFileName(characterName), FileMode.Create);
                XmlSerializer serializer = new XmlSerializer(typeof(List<Spell>));
                serializer.Serialize(writer, this.spells);
                writer.Close();
                _host.EchoText("SpellTimer settings saved.");
            }
            catch (IOException ex)
            {
                _host.EchoText("Error writing SpellTimer settings file: " + ex.Message);
                ErrorLog.Write(ex);
            }
        }

        private void readSpellList()
        {
            try
            {
                this.spells = new List<Spell>();

                string characterName = this._host.get_Variable("charactername");
                if (characterName.Length == 0)
                    return;

                this._host.EchoText("Loading saved spells for SpellTimer for " + characterName);

                string fileName = this.getXmlFileName(characterName);
                if (File.Exists(fileName))
                {
                    try
                    {
                        using (Stream stream = File.Open(fileName, FileMode.Open))
                        {
                            XmlSerializer serializer = new XmlSerializer(typeof(List<Spell>));
                            this.spells = (List<Spell>)serializer.Deserialize(stream);

                            //## Spellnames sometimes have spaces, and we weren't trimming them, so this will update existing files.
                            foreach (Spell spell in this.spells)
                            {
                                spell.name = spell.name.Trim();
                            }

                            this._host.EchoText("We have " + this.spells.Count + " spells in the list.");
                        }
                    }
                    catch (IOException ex)
                    {
                        _host.EchoText("Error reading SpellTimer settings file: " + ex.Message);
                    }
                }

                this.setGenieVariables();
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
            }

        }

        private string spellNameToVariableName(string spellVarName)
        {
            spellVarName = spellVarName.Replace(" ", "");
            spellVarName = spellVarName.Replace("'", "");
            spellVarName = spellVarName.Replace("-", "");

            return spellVarName;
        }

        private string getXmlFileName(string charName)
        {
            return this._filePath + "SpellTimerSpells" + charName + ".xml";
        }

        public void ParentClosing()
        {
            this.serializeSpellList();
        }

        public string Name
        {
            get { return "Spell Timer"; }
        }

        public string Version
        {
            get { return "1.8"; }
        }

        public string Description
        {
            get { return "Turns the spell timer window into persistent variables that can be tested in scripts."; }
        }

        public string Author
        {
            get { return "UFTimmy @ AIM"; }
        }

        public bool Enabled
        {
            get { return _enabled; }
            set { _enabled = value; }
        }

        public void Show()
        {
            if (_form == null || _form.IsDisposed)
            {
                _form = new SpellTimerForm();
                _form.Text = "SpellTimer Plugin v" + Version;
                _form.MdiParent = _host.ParentForm;
            }
            _form.Show();
            _form.BringToFront();
        }

        public void VariableChanged(string variable)
        {
        }

    }
}