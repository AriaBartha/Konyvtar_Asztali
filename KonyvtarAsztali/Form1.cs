using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace KonyvtarAsztali
{
    public partial class Form1 : Form
    {
        private BookService bookService;
        public Form1()
        {
            InitializeComponent();
            dataGridViewBooks.AutoGenerateColumns = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                bookService = new BookService();
                RefreshBooksGrid();
            }
            catch(MySqlException ex)
            {
                MessageBox.Show(ex.Message, "Hiba történt az adatbáziskapcsolat kialakításakor.");
                this.Close();
            }
        }

        private void RefreshBooksGrid()
        {
            dataGridViewBooks.DataSource = bookService.GetBooks();
            dataGridViewBooks.ClearSelection();
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {   
            if (dataGridViewBooks.SelectedRows.Count == 0)
            {
                MessageBox.Show("Válassza ki a törölni kívánt elemet.");
                return;
            }
            DialogResult result = MessageBox.Show("Biztos szeretné törölni a kiválasztott könyvet?", "Biztos?", MessageBoxButtons.YesNo);
            if (result != DialogResult.Yes)
            {
                return;
            }
           
            try
            {
                Book selected = dataGridViewBooks.SelectedRows[0].DataBoundItem as Book;
                if (bookService.DeleteBook(selected.Id))
                {
                    MessageBox.Show("Sikeres törlés");
                }
                else
                {
                    MessageBox.Show("A könyv már korábban törlésre került", "Hiba történt a törlés során");
                }
                RefreshBooksGrid();
            }
            catch(MySqlException ex)
            {
                MessageBox.Show(ex.Message, "Hiba történt a törlés során");
            }

        }
    }
}
