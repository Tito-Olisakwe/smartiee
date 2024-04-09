# Test Case Documentation

This document provides detailed descriptions of test cases executed for the SmartieeWeb application, including their outcomes and notes on their execution. All tests have been performed successfully, validating the functionality of the application as per the requirements.

## MainLayoutTests

### TC001: Component Initializes With Default Category Name

- **Description:** Ensures that the MainLayout component correctly displays the default category name as provided by the AppStateService upon initialization.
- **Pre-conditions:** The application is loaded, and no user actions have been taken.
- **Test Steps:**
  1. Instantiate the MainLayout component within the test context.
  2. Inject a mock AppStateService that returns "Default Category" as the current category name.
- **Expected Outcome:** The component's markup should include the text "Default Category".
- **Actual Outcome:** The component's markup correctly included "Default Category".
- **Status:** Passed
- **Notes:** The component successfully initializes with the default category name, indicating proper state management and component rendering.

### TC002: Component Updates Category Name On AppStateService Change

- **Description:** Checks if the MainLayout component updates its displayed category name in response to changes in the AppStateService.
- **Pre-conditions:** The MainLayout component is already initialized with a default category name.
- **Test Steps:**
  1. Change the AppStateService's current category name to "New Category".
  2. Trigger the AppStateService's OnChange event.
- **Expected Outcome:** The component's markup should update to include the text "New Category".
- **Actual Outcome:** The component's markup updated to reflect "New Category" as expected.
- **Status:** Passed
- **Notes:** This test verifies that the component is reactive to AppStateService changes, ensuring dynamic content updates.

## NavMenuTests

### TC003: Component Displays Categories After Initialization

- **Description:** Verify that the NavMenu component displays categories fetched from the DataService upon initialization.
- **Pre-conditions:** DataService returns at least one category.
- **Test Steps:**
  1. Render the NavMenu component.
- **Expected Outcome:** "Science" is found in the component's markup.
- **Actual Outcome:** "Science" category was successfully displayed in the component's markup.
- **Status:** Passed
- **Notes:** The NavMenu component correctly fetches and displays category data, validating the integration with the DataService.

### TC004: Component Shows Error When Categories Unavailable

- **Description:** Ensure NavMenu displays an error when no categories data is available.
- **Pre-conditions:** DataService returns null for categories data.
- **Test Steps:**
  1. Render the NavMenu component with DataService setup to return null.
- **Expected Outcome:** "Categories Unavailable" is displayed in the component's markup.
- **Actual Outcome:** The message "Categories Unavailable" was correctly displayed.
- **Status:** Passed
- **Notes:** Error handling within the NavMenu component functions as expected, providing user feedback when data is unavailable.

### TC005: Toggle NavMenu Changes Visibility

- **Description:** Verify navbar toggler changes the visibility of the navigation menu.
- **Pre-conditions:** NavMenu is initially in a collapsed state.
- **Test Steps:**
  1. Render the NavMenu component.
  2. Click the navbar toggler button.
- **Expected Outcome:** The navigation menu visibility changes as indicated by the presence or absence of the "collapse" class.
- **Actual Outcome:** The navigation menu's visibility changed as expected upon clicking the toggler.
- **Status:** Passed
- **Notes:** This test confirms the NavMenu's responsiveness and interactive elements function correctly.

### TC006: Navigate To Category Calls Correct Service Method

- **Description:** Confirm clicking a category button navigates to the correct category page.
- **Pre-conditions:** At least one category is displayed in the NavMenu.
- **Test Steps:**
  1. Click on a category button.
- **Expected Outcome:** The application navigates to the difficulty selection page for the chosen category.
- **Actual Outcome:** The application navigated to the difficulty selection page as expected.
- **Status:** Passed
- **Notes:** The test verifies navigation functionality, ensuring users can move from selecting a category to taking a quiz.

## CategoriesTests

### TC007: Categories Page Loads and Displays Categories

- **Description:** Verify the Categories page loads and correctly displays all available categories from the DataService.
- **Pre-conditions:** DataService returns a list of categories including "Science" and "Math".
- **Test Steps:**
  1. Render the Categories component.
