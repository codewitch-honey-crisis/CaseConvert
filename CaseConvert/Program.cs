using CaseConvert;
var exps = new string[]
{
	"802_11b",
	"foobar",
	"foo_baz",
	"IPAddress",
	"SQL92",
	"WiFi",
	"ISO8601",
	"fuBar",
	"C89",
	"sql_92",
	"car_v1",
	"_whiteSpace"
};

for(int i = 0; i < exps.Length; i++)
{
	var sa = CaseConverter.SplitCase(exps[i]);
	Console.WriteLine("segments: "+string.Join(", ",sa)); 
	Console.WriteLine(CaseConverter.Convert(exps[i], CaseStyle.PascalCase));
	Console.WriteLine(CaseConverter.Convert(exps[i], CaseStyle.CamelCase));
	Console.WriteLine(CaseConverter.Convert(exps[i], CaseStyle.SnakeCase));
	Console.WriteLine(CaseConverter.Convert(exps[i], CaseStyle.DashCase));
	Console.WriteLine(CaseConverter.Convert(exps[i], CaseStyle.MacroCase));
	Console.WriteLine();
}