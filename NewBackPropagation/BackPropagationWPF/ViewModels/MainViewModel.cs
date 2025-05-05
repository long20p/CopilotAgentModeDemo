using BackPropagationWPF.Extensions;
using BackPropagationWPF.Models;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace BackPropagationWPF.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private Network? network;
        private string statusMessage = "Ready";
        private string resultMessage = "";
        private double progress = 0;
        private bool isTraining = false;
        private bool isNetworkTrained = false;
        private string trainingFolderPath = "";
        private int hiddenLayerCount = 1;
        private string hiddenLayerElements = "15";
        private int maxIterations = 10000;
        private double allowedError = 0.01;
        private ImageSource? smallImage;

        // Collection of letters and their probabilities for the result display
        private ObservableCollection<LetterProbability> resultLetters = new ObservableCollection<LetterProbability>();

        // Training set
        private List<Pattern> trainingSet = new List<Pattern>();

        #region Properties

        public string StatusMessage
        {
            get => statusMessage;
            set
            {
                statusMessage = value;
                OnPropertyChanged();
            }
        }

        public string ResultMessage
        {
            get => resultMessage;
            set
            {
                resultMessage = value;
                OnPropertyChanged();
            }
        }

        public double Progress
        {
            get => progress;
            set
            {
                progress = value;
                OnPropertyChanged();
            }
        }

        public bool IsTraining
        {
            get => isTraining;
            set
            {
                isTraining = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(CanTrain));
                OnPropertyChanged(nameof(CanRecognize));
            }
        }

        public bool IsNetworkTrained
        {
            get => isNetworkTrained;
            set
            {
                isNetworkTrained = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(CanRecognize));
            }
        }

        public string TrainingFolderPath
        {
            get => trainingFolderPath;
            set
            {
                trainingFolderPath = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(CanTrain));
            }
        }

        public int HiddenLayerCount
        {
            get => hiddenLayerCount;
            set
            {
                hiddenLayerCount = value;
                OnPropertyChanged();
            }
        }

        public string HiddenLayerElements
        {
            get => hiddenLayerElements;
            set
            {
                hiddenLayerElements = value;
                OnPropertyChanged();
            }
        }

        public int MaxIterations
        {
            get => maxIterations;
            set
            {
                maxIterations = value;
                OnPropertyChanged();
            }
        }

        public double AllowedError
        {
            get => allowedError;
            set
            {
                allowedError = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<LetterProbability> ResultLetters
        {
            get => resultLetters;
            set
            {
                resultLetters = value;
                OnPropertyChanged();
            }
        }

        public ImageSource? SmallImage
        {
            get => smallImage;
            set
            {
                smallImage = value;
                OnPropertyChanged();
            }
        }

        public bool CanTrain => !string.IsNullOrEmpty(TrainingFolderPath) && !IsTraining;
        public bool CanRecognize => IsNetworkTrained && !IsTraining;

        #endregion

        #region Commands        
        private ICommand? browseCommand;
        public ICommand BrowseCommand => browseCommand ??= new RelayCommand(_ => BrowseForTrainingFolder());

        private ICommand? trainCommand;
        public ICommand TrainCommand => trainCommand ??= new RelayCommand(_ => Train(), _ => CanTrain);

        private ICommand? recognizeCommand;
        public ICommand RecognizeCommand => recognizeCommand ??= new RelayCommand(Recognize, _ => CanRecognize);

        private ICommand? resetCommand;
        public ICommand ResetCommand => resetCommand ??= new RelayCommand(Reset);

        #endregion

        /// <summary>
        /// Browse for a folder containing training images
        /// </summary>
        private void BrowseForTrainingFolder()
        {
            var dialog = new OpenFolderDialog
            {
                Title = "Select Training Images Folder",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
            };

            if (dialog.ShowDialog() == true)
            {
                TrainingFolderPath = dialog.FolderName;
                StatusMessage = $"Selected training folder: {TrainingFolderPath}";
            }
        }

        /// <summary>
        /// Train the neural network
        /// </summary>
        private async void Train()
        {
            try
            {                
                // Parse hidden layer configuration
                int[]? layerElements = ParseHiddenLayerElements();
                if (layerElements == null || layerElements.Length != HiddenLayerCount)
                {
                    MessageBox.Show(
                        $"Invalid hidden layer configuration. Please provide {HiddenLayerCount} comma-separated values.",
                        "Configuration Error",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                    return;
                }

                IsTraining = true;
                StatusMessage = "Generating training set...";
                Progress = 0;

                // Generate training set
                GenerateTrainingSet();
                if (trainingSet.Count == 0)
                {
                    MessageBox.Show(
                        "No training patterns found. Please check the training folder.",
                        "Training Error",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                    IsTraining = false;
                    return;
                }

                StatusMessage = "Initializing neural network...";

                // Create and initialize the network
                BackPropagationConfig.InputCount = trainingSet[0].InputsX.Length;
                BackPropagationConfig.OutputCount = trainingSet[0].ExpectedOutput.Length;

                network = new Network(
                    HiddenLayerCount,
                    layerElements,
                    BackPropagationConfig.OutputCount,
                    BackPropagationConfig.InputCount);

                network.AllowedError = AllowedError;
                // Subscribe to training progress updates
                network.OnTrainingProgress += (message, error, round) =>
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        StatusMessage = message;
                        Progress = Math.Min(1.0, (double)round / MaxIterations);
                    });
                };

                // Train the network
                await network.TrainAsync(trainingSet, MaxIterations);

                IsNetworkTrained = true;
                //StatusMessage = "Training completed.";
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error during training: {ex.Message}",
                    "Training Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                StatusMessage = "Training failed.";
            }
            finally
            {
                IsTraining = false;
                Progress = 1.0;
            }
        }

        /// <summary>
        /// Recognize a drawing
        /// </summary>
        private void Recognize(object? parameter)
        {
            try
            {
                if (network == null || !IsNetworkTrained)
                {
                    MessageBox.Show(
                        "Network is not trained yet.",
                        "Recognition Error",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);
                    return;
                }

                // Get drawing canvas from parameter
                if (parameter is not Controls.DrawingCanvas canvas)
                {
                    return;
                }

                // Clear previous results
                ResultMessage = "Processing...";
                SmallImage = null;
                ResultLetters.Clear();

                // Convert drawing to input array
                ImageSource reducedImage;
                double[] inputs = canvas.ConvertToInputArray(out reducedImage);
                // Subscribe to recognition results
                network.OnRecognitionResult += (result, probabilities) =>
                {
                    System.Windows.Application.Current.Dispatcher.Invoke(() =>
                    {
                        ResultMessage = result;
                        SmallImage = reducedImage;
                        ResultLetters.Clear();

                        // Add all letter probabilities to the collection
                        foreach (var pair in probabilities)
                        {
                            ResultLetters.Add(new LetterProbability
                            {
                                Letter = pair.Key,
                                Probability = pair.Value
                            });
                        }
                    });
                };

                // Recognize the pattern
                network.Recognize(inputs, BackPropagationConfig.Letters);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error during recognition: {ex.Message}",
                    "Recognition Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                ResultMessage = "Recognition failed.";
            }
        }

        /// <summary>
        /// Reset the drawing canvas
        /// </summary>
        private void Reset(object? parameter)
        {
            // This will be called from the UI, which is responsible for resetting the canvas
            if (parameter is not Controls.DrawingCanvas canvas)
            {
                return;
            }

            canvas.ResetCanvas();
            SmallImage = null;
            ResultMessage = "";
            ResultLetters.Clear();
        }
        /// <summary>
        /// Parse hidden layer elements from the string input
        /// </summary>
        private int[]? ParseHiddenLayerElements()
        {
            try
            {
                string[] parts = HiddenLayerElements.Split(',');
                int[] result = new int[parts.Length];

                for (int i = 0; i < parts.Length; i++)
                {
                    result[i] = int.Parse(parts[i].Trim());

                    if (result[i] <= 0)
                    {
                        MessageBox.Show(
                            "Hidden layer elements must be positive numbers.",
                            "Configuration Error",
                            MessageBoxButton.OK,
                            MessageBoxImage.Error);
                        return null;
                    }
                }

                return result;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Generate training patterns from images in the training folder
        /// </summary>
        private void GenerateTrainingSet()
        {
            trainingSet.Clear();

            if (string.IsNullOrEmpty(TrainingFolderPath) || !Directory.Exists(TrainingFolderPath))
            {
                return;
            }

            string[] imageFiles = Directory.GetFiles(TrainingFolderPath, "*.bmp");

            for (int i = 0; i < imageFiles.Length && i < BackPropagationConfig.Letters.Length; i++)
            {
                // Load the image
                BitmapImage bmpImage = new BitmapImage();
                bmpImage.BeginInit();
                bmpImage.UriSource = new Uri(imageFiles[i]);
                bmpImage.EndInit();

                // Convert to FormatConvertedBitmap to ensure consistent processing
                FormatConvertedBitmap formatConvertedBitmap = new FormatConvertedBitmap();
                formatConvertedBitmap.BeginInit();
                formatConvertedBitmap.Source = bmpImage;
                formatConvertedBitmap.DestinationFormat = System.Windows.Media.PixelFormats.Bgra32;
                formatConvertedBitmap.EndInit();

                // Convert to WriteableBitmap for pixel access
                int width = formatConvertedBitmap.PixelWidth;
                int height = formatConvertedBitmap.PixelHeight;
                WriteableBitmap writeableBitmap = new WriteableBitmap(formatConvertedBitmap);

                // Process the image to inputs
                double[] inputs = new double[width * height];

                writeableBitmap.Lock();

                try
                {
                    unsafe
                    {
                        for (int y = 0; y < height; y++)
                        {
                            for (int x = 0; x < width; x++)
                            {
                                var color = writeableBitmap.GetPixel(x, y);
                                double value = 1.0 - ((color.R + color.G + color.B) / (3.0 * 255.0));
                                inputs[y * width + x] = value > 0 ? 1.0 : 0.0;
                            }
                        }
                    }
                }
                finally
                {
                    writeableBitmap.Unlock();
                }

                // Create expected outputs (one-hot encoding)
                double[] outputs = new double[BackPropagationConfig.Letters.Length];
                outputs[i] = 1.0;

                // Add the pattern to the training set
                Pattern pattern = new Pattern(inputs, outputs);
                trainingSet.Add(pattern);
            }
        }
        #region INotifyPropertyChanged Implementation

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }    /// <summary>
         /// Represents a letter with its recognition probability
         /// </summary>
    public class LetterProbability : INotifyPropertyChanged
    {
        private string letter = string.Empty;
        private double probability;

        public string Letter
        {
            get => letter;
            set
            {
                letter = value;
                OnPropertyChanged();
            }
        }

        public double Probability
        {
            get => probability;
            set
            {
                probability = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }    /// <summary>
         /// Simple relay command implementation
         /// </summary>
    public class RelayCommand : ICommand
    {
        private readonly Action<object?> execute;
        private readonly Predicate<object?>? canExecute;

        public RelayCommand(Action<object?> execute, Predicate<object?>? canExecute = null)
        {
            this.execute = execute ?? throw new ArgumentNullException(nameof(execute));
            this.canExecute = canExecute;
        }

        public bool CanExecute(object? parameter)
        {
            return canExecute == null || canExecute(parameter);
        }

        public void Execute(object? parameter)
        {
            execute(parameter);
        }

        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}
