# Neural Network Backpropagation - Letter Recognition

This application demonstrates a neural network implementation using the backpropagation algorithm for letter recognition. The application is built using .NET 8 and WPF.

## Features

- Neural network with configurable architecture (hidden layers, neurons per layer)
- Interactive drawing canvas for letter input
- Training with custom image datasets
- Real-time visualization of recognition results
- Asynchronous training with progress reporting

## Requirements

- .NET 8 SDK or higher
- Windows operating system

## How to Use

### Training the Network

1. Click the "Browse" button to select a folder containing letter images for training
   - Each image should be a bitmap (.bmp) file
   - Images should be named with the letter they represent (e.g., "A.bmp", "B.bmp", etc.)
   - Ideally, each image should be a 30x30 pixel bitmap with a white background and black letter

2. Configure the network architecture:
   - Set the number of hidden layers
   - Specify the number of neurons in each hidden layer (comma-separated values)
   - Adjust the maximum iterations and allowed error threshold as needed

3. Click "Train Network" to start the training process
   - The progress bar will show the training progress
   - The status message will update with the current training status
   - Training will stop when either the error threshold is reached or the maximum iterations are completed

### Recognizing Letters

1. Draw a letter in the drawing canvas using your mouse
2. Click "Recognize" to process the drawing through the trained network
3. View the recognition results, including:
   - The most likely letter and its confidence percentage
   - A list of all possible letters with their probabilities

4. Click "Reset" to clear the drawing canvas for a new letter

## Technical Implementation

This application implements a multilayer feed-forward neural network with:
- Input layer: 900 neurons (30x30 pixel image)
- Configurable hidden layers
- Output layer: 26 neurons (one for each letter A-Z)
- Sigmoid activation function
- Backpropagation learning algorithm

## Notes

- Training larger networks or with more complex datasets may take longer
- For best results, use consistent letter images with good contrast
- The application works best with clearly drawn letters that fill most of the drawing area
