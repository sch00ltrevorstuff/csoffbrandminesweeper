namespace csoffbrandminesweeper
{
    public partial class Form1 : Form
    {
        private class Gridsq
        {
            public int x, y, minenum;
            public string stringminenum;
            public bool cleared, hasmine, hasflag;
            //Prints the grid square status
            public string Displaysq()
            {
                if (hasmine) return " ";
                else if (minenum == 0) return " ";
                else
                {
                    stringminenum = minenum.ToString();
                    return stringminenum;
                }
            }
            //If selected then checks if has mine. If has mine then returns false, otherwise true.
            public bool Clearing()
            {
                if (hasmine == true) return false;
                else return true;
            }
        };
        const int xdim = 10;
        const int ydim = 10;
        const int totalmines = 15;
        //Creates the grid
        static List<List<Gridsq>> Funcgridsq()
        {
            List<List<Gridsq>> gridsqlist = new List<List<Gridsq>>();
            for (int i = 0; i < xdim; i++)
            {
                gridsqlist.Add(new List<Gridsq>());
                for (int j = 0; j < ydim; j++)
                {
                    Gridsq newgridsq = new Gridsq();
                    newgridsq.x = i;
                    newgridsq.y = j;
                    newgridsq.cleared = false;
                    newgridsq.hasflag = false;
                    newgridsq.hasmine = false;
                    newgridsq.minenum = 0;
                    gridsqlist[i].Add(newgridsq);
                }
            }
            return gridsqlist;
        }



        static void Funcminelocs(List<List<Gridsq>> gridsqs)
        {
            List<List<int>> minelocslist = new List<List<int>>();
            Random rnd = new Random();
            int xrand = 0, yrand = 0;
            for (int i = 0; i < totalmines; i++)
            {
                //Keep trying to make a new mine location until it finds a spot that doesn't have one already.
                if (i == 0)
                {
                    xrand = rnd.Next(xdim);
                    yrand = rnd.Next(ydim);
                }
                else
                {
                    bool loctaken = true; //For checking if there is a mine there already or not, assume it is taken.
                    while (loctaken)
                    {
                        xrand = rnd.Next(xdim);
                        yrand = rnd.Next(ydim);

                        //Checks if taken, but only if it is not the first time

                        for (int j = i - 1; j >= 0; j--)
                        {
                            if ((xrand == minelocslist[j][0]) && (yrand == minelocslist[j][1]))
                            {
                                loctaken = true;
                                break;
                            }
                            else
                            {
                                loctaken = false;
                            }
                        }
                    }
                }
                List<int> mineloc = new List<int>() { xrand, yrand };
                minelocslist.Add(mineloc);
                gridsqs[mineloc[0]][mineloc[1]].hasmine = true;
                //Add mine counter to adjacent squares that exist
                //Minus 1 because of 0 index
                if ((mineloc[0] != xdim - 1) && (mineloc[1] != ydim - 1)) gridsqs[mineloc[0] + 1][mineloc[1] + 1].minenum++;
                if ((mineloc[0] != xdim - 1)) gridsqs[mineloc[0] + 1][mineloc[1]].minenum++;
                if ((mineloc[0] != xdim - 1) && (mineloc[1] != 0)) gridsqs[mineloc[0] + 1][mineloc[1] - 1].minenum++;
                if ((mineloc[1] != 0)) gridsqs[mineloc[0]][mineloc[1] - 1].minenum++;
                if ((mineloc[0] != 0) && (mineloc[1] != 0)) gridsqs[mineloc[0] - 1][mineloc[1] - 1].minenum++;
                if ((mineloc[0] != 0)) gridsqs[mineloc[0] - 1][mineloc[1]].minenum++;
                if ((mineloc[0] != 0) && (mineloc[1] != ydim - 1)) gridsqs[mineloc[0] - 1][mineloc[1] + 1].minenum++;
                if ((mineloc[1] != ydim - 1)) gridsqs[mineloc[0]][mineloc[1] + 1].minenum++;
            }
        }
        static void Gridclearing(List<List<Gridsq>> gridsqs, int xchoice, int ychoice)
        {
            bool moretoclear = true;
            if (gridsqs[xchoice][ychoice].Clearing())
            {
                gridsqs[xchoice][ychoice].cleared = true;
                //Clear more squares
                if (gridsqs[xchoice][ychoice].minenum == 0)
                {
                    List<List<int>> checkthese = new List<List<int>>(); //Vector to which squares surrounding what
                    List<int> choices = new List<int>() { xchoice, ychoice };
                    checkthese.Add(choices);
                    List<List<int>> tempfor_checkthese = new List<List<int>>();
                    while (moretoclear)
                    {
                        for (int i = 0; i < checkthese.Count(); i++)
                        {
                            //Look up
                            if ((checkthese[i][0] != xdim - 1) && (gridsqs[checkthese[i][0] + 1][checkthese[i][1]].hasmine == false) && (gridsqs[checkthese[i][0] + 1][checkthese[i][1]].cleared == false))
                            {
                                gridsqs[checkthese[i][0] + 1][checkthese[i][1]].cleared = true;
                                if (gridsqs[checkthese[i][0] + 1][checkthese[i][1]].minenum == 0)
                                {
                                    List<int> looked = new List<int>() { checkthese[i][0] + 1, checkthese[i][1] };
                                    tempfor_checkthese.Add(looked); //Put in vector for next iteration
                                }
                            }
                            //Look down
                            if ((checkthese[i][0] != 0) && (gridsqs[checkthese[i][0] - 1][checkthese[i][1]].hasmine == false) && (gridsqs[checkthese[i][0] - 1][checkthese[i][1]].cleared == false))
                            {
                                gridsqs[checkthese[i][0] - 1][checkthese[i][1]].cleared = true;
                                if (gridsqs[checkthese[i][0] - 1][checkthese[i][1]].minenum == 0)
                                {
                                    List<int> looked = new List<int>() { checkthese[i][0] - 1, checkthese[i][1] };
                                    tempfor_checkthese.Add(looked); //Put in vector for next iteration
                                }
                            }
                            //Look right
                            if ((checkthese[i][1] != ydim - 1) && (gridsqs[checkthese[i][0]][checkthese[i][1] + 1].hasmine == false) && (gridsqs[checkthese[i][0]][checkthese[i][1] + 1].cleared == false))
                            {
                                gridsqs[checkthese[i][0]][checkthese[i][1] + 1].cleared = true;
                                if (gridsqs[checkthese[i][0]][checkthese[i][1] + 1].minenum == 0)
                                {
                                    List<int> looked = new List<int>() { checkthese[i][0], checkthese[i][1] + 1 };
                                    tempfor_checkthese.Add(looked); //Put in vector for next iteration
                                }
                            }
                            //Look left
                            if ((checkthese[i][1] != 0) && (gridsqs[checkthese[i][0]][checkthese[i][1] - 1].hasmine == false) && (gridsqs[checkthese[i][0]][checkthese[i][1] - 1].cleared == false))
                            {
                                gridsqs[checkthese[i][0]][checkthese[i][1] - 1].cleared = true;
                                if (gridsqs[checkthese[i][0]][checkthese[i][1] - 1].minenum == 0)
                                {
                                    List<int> looked = new List<int>() { checkthese[i][0], checkthese[i][1] - 1 };
                                    tempfor_checkthese.Add(looked); //Put in vector for next iteration
                                }
                            }
                        }
                        checkthese.Clear();
                        checkthese = tempfor_checkthese;

                        if (checkthese.Count() == 0)
                        {
                            break;
                        }
                    }
                }
            }
        }
        static bool GameWon(List<List<Gridsq>> gridsqs)
        {
            bool userwon = false;
            for (int i = 0; i < gridsqs.Count(); i++)
            {
                for (int j = 0; j < gridsqs[i].Count(); j++)
                {
                    if (((gridsqs[i][j].hasmine == false) && (gridsqs[i][j].cleared == true)) || ((gridsqs[i][j].hasmine == true) && (gridsqs[i][j].cleared == false)))
                    {
                        userwon = true;
                    }
                    else
                    {
                        userwon = false;
                        return userwon;
                    }
                }
            }
            return userwon;
        }
        private void RevealMines(List<List<Gridsq>> gridsqs)
        {
            int x = 0, y = 0;
            foreach (Control control in tableLayoutPanel1.Controls)
            {
                Label iconLabel = control as Label;
                if (iconLabel != null)
                {
                    iconLabel.Text = gridsqs[y][x].Displaysq();
                    if (gridsqs[y][x].hasmine)
                    {
                        iconLabel.ForeColor = Color.Black;
                        iconLabel.BackColor = Color.Red;
                        iconLabel.Image=Image.FromFile("C:\\Users\\Trevor\\Pictures\\mineicon.png");
                    }
                }
                if (x == xdim - 1)
                {
                    y++;
                    x = 0;
                }
                else x++;
            }
        }

        private void AssignIconsToSquares(List<List<Gridsq>> gridsqs)
        {
            int x = 0, y = 0;
            foreach (Control control in tableLayoutPanel1.Controls)
            {
                Label iconLabel = control as Label;
                if (iconLabel != null)
                {
                    iconLabel.Text = gridsqs[y][x].Displaysq();
                    if (gridsqs[y][x].cleared == false) iconLabel.ForeColor = iconLabel.BackColor;
                    else if (gridsqs[y][x].Displaysq() == " ") iconLabel.BackColor = Color.Beige;
                    else
                    {
                        iconLabel.ForeColor = Color.Black;
                        iconLabel.BackColor = Color.Beige;
                    }
                }
                if (x == xdim - 1)
                {
                    y++;
                    x = 0;
                }
                else x++;
            }
        }

        public Form1()
        {
            List<List<Gridsq>> gridsqs = Funcgridsq();
            Funcminelocs(gridsqs);

            InitializeComponent(gridsqs);


            AssignIconsToSquares(gridsqs);

        }

        /// <summary>
        /// Every label's Click event is handled by this event handler
        /// </summary>
        /// <param name="sender">The label that was clicked</param>
        /// <param name="e"></param>
        private void label1_Click(object sender, EventArgs e)
        {
            Label clickedLabel = sender as Label;

            if (clickedLabel != null)
            {
                // If the clicked label is black, the player clicked
                // an icon that's already been revealed --
                // ignore the click
                if (clickedLabel.ForeColor == Color.Black)
                    return;

                clickedLabel.ForeColor = Color.Black;
                int xchoice, ychoice;
                xchoice = 9 - clickedLabel.TabIndex / 10;
                ychoice = 9 - clickedLabel.TabIndex % 10;
                if (gridsqs[xchoice][ychoice].hasmine)
                {
                    RevealMines(gridsqs);
                    MessageBox.Show("You lose!", "MINE HIT");
                    Close();
                }
                Gridclearing(gridsqs, xchoice, ychoice);
                AssignIconsToSquares(gridsqs);
                if (GameWon(gridsqs))
                {
                    MessageBox.Show("You win!", "Congratulations");
                    Close();
                }
            }
        }
    }
}