- **Expected Outcome:** Both "Science" and "Math" are displayed, with buttons for each category.
- **Actual Outcome:** The page successfully displayed "Science" and "Math" with a button for each category and one additional button possibly for navigation or selection.
- **Status:** Passed
- **Notes:** This confirms the Categories page can fetch and display category data as expected.

### TC008: Categories Page Navigates to Category When Selected

- **Description:** Ensure selecting a category on the Categories page navigates to the correct difficulty selection page for that category.
- **Pre-conditions:** The DataService returns a list with one category, "Science".
- **Test Steps:**
  1. Render the Categories component.
  2. Click the "Science" category button.
- **Expected Outcome:** The application navigates to "/difficulty/1", indicating the selection of the "Science" category.
- **Actual Outcome:** Navigation to "/difficulty/1" was successful, indicating the correct category page was loaded.
- **Status:** Passed
- **Notes:** This test validates that the category selection leads to the appropriate navigation within the application, with the AppStateService updating accordingly.

## ConfirmModalTests

### TC009: Confirm Button Closes Modal and Invokes OnConfirm

- **Description:** Verify clicking the confirm button closes the modal and invokes the OnConfirm callback with the correct option index.
- **Pre-conditions:** The ConfirmModal component is rendered with an OnConfirm callback.
- **Test Steps:**
  1. Show the modal with a specified option index.
  2. Click the confirm button.
- **Expected Outcome:** The modal closes, and the OnConfirm callback is invoked with the specified index.
- **Actual Outcome:** The modal closed as expected, and the OnConfirm callback was invoked with the index 5.
- **Status:** Passed
- **Notes:** This test validates the confirm functionality of the modal, ensuring it behaves correctly when the user confirms.

### TC010: Cancel Button Closes Modal and Invokes OnCancel

- **Description:** Ensure clicking the cancel button closes the modal and invokes the OnCancel callback.
- **Pre-conditions:** The ConfirmModal component is rendered with an OnCancel callback.
- **Test Steps:**
  1. Show the modal.
  2. Click the cancel button.
- **Expected Outcome:** The modal is no longer visible, and the OnCancel callback is invoked.
- **Actual Outcome:** The modal closed as expected, and the OnCancel callback was successfully invoked.
- **Status:** Passed
- **Notes:** This test checks the cancel functionality, ensuring the modal closes and triggers the appropriate callback when canceled.

### TC011: Show Method Makes Modal Visible with Correct Message

- **Description:** Confirm the Show method correctly makes the modal visible and displays the passed message.
- **Pre-conditions:** ConfirmModal component is initialized but not yet shown.
- **Test Steps:**
  1. Invoke the Show method with a test message and an option index.
- **Expected Outcome:** The modal becomes visible with "display:block;" and displays the test message.
- **Actual Outcome:** The modal was correctly displayed with "display:block;" and contained the "Test message".
- **Status:** Passed
- **Notes:** This test verifies the Show method functions correctly, displaying the modal with the intended message.

## DifficultyTests

### TC012: Difficulty Selection Component Renders Correctly

- **Description:** Verify the Difficulty selection component renders correctly with all difficulty level buttons.
- **Pre-conditions:** None.
- **Test Steps:**
  1. Render the Difficulty component.
- **Expected Outcome:** Three buttons labeled "Easy", "Medium", and "Hard" are rendered.
- **Actual Outcome:** The component rendered three buttons as expected, each labeled with the corresponding difficulty level.
- **Status:** Passed
- **Notes:** This test confirms the component correctly displays the options for quiz difficulty.

### TC013: Difficulty Buttons Navigate Correctly

- **Description:** Ensure that clicking on any difficulty level button correctly navigates to the respective quiz difficulty URL.
- **Pre-conditions:** The Difficulty component is rendered with a predefined category ID.
- **Test Steps for Each Difficulty Level:**
  1. Click on the "Easy" difficulty button.
  2. Click on the "Medium" difficulty button.
  3. Click on the "Hard" difficulty button.
