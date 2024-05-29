using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Test
{
    public partial class Form1 : Form
    {
        private int[,] adjacencyMatrix;
        private int vertexCount;
        private Point[] points;

        public Form1()
        {
            InitializeComponent();
            InitializeComboBox();
            redraw();
            this.Paint += new PaintEventHandler(DrawGraph);
        }

        private void InitializeComboBox()
        {

            comboBox1.Items.Add(3);
            comboBox1.Items.Add(4);
            comboBox1.Items.Add(5);
            comboBox1.SelectedIndex = 0;
            comboBox1.SelectedIndexChanged += new EventHandler(comboBox1_SelectedIndexChanged);
            this.Controls.Add(comboBox1);
        }

        private void Draw()
        {
            vertexCount = adjacencyMatrix.GetLength(0);
            points = new Point[vertexCount];
            this.Invalidate(); // Заставляємо форму перемалюватися
        }

        private void DrawGraph(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Pen pen = new Pen(Color.Black);
            Brush brush = new SolidBrush(Color.Blue);

            // Розраховуємо позиції вершин у колі
            int radius = 300;
            Point center = new Point(ClientSize.Width / 2, ClientSize.Height / 2);
            for (int i = 0; i < vertexCount; i++)
            {
                double angle = 2 * Math.PI * i / vertexCount;
                int x = center.X + (int)(radius * Math.Cos(angle));
                int y = center.Y + (int)(radius * Math.Sin(angle));
                points[i] = new Point(x, y);
            }

            // Малюємо ребра
            for (int i = 0; i < vertexCount; i++)
            {
                for (int j = 0; j < vertexCount; j++)
                {
                    if (adjacencyMatrix[i, j] == 1)
                    {
                        g.DrawLine(pen, points[i], points[j]);
                    }
                }
            }

            // Малюємо вершини з їх номерами
            int vertexRadius = 20;
            Font font = new Font("Arial", 10);
            StringFormat stringFormat = new StringFormat();
            stringFormat.Alignment = StringAlignment.Center;
            stringFormat.LineAlignment = StringAlignment.Center;
            for (int i = 0; i < vertexCount; i++)
            {
                g.FillEllipse(brush, points[i].X - vertexRadius / 2, points[i].Y - vertexRadius / 2, vertexRadius, vertexRadius);
                g.DrawString((i + 1).ToString(), font, Brushes.White, points[i], stringFormat);
            }
        }


        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            redraw();
        }

        private void redraw()
        {
            switch (comboBox1.SelectedIndex)
            {
                case 0:
                    adjacencyMatrix = new int[,]
                    {
                        { 0, 1, 0, 1, 1, 0 },
                        { 1, 0, 0, 1, 0, 1 },
                        { 0, 0, 0, 0, 0, 0 },
                        { 1, 1, 0, 0, 0, 1 },
                        { 1, 0, 0, 0, 0, 1 },
                        { 0, 1, 0, 1, 1, 0 }
                    };
                    break;
                case 1:
                    adjacencyMatrix = new int[,]
                    {
                        { 0, 1, 1, 1, 1, 0 },
                        { 1, 0, 0, 1, 1, 0 },
                        { 1, 0, 0, 1, 0, 0 },
                        { 1, 1, 1, 0, 1, 1 },
                        { 1, 1, 0, 1, 0, 1 },
                        { 0, 0, 0, 1, 1, 0 }
                    };
                    break;
                case 2:
                    adjacencyMatrix = new int[,]
                    {
                        { 0, 1, 0, 0, 1, 1, 0, 1 },
                        { 1, 0, 1, 0, 0, 1, 0, 0 },
                        { 0, 1, 0, 1, 0, 0, 1, 0 },
                        { 0, 0, 1, 0, 1, 0, 0, 0 },
                        { 1, 0, 0, 1, 0, 1, 0, 1 },
                        { 1, 1, 0, 0, 1, 0, 1, 0 },
                        { 0, 0, 1, 0, 0, 1, 0, 1 },
                        { 1, 0, 0, 0, 1, 0, 1, 0 }
                    };
                    break;
            }
            Draw();
        }


        private void BFS(int startVertex)
        {
            // Створення масиву для відстеження відвіданих вершин
            bool[] visited = new bool[vertexCount];
            // Створення черги для обробки вершин у процесі обходу
            Queue<int> queue = new Queue<int>();
            // Рядок для збереження журналу обходу
            StringBuilder log = new StringBuilder();

            // Позначаємо початкову вершину як відвідану та додаємо її до черги
            visited[startVertex] = true;
            queue.Enqueue(startVertex);

            log.AppendLine("BFS traversal starting from vertex " + (startVertex + 1) + ":");

            while (queue.Count > 0)
            {
                // Вибираємо вершину з черги
                int currentVertex = queue.Dequeue();
                // Додаємо поточну вершину до журналу обходу
                log.AppendLine("Visited vertex: " + (currentVertex + 1));

                // Отримуємо всі сусідні вершини поточної вершини
                for (int i = 0; i < vertexCount; i++)
                {
                    // Перевіряємо, чи існує ребро між поточною вершиною та вершиною i, і чи вона не відвідана
                    if (adjacencyMatrix[currentVertex, i] == 1 && !visited[i])
                    {
                        // Позначаємо вершину як відвідану та додаємо її до черги
                        visited[i] = true;
                        queue.Enqueue(i);
                    }
                }
            }

            // Виводимо журнал обходу у повідомленні
            MessageBox.Show(log.ToString());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            BFS(0);
        }
    }
}
