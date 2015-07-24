# SpellTimerPlugin
Plugin for the Genie3 client (http://genieclient.com)

I have created a plugin that takes the spells from the new Spell Timer window just released and stores the data as Genie variables so that you can keep track of what buffs you have active.

For example, if you wanted to make sure you always had Clear Vision active, you could use this snippet:
if (!$SpellTimer.ClearVision.active) then <cast cv>

The plugin also keeps track of the duration, so you could rebuff before the buff falls off.
if ($SpellTimer.ClearVision.duration < 2) then <cast cv>

Additionally, you can type /spelltimer and the plugin will output a list of spells it knows about, their duration, and whether they are active or not.

To see a list of all the variables it keeps track of, type #var Spell:
Active variables:
Filter: Spell
$SpellTimer.ClearVision.active=1
$SpellTimer.ClearVision.duration=8

Please let me know if you encounter any issues. I actually only have a level 1 MM in prime, so I don't even have two spells to test with.
