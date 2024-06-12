using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;

namespace WindowsFormsApplication7
{
    public partial class Form1 : Form
    {
        int cR, cG, cB;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "archivos jpg|*.jpg";
            openFileDialog1.ShowDialog();
            Bitmap bmp = new Bitmap(openFileDialog1.FileName);
            pictureBox1.Image = bmp;
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            Bitmap bmp = new Bitmap(pictureBox1.Image);
            Color c = new Color();
            int sR, sG, sB;
            sR = 0;
            sG = 0;
            sB = 0;
            for (int i = e.X; i < e.X + 10; i++)
                for (int j = e.Y; j < e.Y + 10; j++)
                {
                    c = bmp.GetPixel(i, j);
                    sR = sR + c.R;
                    sG = sG + c.G;
                    sB = sB + c.B;
                }
            sR = sR / 100;
            sG = sG / 100;
            sB = sB / 100;
            cR = sR;
            cG = sG;
            cB = sB;
            textBox1.Text = sR.ToString();
            textBox2.Text = sG.ToString();
            textBox3.Text = sB.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Bitmap bmp = new Bitmap(pictureBox1.Image);
            Bitmap bmp2 = new Bitmap(bmp.Width, bmp.Height);
            Color c = new Color();
            for (int i = 0; i < bmp.Width; i++)
                for (int j = 0; j < bmp.Height; j++)
                {
                    c = bmp.GetPixel(i, j);
                    if (((74 <= c.R) && (c.R <= 104)) && ((84 <= c.G) && (c.G <= 114)) && ((74 <= c.B) && (c.B <= 104)))
                        bmp2.SetPixel(i, j, Color.Black);
                    else
                        bmp2.SetPixel(i, j, Color.FromArgb(c.R, c.G, c.B));
                }
            pictureBox1.Image = bmp2;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Bitmap bmp = new Bitmap(pictureBox1.Image);
            Bitmap bmp2 = new Bitmap(bmp.Width, bmp.Height);
            Color c = new Color();
            int sR, sG, sB;
            for (int i = 0; i < bmp.Width - 10; i = i + 10)
                for (int j = 0; j < bmp.Height - 10; j = j + 10)
                {
                    sR = 0; sG = 0; sB = 0;
                    for (int ip = i; ip < i + 10; ip++)
                        for (int jp = j; jp < j + 10; jp++)
                        {
                            c = bmp.GetPixel(ip, jp);
                            sR = sR + c.R;
                            sG = sG + c.G;
                            sB = sB + c.B;
                        }
                    sR = sR / 100;
                    sG = sG / 100;
                    sB = sB / 100;

                    if (((cR - 20 <= sR) && (sR <= cR + 20)) && ((cG - 20 <= sG) && (sG <= cG + 20)) && ((cB - 20 <= sB) && (sB <= cB + 20)))
                    {
                        for (int ip = i; ip < i + 10; ip++)
                            for (int jp = j; jp < j + 10; jp++)
                            {
                                bmp2.SetPixel(ip, jp, Color.Black);
                            }
                    }
                    else
                    {
                        for (int ip = i; ip < i + 10; ip++)
                            for (int jp = j; jp < j + 10; jp++)
                            {
                                c = bmp.GetPixel(ip, jp);
                                bmp2.SetPixel(ip, jp, Color.FromArgb(c.R, c.G, c.B));
                            }
                    }

                }
            pictureBox1.Image = bmp2;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            // Establecer la cadena de conexión
            string connectionString = "Data Source=DESKTOP-1JHK3TK;Initial Catalog=Imagen;Integrated Security=True";

            // Crear la conexión y el comando
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    // Establecer la conexión y el comando
                    cmd.Connection = con;
                    con.Open();

                    // Construir la consulta SQL con parámetros para evitar la inyección SQL
                    cmd.CommandText = "INSERT INTO texturas(descripcion, cR, cG, cB, colorpintar) VALUES (@descripcion, @cR, @cG, @cB, @colorpintar)";
                    cmd.Parameters.AddWithValue("@descripcion", textBox4.Text);
                    cmd.Parameters.AddWithValue("@cR", int.Parse(textBox1.Text));
                    cmd.Parameters.AddWithValue("@cG", int.Parse(textBox2.Text));
                    cmd.Parameters.AddWithValue("@cB", int.Parse(textBox3.Text));
                    cmd.Parameters.AddWithValue("@colorpintar", textBox5.Text);

                    // Ejecutar la consulta
                    cmd.ExecuteNonQuery();
                }
            }

            // Mostrar los datos actualizados en el DataGridView
            mostrar();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            listarNuevasImagenes();
        }

        private void listarNuevasImagenes()
        {
            // Establecer la cadena de conexión para SQL Server
            string connectionString = "Data Source=DESKTOP-1JHK3TK;Initial Catalog=Imagen;Integrated Security=True";

            // Crear la conexión y el comando
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    // Establecer la conexión y el comando
                    cmd.Connection = con;
                    con.Open();

                    // Consulta SQL para obtener las nuevas imágenes
                    cmd.CommandText = "SELECT descripcion, cR, cG, cB, colorpintar FROM texturas ORDER BY id DESC";

                    // Ejecutar la consulta y leer los resultados
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        StringBuilder listado = new StringBuilder();
                        while (reader.Read())
                        {
                            listado.AppendFormat("Descripción: {0}, cR: {1}, cG: {2}, cB: {3}, Color a Pintar: {4}\n",
                                reader["descripcion"], reader["cR"], reader["cG"], reader["cB"], reader["colorpintar"]);
                        }

                        // Mostrar el listado (por ejemplo, en un MessageBox o en un TextBox)
                        MessageBox.Show(listado.ToString(), "Listado de Nuevas Imágenes");
                    }
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            mostrar();
        }

        private void mostrar()
        {
            // Establecer la cadena de conexión
            string connectionString = "Data Source=DESKTOP-1JHK3TK;Initial Catalog=Imagen;Integrated Security=True";

            // Crear la conexión y el adaptador de datos
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlDataAdapter ada = new SqlDataAdapter())
                {
                    // Establecer la conexión y el comando
                    ada.SelectCommand = new SqlCommand("SELECT * FROM texturas", con);
                    DataSet ds = new DataSet();

                    // Abrir la conexión, llenar el DataSet y asignarlo al DataGridView
                    con.Open();
                    ada.Fill(ds);
                    dataGridView1.DataSource = ds.Tables[0];
                }
            }
        }

    }
}
