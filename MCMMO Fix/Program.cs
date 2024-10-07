using System;
using System.Globalization;
using System.IO;
using System.Reflection.Metadata;
using CsvHelper;
using CsvHelper.Configuration;

Dictionary<string, UserLevels> beforeUsers = new();
Dictionary<string, UserLevels> afterUsers = new();

Dictionary<string, UserLevels> adjustedUsers = new();

string beforeFilepath = "before.users";
string afterFilepath = "after.users";

ParseFileToObject(beforeUsers, beforeFilepath);
ParseFileToObject(afterUsers, afterFilepath);

CrunchNumbers(beforeUsers, afterUsers, adjustedUsers);

// Write the new dict object to CSV for readability
using (var writer = new StreamWriter("adjusted.csv"))
using (var csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture)))
{
	csv.WriteRecords(adjustedUsers);
}

Console.Write(adjustedUsers.ToString());

static void ParseFileToObject(Dictionary<string, UserLevels> beforeUsers, string beforeFilepath)
{
	foreach (string line in File.ReadLines(beforeFilepath).Where(s => s.Contains("mcMMO Database created on") == false))
	{
		var parse = line.Split(":");
		string playerName = parse[0];

		UserLevels playerLevels = new()
		{
			MINING = int.Parse(parse[1]),
			WOODCUTTING = int.Parse(parse[5]),
			REPAIR = int.Parse(parse[7]),
			UNARMED = int.Parse(parse[8]),
			HERBALISM = int.Parse(parse[9]),
			EXCAVATION = int.Parse(parse[10]),
			ARCHERY = int.Parse(parse[11]),
			SWORDS = int.Parse(parse[12]),
			AXES = int.Parse(parse[13]),
			ACROBATICS = int.Parse(parse[14]),
			TAMING = int.Parse(parse[24]),
		};
		beforeUsers.Add(playerName, playerLevels);
	}
}

static int DetermineNewValue(int newVal, int oldVal, string skillName, string playerName)
{
	// If a player is deemed to have earned too many levels based on thresholds
	if (newVal - oldVal > Constants.thresholds[skillName])
	{
		Console.Write($"{playerName}'s {skillName} skill should be reduced from {newVal} to {oldVal + Constants.thresholds[skillName]} \n");
		return oldVal + Constants.thresholds[skillName];
	}
	return newVal;
}

static void CrunchNumbers(Dictionary<string, UserLevels> beforeUsers, Dictionary<string, UserLevels> afterUsers, Dictionary<string, UserLevels> adjustedUsers)
{
	foreach (var i in afterUsers)
	{
		var j = new UserLevels();
		if (beforeUsers.ContainsKey(i.Key))
		{
			j = beforeUsers[i.Key];
		}

		UserLevels adjustedPlayerLevels = new()
		{
			MINING = DetermineNewValue(i.Value.MINING, j.MINING, "MINING", i.Key),
			WOODCUTTING = DetermineNewValue(i.Value.WOODCUTTING, j.WOODCUTTING, "WOODCUTTING", i.Key),
			REPAIR = DetermineNewValue(i.Value.REPAIR, j.REPAIR, "REPAIR", i.Key),
			UNARMED = DetermineNewValue(i.Value.UNARMED, j.UNARMED, "UNARMED", i.Key),
			HERBALISM = DetermineNewValue(i.Value.HERBALISM, j.HERBALISM, "HERBALISM", i.Key),
			EXCAVATION = DetermineNewValue(i.Value.EXCAVATION, j.EXCAVATION, "EXCAVATION", i.Key),
			ARCHERY = DetermineNewValue(i.Value.ARCHERY, j.ARCHERY, "ARCHERY", i.Key),
			SWORDS = DetermineNewValue(i.Value.SWORDS, j.SWORDS, "SWORDS", i.Key),
			AXES = DetermineNewValue(i.Value.MINING, j.MINING, "MINING", i.Key),
			ACROBATICS = DetermineNewValue(i.Value.AXES, j.AXES, "AXES", i.Key),
			TAMING = DetermineNewValue(i.Value.TAMING, j.TAMING, "TAMING", i.Key),
		};

		adjustedUsers.Add(i.Key, adjustedPlayerLevels);
	}
}