using Petshop.DAL;
using Petshop.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Petshop.Views
{
    /// <summary>
    /// Interaction logic for pageManterAnimal.xaml
    /// </summary>
    public partial class pageManterAnimal : Page
    {

        private Animal a;

        private List<dynamic> itens = new List<dynamic>();

        public pageManterAnimal(int id = -1)
        {
            InitializeComponent();
            dtaAnimal.CanUserDeleteRows = dtaAnimal.CanUserAddRows = false;
            dtaAnimal.IsReadOnly = true;

            LimparFormulario();

            cboEspecie.ItemsSource = EspecieDAO.Listar;
            cboEspecie.DisplayMemberPath = "Nome";
            cboEspecie.SelectedValuePath = "Id";

            cboDono.ItemsSource = ClienteDAO.Listar;
            cboDono.DisplayMemberPath = "Nome";
            cboDono.SelectedValuePath = "Id";

            if(id != -1)
            {
                txtID.Text = id.ToString();
                Buscar();
            }
        }

        private void LimparFormulario()
        {
            txtID.Clear();
            txtNome.Clear();
            cboDono.SelectedIndex = -1;
            cboEspecie.SelectedIndex = -1;
            dtpData.SelectedDate = null;

            btnCadastrar.IsEnabled = btnBuscar.IsEnabled = txtID.IsEnabled = true;
            btnAlterar.IsEnabled = btnRemover.IsEnabled = false;
        }

        private bool CamposVazios()
        {
            if (String.IsNullOrWhiteSpace(txtNome.Text) || cboEspecie.SelectedIndex == -1 || dtpData.SelectedDate == null)
            {
                MessageBox.Show("Um ou mais campos vazios.", "Pet Shop", MessageBoxButton.OK, MessageBoxImage.Error);
                return true;
            }
            return false;
        }

        private void btnCadastrar_Click(object sender, RoutedEventArgs e)
        {
            if (CamposVazios())
                return;

            a = new Animal
            {
                Nome = txtNome.Text,
                DataNasc = (DateTime) dtpData.SelectedDate,
                Especie = EspecieDAO.BuscarPorID((int)cboEspecie.SelectedValue)                
            };

            if (cboDono.SelectedIndex != -1)
                a.Cliente = (Cliente)cboDono.SelectedItem;

            if (AnimalDAO.Cadastrar(a))
            {
                MessageBox.Show($"Animal \"{a.Nome}\" Cadastrato Com Sucesso.", "Pet Shop", MessageBoxButton.OK, MessageBoxImage.Information);
                LimparFormulario();
                Buscar();//Refresh
            }
            else
                MessageBox.Show("Alguma coisa deu errado na hora de salvar no banco de Dados.", "Pet Shop", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private string PegarNomeCliente(Cliente c)
        {
            if (c == null)
                return "Sem Dono";

            return c.Nome;
        }

        private void Buscar()
        {
            if (String.IsNullOrWhiteSpace(txtID.Text))
            {
                //como estava ele iria limpar o datagrid sem repopular depois passei pra k
                itens.Clear();
                foreach (Animal animal in AnimalDAO.Listar)
                {
                    itens.Add(new
                    {
                        //id para poder buscar
                        Id = animal.Id.ToString(),
                        Nome = animal.Nome,
                        Especie = animal.Especie.Nome,
                        DataNasc = animal.DataNasc,
                        Dono = PegarNomeCliente(animal.Cliente)                        
                    });
                }
                dtaAnimal.ItemsSource = itens;
                dtaAnimal.Items.Refresh();
                return;
            }
            a = AnimalDAO.BuscarPorID(Convert.ToInt32(txtID.Text));

            if (a == null)
            {
                MessageBox.Show("Animal não encontrado.", "Pet Shop", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            dtpData.SelectedDate = a.DataNasc;
            txtNome.Text = a.Nome;

            cboEspecie.SelectedIndex = cboEspecie.Items.IndexOf(a.Especie);

            if (a.Cliente != null)
                cboDono.SelectedIndex = cboDono.Items.IndexOf(a.Cliente);

            btnAlterar.IsEnabled = btnRemover.IsEnabled = true;
            btnCadastrar.IsEnabled = btnBuscar.IsEnabled = txtID.IsEnabled = false;
        }

        private void btnBuscar_Click(object sender, RoutedEventArgs e)
        {
            Buscar();
        }

        private void btnAlterar_Click(object sender, RoutedEventArgs e)
        {
            if (CamposVazios())
                return;

            a.Nome = txtNome.Text;
            a.DataNasc = (DateTime)dtpData.SelectedDate;
            a.Especie = (Especie)cboEspecie.SelectedItem;    

            if (cboDono.SelectedIndex != -1)
                a.Cliente = (Cliente)cboDono.SelectedItem;

            AnimalDAO.Alterar(a);
            MessageBox.Show($"Animal \"{a.Nome}\" Aterado Com Sucesso.", "Pet Shop", MessageBoxButton.OK, MessageBoxImage.Information);
            LimparFormulario();
            Buscar();//Refresh
        }

        private void btnRemover_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(txtID.Text))
            {
                MessageBox.Show("Campo ID Vazio.", "Pet Shop", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (AnimalDAO.Remover(a))
            {
                MessageBox.Show($"Animal \"{a.Nome}\" Removido Com Sucesso.", "Pet Shop", MessageBoxButton.OK, MessageBoxImage.Information);
                LimparFormulario();
                Buscar();//Refresh
            }
            else
                MessageBox.Show($"Animal \"{a.Nome}\" Não pode ser removido pois está em uso.", "Pet Shop", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void btnLimpar_Click(object sender, RoutedEventArgs e)
        {
            LimparFormulario();
        }

       private void cboEspecie_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cboEspecie.SelectedIndex == -1)
                return;

            lblPorte.Content = ((Especie)cboEspecie.SelectedItem).Porte.Nome;
        }

        private void dtaAnimal_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            txtID.Text = (string)((dynamic)((DataGrid)sender).CurrentItem).Id;
            Buscar();
        }
    }
}
