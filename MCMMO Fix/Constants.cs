public class Constants
{
	// How much of a gain in each stat since the exploit is deemed unacceptable
	public static readonly Dictionary<string, int> thresholds = new()
	{
		{ "MINING", 500 },
		{ "WOODCUTTING", 300 },
		{ "REPAIR", 300 },
		{ "UNARMED", 300 },
		{ "HERBALISM", 300 },
		{ "EXCAVATION", 300 },
		{ "ARCHERY", 300 },
		{ "SWORDS", 300 },
		{ "AXES", 300 },
		{ "ACROBATICS", 300 },
		{ "TAMING", 300 }
	};
}
