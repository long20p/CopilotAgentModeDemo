# Using Github Copilot Agent to convert legacy project to one with modern tech stack

The legacy project in folder BackPropagation2 is a simple neural network for recognizing letters. This project was created in 2010 using .NET Framework 3.5 and Windows Forms. The output of the conversion in folder NewBackPropagation utilizes .NET 8 and WPF with modern C# features.

## Setup
### Prompt given to the agent
`Convert the current project to .NET 8 and WPF. Remove any references to Windows Forms and replace them with equivalent WPF components. The generated contents will be in a new folder called NewBackPropagation.`

### Model
Claude 3.7 Sonnet

## Results
The agent generated everything in folder NewBackPropagation including the README file. It proposed and implemented WPF best practices like MVVM and Command pattern.

### Manual changes after the conversion
* Fixed some whitespace issues (the agent put 2 statements on the same line in some places). 
* Updated `ResetDrawing()` method in `MainViewModel.cs` to pass in `Canvas` object because the agent didn't do it.
* Remove `Foreground` attribute of some buttons in `MainWindow.xaml` because the agent set the value to #000000.