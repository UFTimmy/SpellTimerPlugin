using System;
using System.IO;
using GeniePlugin.Interfaces;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Collections;
using System.Xml.Serialization;

namespace SpellTimerPlugin
{
    //## Value class for holding spell info
    public class Spell
    {
        public string name;
        public bool active = false;
        public int duration = 0;
    }

    public class Class1 : IPlugin
    {
        private bool _enabled = true;
        private string _filePath;
        
        //## List of all spells we know about.
        private List<Spell> spells;

        static void Main(string[] args)
        {
        }

        private IHost _host;

        public void Initialize(IHost host)
        {
            this._host = host;
            _filePath = _host.get_Variable("PluginPath");
            //fix for Genie versions prior to 3.2
            if (string.IsNullOrEmpty(_filePath))
                _filePath = System.Windows.Forms.Application.StartupPath + @"\Plugins\";
            if (!_filePath.EndsWith(@"\"))
                _filePath += @"\";
            this.readSpellList();
        }

        public void Show()
        {
            
        }

        public void VariableChanged(string variable)
        {
        }

        public string ParseText(string text, string window)
        {
            if (text.StartsWith("You look around, taking a moment"))
            {
                //this._host.EchoText("We connected");
                this.readSpellList();
            }

            if (window.Equals("percwindow"))
            {
                Regex pattern = new Regex("(.+?)\\s+\\((.+)\\)");
                Match match = pattern.Match(text);
                if (match.Success)
                {
                    string spellName = match.Groups[1].Value;

                    Regex durationPattern = new Regex("(\\d+) roisaen");
                    Match durationMatch = durationPattern.Match(match.Groups[2].Value);

                    int duration;
                    if (durationMatch.Success)
                    {
                        duration = Convert.ToInt32(durationMatch.Groups[1].Value);
                    }
                    else if (match.Groups[2].Value.Equals("OM") 
                        || match.Groups[2].Value.Equals("Indefinite")
                        || spellName.Equals("Osrel Meraud"))
                    {
                        duration = 999;
                    }
                    else
                    {
                        duration = 0;
                    }

                    bool spellInList = false;
                    foreach (Spell spell in this.spells)
                    {
                        if (spell.name.Equals(spellName))
                        {
                            spellInList = true;

                            spell.active = true;
                            spell.duration = duration;
                            break;
                        }
                    }

                    if (!spellInList)
                    {
                        this.spells.Add(new Spell { name = spellName, active = true, duration = duration });
                    }

                    this.setGenieVariables();
                }
            }

            return text;
        }

        private void setGenieVariables()
        {            
            //this._host.EchoText("We have " + this.spells.Count + " spells in the list.");

            //## Set Genie variables
            foreach (Spell spell in this.spells)
            {
                this._host.SendText("#var SpellTimer." + this.spellNameToVariableName(spell.name) + ".active " + (spell.active ? "1" : "0"));
                this._host.SendText("#var SpellTimer." + this.spellNameToVariableName(spell.name) + ".duration " + spell.duration.ToString());
                //this._host.SendText("#save vars");

                //this._host.EchoText("Setting " + spellVarName + " to " + (spell.active ? "1" : "0"));
            }

            this.serializeSpellList();
        }

        private void verifyGenieVariables()
        {
            foreach (Spell spell in this.spells)
            {
                string active = this._host.get_Variable("SpellTimer." + this.spellNameToVariableName(spell.name) + ".active");
                string duration = this._host.get_Variable("SpellTimer." + this.spellNameToVariableName(spell.name) + ".duration");

                if (!active.Equals(spell.active ? "1" : "0"))
                {
                    this._host.EchoText("Genie SpellTimer active variable not set to expected.");
                    this._host.EchoText("--- " + this.spellNameToVariableName(spell.name));
                    this._host.EchoText("--- Expected: " + (spell.active ? "1" : "0"));
                    this._host.EchoText("--- Actual: " + active);
                    this._host.SendText("/notify Genie variable not set to expected.");
                }
                if (!duration.Equals(spell.duration.ToString()))
                {
                    this._host.EchoText("Genie SpellTimer duration variable not set to expected.");
                    this._host.EchoText("--- " + this.spellNameToVariableName(spell.name));
                    this._host.EchoText("--- Expected: " + spell.duration.ToString());
                    this._host.EchoText("--- Actual: " + duration);
                    this._host.SendText("/notify Genie variable not set to expected.");
                }
            }
        }

        private string spellNameToVariableName(string spellVarName)
        {
            spellVarName = spellVarName.Replace(" ", "");
            spellVarName = spellVarName.Replace("'", "");
            spellVarName = spellVarName.Replace("-", "");

            return spellVarName;
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
            }
            catch (IOException ex)
            {
                _host.EchoText("Error writing SpellTimer settings file: " + ex.Message);
            }
        }

        private void readSpellList()
        {
            this.spells = new List<Spell>();

            string characterName = this._host.get_Variable("charactername");
            if (characterName.Length == 0)
                return;

            this._host.EchoText("Loading saved spells for SpellTimer for " + characterName);

            string fileName = getXmlFileName(characterName);
            if (File.Exists(fileName))
            {
                try
                {
                    using (Stream stream = File.Open(fileName, FileMode.Open))
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(List<Spell>));
                        this.spells = (List<Spell>)serializer.Deserialize(stream);

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

        private string getXmlFileName(string charName)
        {
            return _filePath + "SpellTimerSpells" + charName + ".xml";
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
                        this._host.EchoText(spell.name + " (" + spell.duration + " roiasen)");
                    }
                }
                this._host.EchoText("Inactive:");
                foreach (Spell spell in this.spells)
                {
                    if (spell.active == false)
                    {
                        this._host.EchoText(spell.name + " (" + spell.duration + " roiasen)");
                    }
                }

                return "";
            }
            else
            {
                return text;
            }
        }

        public void ParseXML(string xml)
        {
            if (_enabled == false)
                return;

            if (xml.Equals("<clearStream id=\"percWindow\"/>"))
            {
                //this._host.EchoText("Clear spells");
                this.verifyGenieVariables();

                foreach (Spell spell in this.spells)
                {
                    spell.active = false;
                    spell.duration = 0;
                }

                this.setGenieVariables();
            }
        }

        public void ParentClosing()
        {
        }

        public string Name
        {
            get { return "Spell Timer"; }
        }

        public string Version
        {
            get { return "1.5"; }
        }

        public string Description
        {
            get { return "Turns the spell timer window into persistent variables.";  }
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
    }
}
