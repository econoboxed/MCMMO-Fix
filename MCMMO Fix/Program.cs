using System;
using System.IO;
using System.Reflection.Metadata;

Dictionary<string, UserLevels> beforeUsers = new();
Dictionary<string, UserLevels> afterUsers = new();

Dictionary<string, UserLevels> adjustedUsers = new();

string beforeFilepath = "before.users";
string afterFilepath = "after.users";

ParseFileToObject(beforeUsers, beforeFilepath);
ParseFileToObject(afterUsers, afterFilepath);

foreach (var i in afterUsers)
{
	var j = new UserLevels();
	if (beforeUsers.ContainsKey(i.Key)){
		j = beforeUsers[i.Key];
	}

	UserLevels adjustedPlayerLevels = new()
	{
		MINING = DetermineNewValue(i.Value.MINING, j.MINING, "MINING"),
		WOODCUTTING = DetermineNewValue(i.Value.WOODCUTTING, j.WOODCUTTING, "WOODCUTTING"),
		REPAIR = DetermineNewValue(i.Value.REPAIR, j.REPAIR, "REPAIR"),
		UNARMED = DetermineNewValue(i.Value.UNARMED, j.UNARMED, "UNARMED"),
		HERBALISM = DetermineNewValue(i.Value.HERBALISM, j.HERBALISM, "HERBALISM"),
		EXCAVATION = DetermineNewValue(i.Value.EXCAVATION, j.EXCAVATION, "EXCAVATION"),
		ARCHERY = DetermineNewValue(i.Value.ARCHERY, j.ARCHERY, "ARCHERY"),
		SWORDS = DetermineNewValue(i.Value.SWORDS, j.SWORDS, "SWORDS"),
		AXES = DetermineNewValue(i.Value.MINING, j.MINING, "MINING"),
		ACROBATICS = DetermineNewValue(i.Value.AXES, j.AXES, "AXES"),
		TAMING = DetermineNewValue(i.Value.TAMING, j.TAMING, "TAMING"),
	};

	adjustedUsers.Add(i.Key, adjustedPlayerLevels);
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

static int DetermineNewValue(int newVal, int oldVal, string skillName)
{
	return oldVal - newVal > Constants.thresholds[skillName] ? oldVal + Constants.thresholds[skillName] : newVal;
}