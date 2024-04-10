# Smartiee Quiz Application

Welcome to the Smartiee Quiz application repository. This document serves as a guide for setting up the application locally and navigating the web-based quiz platform.

## Getting Started

To run the Smartiee Quiz application, you will need to clone this repository to your local machine. Ensure you have [.NET](https://dotnet.microsoft.com/download) installed to build and run the projects.

### Cloning the Repository

Open your terminal or command prompt and run the following command:

```bash
git clone https://github.com/Tito-Olisakwe/smartiee.git
```

## Projects in the Repository

There are three main projects in this repository:

- Smartiee: A console-based version of the quiz application.
- SmartieeWeb: The web version of the quiz application and the primary focus of this README.
- SmartieeWeb.Tests: Contains unit tests for the application.

## Running the Web Version (SmartieeWeb)

Navigate to the SmartieeWeb directory from your terminal:

```bash
cd SmartieeWeb
```

Build the project:

```bash
dotnet build
```

Run the application:

```bash
dotnet run
```

After running the application, you can access the Smartiee Quiz Web application in your web browser.

## Using the Smartiee Quiz Web Application

Once the application is running, follow these steps to take a quiz:

### Accessing the Quiz:

Open your web browser and navigate to `http://localhost:5000` (or the URL provided in your terminal).

### Starting a Quiz:

The homepage will display a button which leads to a list of available quiz categories. The sidebar also has the list of categories that you can navigate to anytime. Select a category, difficulty level, and if you want to be timed, to begin the quiz.

### Taking the Quiz:

You will be presented with one question at a time. Select an answer from the given options and confirm it to proceed.

### Reviewing Results:

After submitting an answer for the last question or if your time runs out, your results will be displayed.

### Retaking or Exiting:

You have the option to retake the quiz with the same settings or exit and choose a new category.


## Demo

You can watch a video of how the application works here: [Smartiee Quiz Application Demo](https://www.youtube.com)

