public class Constants
{
	// How much of a gain in each stat since the exploit is deemed unacceptable
	public static readonly Dictionary<string, int> thresholds = new()
	{
		{ "MINING", 250 },
		{ "WOODCUTTING", 150 },
		{ "REPAIR", 150 },
		{ "UNARMED", 100 },
		{ "HERBALISM", 150 },
		{ "EXCAVATION", 150 },
		{ "ARCHERY", 150 },
		{ "SWORDS", 150 },
		{ "AXES", 150 },
		{ "ACROBATICS", 150 },
		{ "TAMING", 150 }
	};
}
