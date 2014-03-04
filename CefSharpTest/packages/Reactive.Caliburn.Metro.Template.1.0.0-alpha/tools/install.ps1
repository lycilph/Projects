param($installPath, $toolsPath, $package, $project)

$file = $project.ProjectItems.Item("App.xaml")
$file.Properties.Item("BuildAction").Value = [int]2