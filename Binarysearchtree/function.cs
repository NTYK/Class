using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;

namespace Binary_search_tree
{
    public class Function : IDisposable 
    {
        // 二分探索木のノード
        public class Node
        {
            public string Value;
            public Node Left;
            public Node Right;

            public Button button;
            public int space_left;
            public int space_right;
            public int left;
            public int right;

            // コンストラクタ
            public Node(string value)
            {
                //値の設定
                this.Value = value;

                //コントロール(ノード)の設定
                this.button = new System.Windows.Forms.Button();
                this.button.Size = new Size(70, 70);
                System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
                path.AddEllipse(new Rectangle(10, 10, radius, radius));
                this.button.Region = new Region(path);
                this.button.BackColor = Color.White;
                this.button.Text = value;
                this.button.Font = new Font("Arial", 15, FontStyle.Bold);
                //円の描写
                Bitmap canvas = new Bitmap(this.button.Width, this.button.Height);
                Pen p = new Pen(Color.Black, 5);
                Graphics g = Graphics.FromImage(canvas);
                g.DrawEllipse(p, 10, 10, 50, 50);
                g.Dispose();
                this.button.Image = canvas;                
            }

            //ノードがクリックされた時
            public void button_Click(object sender, EventArgs e)
            {
                Function.Parent(this.Value);
                Function.Root();
                Function.Child(this.Value);
                Function.Sibling();
                this.button.BackColor = Color.Aqua;
            }
        }

        //インスタンスの破棄
        public void Dispose()
        {
            if (root.Left != null)
                Dispose(root.Left);
            if (root.Right != null)
                Dispose(root.Right);
            root.button.Visible = false;
            root = null;
        }

        //インスタンスの破棄
        public void Dispose(Node node)
        {
            if (node.Left != null)
                Dispose(node.Left);
            if (node.Right != null)
                Dispose(node.Right);
            node.button.Visible = false;
            node = null;
        }

        //ルート
        private Node root;
        private static Node s_root;
        //ノード数
        private int node_count;
        //ノードをつなぐ線の色・太さ
        private static Pen pen = new Pen(Color.Black, 2);
        //ノード間の距離(縦)
        private static int space_y = 100;
        //ノード(円)の直径
        public static int radius;
        //同じ世代？のノード
        private List<string> sibling = new List<string>();

        private static Form1 form;

        //コンストラクタ
        public Function(Form1 f)
        {
            form = f;
            node_count = 0;
            radius = 50;
        }

        //このプログラムでは使用しない
        private static int rank(char sign)
        {
            if (sign == '*' || sign == '/' || sign == '%' || sign == '×')
                return 0;
            if (sign == '+' || sign == '-')
                return 1;
            return 2;
        }

        //逆ポーランド記法　このプログラムでは使用しない
        public static bool sort(ref string text)
        {
            MessageBox.Show(text);
            string buffer = "";
            char buf;
            Stack<char> stack = new Stack<char>();
            for (int i = 0; i < text.Length; i++)
            {
                //数値・文字ならバッファに追加
                if (Char.IsDigit(text[i]) || Char.IsUpper(text[i]) || Char.IsLower(text[i]))
                {
                    buffer += text[i];
                    MessageBox.Show(buffer);
                }
                else if (text[i] == ')')
                {
                    buf = text[i];
                    while (stack.Count != 0 && (buf = stack.Pop()) != '(')
                        buffer += buf;
                    if (buf != '(')
                        return false;
                }
                else if (text[i] == '(')
                    stack.Push(text[i]);
                else if (stack.Count == 0)
                    stack.Push(text[i]);
                else
                {
                    while (stack.Count != 0)
                    {
                        if (rank(text[i]) > rank(stack.Peek()))
                            buffer += stack.Pop();
                        else
                        {
                            stack.Push(text[i]);
                            break;
                        }

                    }
                }
            }
            while (stack.Count != 0)
                buffer += stack.Pop();
            MessageBox.Show(buffer);

            return true;
        }

