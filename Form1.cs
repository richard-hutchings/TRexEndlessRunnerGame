using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TRex_endless_runner_game
{
    public partial class Form1 : Form
    {
        //private const string Obstacle = "obstacle ";
        bool jumping = false;  // boolean to check if player is jumping
        int jumpSpeed = 10; //int for jumping speed
        int force = 12;  // int for the force of the jump
        int score = 0;  //int to keep score  
        int obstacleSpeed = 10;  // int to set obstacle speed
        Random rnd = new Random();  //creates a new random class

        public Form1()
        {
            InitializeComponent();

            resetGame();  // run the reset game functionn
        }

        private void gameEvent(object sender, EventArgs e)
        {
            // linking the jumpspeed int with the player picture boxes to location
            trex.Top += jumpSpeed;

            //show the score on the score text label
            scoreText.Text = "Score: " + score;

            //if jumping is true and force is less than 0, change jumping to false
            if (jumping && force < 0)
            {
                jumping = false;
            }

            //if jumping is true, change jump speed to -12, reduce force by 1
            if (jumping)
            {
                jumpSpeed = -12;
                force -= 1;
            }
            else
            {
                // change jump speed to 12
                jumpSpeed = 12;
            }

            foreach (Control x in this.Controls)
            {
                //x is a pictureBOx and has a tag of obstacle
                if (x is PictureBox && x.Tag == "obstacle")
                {
                    x.Left -= obstacleSpeed; //Moves the obstacle to the left

                    if(x.Left + x.Width < -120)
                    {
                        //if the obstacles have gone offScreen we respawn it from the far right
                        //in this case we are calculating the form width and a random number btween 200 & 800
                        x.Left = this.ClientSize.Width + rnd.Next(200, 800);
                        score++; //Add one to the score
                    }

                    // if the t rex collides with the obstacles
                    if (trex.Bounds.IntersectsWith(x.Bounds))
                    {
                        gameTimer.Stop(); //stops the timer
                        trex.Image = Properties.Resources.dead; // changes Trex image to dead one
                        scoreText.Text += "  Press R to Restart!"; // shows press R to Restart
                    }
                }
            }
            //if trex top is greater than or = 380 & not jumping
            if ( trex.Top >= 380 && !jumping)
            { 
                //then
                force = 12; // sets force to 8
                trex.Top = floor.Top - trex.Height; // placement of player ontop of pictureBox
                jumpSpeed = 0; // set jumpspeed to 0
            }
            if ( score >= 10)
            {
                //changes obstacle speed to 15
                obstacleSpeed = 15;
            }
        }

        private void keyisdown(object sender, KeyEventArgs e)
        {
            //if player presses the space bar and jumping bool is false then set to true
            if( e.KeyCode == Keys.Space && !jumping)
            {
                jumping = true;
            }
        }

        private void keyisup(object sender, KeyEventArgs e)
        {
            // if the R key is pressed & released then we run reset function
            if( e.KeyCode == Keys.R)
            {
                resetGame();
            }

            //when the keys are released we check if jumping is true 
            //if so then we need to set it back to false so the player can jump again
            if ( jumping)
            {
                jumping = false;
            }

        }

        public void resetGame()
        {
            //Reset function
            force = 12; // force set to 12
            trex.Top = floor.Top - trex.Height; // place player on top of picture box
            jumpSpeed = 0; // sets jumpspeed to 0
            jumping = false; // Sets jumping to false
            score = 0; // Sets score to 0
            obstacleSpeed = 10; // sets obstacle speed to 10
            scoreText.Text = "Score: " + score; //resets score
            trex.Image = Properties.Resources.running; //changes trex image to running

            foreach (Control x in this.Controls)
            {
                //if X is a pictureBox & has a tag as an obstacle
                if ( x is PictureBox && x.Tag == "obstacle")
                {
                    int position = rnd.Next(600, 1000); // generates a random no between 600 & 1000
                    x.Left = 640 + (x.Left + position + x.Width * 3); //changes obstacle position to a random location
                }
            }

            gameTimer.Start(); // starts the timer
        }
    }
}
