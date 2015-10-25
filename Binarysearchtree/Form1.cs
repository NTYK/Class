using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Binary_search_tree
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Function Tree = new Function();
            string text = textBox1.Text;
            int sucess = Tree.Check(ref text);
            if (sucess == 0)
            {
                text = "X=" + text;
                Tree.Create(text, 0);
                Tree.Draw(panel4);
            }
            else if(sucess == 1)
                MessageBox.Show("数式に関係のない文字が入っています。");
            else
                MessageBox.Show("'('と')'の数が合いません。");
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Function Tree = new Function();
            string text = textBox2.Text;
            if (Tree.Char_Check(ref text))
            {
                string str = "";
                for (int i = 0; i < text.Length; i++)
                {
                    if (Char.IsDigit(text[i]))
                        str += text[i].ToString();
                    else if (str != "" && (text[i] == ',' || text[i] == ' '))
                    {
                        Tree.Insert(str);
                        str = "";
                    }
                }
                if (str != "")
                    Tree.Insert(str);
                Tree.add_info();
                Tree.Draw(panel4);
            }
            else
                MessageBox.Show("データの中に関係のない文字が含まれています。");
        }
    }
}