- **Expected Outcome:** Clicking a difficulty button navigates to "/timed/1/{difficulty}", where {difficulty} matches the button clicked.
- **Actual Outcome:** Each button navigated correctly to its respective URL, confirming the navigation logic works as expected.
- **Status:** Passed
- **Notes:** This test ensures users can select a quiz difficulty and be navigated to the appropriate page.

## IndexTests

### TC014: Continue Button Navigates to Categories

- **Description:** Verify that clicking the continue button on the Index page correctly navigates the user to the Categories page.
- **Pre-conditions:** The Index page is rendered, displaying a continue button to the user.
- **Test Steps:**
  1. Click the continue button on the Index page.
- **Expected Outcome:** The application navigates to the "/categories" URL, showing the Categories page to the user.
- **Actual Outcome:** The application successfully navigated to "/categories" upon button click, as expected.
- **Status:** Passed
- **Notes:** This test confirms the Index page's primary navigation functionality is working as intended, facilitating smooth user flow within the application.

## QuizTests

### TC015: Quiz Component Shows Loading On Initial Load

- **Description:** Verify the Quiz component displays a loading message upon initial load while questions are fetched.
- **Pre-conditions:** None.
- **Test Steps:**
  1. Render the Quiz component with initial parameters.
- **Expected Outcome:** "Loading questions..." appears in the component's markup during question fetching.
- **Actual Outcome:** The loading message was displayed as expected.
- **Status:** Passed
- **Notes:** Confirms asynchronous question fetching doesn't leave the user without feedback.

### TC016: Shows Loading Message While Questions Are Being Fetched

- **Description:** Ensure the Quiz component displays a loading message while fetching questions asynchronously.
- **Pre-conditions:** Mock QuizService to delay returning questions.
- **Test Steps:**
  1. Render the Quiz component under test conditions.
- **Expected Outcome:** Component markup shows "Loading questions..." during the fetch process.
- **Actual Outcome:** Loading message displayed correctly while fetching.
- **Status:** Passed
- **Notes:** Verifies user feedback during data fetching, improving user experience.

### TC017: Displays Times Up Message When Time Expires

- **Description:** Verify the "Time's up! Your final score:" message displays when quiz time expires.
- **Pre-conditions:** Quiz is initialized and started with a timed setting.
- **Test Steps:**
  1. Simulate time expiration in the Quiz component.
- **Expected Outcome:** "Time's up! Your final score:" message appears in the component's markup.
- **Actual Outcome:** The message displayed correctly upon time expiration.
- **Status:** Passed
- **Notes:** Ensures users are informed when the quiz time has expired, along with their final score.

### TC018: Navigates To Results When Time Expires

- **Description:** Confirm Quiz component navigation to results page upon time expiration.
- **Pre-conditions:** Quiz component is initialized in a timed mode.
- **Test Steps:**
  1. Simulate time expiration in the Quiz component.
- **Expected Outcome:** Application navigates to "/results/5", showing the results page with the final score.
- **Actual Outcome:** Navigation to the results page was successful, with the correct final score URL.
- **Status:** Passed
- **Notes:** Validates correct application flow from quiz completion to showing results based on timing.

### TC019: Correct Answer Selection Updates Score and Provides Feedback

- **Description:** Verify selecting the correct answer updates the score and provides positive feedback.
- **Pre-conditions:** A question is displayed to the user with multiple-choice answers.
- **Test Steps:**
  1. Select the correct answer for the question.
  2. Confirm the answer selection.
- **Expected Outcome:** The "Correct!" message is displayed, and the score is updated accordingly.
- **Actual Outcome:** The correct answer was acknowledged with a "Correct!" message, and the score was updated.
- **Status:** Passed
- **Notes:** Validates that correct answers are recognized and positively reinforced.

### TC020: Incorrect Answer Selection Does Not Update Score But Provides Correct Feedback

- **Description:** Ensure selecting an incorrect answer doesn't update the score but provides feedback on the correct answer.
- **Pre-conditions:** A question with one correct and several incorrect answers is presented.
- **Test Steps:**
  1. Select an incorrect answer.
  2. Confirm the answer selection.
- **Expected Outcome:** An "Incorrect!" message displays along with the correct answer.
- **Actual Outcome:** The application correctly indicated the incorrect selection and provided the correct answer.
- **Status:** Passed
- **Notes:** Important for educational feedback, showing the correct answer aids in learning.

