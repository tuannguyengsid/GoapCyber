param($installPath, $toolsPath, $package, $project)

$appStartTemplatesFolder = $project.ProjectItems.Item("App_Start");

# Remove the App_Start file for other languages
if ($project.Type -eq "C#") {
	$appStartTemplatesFolder.ProjectItems.Item("AttributeRouting.vb").Delete();
} elseif ($project.Type -eq "VB.NET")  {
	$appStartTemplatesFolder.ProjectItems.Item("AttributeRouting.cs").Delete();
}