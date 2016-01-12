using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Route_Search
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        //木構造のノードはNodeクラスで表現する
        class Node
        {
            //ノードの中に表示する値や、このノードを基準に持つ子ノード、ボタンクラス
            //ボタンの配置に関わる座標情報をもつメンバ変数を定義する
            public string Value;
            public Node[] node = new Node[4];
            public Button button;
            public int center_x;
            public int center_y;
            public int x;
            public int y;

            //コンストラクタ
            public Node(string value)
            {
                //値の設定
                this.Value = value;

                //コントロール(ノード)の設定
                this.button = new Button();
                this.button.Size = new Size(70, 70);
                System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
                path.AddEllipse(new Rectangle(10, 10, 50, 50));
                this.button.Region = new Region(path);
                this.button.BackColor = Color.White;
                this.button.Text = value;
                this.button.Font = new Font("Arial", 15, FontStyle.Bold);
                Bitmap canvas = new Bitmap(this.button.Width, this.button.Height);
                Pen p = new Pen(Color.Black, 5);
                Graphics g = Graphics.FromImage(canvas);
                g.DrawEllipse(p, 10, 10, 50, 50);
                g.Dispose();
                this.button.Image = canvas;
            }
        };

        //ボタンの作成
        Button bt1 = new Button();
        Button bt2 = new Button();
        Button bt3 = new Button();
        Button bt4 = new Button();
        Button bt5 = new Button();
        Button bt6 = new Button();
        Button bt7 = new Button();
        Button bt8 = new Button();
        Button bt9 = new Button();

        //ラベルの作成
        Label label1;
        Label label2;
        Label label3;
        Label label4;

        //テキストボックス作成
        TextBox tb = new TextBox();
        
        //タブコントロール作成
        TabControl tab = new TabControl();

        //パネル作成
        Panel panel1 = new Panel();
        Panel panel2 = new Panel();

        //ピクチャーボックス作成
        PictureBox pb1 = new PictureBox();
        PictureBox pb2 = new PictureBox();
        PictureBox pb3 = new PictureBox();
        PictureBox pb4 = new PictureBox();

        //データ格納
        int[,] data;
        int[] d_node_count;
        int[] b_node_count;
            
        private void Form1_Load(object sender, EventArgs e)
        {
            //フォームの初期化
            this.Text = "Route_Search";
            this.FormBorderStyle = FormBorderStyle.FixedSingle;

            //ラベルの初期化
            label1 = new Label();
            label1.Text = "このプログラムは\n迷路探索プログラムです。\n探索の様子を表す木構造と\n迷路の回答を表示します。";
            label1.Font = new Font("メイリオ", 15, FontStyle.Bold);
            label1.Location = new Point(15, 69);
            label1.AutoSize = true;
            label1.TextAlign = ContentAlignment.MiddleCenter;
            this.Controls.Add(label1);
            label1.Click += new EventHandler(label1_Click);

            label2 = new Label();
            label2.Text = "テキストボックスに\nデータを入力してく\nださい。\n・0:走行可能\n・1:走行不可能\n・2:スタート\n・3:ゴール\n" + 
                "[例]\n2, 0, 0  左のように数字\n0, 1, 0  の間に\",\"を入れ\n0, 0, 3  てください";
            label2.Font = new Font("メイリオ", 7, FontStyle.Bold);
            label2.Location = new Point(170, 50);
            label2.AutoSize = true;;
            this.Controls.Add(label2);
            label2.Click += new EventHandler(label1_Click);
            label2.Hide();

            label3 = new Label();
            label3.Font = new Font("メイリオ", 10, FontStyle.Bold);
            label3.Location = new Point(10, 430);
            label3.BackColor = Color.Gainsboro;
            label3.BorderStyle = BorderStyle.Fixed3D;
            label3.AutoSize = true; ;
            this.Controls.Add(label3);
            label3.Hide();

            label4 = new Label();
            label4.Font = new Font("メイリオ", 10, FontStyle.Bold);
            label4.Location = new Point(10, 430);
            label4.BackColor = Color.Gainsboro;
            label4.BorderStyle = BorderStyle.Fixed3D;
            label4.AutoSize = true; ;
            this.Controls.Add(label4);
            label4.Hide();

            //ボタンの初期化
            bt1.Text = "ファイル入力";
            bt1.Location = new Point(20, 100);
            bt1.Size = new Size(112, 60);
            bt1.Font = new Font("メイリオ", 9, FontStyle.Bold);
            bt1.Click += new EventHandler(bt1_Click);
            this.Controls.Add(bt1);
            bt1.Hide();

            bt2.Text = "キーボード入力";
            bt2.Location = new Point(152, 100);
            bt2.Size = new Size(112, 60);
            bt2.Font = new Font("メイリオ", 9, FontStyle.Bold);
            bt2.Click += new EventHandler(bt2_Click);
            this.Controls.Add(bt2);
            bt2.Hide();

            bt3.Text = "参照";
            bt3.Size = new Size(50, 25);
            bt3.Location = new Point(225, 50);
            bt3.Font = new Font("メイリオ", 10, FontStyle.Bold);
            bt3.Click += new EventHandler(bt3_Click);
            this.Controls.Add(bt3);
            bt3.Hide();

            bt4.Text = "表示";
            bt4.Size = new Size(70, 35);
            bt4.Location = new Point(210, 220);
            bt4.Font = new Font("メイリオ", 10, FontStyle.Bold);
            bt4.Click += new EventHandler(bt4_Click);
            this.Controls.Add(bt4);
            bt4.Hide();

            bt5.Text = "表示";
            bt5.Size = new Size(70, 35);
            bt5.Location = new Point(210, 220);
            bt5.Font = new Font("メイリオ", 10, FontStyle.Bold);
            bt5.Click += new EventHandler(bt5_Click);
            this.Controls.Add(bt5);
            bt5.Hide();

            bt6.Text = "データの変更";
            bt6.Size = new Size(110, 35);
            bt6.Location = new Point(360, 425);
            bt6.Font = new Font("メイリオ", 10, FontStyle.Bold);
            bt6.Click += new EventHandler(bt6_Click);
            this.Controls.Add(bt6);
            bt6.Hide();

            bt7.Text = "深さ優先探索";
            bt7.Size = new Size(110, 35);
            bt7.Location = new Point(230, 10);
            bt7.Font = new Font("メイリオ", 10, FontStyle.Bold);
            bt7.Click += new EventHandler(bt7_Click);
            this.Controls.Add(bt7);
            bt7.Hide();

            bt8.Text = "幅優先探索";
            bt8.Size = new Size(110, 35);
            bt8.Location = new Point(360, 10);
            bt8.Font = new Font("メイリオ", 10, FontStyle.Bold);
            bt8.Click += new EventHandler(bt8_Click);
            this.Controls.Add(bt8);
            bt8.Hide();

            bt9.Text = "戻る";
            bt9.Size = new Size(70, 35);
            bt9.Location = new Point(130, 220);
            bt9.Font = new Font("メイリオ", 10, FontStyle.Bold);
            bt9.Click += new EventHandler(bt9_Click);
            this.Controls.Add(bt9);
            bt9.Hide();

            //テキストボックスの初期化
            tb.Location = new Point(15, 50);
            tb.Font = new Font("MS UI Gothic", 13);
            this.Controls.Add(tb);
            tb.Hide();
        }

        //label1がクリックされたときに動作するイベント
        //データの入力方法の選択方法に画面を切り替える
        private void label1_Click(object sender, EventArgs e)
        {
            if (label1.Text == "このプログラムは\n迷路探索プログラムです。\n探索の様子を表す木構造と\n迷路の回答を表示します。")
                //データの入力選択画面に必要なコントロールの設定
                select();
        }

        //データの入力方法の選択方法に画面を切り替えるためのコントロールのプロパティ変更関数
        private void select()
        {
            //ラベルのプロパティを変更
            label1.Location = new Point(72, 60);
            label1.Text = "データの入力";
            label1.Show();

            //ボタンの配置
            bt1.Show();
            bt2.Show();

            //テキストボックスの設定
            tb.Multiline = false;
            tb.ScrollBars = ScrollBars.None;
            tb.WordWrap = true;

        }

        //"ファイル入力"ボタンをクリックしたときに動作するイベント
        private void bt1_Click(object sender, EventArgs e)
        {
            //ラベルのプロパティを変更
            label1.Location = new Point(15, 15);
            label1.Text = "ファイル入力";

            //ボタンを非表示
            bt1.Hide();
            bt2.Hide();
            
            //ボタンを表示
            bt3.Show();
            bt4.Show();
            bt9.Show();

            //テキストボックスの設定
            tb.Size = new Size(200, 40);
            tb.Show();            
        }

        //"キーボード入力"ボタンをクリックしたときに動作するイベント
        private void bt2_Click(object sender, EventArgs e)
        {
            //ラベルのプロパティを変更
            label1.Location = new Point(15, 15);
            label1.Text = "キーボード入力";

            label2.Show();

            //ボタンを非表示
            bt1.Hide();
            bt2.Hide();

            //ボタンの表示
            bt5.Show();
            bt9.Show();

            //テキストボックスの設定
            tb.Size = new Size(150, 150);
            tb.Multiline = true;
            tb.ScrollBars = ScrollBars.Both;
            tb.WordWrap = false;
            tb.Show();
        }

        //ファイル入力画面時に表示される"参照"ボタンをクリックしたときに動作するイベント
        private void bt3_Click(object sender, EventArgs e)
        {
            //ダイアログを表示し、読み込むファイル名を取得する
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                tb.Text = openFileDialog1.FileName;
            }
        }

        //ファイル入力画面時に表示される"表示"ボタンをクリックしたときに動作するイベント
        private void bt4_Click(object sender, EventArgs e)
        {
            // データ読み込み変数
            string result = string.Empty;

            // ファイルの存在チェック
            if (System.IO.File.Exists(@tb.Text))
            {
                // StreamReaderでファイルを読み込む
                System.IO.StreamReader reader = (new System.IO.StreamReader(@tb.Text, System.Text.Encoding.GetEncoding("us-ascii")));
                // 読み込みできる文字がなくなるまで繰り返す
                while (reader.Peek() >= 0)
                {
                    result += reader.ReadLine() + System.Environment.NewLine;
                }

                reader.Close();
                
                //木構造・マップを作成し、pattern変数に結果を代入する(0の場合成功)
                int pattern = create(result);
                if (pattern != 0)
                {
                    //コントロールを非表示
                    tab.Hide();
                    bt6.Hide();
                    bt7.Hide();
                    bt8.Hide();

                    //フォームをリサイズ
                    this.Size = new Size(300, 300);

                    //失敗時の理由を表示する
                    if (pattern == 1)
                    {
                        MessageBox.Show("スタート(2)、またはゴール(3)がありません。\n確認してください",
                         "エラー",
                         MessageBoxButtons.OK,
                         MessageBoxIcon.Error);
                    }
                    else if (pattern == 2)
                    {
                        MessageBox.Show("ゴールに辿りつけない迷路です。",
                         "警告",
                         MessageBoxButtons.OK,
                         MessageBoxIcon.Exclamation);
                    }
                    else if (pattern == 3)
                    {
                        MessageBox.Show("入力に不要な文字が含まれています。\n入力内容を確認してください。",
                         "エラー",
                         MessageBoxButtons.OK,
                         MessageBoxIcon.Error);
                    }
                    else if (pattern == 4)
                    {
                        MessageBox.Show("改行が含まれています。\n入力内容を確認してください。",
                         "エラー",
                         MessageBoxButtons.OK,
                         MessageBoxIcon.Error);
                    }
                }
                else
                    tb.Text = "";
            }
            else
            {
                MessageBox.Show("ファイルが存在しません。\nパスを確認してください。",
                 "警告",
                 MessageBoxButtons.OK,
                 MessageBoxIcon.Exclamation);
            }
        }

        //キーボード入力画面時に表示される"表示"ボタンをクリックしたときに動作するイベント
        private void bt5_Click(object sender, EventArgs e)
        {
            //テキストボックスに何か入力されているか
            if (tb.Text != "")
            {
                //木構造・マップを作成し、pattern変数に結果を代入する(0の場合成功)
                int pattern = create(tb.Text);
                if (pattern != 0)
                {
                    //コントロールを非表示
                    tab.Hide();
                    bt6.Hide();
                    bt7.Hide();
                    bt8.Hide();

                    //フォームをリサイズ
                    this.Size = new Size(300, 300);

                    //失敗時の理由を表示する
                    if (pattern == 1)
                    {
                        MessageBox.Show("スタート(2)、またはゴール(3)がありません。\n確認してください",
                         "エラー",
                         MessageBoxButtons.OK,
                         MessageBoxIcon.Error);
                    }
                    else if (pattern == 2)
                    {
                        MessageBox.Show("ゴールに辿りつけない迷路です。",
                         "警告",
                         MessageBoxButtons.OK,
                         MessageBoxIcon.Exclamation);
                    }
                    else if (pattern == 3)
                    {
                        MessageBox.Show("入力に不要な文字が含まれています。\n入力内容を確認してください。",
                         "エラー",
                         MessageBoxButtons.OK,
                         MessageBoxIcon.Error);
                    }
                    else if (pattern == 4)
                    {
                        MessageBox.Show("改行が含まれています。\n入力内容を確認してください。",
                         "エラー",
                         MessageBoxButtons.OK,
                         MessageBoxIcon.Error);
                    }

                }
                else
                    tb.Text = "";
            }
            else
                MessageBox.Show("テキストボックスにデータを入力してください。",
                 "警告",
                 MessageBoxButtons.OK,
                 MessageBoxIcon.Exclamation);

        }

        //"戻る"ボタンをクリックしたときに 動作するイベント
        private void bt6_Click(object sender, EventArgs e)
        {
            //データの入力選択画面に必要なコントロールの設定
            select();

            //コントロールを非表示
            tab.Hide();
            bt6.Hide();
            bt7.Hide();
            bt8.Hide();

            label1.BorderStyle = BorderStyle.None;
            label1.BackColor = SystemColors.Control;

            //フォームをリサイズ
            this.Size = new Size(300, 300);
        }

        private int create(string content)
        {
            //何×何か
            int width_max = 0, height_max = 0, start_x = 0, start_y = 0;
            bool two_flag = false, three_flag = false;
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            System.IO.StringReader rs = new System.IO.StringReader(content);
            //1行ずつ読み込む
            while(rs.Peek() > -1)
            {
                int count = 0;
                string t = rs.ReadLine();
                for(int i = 0; i < t.Length; i++)
                {
                    if (!Char.IsDigit(t[i]))
                    {
                        //0,1,2,3以外の文字が含まれていないか
                        if (!(t[i] == ' ' || t[i] == ','))
                            return 3;
                    }
                    else
                    {
                        count++;
                        if (t[i] == '2')
                            two_flag = true;
                        if (t[i] == '3')
                            three_flag = true;
                    }
                }
                //更新
                if (width_max < count)
                    width_max = count;
                if (count != 0)
                    height_max++;
                else
                {
                    //空行が含まれているか
                    if (width_max == 0)
                        return 4;
                }
            }
            //2,3を含んでいたか
            if (two_flag == false || three_flag == false)
                return 1;
            
            //ボタン設定
            bt6.Show();
            bt7.Show();
            bt8.Show();

            //タブコントロールを表示
            tab = new TabControl();
            tab.Size = new Size(460, 385);
            tab.Location = new Point(10,35);
            TabPage tab1 = new TabPage("木構造");
            tab.TabPages.Add(tab1);
            TabPage tab2 = new TabPage("迷路回答");
            tab.TabPages.Add(tab2);
            this.Controls.Add(tab);
            
            //パネルの設定
            panel1 = new Panel();
            panel1.BorderStyle = BorderStyle.Fixed3D;
            panel1.Location = new Point(5, 5);
            panel1.Size = new Size(440, 350);
            panel1.BackColor = Color.White;
            panel1.AutoScroll = true;
            tab1.Controls.Add(panel1);

            panel2 = new Panel();
            panel2.BorderStyle = BorderStyle.Fixed3D;
            panel2.Location = new Point(5, 5);
            panel2.Size = new Size(440, 350);
            panel2.BackColor = Color.White;
            panel2.AutoScroll = true;
            tab2.Controls.Add(panel2);

            //ピクチャーボックスの設定
            pb1 = new PictureBox();
            pb1.Location = new Point(0, 0);
            pb1.Size = new Size(400, 345);
            pb1.BackColor = Color.White;
            panel1.Controls.Add(pb1);

            pb2 = new PictureBox();
            pb2.Location = new Point(0, 0);
            pb2.BackColor = Color.White;
            panel2.Controls.Add(pb2);

            pb3 = new PictureBox();
            pb3.Location = new Point(0, 0);
            pb3.Size = new Size(400, 345);
            pb3.BackColor = Color.White;
            panel1.Controls.Add(pb3);
            pb3.Hide();

            pb4 = new PictureBox();
            pb4.Location = new Point(0, 0);
            pb4.BackColor = Color.White;
            panel2.Controls.Add(pb4);
            pb4.Hide();

            //マップの土台になるピクチャーボックスの設定
            if (width_max > 7)
            {
                pb2.Width = 10 + 10 * width_max + 40 * width_max;
                pb4.Width = 10 + 10 * width_max + 40 * width_max;
            }
            else
            {
                pb2.Width = 400;
                pb4.Width = 400;
            }
            if (height_max > 5)
            {
                pb2.Height = 10 + 10 * height_max + 40 * height_max;
                pb4.Height = 10 + 10 * height_max + 40 * height_max;
            }
            else
            {
                pb2.Height = 345;
                pb4.Height = 345;
            }
            
            //描画設定
            Bitmap canvas2 = new Bitmap(pb2.Width, pb2.Height);
            Graphics g2 = Graphics.FromImage(canvas2);
            Bitmap canvas4 = new Bitmap(pb4.Width, pb4.Height);
            Graphics g4 = Graphics.FromImage(canvas4);
            
            //動的配列
            data = new int[height_max + 1, width_max + 1];
            d_node_count = new int[100];
            b_node_count = new int[100];

            //ルート(木構造)
            Node root1 = null, root2 = null;

            //データ格納変数に値を入れる
            int y = 0;
            rs = new System.IO.StringReader(content);
            while (rs.Peek() > -1)
            {
                int x = 0;
                string t = rs.ReadLine();
                for (int k = 0; k < t.Length; k++)
                {
                    if (Char.IsDigit(t[k]))
                    {
                        if (t[k] == '0')
                            data[y, x++] = 1;
                        else if (t[k] == '1')
                            data[y, x++] = 0;
                        else
                            data[y, x++] = t[k] - 48;
                        if (t[k] == '2')
                        {
                            start_x = x-1;
                            start_y = y;
                        }
                    }
                }
                while (x < width_max)
                    data[y, x++] = 0;
                y++;
            }
            //一度通っているか記録するbool配列の定義
            bool[,,] width = width = new bool[1,height_max + 1, width_max];
            bool[,,] height = height = new bool[1,height_max, width_max + 1];

            Pen pen = new Pen(Color.FromArgb(255, 255, 0, 0), 10);
            Queue<int> queue_x = new Queue<int>();
            Queue<int> queue_y = new Queue<int>();
            Queue<int> Parents = new Queue<int>();
            queue_x.Enqueue(start_x);
            queue_y.Enqueue(start_y);
            Parents.Enqueue(4);

            //時間計測開始
            sw.Start();
            //深さ優先探索
            if (!depth_first_search(start_x, start_y, width, height, 0, null, ref root1, g2, pen))
                return 2;
            //時間計測終了
            sw.Stop();
            label3.Text = "探索時間 " + sw.Elapsed;

            sw = new System.Diagnostics.Stopwatch();

            //一度通っているか記録するbool配列を再定義
            width = new bool[1, height_max + 1, width_max];
            height = new bool[1, height_max, width_max + 1];
            Node[] n = new Node[1];
            n[0] = null;

            //時間計測開始
            sw.Start();
            //幅優先探索
            breadth_first_search(queue_x, queue_y, width, height, width_max, height_max, 0, Parents, n, ref root2, g4, pen);
            //時間計測終了
            sw.Stop();
            label4.Text = "探索時間 " + sw.Elapsed;

            root1.button.BackColor = Color.Blue;
            root2.button.BackColor = Color.Blue;

            //描画設定
            Bitmap canvas1 = new Bitmap(pb1.Width, pb1.Height);
            Graphics g1 = Graphics.FromImage(canvas1);
            Bitmap canvas3 = new Bitmap(pb3.Width, pb3.Height);
            Graphics g3 = Graphics.FromImage(canvas3);

            pen = new Pen(Color.FromArgb(255, 0, 0, 0), 3);
            d_branch_draw(root1, g1, pen);
            b_branch_draw(root2, g3, pen);

            d_maze(width_max, height_max);
            b_maze(width_max, height_max);

            g1.Dispose();
            g2.Dispose();
            g3.Dispose();
            g4.Dispose();
            pb1.Image = canvas1;
            pb2.Image = canvas2;
            pb3.Image = canvas3;
            pb4.Image = canvas4;

            //ラベルの設定
            label3.Show();
            
            //フォームをリサイズ
            this.Size = new Size(500, 500);

            //ウィンドウ上にあるコントロールをすべて非表示
            label1.Hide();
            label2.Hide();
            tb.Hide();
            bt3.Hide();
            bt4.Hide();
            bt5.Hide();
            bt9.Hide();

            //作成成功時に表示するメッセージ
            MessageBox.Show("木構造と迷路の回答を表示します",
             "成功",
             MessageBoxButtons.OK);

            return 0;
        }
        
        //"深さ優先探索"ボタンをクリックしたときに動作する
        private void bt7_Click(object sender, EventArgs e)
        {
            pb1.Show();
            pb2.Show();
            pb3.Hide();
            pb4.Hide();
            label3.Show();
            label4.Hide();
        }

        //"幅優先探査"ボタンをクリックしたときに動作する
        private void bt8_Click(object sender, EventArgs e)
        {
            pb1.Hide();
            pb2.Hide();
            pb3.Show();
            pb4.Show();
            label3.Hide();
            label4.Show();
        }

        //"データ変更"ボタンをクリックしたときに動作する
        private void bt9_Click(object sender, EventArgs e)
        {
            //データの入力選択画面に必要なコントロールの設定
            select();

            //コントロールを非表示
            bt3.Hide();
            bt4.Hide();
            bt5.Hide();
            bt9.Hide();
            label2.Hide();
            label3.Hide();
            label4.Hide();                
            tb.Hide();
            tb.Text = "";
        }

        //深さ優先探索
        private bool depth_first_search(int put_x, int put_y, bool[,,] width, bool[,,] height, int count, Node parent, ref Node root, Graphics g, Pen pen)
        {
            Node n = new Node(put_x.ToString() + ", " + put_y.ToString());

            if (count > 4)
            {
                pb1.Size = new Size(pb1.Width, 90 + 70 * count);
            }
            if (d_node_count[count] > 5)
            {
                pb1.Size = new Size(20 + 70 * d_node_count[count], pb1.Height);
            }

            n.button.Location = new Point(10 + 70 * d_node_count[count], 10 + 70 * count);
            n.center_x = 10 + 35 + 70 * d_node_count[count];
            n.center_y = 10 + 35 + 70 * count;
            d_node_count[count]++;
            n.button.Text = n.Value;
            pb1.Controls.Add(n.button);
            if (data[put_y, put_x] == 3)
                n.button.BackColor = Color.Green;

            if (parent != null)
            {
                if (parent.node[0] == null)
                    parent.node[0] = n;
                else if (parent.node[1] == null)
                    parent.node[1] = n;
                else if (parent.node[2] == null)
                    parent.node[2] = n;
                else if (parent.node[3] == null)
                    parent.node[3] = n;
            }
            else
                root = n;

            if (data[put_y, put_x] == 3)
                return true;
            else if (data[put_y, put_x] != 0)
            {
                if (put_x > 0 && !width[0, put_y, put_x - 1])
                {
                    width[0, put_y, put_x - 1] = true;
                    if (depth_first_search(put_x - 1, put_y, width, height, count + 1, n, ref root, g, pen))
                    {
                        g.DrawLine(pen, 30 + 10 * put_x + 40 * put_x, 30 + 10 * put_y + 40 * put_y, 30 + 10 * (put_x - 1) + 40 * (put_x - 1), 30 + 10 * put_y + 40 * put_y);
                        return true;
                    }
                }
                if (put_y > 0 && !height[0, put_y - 1, put_x])
                {
                    height[0, put_y - 1, put_x] = true;
                    if (depth_first_search(put_x, put_y - 1, width, height, count + 1, n, ref root, g, pen))
                    {
                        g.DrawLine(pen, 30 + 10 * put_x + 40 * put_x, 30 + 10 * put_y + 40 * put_y, 30 + 10 * put_x + 40 * put_x, 30 + 10 * (put_y - 1) + 40 * (put_y - 1));
                        return true;
                    }
                }
                if (!width[0, put_y, put_x])
                {
                    width[0, put_y, put_x] = true;
                    if (depth_first_search(put_x + 1, put_y, width, height, count + 1, n, ref root, g, pen))
                    {
                        g.DrawLine(pen, 30 + 10 * put_x + 40 * put_x, 30 + 10 * put_y + 40 * put_y, 30 + 10 * (put_x + 1) + 40 * (put_x + 1), 30 + 10 * put_y + 40 * put_y);
                        return true;
                    }
                }
                if (!height[0, put_y, put_x])
                {
                    height[0, put_y, put_x] = true;
                    if (depth_first_search(put_x, put_y + 1, width, height, count + 1, n, ref root, g, pen))
                    {
                        g.DrawLine(pen, 30 + 10 * put_x + 40 * put_x, 30 + 10 * put_y + 40 * put_y, 30 + 10 * put_x + 40 * put_x, 30 + 10 * (put_y + 1) + 40 * (put_y + 1));
                        return true;
                    }
                }
            }     
            return false;
        }

        //木構造を表示する際にノード間をつなぐブランチを描写する関数　(深さ優先探索)
        private void d_branch_draw(Node parent, Graphics g, Pen pen)
        {
            if (parent.node[0] != null)
            {
                g.DrawLine(pen, parent.center_x, parent.center_y, parent.node[0].center_x, parent.node[0].center_y);
                d_branch_draw(parent.node[0], g, pen);
            }
            if (parent.node[1] != null)
            {
                g.DrawLine(pen, parent.center_x, parent.center_y, parent.node[1].center_x, parent.node[1].center_y);
                d_branch_draw(parent.node[1], g, pen);
            }
            if (parent.node[2] != null)
            {
                g.DrawLine(pen, parent.center_x, parent.center_y, parent.node[2].center_x, parent.node[2].center_y);
                d_branch_draw(parent.node[2], g, pen);
            }
            if (parent.node[3] != null)
            {
                g.DrawLine(pen, parent.center_x, parent.center_y, parent.node[3].center_x, parent.node[3].center_y);
                d_branch_draw(parent.node[3], g, pen);
            }
        }

        //マップを作成する関数　(深さ優先探索)
        private void d_maze(int width, int height)
        {
            for(int i = 0; i < width; i++)
            {
                for(int j = 0; j < height ; j++)
                {
                    d_maze_create(data[j,i], i, j);
                }
            }
        }

        //マップを作成する関数　(深さ優先探索)
        private void d_maze_create(int data, int x, int y)
        {
            Button bt = new Button();
            bt.Size = new Size(40, 40);
            bt.Location = new Point(10 + 10 * x + 40 * x, 10 + 10 * y + 40 * y);
            System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
            path.AddEllipse(new Rectangle(5, 5, 30, 30));
            bt.Region = new Region(path);
            Bitmap canvas = new Bitmap(bt.Width, bt.Height);
            Pen p = new Pen(Color.Black, 5);
            Graphics g = Graphics.FromImage(canvas);
            g.DrawEllipse(p, 5, 5, 30, 30);
            g.Dispose();
            bt.Image = canvas;
            pb2.Controls.Add(bt);

            if(data == 0)
                bt.BackColor = Color.White;
            if(data == 1)
                bt.BackColor = Color.Black;
            if(data == 2)
                bt.BackColor = Color.Blue;
            if (data == 3)
                bt.BackColor = Color.Green;
        }

        //幅優先探索
        private Node breadth_first_search(Queue<int> queue_x, Queue<int> queue_y, bool[, ,] width, bool[, ,] height, int width_max, int height_max, int count, Queue<int> Parents, Node[] parent, ref Node root, Graphics g, Pen pen)
        {
            int num = 0, parent_count = 0, target, number = 0, p_number = 0, con = 0, counter = 0, success = 0;
            Queue<int> next_queue_x = new Queue<int>();
            Queue<int> next_queue_y = new Queue<int>();
            Queue<int> buf_queue_x = new Queue<int>(queue_x);
            Queue<int> buf_queue_y = new Queue<int>(queue_y);
            Queue<int> Max = new Queue<int>();
            Queue<int> Num = new Queue<int>();
            
            while (queue_x.Count != 0 && queue_y.Count != 0)
            {
                int x = queue_x.Dequeue();
                int y = queue_y.Dequeue();
                number = 0;
                if (data[y, x] == 0)
                {
                    Max.Enqueue(0);
                    num++;
                    continue;
                }
                else
                {
                    if (x > 0 && !width[num, y, x - 1])
                    {
                        next_queue_x.Enqueue(x - 1);
                        next_queue_y.Enqueue(y);
                        number++;
                    }
                    if (y > 0 && !height[num, y - 1, x])
                    {
                        next_queue_x.Enqueue(x);
                        next_queue_y.Enqueue(y - 1);
                        number++;
                    }
                    if (width_max - 1 > x && !width[num, y, x])
                    {
                        next_queue_x.Enqueue(x + 1);
                        next_queue_y.Enqueue(y);
                        number++;
                    }
                    if (height_max - 1 > y && !height[num, y, x])
                    {
                        next_queue_x.Enqueue(x);
                        next_queue_y.Enqueue(y + 1);
                        number++;
                    }
                    Max.Enqueue(number);
                    Num.Enqueue(num);
                }
                num++;
            }
            bool[, ,] next_width = new bool[next_queue_x.Count, height_max + 1, width_max];
            bool[, ,] next_height = new bool[next_queue_x.Count, height_max, width_max + 1];
            Queue<int> Parent_count = new Queue<int>(Max);
            Node[] Parent = new Node[Num.Count];
            target = Max.Dequeue();

            for (int i = 0; i < next_queue_x.Count; i++)
            {
                if (parent_count == target)
                {
                    parent_count = 0;
                    target = Max.Dequeue();
                    i--;
                    counter++;
                    continue;
                }
                parent_count++;
                for (int j = 0; j < height_max + 1; j++)
                {
                    for (int k = 0; k < width_max; k++)
                    {
                        next_width[i, j, k] = width[counter, j, k];
                    }
                }

                for (int j = 0; j < height_max; j++)
                {
                    for (int k = 0; k < width_max + 1; k++)
                    {
                        next_height[i, j, k] = height[counter, j, k];
                    }
                }
            }

            number = 0;
            num = Num.Dequeue();
            success = Parents.Dequeue();
            counter = 0;
            while (buf_queue_x.Count != 0 && buf_queue_y.Count != 0)
            {
                int x = buf_queue_x.Dequeue();
                int y = buf_queue_y.Dequeue();
                while (success == counter)
                {
                    if(success != 0)
                    con++;
                    success = Parents.Dequeue();
                    counter = 0;
                }

                Node n = new Node(x.ToString() + ", " + y.ToString());
                n.button.Location = new Point(10 + 70 * b_node_count[count], 10 + 70 * count);
                n.center_x = 10 + 35 + 70 * b_node_count[count];
                n.center_y = 10 + 35 + 70 * count;
                b_node_count[count]++;
                n.button.Text = n.Value;
                pb3.Controls.Add(n.button);
                n.x = x;
                n.y = y;

                counter++;

                if (data[y, x] == 3)
                    n.button.BackColor = Color.Green;
                
                if (parent[con] != null)
                {
                    if (parent[con].node[0] == null)
                        parent[con].node[0] = n;
                    else if (parent[con].node[1] == null)
                        parent[con].node[1] = n;
                    else if (parent[con].node[2] == null)
                        parent[con].node[2] = n;
                    else if (parent[con].node[3] == null)
                        parent[con].node[3] = n;
                }
                else
                    root = n;

                if (count > 3)
                {
                    pb3.Size = new Size(pb3.Width, 90 + 70 * count);
                }
                if (b_node_count[count] > 5)
                {
                    pb3.Size = new Size(20 + 70 * b_node_count[count], pb3.Height);
                }

                if (data[y, x] == 0)
                    continue;
                else if (data[y, x] == 3)
                    return n;
                else
                {
                    Parent[p_number++] = n;
                    if (x > 0 && !width[num, y, x - 1])
                    {
                        next_width[number++, y, x - 1] = true;
                    }
                    if (y > 0 && !height[num, y - 1, x])
                    {
                        next_height[number++, y - 1, x] = true;
                    }
                    if (width_max - 1 > x && !width[num, y, x])
                    {
                        next_width[number++, y, x] = true;
                    }
                    if (height_max - 1 > y && !height[num, y, x])
                    {
                        next_height[number++, y, x] = true;
                    }
                   if (Num.Count != 0)
                        num = Num.Dequeue();
                }
            }

            Node next = breadth_first_search(next_queue_x, next_queue_y, next_width, next_height, width_max, height_max, count + 1, Parent_count, Parent, ref root, g, pen);
            for (int i = 0; ; i++)
                for (int j = 0; j < 4; j++ )
                    if (Parent[i].node[j] == next)
                    {
                        g.DrawLine(pen, 30 + 10 * Parent[i].x + 40 * Parent[i].x, 30 + 10 * Parent[i].y + 40 * Parent[i].y, 30 + 10 * next.x + 40 * next.x, 30 + 10 * next.y + 40 * next.y);
                        return Parent[i];
                    }
        }

        //木構造を表示する際にノード間をつなぐブランチを描写する関数　(幅優先探索)
        private void b_branch_draw(Node parent, Graphics g, Pen pen)
        {
            if (parent.node[0] != null)
            {
                g.DrawLine(pen, parent.center_x, parent.center_y, parent.node[0].center_x, parent.node[0].center_y);
                d_branch_draw(parent.node[0], g, pen);
            }
            if (parent.node[1] != null)
            {
                g.DrawLine(pen, parent.center_x, parent.center_y, parent.node[1].center_x, parent.node[1].center_y);
                d_branch_draw(parent.node[1], g, pen);
            }
            if (parent.node[2] != null)
            {
                g.DrawLine(pen, parent.center_x, parent.center_y, parent.node[2].center_x, parent.node[2].center_y);
                d_branch_draw(parent.node[2], g, pen);
            }
            if (parent.node[3] != null)
            {
                g.DrawLine(pen, parent.center_x, parent.center_y, parent.node[3].center_x, parent.node[3].center_y);
                d_branch_draw(parent.node[3], g, pen);
            }
        }

        //マップを作成する関数　(幅優先探索)
        private void b_maze(int width, int height)
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    b_maze_create(data[j, i], i, j);
                }
            }
        }

        //マップを作成する関数　(幅優先探索)
        private void b_maze_create(int data, int x, int y)
        {
            Button bt = new Button();
            bt.Size = new Size(40, 40);
            bt.Location = new Point(10 + 10 * x + 40 * x, 10 + 10 * y + 40 * y);
            System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
            path.AddEllipse(new Rectangle(5, 5, 30, 30));
            bt.Region = new Region(path);
            Bitmap canvas = new Bitmap(bt.Width, bt.Height);
            Pen p = new Pen(Color.Black, 5);
            Graphics g = Graphics.FromImage(canvas);
            g.DrawEllipse(p, 5, 5, 30, 30);
            g.Dispose();
            bt.Image = canvas;
            pb4.Controls.Add(bt);
            if (data == 0)
                bt.BackColor = Color.White;
            if (data == 1)
                bt.BackColor = Color.Black;
            if (data == 2)
                bt.BackColor = Color.Blue;
            if (data == 3)
                bt.BackColor = Color.Green;
        }
    }
}