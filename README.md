## For adjusting MCMMO levels

## How to use:
1. Provide the two log files bookending the exploit time. This solution assumes the exact formatting that fruit provided (with two logs included), though the ParseFile method in Program.cs can be easily modified.
2. Update Constants.threshholds in Constants.cs. The way to think about these values is "what is the maximum amount of levels in each stat players should be allowed to retain
3. Put log files in /Logs
4. Run the project

## How to read the results
The project outputs a .csv file of what the update numbers should be.
The project also writes all specific changes to the console.