        //数式が正しいか確認
        public int Check(ref string text)
        {
            int right = 0, left = 0;
            //textbox1に入力された文字列を１つずつ調べる
            for (int i = 0; i < text.Length; i++)
            {
                if (!Char.IsDigit(text[i]) && !Char.IsUpper(text[i]) && !Char.IsLower(text[i]))
                    if (text[i] != '+' && text[i] != '-' && text[i] != '*' && text[i] != '/' && text[i] != '%' && text[i] != '×' && text[i] != '÷')
                        if (text[i] == '(')
                            left++;
                        else if (text[i] == ')')
                            right++;
                        else
                            //数字・大文字・小文字・符号以外の文字が数式の中にあることは正しくない
                            return 1;
            }
            //'('と')'の数が同じでなければ数式は正しくない
            if (left != right)
                return 2;
            return 0;
        }

        //データ群が正しいか確認
        public bool Char_Check(ref string text)
        {
            //textbox2に入力された文字列を１つずつ調べる
            for (int i = 0; i < text.Length; i++)
                if (!Char.IsDigit(text[i]) && text[i] != ' ' && text[i] != ',')
                    //数字・区切り文字以外の文字が数式の中にあることは正しくない
                    return false;            
            return true;
        }

        //数式の二分探索木を作成
        public void Create(string text, int left)
        {
            //スタックを用いることで'('と')'の間の式をまとめる
            Stack<char> stack = new Stack<char>();
            for (int i = 0; i < text.Length; i++)
                if (!Char.IsDigit(text[i]) && !Char.IsUpper(text[i]) && !Char.IsLower(text[i]))
                {
                    if (text[i] == '(')
                        stack.Push(text[i]);
                    else if (text[i] == ')')
                        stack.Pop();
                    else{
                        //符号の場合
                        //ルートを作成
                        root = new Node(text[i].ToString());
                        //Clickイベントハンドラを追加する
                        root.button.Click += new EventHandler(root.button_Click);
                        //ノード数をインクリメント
                        node_count++;
                        s_root = root;
                        //左の子へ
                        Create(root, text, left, i, true);

                        if (root.Left.space_right < root.Left.right)
                            root.space_left = root.Left.right + 1;
                        else
                            root.space_left = root.Left.space_right + 1;
                        if (root.Left.left < root.Left.right)
                            root.left = root.Left.right + 1;
                        else
                            root.left = root.Left.left + 1;

                        //右の子へ
                        Create(root, text, i+1, text.Length, false);

                        if (root.Right.space_left < root.Left.left)
                            root.space_right = root.Right.left + 1;
                        else
                            root.space_right = root.Right.space_left + 1;
                        if (root.Right.left < root.Right.right)
                            root.right = root.Right.right + 1;
                        else
                            root.right = root.Right.left + 1;

                        return;
                    }
                }
        }

