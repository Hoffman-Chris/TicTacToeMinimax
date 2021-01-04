using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TicTacToe
{
    public partial class Form1 : Form
    {
        bool turn = true; // true = X turn (human player), false = Y turn (AI player)
        int turnCount = 0;
        
        public Form1()
        {
            InitializeComponent();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("By Chris", "Tic Tac Toe About");
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        /*
         * For when the human player clicks a button to take their turn.
         * AI makes it's move in response to human player turn as long as there has not been a winner.
         */
        private void button_click(object sender, EventArgs e)
        {
            Button buttonClicked = (Button)sender;
            buttonClicked.Enabled = false;
            buttonClicked.Text = "O";

            turnCount++;
            turn = true;

            // AI makes it's move in response
            if (!checkForWinner())
            {
                moveAI();
                turn = false;
                turnCount++;
                checkForWinner();
            }

        }

        /*
         * Checks the board to see if there has been a winner and, if so, displays a message.
         */
        private bool checkForWinner()
        {

            bool winner = false;

            // horizontal check
            if (A0.Text == A1.Text && A1.Text == A2.Text && !A0.Enabled)
                winner = true;
            if (B0.Text == B1.Text && B1.Text == B2.Text && !B0.Enabled)
                winner = true;
            if (C0.Text == C1.Text && C1.Text == C2.Text && !C0.Enabled)
                winner = true;

            // vertical check
            if (A0.Text == B0.Text && B0.Text == C0.Text && !A0.Enabled)
                winner = true;
            if (A1.Text == B1.Text && B1.Text == C1.Text && !A1.Enabled)
                winner = true;
            if (A2.Text == B2.Text && B2.Text == C2.Text && !A2.Enabled)
                winner = true;

            // diagonal check
            if (A0.Text == B1.Text && B1.Text == C2.Text && !A0.Enabled)
                winner = true;
            if (A2.Text == B1.Text && B1.Text == C0.Text && !A2.Enabled)
                winner = true;

            if (winner)
            {
                disableButtons();
                String winningPlayer = "";
                if (turn)
                    winningPlayer = "O";
                else
                    winningPlayer = "X";

                MessageBox.Show(winningPlayer + " has won the game.");
            }
            else
            {
                if (turnCount == 9)
                    MessageBox.Show("It was a tie.");
            }

            return winner;

        }

        /*
         * Disables all buttons on the tic tac toe board.
         */
        private void disableButtons()
        {
            try
            {
                foreach (Control c in Controls)
                {
                    Button b = (Button)c;
                    b.Enabled = false;
                }
            }
            catch { }

        }

        /*
         * Starts a new game from scratch.
         */
        private void newGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            turnCount = 0;
            turn = true;
            try
            {
                foreach (Control c in Controls)
                {
                    Button b = (Button)c;
                    b.Enabled = true;
                    b.Text = "";
                }
            }
            catch { }
        }

        // Minimax

        /*
         * Private class Move that represents a move the AI can make.
         * Stores the index of the possible move and the score associated with that move.
         */
        private class Move
        {
            private int index;
            private int score;

            public Move(int index, int score)
            {
                this.index = index;
                this.score = score;
            }

            public void setIndex( int index)
            {
                this.index = index;
            }

            public void setScore( int score)
            {
                this.score = score;
            }

            public int getIndex()
            {
                return index;
            }

            public int getScore()
            {
                return score;
            }
        }

        /*
         * Takes the current game board state, calculates the best possible move, and makes a move accordingly.
         */
        private void moveAI()
        {
            String[] originalBoard = { A0.Text, A1.Text, A2.Text, B0.Text, B1.Text, B2.Text, C0.Text, C1.Text, C2.Text };
            int moveIndex;
            if( turnCount == 1 && B1.Text == "" )
            {
                moveIndex = 4;
            }
            else
            {
                moveIndex = minimax(originalBoard, "X", 0).getIndex();
            }
            
            if (moveIndex == 0)
            {
                A0.Text = "X";
                A0.Enabled = false;
            }
            else if (moveIndex == 1)
            {
                A1.Text = "X";
                A1.Enabled = false;
            }
            else if (moveIndex == 2)
            {
                A2.Text = "X";
                A2.Enabled = false;
            }
            else if (moveIndex == 3)
            {
                B0.Text = "X";
                B0.Enabled = false;
            }
            else if (moveIndex == 4)
            {
                B1.Text = "X";
                B1.Enabled = false;
            }
            else if (moveIndex == 5)
            {
                B2.Text = "X";
                B2.Enabled = false;
            }
            else if (moveIndex == 6)
            {
                C0.Text = "X";
                C0.Enabled = false;
            }
            else if (moveIndex == 7)
            {
                C1.Text = "X";
                C1.Enabled = false;
            }
            else if (moveIndex == 8)
            {
                C2.Text = "X";
                C2.Enabled = false;
            }

        }

        /*
         * Implementation of minimax algorithm for calculating best possible move.
         */
        private Move minimax(String[] board, String player, int depth)
        {
            String human = "O";
            String ai = "X";
            ArrayList availableSpots = emptyIndexes(board);

            if (winning(board, human))
            {
                return new Move(-1, -10 + depth);
            }
            else if (winning(board, ai))
            {
                return new Move(-1, 10 - depth);
            }
            else if (availableSpots.Count == 0)
            {
                return new Move(-1, 0);
            }

            Move[] moves = new Move[availableSpots.Count];

            for( int i = 0; i < availableSpots.Count; i++)
            {
                Move move = new Move((int)availableSpots[i], -1);

                board[(int)availableSpots[i]] = player;

                if (player == ai)
                {
                    move.setScore(minimax(board, human, depth++).getScore());
                }
                else
                {
                    move.setScore(minimax(board, ai, depth++).getScore());
                }

                board[(int)availableSpots[i]] = "";

                moves[i] = move;
            }

            int bestMove = -1;
            if (player == ai)
            {
                int bestScore = -99999;
                for (int i = 0; i < moves.Length; i++)
                {
                    if (moves[i].getScore() > bestScore)
                    {
                        bestScore = moves[i].getScore();
                        bestMove = i;
                    }
                }
            }
            else
            {
                int bestScore = 999999;
                for (int i = 0; i < moves.Length; i++)
                {
                    if(moves[i].getScore() < bestScore)
                    {
                        bestScore = moves[i].getScore();
                        bestMove = i;
                    }
                }
            }
            
            return moves[bestMove];
        }

       
        /*
         * Finds all empty indexes on the board that the AI could possibly make a move to.
         */
        private ArrayList emptyIndexes(String[] board)
        {
            ArrayList freeSpaces = new ArrayList();
            for (int i = 0; i < board.Length; i++)
            {
                if (board[i] == "")
                    freeSpaces.Add(i);
            }

            return freeSpaces;
        }

        /*
         * Checks if the state of the board is a winning state.
         * Used in minimax algorithm to assign score values to moves.
         */
        private bool winning( String[] board, String player )
        {
            // horizontal check
            if (board[0] == player && board[1] == player && board[2] == player)
                return true;
            if (board[3] == player && board[4] == player && board[5] == player)
                return true;
            if (board[6] == player && board[7] == player && board[8] == player)
                return true;

            // vertical check
            if (board[0] == player && board[3] == player && board[6] == player)
                return true;
            if (board[1] == player && board[4] == player && board[7] == player)
                return true;
            if (board[2] == player && board[5] == player && board[8] == player)
                return true;

            // diagonal check
            if (board[0] == player && board[4] == player && board[8] == player)
                return true;
            if (board[2] == player && board[4] == player && board[6] == player)
                return true;

            // loss
            return false;
        }
    }
}
