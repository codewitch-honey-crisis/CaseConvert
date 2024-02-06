using CaseConvert;
var exps = new string[]
{
	"foobar",
	"foo_baz",
	"IPAddress",
	"WiFi",
	"ISO960",
	"fuBar"
};

for(int i = 0; i < exps.Length; i++)
{
	Console.WriteLine(CaseConverter.Convert(exps[i],CaseStyle.PascalCase));
	Console.WriteLine(CaseConverter.Convert(exps[i], CaseStyle.CamelCase));
	Console.WriteLine(CaseConverter.Convert(exps[i], CaseStyle.SnakeCase));
	Console.WriteLine(CaseConverter.Convert(exps[i], CaseStyle.DashCase));
	Console.WriteLine(CaseConverter.Convert(exps[i], CaseStyle.MacroCase));
	Console.WriteLine();
}