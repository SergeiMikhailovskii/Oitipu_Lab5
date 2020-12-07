using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace Lab4
{
    public partial class Form1 : Form
    {
        private DataTable dataTable;
        private MySqlConnection connection;
        private MySqlCommandBuilder mySqlCommandBuilder;
        private MySqlDataAdapter mySqlDataAdapter;
        private string query;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string connectionString = "server=localhost;user id=Sergei;password=12345;persistsecurityinfo=True;database=lab4";

            // костыль, чтобы после сохранения отобразить идшники
            connection = new MySqlConnection(connectionString);
            connection.Open();

            query = "SELECT * FROM Material";

            mySqlDataAdapter = new MySqlDataAdapter(query, connection);
            mySqlCommandBuilder = new MySqlCommandBuilder(mySqlDataAdapter);


            dataTable = new DataTable();
            mySqlDataAdapter.Fill(dataTable);

            dataGridView1.DataSource = dataTable;
            dataGridView1.AutoGenerateColumns = true;
            dataGridView1.Columns["id"].ReadOnly = true;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            mySqlDataAdapter.Update(dataTable);

            mySqlDataAdapter = new MySqlDataAdapter(query, connection);
            mySqlCommandBuilder = new MySqlCommandBuilder(mySqlDataAdapter);

            dataTable = new DataTable();
            mySqlDataAdapter.Fill(dataTable);
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            dataTable.RejectChanges();
            dataGridView1.DataSource = dataTable;
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            int selectedRow = dataGridView1.CurrentCell.RowIndex;
            dataTable.Rows[selectedRow].Delete();
            dataTable.AcceptChanges();
        }

        private void dataGridView1_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.ColumnIndex == dataGridView1.Columns["dateShipment"].Index)
            {
                dataGridView1.Rows[e.RowIndex].ErrorText = "";

                if (dataGridView1.Rows[e.RowIndex].IsNewRow) { return; }
                if (!(new Regex("[0-3][0-9].[0-1][0-9].20[0-2][0-9] [0-2][0-9]:[0-5][0-9]").Match(e.FormattedValue.ToString()).Success))
                {
                    e.Cancel = true;
                    MessageBox.Show("Invalid date");
                }
            }
        }
    }
}
