using Petshop.DAL;
using Petshop.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
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
    /// Interaction logic for pageManterServico.xaml
    /// </summary>
    public partial class pageManterServico : Page
    {
        private Servico s;
        private List<dynamic> itens = new List<dynamic>();
        public pageManterServico(int id = -1)
        {
            InitializeComponent();
            //Impedir usuário de mexer no datagrid
            dtaServicos.CanUserAddRows = dtaServicos.CanUserDeleteRows = false;
            dtaServicos.IsReadOnly = true;

            LimparFormulario();

            lblIdentificarPage.FontWeight = FontWeights.Medium;

            cboTipo.ItemsSource = TipoServicoDAO.Listar;
            cboTipo.DisplayMemberPath = "Nome";
            cboTipo.SelectedValuePath = "Id";

            cboDuracao.Items.Add("15 Minutos");
            cboDuracao.Items.Add("30 Minutos");
            cboDuracao.Items.Add("45 Minutos");
            cboDuracao.Items.Add("60 Minutos");

            if(id != -1)
            {
                txtID.Text = id.ToString();
                Buscar();
            }

        }

        private void LimparFormulario()
        {
            txtPreco.Clear();
            txtNome.Clear();
            txtID.Clear();
            txtDescricao.Clear();

            cboDuracao.SelectedIndex = cboTipo.SelectedIndex = -1;

            s = null;

            txtNome.Focus();

            btnAlterar.IsEnabled = btnRemover.IsEnabled = false;
            btnCadastrar.IsEnabled = btnBuscar.IsEnabled = txtID.IsEnabled = true;
        }

        private bool CamposVazios()
        {
            if (String.IsNullOrWhiteSpace(txtNome.Text) || String.IsNullOrWhiteSpace(txtDescricao.Text) || String.IsNullOrWhiteSpace(txtPreco.Text) || cboDuracao.SelectedIndex == -1 || cboTipo.SelectedIndex == -1)
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

            s = new Servico
            {
                Nome = txtNome.Text,
                Descricao = txtDescricao.Text,
                Duracao = Convert.ToInt32(((string)cboDuracao.SelectedItem).Substring(0, 2)),
                TipoServico = (TipoServico)cboTipo.SelectedItem,
                Preco = Convert.ToDouble(txtPreco.Text.Replace(',', '.'))
            };

            if (ServicoDAO.Cadastrar(s))
            {
                MessageBox.Show($"Serviço \"{s.Nome}\" Cadastrato Com Sucesso.", "Pet Shop", MessageBoxButton.OK, MessageBoxImage.Information);
                LimparFormulario();
                Buscar();//Refresh
            }
            else
                MessageBox.Show($"Algo deu errado na hora de cadastrar o Serviço.", "Pet Shop", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Buscar()
        {
            if (String.IsNullOrWhiteSpace(txtID.Text))
            {
                itens.Clear();
                //dtaServicos.Items.Clear();// limpe apenas a lista, não precisa limpar o datagrid, nem pode, da erro
                foreach (Servico servico in ServicoDAO.Listar)
                {
                    itens.Add(new
                    {
                        //id para poder buscar
                        Id = servico.Id.ToString(),
                        Nome = servico.Nome,
                        Descricao = servico.Descricao,
                        Duracao = servico.Duracao,
                        TipoDeServico = servico.TipoServico.Nome,
                        Preco = servico.Preco.ToString("C2", CultureInfo.GetCultureInfoByIetfLanguageTag("pt-BR"))
                    });
                }
                dtaServicos.ItemsSource = itens;
                dtaServicos.Items.Refresh();
                //não gosto de ficar colocando um monte de else, se for vários vai encadeando e jogando tudo pro lado...
                return;
            }

            s = ServicoDAO.BuscarPorID(Convert.ToInt32(txtID.Text));

            if (s == null)
            {
                MessageBox.Show($"Servico de ID {txtID.Text} não encontrado.", "Pet Shop", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            txtNome.Text = s.Nome;
            txtDescricao.Text = s.Descricao;
            cboDuracao.SelectedIndex = cboDuracao.Items.IndexOf(s.Duracao + " Minutos");
            cboTipo.SelectedIndex = cboTipo.Items.IndexOf(s.TipoServico);
            txtPreco.Text = s.Preco.ToString();

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


            s.Nome = txtNome.Text;
            s.Descricao = txtDescricao.Text;
            s.Duracao = Convert.ToInt32(((string)cboDuracao.SelectedItem).Substring(0, 2));
            s.TipoServico = (TipoServico)cboTipo.SelectedItem;
            s.Preco = Convert.ToDouble(txtPreco.Text.Replace(',', '.'));


            ServicoDAO.Alterar(s);
            MessageBox.Show($"Serviço \"{s.Nome}\" Alterado Com Sucesso.", "Pet Shop", MessageBoxButton.OK, MessageBoxImage.Information);
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

            if (ServicoDAO.Remover(s))
            {
                MessageBox.Show($"Servico \"{s.Nome}\" Removido Com Sucesso.", "Pet Shop", MessageBoxButton.OK, MessageBoxImage.Information);
                LimparFormulario();
                Buscar();//Refresh
            }
            else
                MessageBox.Show($"Servico \"{s.Nome}\" não pode ser removido pois está sendo usado.", "Pet Shop", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void btnLimpar_Click(object sender, RoutedEventArgs e)
        {
            LimparFormulario();
        }

        private void dtaServicos_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            txtID.Text = (string)((dynamic)((DataGrid)sender).CurrentItem).Id;
            Buscar();
        }
    }
}
