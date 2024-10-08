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

string[] logs = Directory.GetFiles("Logs");

foreach (var i in logs)
{
	ParseLogs(beforeUsers, i);
}

List<string> changes = new();
string outputFilePath = "output.txt";
File.Delete(outputFilePath);

ParseFileToObject(afterUsers, afterFilepath);

CrunchNumbers(beforeUsers, afterUsers, adjustedUsers, changes);

File.WriteAllLines(outputFilePath, changes);
// Write the new dict object to CSV for readability
// using (var writer = new StreamWriter("adjusted.csv"))
// using (var csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture)))
// {
// 	csv.WriteRecords(adjustedUsers);
// }


static void ParseLogs(Dictionary<string, UserLevels> beforeUsers, string path)
{
	foreach (string line in File.ReadLines(path).Where(s => s.Contains("has reached level") == true))
	{
		string player = "";
		string stat = "";
		int level;

		var temp = line.Split("(mcMMO) ");
		temp = temp[1].Split(" has reached level ");

		player = temp[0].Contains(" ") ? temp[0].Split(" ")[1] : temp[0];

		temp = temp[1].Split(" in ");

		level = int.Parse(temp[0].Replace(",", String.Empty));
		stat = temp[1].Split("!")[0].ToUpper();

		if (!beforeUsers.ContainsKey(player))
		{
			beforeUsers.Add(player, new UserLevels());
		}

		switch (stat)
		{
			case "MINING":
				beforeUsers[player].MINING = Math.Max(level, beforeUsers[player].MINING);
				break;

			case "WOODCUTTING":
				beforeUsers[player].WOODCUTTING = Math.Max(level, beforeUsers[player].WOODCUTTING);
				break;

			case "REPAIR":
				beforeUsers[player].REPAIR = Math.Max(level, beforeUsers[player].REPAIR);
				break;

			case "UNARMED":
				beforeUsers[player].UNARMED = Math.Max(level, beforeUsers[player].UNARMED);
				break;

			case "HERBALISM":
				beforeUsers[player].HERBALISM = Math.Max(level, beforeUsers[player].HERBALISM);
				break;

			case "EXCAVATION":
				beforeUsers[player].EXCAVATION = Math.Max(level, beforeUsers[player].EXCAVATION);
				break;

			case "ARCHERY":
				beforeUsers[player].ARCHERY = Math.Max(level, beforeUsers[player].ARCHERY);
				break;

			case "SWORDS":
				beforeUsers[player].SWORDS = Math.Max(level, beforeUsers[player].SWORDS);
				break;

			case "AXES":
				beforeUsers[player].AXES = Math.Max(level, beforeUsers[player].AXES);
				break;

			case "ACROBATICS":
				beforeUsers[player].ACROBATICS = Math.Max(level, beforeUsers[player].ACROBATICS);
				break;

			case "TAMING":
				beforeUsers[player].TAMING = Math.Max(level, beforeUsers[player].TAMING);
				break;

			case "FISHING":
				beforeUsers[player].FISHING = Math.Max(level, beforeUsers[player].FISHING);
				break;

			case "ALCHEMY":
				beforeUsers[player].ALCHEMY = Math.Max(level, beforeUsers[player].ALCHEMY);
				break;

			default:
				throw new Exception();
				break;
		}
	}
}

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

static int DetermineNewValue(int newVal, int oldVal, string skillName, string playerName, List<string> changes)
{
	// Leave commented unless we're addressing MCMMO XP cap issues
	// if (skillName == "ACROBATICS" || skillName == "ARCHERY")
	// {
	// 	oldVal = oldVal/8;
	// }

	// If a player is deem≥ed to have earned too many levels based on thresholds
	if (newVal - oldVal > Constants.thresholds[skillName])
	{
		changes.Add($"{playerName}'s {skillName} skill should be reduced from {newVal} to {oldVal + Constants.thresholds[skillName]} \n");
		return oldVal + Constants.thresholds[skillName];
	}
	return newVal;
}

static void CrunchNumbers(Dictionary<string, UserLevels> beforeUsers, Dictionary<string, UserLevels> afterUsers, Dictionary<string, UserLevels> adjustedUsers, List<string> changes)
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
			MINING = DetermineNewValue(i.Value.MINING, j.MINING, "MINING", i.Key, changes),
			WOODCUTTING = DetermineNewValue(i.Value.WOODCUTTING, j.WOODCUTTING, "WOODCUTTING", i.Key, changes),
			REPAIR = DetermineNewValue(i.Value.REPAIR, j.REPAIR, "REPAIR", i.Key, changes),
			UNARMED = DetermineNewValue(i.Value.UNARMED, j.UNARMED, "UNARMED", i.Key, changes),
			HERBALISM = DetermineNewValue(i.Value.HERBALISM, j.HERBALISM, "HERBALISM", i.Key, changes),
			EXCAVATION = DetermineNewValue(i.Value.EXCAVATION, j.EXCAVATION, "EXCAVATION", i.Key, changes),
			ARCHERY = DetermineNewValue(i.Value.ARCHERY, j.ARCHERY, "ARCHERY", i.Key, changes),
			SWORDS = DetermineNewValue(i.Value.SWORDS, j.SWORDS, "SWORDS", i.Key, changes),
			AXES = DetermineNewValue(i.Value.AXES, j.AXES, "AXES", i.Key, changes),
			ACROBATICS = DetermineNewValue(i.Value.ACROBATICS, j.ACROBATICS, "ACROBATICS", i.Key, changes),
			TAMING = DetermineNewValue(i.Value.TAMING, j.TAMING, "TAMING", i.Key, changes),
		};

		adjustedUsers.Add(i.Key, adjustedPlayerLevels);
	}
}