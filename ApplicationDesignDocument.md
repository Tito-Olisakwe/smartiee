# Smartiee Application Design Document

## Overview
The Smartiee Quiz application is a web-based interactive platform built with Blazor WebAssembly. This document outlines the key components of the application's design including data structures, classes, and services.

## Data Structures

### Category
The `Category` class represents a quiz category. Each category has an ID and a name, serving as a way to organize questions into different topics or themes.

### Question
The `Question` class encapsulates all details of a quiz question. This includes the category ID, difficulty level, the question text, a list of possible answers, the index of the correct answer, and an optional explanation for the answer.

### TriviaData
The `TriviaData` class acts as a repository for all quiz data, containing lists of both `Category` and `Question` objects. It aggregates the data needed to run the quizzes.

## Classes and Services

### AppStateService
This service manages the application's state, especially the current category selected by the user. It provides mechanisms to update and broadcast state changes throughout the app.

### DataService
Responsible for fetching the quiz data from external sources like a JSON file. It utilizes `HttpClient` to asynchronously retrieve `TriviaData`.

### QuizService
Handles the logic of the quiz operation such as loading questions, scoring, and timing. It offers methods for processing user answers and maintaining the overall quiz state.

## Namespace Utilization
- `SmartieeWeb.Models`: Utilized by services and components to represent quiz data. Classes like `Category`, `Question`, and `TriviaData` fall under this namespace.
- `SmartieeWeb.Services`: Used by Razor components and the application's main logic to handle operations such as state management (`AppStateService`), data fetching (`DataService`), and quiz logic (`QuizService`).
- `SmartieeWeb.Pages`: The namespace for the user interface of the application. It contains Razor components like `Index`, `Categories`, `Quiz`, `Results`, and other pages that render the quiz's interactive elements.

## Design Rationale
Blazor WebAssembly is chosen for its ability to leverage .NET on the client side, promoting code reuse and consistency. The design follows a structured approach to manage quiz data, ensuring maintainability and scalability.

## Dependencies
The quiz questions and categories are sourced from a JSON file, `TriviaData.json`.

## Exception Handling Strategy
`try-catch` blocks are used to gracefully handle exceptions, which are logged to provide insight during development and to facilitate debugging. This strategy helps to prevent runtime crashes and contributes to a seamless user experience.
