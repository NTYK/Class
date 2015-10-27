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

        private bool open = false;
        private Function Tree;

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("'何も入力されていません。",
                "エラー",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
                return;
            }
            Function Buf = new Function(this);
            string text = textBox1.Text;
            int sucess = Buf.Check(ref text);
            if (sucess == 0)
            {
                Bitmap canvas = new Bitmap(pictureBox1.Width + Function.radius * textBox2.Text.Length, pictureBox1.Height);
                Graphics g = Graphics.FromImage(canvas);

                if (Tree != null)
                {
                    Tree.Dispose();
                    Tree = null;
                    pictureBox1.Image = canvas;
                }
                Tree = Buf;

                text = "X=" + text;
                Tree.Create(text, 0);
                Tree.Draw(pictureBox1, this, g);
                g.Dispose();
                pictureBox1.Image = canvas;
                Tree.height();
            }
            else if (sucess == 1)
                MessageBox.Show("数式に関係のない文字が入っています。",
                "エラー",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            else
                MessageBox.Show("'('と')'の数が合いません。",
                "エラー",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (textBox2.Text == "")
            {
                MessageBox.Show("'何も入力されていません。",
                "エラー",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
                return;
            }
            Function Buf = new Function(this);
            string text = textBox2.Text;
            if (Buf.Char_Check(ref text))
            {
                Bitmap canvas = new Bitmap(pictureBox1.Width + Function.radius * textBox2.Text.Length, pictureBox1.Height);
                Graphics g = Graphics.FromImage(canvas);

                if (Tree != null)
                {
                    Tree.Dispose();
                    Tree = null;
                    pictureBox1.Image = canvas;
                }
                Tree = Buf;

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
                Tree.Draw(pictureBox1, this, g);
                g.Dispose();
                pictureBox1.Image = canvas;
                open = true;
                Tree.height();
            }
            else
                MessageBox.Show("データの中に関係のない文字が含まれています。",
                "エラー",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
        
        private void tabControl1_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if (Tree != null)
            {
                DialogResult result = MessageBox.Show("表示中の2分木を閉じることになります。タブを切り替えますか？",
                                                        "確認",
                                                        MessageBoxButtons.YesNo,
                                                        MessageBoxIcon.Exclamation,
                                                        MessageBoxDefaultButton.Button2);
                if (result == DialogResult.Yes)
                {
                    Bitmap canvas = new Bitmap(pictureBox1.Width, pictureBox1.Height);
                    Graphics g = Graphics.FromImage(canvas);
                    Tree.Dispose();
                    Tree = null;
                    pictureBox1.Image = canvas;
                    textBox1.Text = "";
                    textBox2.Text = "";
                    label4.Text = "";
                    label11.Text = "";
                    label12.Text = "";
                    label13.Text = "";
                    label14.Text = "";
                    open = false;
                }
                else
                    e.Cancel = true;
            }
        }

        //preorder
        private void button2_Click(object sender, EventArgs e)
        {
            string str = "";
            Tree.Preorder(ref str);
            label4.Text = str;
        }

        //postorder
        private void button3_Click(object sender, EventArgs e)
        {
            string str = "";
            Tree.Postorder(ref str);
            label4.Text = str;
        }

        //inorder
        private void button4_Click(object sender, EventArgs e)
        {
            string str = "";
            Tree.Inorder(ref str);
            label4.Text = str;
        }

        //level order
        private void button5_Click(object sender, EventArgs e)
        {
            string str = "";
            Tree.Levelorder(ref str);
            label4.Text = str;
        }

        //追加・削除
        private void button7_Click(object sender, EventArgs e)
        {
            if(!open)
                MessageBox.Show("二分木を生成してください。",
                "エラー",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            else
            {
                if (textBox3.Text == "")
                {
                    MessageBox.Show("値を入力してください。",
                    "エラー",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                    return;
                }
                else
                {
                    string str = textBox3.Text;
                    for (int i = 0; i < str.Length; i++)
                    {
                        if (!Char.IsDigit(str[i]))
                        {
                            MessageBox.Show("値を入力してください。",
                            "エラー",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                            return;
                        }

                    }

                    Bitmap canvas = new Bitmap(pictureBox1.Width + Function.radius * textBox2.Text.Length, pictureBox1.Height);
                    Graphics g = Graphics.FromImage(canvas);
                    Tree.Unshow();
                    if (radioButton1.Checked)
                        Tree.Insert(str);
                    else if (radioButton2.Checked)
                    {
                        if (Tree.Delete(str))
                            label17.Text = "Success";
                        else
                            label17.Text = "Not find";
                    }
                    Tree.add_info();
                    Tree.Draw(pictureBox1, this, g);
                    g.Dispose();
                    pictureBox1.Image = canvas;
                    Tree.height();
                }
            }
        }
    }
}
