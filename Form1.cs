using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Media;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hangman_Project
{
    public partial class Form1 : Form
    {
        private GameSession CurrentSession = new GameSession();
        public Form1()
        {
            InitializeComponent();

            pbFaceReaction.Parent = pbTheBlade;

            pbFaceReaction.BackColor = Color.Transparent;
        }

        public class GameSession
        {
            public List<string> usedWords { get; set; } = new List<string>();
            public string TargetWord { get; set; } = "";
            public string filePath { get; set; } = "";
            public int WrongAnswers { get; set; } = 0;
            public int GameRounds { get; set; } = 0;
            public int CurrentRound { get; set; } = 0;
            public int LettersToHide { get; set; } = 0;
            public bool IsGameActive { get; set; } = true;
            public bool WonTheGame { get; set; } = false;
            
        }
        void WrongAnswersResult()
        {
            if(CurrentSession.WrongAnswers == 1)
            {
                pbX1.Visible = true;
                WrongAnswerSound();
                pbFaceReaction.Image = (Properties.Resources.AngryFace);
            }
            else if(CurrentSession.WrongAnswers == 2)
            {
                pbX2.Visible = true;
                WrongAnswerSound();
                pbFaceReaction.Image = (Properties.Resources.SadFace);
            }
            else if(CurrentSession.WrongAnswers == 3)
            {
                pbX3.Visible = true;
                WrongAnswerSound();
                pbTheBlade.Image = (Properties.Resources.Mistake2);
                pbFaceReaction.Image = (Properties.Resources.WorriedFace);
            }
            else if(CurrentSession.WrongAnswers == 4)
            {
                pbX4.Visible = true;
                WrongAnswerSound();
                pbTheBlade.Image = (Properties.Resources.Mistake3);
                pbFaceReaction.Image = (Properties.Resources.ScaredFace_removebg_preview);
            }
            else if(CurrentSession.WrongAnswers == 5)
            {
                pbX5.Visible = true;
                WrongAnswerSound();
                pbTheBlade.Image = (Properties.Resources.Mistake4_Last);
                pbFaceReaction.Image = ( Properties.Resources.CryFace);
                CurrentSession.IsGameActive = false;
                CurrentGameLose();
            }
        }
        void MainMenuPanel()
        {
            pnlTheGame.Visible = false;
            pnlWelcomingForm.Visible = true;
            pnlWelcomingForm.BringToFront();
        }
        void GamePanel()
        {
            pnlWelcomingForm.Visible = false;
            pnlTheGame.Visible = true;
            pnlTheGame.BringToFront();
        }
        void CorrectAnswerSound()
        {
            SoundPlayer CorrectSound = new SoundPlayer(Properties.Resources.Correct);
            CorrectSound.Play();
        }
        void WrongAnswerSound()
        {
            SoundPlayer WrongSound = new SoundPlayer(Properties.Resources.WrongAnswer2);
            WrongSound.Play();
        }
        void GameOverSound()
        {
            SoundPlayer GameOver = new SoundPlayer(Properties.Resources.GameOver);
            GameOver.Play();
        }
        void NewLevelSound()
        {
            SoundPlayer NewLevelSound = new SoundPlayer(Properties.Resources.NewLevel);
            NewLevelSound.Play();
        }
        void GameWinSound()
        {
            SoundPlayer soundPlayer = new SoundPlayer(Properties.Resources.GameWon);
            soundPlayer.Play();
        }
        void ResetKeyBoard()
        {
            foreach(Control item in gbKeyboard.Controls)
            {
                if(item is Button)
                {
                    Button btn = (Button)item;

                    btn.Enabled = true;
                }
            }
        }
        void ResetGame()
        {
            CurrentSession.TargetWord = null;
            CurrentSession.filePath = null;
            CurrentSession.WrongAnswers = 0;
            CurrentSession.GameRounds = 0;
            CurrentSession.LettersToHide = 0;
            CurrentSession.IsGameActive = true;
            CurrentSession.WonTheGame = false;
            CurrentSession.usedWords.Clear();

            lblGuessTheWord.Text = "";
            lblCurrentRound.Text = "0";
            lblNumberOfRounds.Text = "0";

            gbKeyboard.Enabled = true;
            ResetKeyBoard();

            gbXCollecter.Visible = true;
            pbTheBlade.Image = Properties.Resources.Mistake1;
            pbFaceReaction.Image = Properties.Resources.LikeFace;
            pbX1.Visible = false;
            pbX2.Visible = false;
            pbX3.Visible = false;
            pbX4.Visible = false;
            pbX5.Visible = false;

            MainMenuPanel();
        }
        void CurrentGameWon()
        {
            gbKeyboard.Enabled = false;
            pbFaceReaction.Image = (Properties.Resources.NiceFace);
            GameWinSound();
            MessageBox.Show("Good Job !","RESPECT",MessageBoxButtons.OK);
            ResetGame();
        }
        void CurrentGameLose()
        {
            gbKeyboard.Enabled=false;
            GameOverSound();
            MessageBox.Show("Bruh","What a Shame",MessageBoxButtons.OK);
            ResetGame();
        }
        
        private void AnswerCheck_Click(object sender, EventArgs e)
        {
            
            Button ClickedButton = (Button)sender;
            ClickedButton.Enabled = false;

            char GuessedLetter = ClickedButton.Text[0];

            bool IsCorrectAnswer = false;

            char[] CurrentDisplay = lblGuessTheWord.Text.Replace(" " , "").ToCharArray();

            for(int i = 0; i < CurrentSession.TargetWord.Length;i++)
            {
                if (CurrentSession.TargetWord[i] == GuessedLetter && CurrentDisplay[i] == '_')
                {
                    CurrentDisplay[i] = GuessedLetter;
                    IsCorrectAnswer = true;
                }
            }

            if(IsCorrectAnswer) 
            {
                lblGuessTheWord.Text = string.Join(" ",CurrentDisplay);
                CorrectAnswerSound();

                if (!lblGuessTheWord.Text.Contains("_"))
                {
                    if (CurrentSession.CurrentRound == CurrentSession.GameRounds)
                    {
                        CurrentGameWon();
                    }
                    else
                    {
                        CurrentSession.CurrentRound++;
                        lblCurrentRound.Text = CurrentSession.CurrentRound.ToString();

                        NewLevelSound();
                        ResetKeyBoard();

                        SelectAWord(CurrentSession.filePath,CurrentSession.LettersToHide);
                    }
                }
            }
            else
            {
                CurrentSession.WrongAnswers++;
                WrongAnswersResult();
                
            }

        }
        void RemoveLetters(int LettersToHide)
        {
            // Take the word and edit it by removing the letters .
            // then set the Word to Private Variable TargetWord.
            char[] DisplayLetters = CurrentSession.TargetWord.ToCharArray();
            Random random = new Random();
            List<int> HiddenIndexs = new List<int>();

            while (HiddenIndexs.Count < LettersToHide)
            {
                int RandIndex = random.Next(0,CurrentSession.TargetWord.Length);

                if(!HiddenIndexs.Contains(RandIndex))
                {
                    HiddenIndexs.Add(RandIndex);
                    DisplayLetters[RandIndex] = '_';
                }
            }

            lblGuessTheWord.Text = string.Join(" ",DisplayLetters);
        }
        void SelectAWord(string filePath , int LettersToHide )
        {
            // Select Word , If used before ignore it , if not add to list to check next time we play.
            // Then Send the Word to Remove the Letters
            string[] WordBank = File.ReadAllLines(filePath);

            Random random = new Random();
            int randomIndex;

            if (CurrentSession.usedWords.Count >= WordBank.Length)
            {
                // it wont activate but for future's edit maybe
                MessageBox.Show("Error , Enough Playing !", "Discord Mod Detected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                MainMenuPanel();
                return;
            }

            do
            {
                randomIndex = random.Next(0, WordBank.Length);
                CurrentSession.TargetWord = WordBank[randomIndex];

            } while (CurrentSession.usedWords.Contains(CurrentSession.TargetWord));

            CurrentSession.usedWords.Add(CurrentSession.TargetWord);
            RemoveLetters(LettersToHide);
        }
        void GameSection()
        {
            lblNumberOfRounds.Text = CurrentSession.GameRounds.ToString();

            CurrentSession.CurrentRound = 1;
            lblCurrentRound.Text = CurrentSession.CurrentRound.ToString();

            gbKeyboard.Enabled = true;
            SelectAWord(CurrentSession.filePath, CurrentSession.LettersToHide);

        }
        void StartGame()
        {
            
            if (rbLvlEasy.Checked)
            {
                CurrentSession.filePath = "EasyWords.txt";
                CurrentSession.LettersToHide = 1;
                CurrentSession.GameRounds = 4;
                GameSection();
            }
            else if (rbMedium.Checked)
            {
                CurrentSession.filePath = "MediumWords.txt";
                CurrentSession.LettersToHide = 2;
                CurrentSession.GameRounds = 5;
                GameSection();
            }
            else if (rbHard.Checked)
            {
                CurrentSession.filePath = "HardWords.txt";
                CurrentSession.LettersToHide = 4;
                CurrentSession.GameRounds = 6;
                GameSection();
            }
        }
        private void btnStartGame_Click_1(object sender, EventArgs e)
        {
            GamePanel();
            StartGame();
            
        }

        private void btnQuit_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
