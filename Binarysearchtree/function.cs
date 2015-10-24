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
    public class Function
    {
        // 二分探索木のノード
        public class Node
        {
            public string Value;
            public Node Left;
            public Node Right;
            // コンストラクタ
            public Node(string value)
            {
                this.Value = value;
            }
        }

        private static Node root;

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
                        //ルートが存在しない場合ルートを作成
                        if (root == null)
                            root = new Node(text[i].ToString());
                        //左の子へ
                        Create(root, text, left, i, true);
                        //右の子へ
                        Create(root, text, i+1, text.Length, false);
                        return;
                    }
                }
        }

        //特定のノードから指定した値を持つノードを追加
        public static void Create(Node node, string text, int left, int right, bool add)
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
                        if (add)
                        {
                            node.Left = n;
                            //再帰呼び出し
                            //左の子へ
                            Create(node.Left, text, left, i, true);
                            //右の子へ
                            Create(node.Left, text, i + 1, text.Length, false);
                        }
                        else
                        {
                            node.Right = n;
                            //再帰呼び出し
                            //左の子へ
                            Create(node.Right, text, left, i, true);
                            //右の子へ
                            Create(node.Right, text, i + 1, text.Length, false);
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
                    Node n = new Node(str);
                    if (add)
                        node.Left = n;
                    else
                        node.Right = n;
                    return;
                }
            }
            if (str != "")
            {
                Node n = new Node(str);
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
                root = new Node(value);
            else
                Insert(root, value);
        }

        //特定のノードから指定した値を持つノードを追加
        private static void Insert(Node node, string value)
        {
            //追加する値がノードの値よりも小さい場合
            if (int.Parse(value) < int.Parse(node.Value))
            {
                //左の子が存在しない場合は挿入
                if (node.Left == null)
                {
                    Node n = new Node(value.ToString());
                    node.Left = n;
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
                    n.Right = node.Right;
                    node.Right = n;
                }
                //再帰呼び出し
                else
                {
                    Insert(node.Right, value);
                }

            }
        }

        public void Draw()
        {
            MessageBox.Show(root.Value.ToString());
        }
    }
}
