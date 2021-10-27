using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace EsempioSchema
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public SqlConnection Conn = new SqlConnection("Data Source=DESKTOP-0JNBS50;Initial Catalog=Tabella;Integrated Security=True");
        //public SqlConnection Conn = new SqlConnection(@"Data Source=INFORMATICA216\INFORMATICA225;Initial Catalog=Auto;Integrated Security=True");
        public SqlCommand cmd;
        public SqlDataReader reader;
        public Tabella Tabella = new Tabella();

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Conn.Open();
                MessageBox.Show("La connessione stata aperta", "", MessageBoxButtons.OK);
            }
            catch (SqlException error)
            {
                MessageBox.Show("Errore: " + error.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            //per controllare se la tabella è stata creata 
            string cont = @"SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'LISTA';";
            cmd = new SqlCommand(cont, Conn);

            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows == false)
            {
                reader.Close();
                cmd.Cancel();
                string creaz = @"CREATE TABLE LISTA(ID int IDENTITY(1, 1) NOT NULL, Nome_Tabella nvarchar(100) NOT NULL);";
                cmd = new SqlCommand(creaz, Conn);
                try
                {
                    cmd.ExecuteNonQuery();  //esegue il comando di creare la tabella, inserisce dati nella tabella
                    MessageBox.Show("Tabella creata.", "", MessageBoxButtons.OK);
                }
                catch (SqlException error)
                {
                    MessageBox.Show("Mi spiace, hai commesso un errore, mi spiego meglio: " + error.ToString());
                }
            }
        }
        private void viewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Conn.State==ConnectionState.Closed)
            {
                Conn.Open();
            }

            cmd = new SqlCommand(@"SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES", Conn);
            SqlDataReader reader = cmd.ExecuteReader();
            List<AAAAAAA> lista = new List<AAAAAAA>();
            
            while (reader.Read())
            {
                lista.Add(new AAAAAAA { Kokko = (string)reader["TABLE_NAME"] });
                dataGridView1.Rows.Add((string)reader["TABLE_NAME"]);
                Tabella.NomeTab = reader.ToString();  
            }
            reader.Close();
            cmd.Cancel();
            Inserimento(lista);
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Conn.State == ConnectionState.Closed)
            {
                MessageBox.Show("La connessione è già chiusa", "", MessageBoxButtons.OK);
            }
            else
            {
                Conn.Close();
                MessageBox.Show("La connessione è stata chiusa correttamente", "", MessageBoxButtons.OK);
            }
        }
        public void Inserimento(List<AAAAAAA> sussybaka)
        {
            string valori = String.Join(",", sussybaka);
            
            string query = $"INSERT INTO dbo.LISTA (Nome_Tabella)VALUES{valori};";
            SqlCommand cmd = new SqlCommand(query, Conn);
            if (Conn.State != ConnectionState.Open)
            {
                Conn.Open();
            }
            cmd.ExecuteNonQuery();
            Conn.Close();
        }
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }

    public class Tabella
    {
        public string NomeTab { get; set; }    
    }

    public class AAAAAAA
    {
        public string Kokko { get; set; }
        public override string ToString()
        {
            string jiji = $"('{Kokko}')";
            return jiji;
        }
    }
}