        //特定のノードから指定した値を持つノードを追加
        public void Create(Node node, string text, int left, int right, bool add)
        {
            //スタックを用いることで'('と')'の間の式をまとめる
            Stack<char> stack = new Stack<char>();
            for (int i = left; i < right; i++)
                if (!Char.IsDigit(text[i]) && !Char.IsUpper(text[i]) && !Char.IsLower(text[i]))
                {
                    if (text[i] == '(')
                        stack.Push(text[i]);
                    else if (text[i] == ')')
                    {
                        if (stack.Count != 0)
                            stack.Pop();
                    }
                    else
                    {
                        //符号の場合
                        //新しいノードの作成
                        Node n = new Node(text[i].ToString());
                        n.button.Click += new EventHandler(n.button_Click);
                        //ノード数をインクリメント
                        node_count++;
                        if (add)
                        {
                            node.Left = n;
                            //再帰呼び出し
                            //左の子へ
                            Create(node.Left, text, left, i, true);

                            if (node.Left.Left.space_right < node.Left.Left.right)
                                node.Left.space_left = node.Left.Left.right + 1;
                            else
                                node.Left.space_left = node.Left.Left.space_right + 1;
                            if (node.Left.Left.left < node.Left.Left.right)
                                node.Left.left = node.Left.Left.right + 1;
                            else
                                node.Left.left = node.Left.Left.left + 1;
                            //右の子へ
                            Create(node.Left, text, i + 1, text.Length, false);

                            if (node.Left.Right.space_left < node.Right.Left.left)
                                node.Left.space_right = node.Left.Right.left + 1;
                            else
                                node.Left.space_right = node.Left.Right.space_left + 1;
                            if (node.Left.Right.left < node.Left.Right.right)
                                node.Left.right = node.Left.Right.right + 1;
                            else
                                node.Left.right = node.Left.Right.left + 1;
                        }
                        else
                        {
                            node.Right = n;
                            //再帰呼び出し
                            //左の子へ
                            Create(node.Right, text, left, i, true);

                            if (node.Right.Left.space_right < node.Right.Left.right)
                                node.Right.space_left = node.Right.Left.right + 1;
                            else
                                node.Right.space_left = node.Right.Left.space_right + 1;
                            if (node.Right.Left.left < node.Right.Left.right)
                                node.Right.left = node.Right.Left.right + 1;
                            else
                                node.Right.left = node.Right.Left.left + 1;

                            //右の子へ
                            Create(node.Right, text, i + 1, text.Length, false);

                            if (node.Right.Right.space_left < node.Right.Right.left)
                                node.Right.space_right = node.Right.Right.left + 1;
                            else
                                node.Right.space_right = node.Right.Right.space_left + 1;
                            if (node.Right.Right.left < node.Right.Right.right)
                                node.Right.right = node.Right.Right.right + 1;
                            else
                                node.Right.right = node.Right.Right.left + 1;
                        }
                        return;
                    }
                }

            string str = "";

            for (int i = left; i < right; i++)
            {
                if (Char.IsDigit(text[i]) || Char.IsUpper(text[i]) || Char.IsLower(text[i]))
                {
                    str += text[i].ToString();
                }
                else if (str != "")
                {
                    //新しいノードの作成
                    Node n = new Node(str);
                    n.button.Click += new EventHandler(n.button_Click);
                    n.left = 0;
                    n.right = 0;
                    n.space_left = 0;
                    n.space_right = 0;
                    //ノード数をインクリメント
                    node_count++;
                    if (add)
                        node.Left = n;
                    else
                        node.Right = n;
                    return;
                }
            }
            if (str != "")
            {
                //新しいノードの作成
                Node n = new Node(str);
                n.button.Click += new EventHandler(n.button_Click);
                n.left = 0;
                n.right = 0;
                n.space_left = 0;
                n.space_right = 0;
                //ノード数をインクリメント
                node_count++;
                if (add)
                    node.Left = n;
                else
                    node.Right = n;
            }
        }

        //指定した値を持つノードを取得
        public Node Search(char value)
        {
            //ルートを指定して二分探索木を探索
            return Search(root, value);
        }

        //特定のノードから指定した値を持つノードを取得
        private static Node Search(Node node, char value)
        {
            //  末端に達した場合はnullを返す。
            if (node == null)
                return null;
            //  キーがノードのキーよりも小さい場合
            //  左の子を使用して再帰呼び出し
            if (value - '0' < int.Parse(node.Value))
                return Search(node.Left, value);
            //  キーがノードのキーよりも大きい場合
            //  右の子を使用して再帰呼び出し
            if (value - '0' > int.Parse(node.Value))
                return Search(node.Right, value);
            //  どちらでもなければ、そのノードを返す
            return node;
        }

        //指定した文字列を追加
        public void Insert(string value)
        {
            //ルートが存在しない場合ルートを作成
            if (root == null)
            {
                root = new Node(value);
                root.button.Click += new EventHandler(root.button_Click);
                //ノード数をインクリメント
                node_count++;
                s_root = root;
            }
            else
                Insert(root, value);
        }

        //特定のノードから指定した値を持つノードを追加
        private void Insert(Node node, string value)
        {
            //追加する値がノードの値よりも小さい場合
            if (int.Parse(value) < int.Parse(node.Value))
            {
                //左の子が存在しない場合は挿入
                if (node.Left == null)
                {
                    Node n = new Node(value.ToString());
                    n.button.Click += new EventHandler(n.button_Click);
                    node.Left = n;
                    //ノード数をインクリメント
                    node_count++;
                }
                //再帰呼び出し
                else
                {
                    Insert(node.Left, value);
                }
            }
            //追加する値がノードの値よりも大きい場合
            if (int.Parse(value) > int.Parse(node.Value))
            {
                //右の子が存在しない場合は挿入
                if (node.Right == null)
                {
                    Node n = new Node(value);
                    n.button.Click += new EventHandler(n.button_Click);
                    n.Right = node.Right;
                    node.Right = n;
                    //ノード数をインクリメント
                    node_count++;
                }
                //再帰呼び出し
                else
                {
                    Insert(node.Right, value);
                }

            }
        }