### TC021: Next Question Button Loads Next Question or Navigates to Results

- **Description:** Confirm the next question button loads the next question or navigates to the results page if there are no further questions.
- **Pre-conditions:** At least one question has been answered.
- **Test Steps:**
  1. Click the "Next Question" button after answering a question.
- **Expected Outcome:** The application either presents the next question or navigates to the results page.
- **Actual Outcome:** Navigation functioned as expected, presenting another question or showing results based on quiz progress.
- **Status:** Passed
- **Notes:** Ensures smooth quiz progression or conclusion.

### TC022: Play Again Button Restarts Quiz With Same Settings

- **Description:** Verify the play again button restarts the quiz with the initial category, difficulty, and timing settings.
- **Pre-conditions:** The quiz has concluded.
- **Test Steps:**
  1. Click the "Play Again" button after completing the quiz.
- **Expected Outcome:** The quiz restarts with the same settings as the initial quiz.
- **Actual Outcome:** The quiz was successfully restarted with the initial settings retained.
- **Status:** Passed
- **Notes:** Allows users to retake the quiz under the same conditions for improvement or review.

## ResultsTests

### TC023: Results Page Initializes Correctly

- **Description:** Verify the Results page correctly displays the user's score and percentage based on the passed score string.
- **Pre-conditions:** None.
- **Test Steps:**
  1. Render the Results component with a "ScoreAsString" parameter value of "7".
- **Expected Outcome:** Component markup shows "Your score: 7/10" and "That's 70%".
- **Actual Outcome:** The page accurately displayed the score and percentage as "Your score: 7/10" and "That's 70%".
- **Status:** Passed
- **Notes:** Validates that the Results page properly calculates and displays results based on the provided score.

### TC024: Play Again Button Navigates Correctly

- **Description:** Ensure the "Play Again" button navigates the user back to the quiz with the previously used settings.
- **Pre-conditions:** The Results component is rendered, and last quiz settings are stored.
- **Test Steps:**
  1. Click the "Play Again" button on the Results page.
- **Expected Outcome:** The application navigates to "/quiz/1/Easy/True", maintaining the last quiz settings.
- **Actual Outcome:** Navigation to "/quiz/1/Easy/True" was successful, indicating correct preservation of quiz settings.
- **Status:** Passed
- **Notes:** Confirms users can retake the quiz with the same settings seamlessly, enhancing user experience.

### TC025: Return To MainMenu Button Navigates To Root

- **Description:** Confirm the "Return to Main Menu" button correctly navigates the user to the application's root page.
- **Pre-conditions:** The Results component is rendered.
- **Test Steps:**
  1. Click the "Return to Main Menu" button on the Results page.
- **Expected Outcome:** The application navigates to the root ("/").
- **Actual Outcome:** Navigation to the root ("/") was executed as expected.
- **Status:** Passed
- **Notes:** Ensures users can easily navigate back to the main menu from the Results page, facilitating navigation flow.

## TimedQuizSelectionTests

### TC026: Quiz Timing Buttons Navigate Correctly

- **Description:** Verify that the quiz timing selection buttons (for timed or untimed quiz options) navigate correctly to the quiz page with the chosen timing setting.
- **Pre-conditions:** The `TimedQuizSelection` component is rendered with a specified category ID and difficulty level.
- **Test Steps:**
  1. Click the button for selecting a timed quiz.
  2. Click the button for selecting an untimed quiz.
- **Expected Outcome for Timed Quiz:** The application navigates to "/quiz/1/Easy/true", indicating the quiz will be timed.
- **Expected Outcome for Untimed Quiz:** The application navigates to "/quiz/1/Easy/false", indicating the quiz will be untimed.
- **Actual Outcome:** For both selections, the application navigated to the correct URLs ("/quiz/1/Easy/true" for timed and "/quiz/1/Easy/false" for untimed), matching the expected outcomes.
- **Status:** Passed
- **Notes:** This test ensures that the user's choice regarding quiz timing is respected, and the application navigates to the correct page reflecting this choice.
