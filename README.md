# The Hangman Game
 
A classic Hangman and Word Guesser game built with C# and the .NET Framework. This desktop application challenges players to guess missing letters in a hidden word before they run out of lives and the hangman meets his fate.
 
This project was developed to practice object-oriented programming, Windows Forms UI design, and file handling in C#.
 
##  Features
 
* **Multiple Difficulty Levels:** Choose between Easy, Medium, and Hard modes. The difficulty dynamically determines the length and complexity of the words.
* **External Word Banks:** Words are loaded at runtime from dedicated text files (`EasyWords.txt`, `MediumWords.txt`, `HardWords.txt`), separating data from the core application logic.
* **Dynamic UI & State Management:** The interface automatically updates to reflect correct guesses in the word blanks, deducts lives for incorrect guesses, and updates the hangman visual state.
* **Seamless Navigation:** Smooth transitions between the Main Menu and the active Game Screen using intelligent panel and form management.
 
### Prerequisites
 
To run or edit this project, you will need:
 
* Microsoft Visual Studio (or Visual Studio Preview) installed on your machine.
* The .NET Framework runtime installed.
### Installation & Execution
 
1. Clone the repository to your local machine:
```bash
git clone https://github.com/OMARXEdition/The-Hangman-Game.git
```
 
2. Navigate to the downloaded folder and open the `Hangman Project.sln` file in Visual Studio.
3. In the Solution Explorer, ensure `Program.cs` is set as the startup object.
4. Press F5 or click the Start button at the top of Visual Studio to compile and launch the game.
## 📁 Project Structure
 
* `Form1.cs`: Contains the core logic for the user interface, button click events, and gameplay loops.
* `Program.cs`: The main entry point for the application.
* `EasyWords.txt` / `MediumWords.txt` / `HardWords.txt`: Plain text files containing the word dictionaries for each specific difficulty.
* `Resources/`: Contains the visual assets, including the sequential hangman state images.