        //データ群関数　ルートに位置・間隔情報を付加
        public void add_info()
        {
            //ルートが左の子を持っている場合
            if (root.Left != null)
            {
                add_info(root.Left);

                if (root.Left.space_right < root.Left.right)
                    root.space_left = root.Left.right + 1;
                else
                    root.space_left = root.Left.space_right + 1;
                if (root.Left.left < root.Left.right)
                    root.left = root.Left.right + 1;
                else
                    root.left = root.Left.left + 1;
            }
            //ルートが左の子を持っている場合
            if (root.Right != null)
            {
                add_info(root.Right);

                if (root.Right.space_left < root.Left.left)
                    root.space_right = root.Right.left + 1;
                else
                    root.space_right = root.Right.space_left + 1;
                if (root.Right.left < root.Right.right)
                    root.right = root.Right.right + 1;
                else
                    root.right = root.Right.left + 1;
            }
        }

        //データ群関数　ルート以外の各ノードに位置・間隔情報を付加
        private static void add_info(Node node)
        {
            //今見ているノードが左の子を持っている場合
            if (node.Left != null)
            {
                add_info(node.Left);

                if (node.Left.space_right < node.Left.right)
                    node.space_left = node.Left.right + 1;
                else
                    node.space_left = node.Left.space_right + 1;
                if (node.Left.left < node.Left.right)
                    node.left = node.Left.right + 1;
                else
                    node.left = node.Left.left + 1;
            }

            //今見ているノードが右の子を持っている場合
            if (node.Right != null)
            {
                add_info(node.Right);

                if (node.Right.space_left < node.Right.left)
                    node.space_right = node.Right.left + 1;
                else
                    node.space_right = node.Right.space_left + 1;
                if (node.Right.left < node.Right.right)
                    node.right = node.Right.right + 1;
                else
                    node.right = node.Right.left + 1;
            }
            //子を持たないノードの場合
            if (node.Left == null && node.Right == null)
            {
                node.left = 0;
                node.right = 0;
                node.space_left = 0;
                node.space_right = 0;
            }
        }

        //右のpanelに二分木を作成
        public void Draw(PictureBox pictureBox, Form form, Graphics g)
        {
            int location_x = pictureBox.Width / 2 - root.button.Width / 2;
            root.button.Location = new Point(location_x, 40);
            pictureBox.Controls.Add(root.button);
            if (root.Left != null)
            {
                Draw(pictureBox, root.Left, g, root.space_left * -1, location_x, 40);
                g.DrawLine(pen, location_x + radius / 2 + 10, 40 + radius / 2 + 10, location_x + 50 * root.space_left * -1 + radius / 2 + 10, 40 + space_y + radius / 2 + 10);
            }
            if (root.Right != null)
            {
                Draw(pictureBox, root.Right, g, root.space_right, location_x, 40);
                g.DrawLine(pen, location_x + radius / 2 + 10, 40 + radius / 2 + 10, location_x + 50 * root.space_right + radius / 2 + 10, 40 + space_y + radius / 2 + 10);
            }
        }

        //右のpanelに二分木を作成
        public static void Draw(PictureBox pictureBox, Node node, Graphics g, int space, int location_x, int location_y)
        {
            location_x += 50 * space;
            location_y += space_y;
            node.button.Location = new Point(location_x, location_y);
            pictureBox.Controls.Add(node.button);
            if (node.Left != null)
            {
                Draw(pictureBox, node.Left, g, node.space_left * -1, location_x, location_y);
                g.DrawLine(pen, location_x + radius / 2 + 10, location_y + radius / 2 + 10, location_x + 50 * node.space_left * -1 + radius / 2 + 10, location_y + space_y + radius / 2 + 10);
            }
            if (node.Right != null)
            {
                Draw(pictureBox, node.Right, g, node.space_right, location_x, location_y);
                g.DrawLine(pen, location_x + radius / 2 + 10, location_y + radius / 2 + 10, location_x + 50 * node.space_right + radius / 2 + 10, location_y + space_y + radius / 2 + 10);
            }
        }

        //preorder
        public void Preorder(ref string str)
        {
            if (root != null)
            {
                str += root.Value;
                if (root.Left != null)
                    Preorder(ref str, root.Left);
                if (root.Right != null)
                    Preorder(ref str, root.Right);
            }
        }

        private static void Preorder(ref string str, Node node)
        {
            str += ", " + node.Value;
            if (node.Left != null)
                Preorder(ref str, node.Left);
            if (node.Right != null)
                Preorder(ref str, node.Right);
        }

        //postorder
        public void Postorder(ref string str)
        {
            if (root != null)
            {
                if (root.Left != null)
                    Postorder(ref str, root.Left);
                if (root.Right != null)
                    Postorder(ref str, root.Right);
                if (str == "")
                    str += root.Value;
                else
                    str += ", " + root.Value;
            }
        }

        private static void Postorder(ref string str, Node node)
        {
            if (node.Left != null)
                Postorder(ref str, node.Left);
            if (node.Right != null)
                Postorder(ref str, node.Right);
            if (str == "")
                str += node.Value;
            else
                str += ", " + node.Value;
        }

        //inorder
        public void Inorder(ref string str)
        {
            if (root != null)
            {
                if (root.Left != null)
                    Inorder(ref str, root.Left);
                if (str == "")
                    str += root.Value;
                else
                    str += ", " + root.Value;
                if (root.Right != null)
                    Inorder(ref str, root.Right);
            }
        }

        private static void Inorder(ref string str, Node node)
        {
            if (node.Left != null)
                Inorder(ref str, node.Left);
            if (str == "")
                str += node.Value;
            else
                str += ", " + node.Value;
            if (node.Right != null)
                Inorder(ref str, node.Right);
        }

        //levelorder
        public void Levelorder(ref string str)
        {
        }

        private static void Levelorder(ref string str, Node node)
        {
        }

        //親
        private static void Parent(string value)
        {
            string str = "";
            Parent_find(ref str, value);
            form.label11.Text = str;
        }

        //根
        private static void Root()
        {
            form.label12.Text = s_root.Value;
        }

        //子
        private static void Child(string value)
        {
            string str = "";
            Child_find(ref str, value);
            form.label13.Text = str;
        }

        //兄弟
        private static void Sibling()
        {

        }

        //親を見つける
        private static void Parent_find(ref string str, string value)
        {
            if (s_root.Value == value)
                return;
            if (int.Parse(value) < int.Parse(s_root.Value))
            {
                if (s_root.Left.Value == value)
                {
                    str = s_root.Value;
                    return;
                }
                Parent_find(ref str, value, s_root.Left);
            }
            else
            {
                if (s_root.Right.Value == value)
                {
                    str = s_root.Value;
                    return;
                }
                Parent_find(ref str, value, s_root.Right);
            }
        }

        private static void Parent_find(ref string str, string value, Node node)
        {
            if (node.Value == value)
                return;
            if (int.Parse(value) < int.Parse(node.Value))
            {
                if (node.Left.Value == value)
                {
                    str = node.Value;
                    return;
                }
                Parent_find(ref str, value, node.Left);
            }
            else
            {
                if (node.Right.Value == value)
                {
                    str = node.Value;
                    return;
                }
                Parent_find(ref str, value, node.Right);
            }
        }

        //子を見つける
        private static void Child_find(ref string str, string value)
        {
            if (s_root.Value == value)
            {
                if (s_root.Left != null)
                    str += s_root.Left.Value;
                if (s_root.Right != null)
                    if(str == "")
                      str += s_root.Right.Value;
                    else
                        str += ", " + s_root.Right.Value;
                return;
            }
            if (int.Parse(value) < int.Parse(s_root.Value))
                Child_find(ref str, value, s_root.Left);
            else
                Child_find(ref str, value, s_root.Right);
        }

        private static void Child_find(ref string str, string value, Node node)
        {
            if (node.Value == value)
            {
                if (node.Left != null)
                    str += node.Left.Value;
                if (node.Right != null)
                    if(str == "")
                        str += node.Right.Value;
                    else
                        str += ", " + node.Right.Value;
                return;
            }
            if (int.Parse(value) < int.Parse(node.Value))
                Child_find(ref str, value, node.Left);
            else
                Child_find(ref str, value, node.Right);
        }
    }
}
