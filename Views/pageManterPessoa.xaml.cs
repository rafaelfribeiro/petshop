using Microsoft.EntityFrameworkCore;
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
    /// Interaction logic for pegeManterPessoa.xaml
    /// </summary>
    public partial class pageManterPessoa : Page
    {
        private bool Cliente;

        private Cliente c;

        private Funcionario f;

        //private List<Animal> Animais;

        private List<dynamic> itens = new List<dynamic>();

        //vou fazer uma listagem de pessoas, de lá vou abrir essa page passando o cpf pra ja buscar a pessoa direto
        public pageManterPessoa(bool cliente, string cpf = null)
        {
            InitializeComponent();
            //impedir o usuário de modificar datagrid
            dtaAnimais.CanUserAddRows = dtaPessoas.CanUserAddRows = dtaPessoas.CanUserDeleteRows = false;
            dtaAnimais.IsReadOnly = dtaPessoas.IsReadOnly = true;

            txtID.IsEnabled = false;
            LimparFormulario();
            Cliente = cliente;

            lblIdentificarPage.FontWeight = FontWeights.Medium;

            if (Cliente)
            {
                //Disable things
                this.lblIdentificarPage.Content = "Manter Clientes";
                this.lblMatricula.Visibility = this.txtMatricula.Visibility = Visibility.Collapsed;

                

                cboAnimais.ItemsSource = AnimalDAO.Listar;
                cboAnimais.DisplayMemberPath = "Nome";
                cboAnimais.SelectedValuePath = "Id";

                if (!String.IsNullOrEmpty(cpf))
                {
                    txtCPF.Text = cpf;
                    BuscarCliente();
                }
            }
            else
            {
                this.lblIdentificarPage.Content = "Manter Funcionários";
                this.lblAnimais.Visibility = this.wpAnimal.Visibility = this.dtaAnimais.Visibility = Visibility.Collapsed;
                this.lblTelefone.Visibility = this.txtTelefone.Visibility = Visibility.Collapsed;
                this.lblEmail.Visibility = this.txtEmail.Visibility = Visibility.Collapsed;
                this.lblEndereco.Visibility = this.txtEndereco.Visibility = Visibility.Collapsed;

                dtaPessoas.Columns.RemoveAt(2);

                if (!String.IsNullOrEmpty(cpf))
                {
                    txtCPF.Text = cpf;
                    BuscarFuncionario();
                }
            }
        }

        private void LimparFormulario()
        {
            txtCPF.Clear();
            txtNome.Clear();
            txtSobrenome.Clear();
            txtEndereco.Clear();
            txtTelefone.Clear();
            txtEmail.Clear();
            txtID.Clear();
            txtMatricula.Clear();
            //Animais.Clear();
            dtaAnimais.ItemsSource = null;
            dtpData.SelectedDate = null;
            f = null;
            //Criar para que adcicionar animais funcione sem precisar cadastrar depois buscar cliente
            c = new Cliente();
            txtCPF.Focus();

            btnAlterar.IsEnabled = btnRemover.IsEnabled = false;
            btnCadastrar.IsEnabled = btnBuscar.IsEnabled = txtCPF.IsEnabled = true;
        }

        #region Cliente
        private bool ClienteCamposVazios()
        {
            if (String.IsNullOrWhiteSpace(txtCPF.Text) || String.IsNullOrWhiteSpace(txtNome.Text) || String.IsNullOrWhiteSpace(txtSobrenome.Text) || String.IsNullOrWhiteSpace(txtEndereco.Text) || String.IsNullOrWhiteSpace(txtTelefone.Text) || String.IsNullOrWhiteSpace(txtEmail.Text) || !dtpData.SelectedDate.HasValue)
            {
                MessageBox.Show("Um ou Mais Campos Vazios.", "Pet Shop", MessageBoxButton.OK, MessageBoxImage.Error);
                return true;
            }
            return false;
        }

        private void CadastrarCliente()
        {
            if (ClienteCamposVazios())
                return;

            c.Cpf = txtCPF.Text;
            c.DataNasc = (DateTime)dtpData.SelectedDate;
            c.Email = txtEmail.Text;
            c.Nome = txtNome.Text;
            c.Sobrenome = txtSobrenome.Text;
            c.Endereco = txtEndereco.Text;
            c.Telefone = txtTelefone.Text;


            /*if (Animais != null && Animais.Count > 0)
                c.Animais = Animais;*/

            if (ClienteDAO.Cadastrar(c))
            {
                MessageBox.Show($"Cliente \"{c.Nome}\" Cadastrato Com Sucesso.", "Pet Shop", MessageBoxButton.OK, MessageBoxImage.Information);
                LimparFormulario();
                BuscarCliente();//Refresh
            }
            else
                MessageBox.Show($"CPF \"{c.Cpf}\" Já está sendo utilizado.", "Pet Shop", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        private void BuscarCliente()
        {
            if (String.IsNullOrWhiteSpace(txtCPF.Text))
            {
                itens.Clear();
                foreach (Cliente cliente in ClienteDAO.Listar)
                {
                    itens.Add(new
                    {
                        Nome = cliente.Nome,
                        Sobrenome = cliente.Sobrenome,
                        Endereco = cliente.Endereco,
                        DataNasc = cliente.DataNasc,
                        Cpf = cliente.Cpf
                    });
                }                
                dtaPessoas.ItemsSource = itens;
                dtaPessoas.Items.Refresh();
                return;
            }

            c = ClienteDAO.BuscarPorCPF(txtCPF.Text);

            if (c == null)
            {
                MessageBox.Show("Cliente Não Encontrado.", "Pet Shop", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            txtCPF.Text = c.Cpf;
            txtEmail.Text = c.Email;
            txtID.Text = c.Id.ToString();
            txtNome.Text = c.Nome;
            txtEndereco.Text = c.Endereco;
            txtSobrenome.Text = c.Sobrenome;
            txtTelefone.Text = c.Telefone;

            dtpData.SelectedDate = c.DataNasc;

            // Animais = c.Animais pegava primeira vez depois nao... provavelmente o clear limpava a lista do c.

            dtaAnimais.ItemsSource = c.Animais;
            dtaAnimais.Items.Refresh();

            btnAlterar.IsEnabled = btnRemover.IsEnabled = true;
            btnCadastrar.IsEnabled = btnBuscar.IsEnabled = txtCPF.IsEnabled = false;
        }
        private void AlterarCliente()
        {
            if (ClienteCamposVazios())
                return;


            c.DataNasc = (DateTime)dtpData.SelectedDate;
            c.Email = txtEmail.Text;
            c.Nome = txtNome.Text;
            c.Endereco = txtEndereco.Text;
            c.Sobrenome = txtSobrenome.Text;
            c.Telefone = txtTelefone.Text;

            //c.Animais = Animais;

            ClienteDAO.Alterar(c);
            MessageBox.Show($"Cliente \"{c.Nome}\" Alterado Com Sucesso.", "Pet Shop", MessageBoxButton.OK, MessageBoxImage.Information);
            LimparFormulario();
            BuscarCliente();

        }
        private void RemoverCliente()
        {
            if (String.IsNullOrWhiteSpace(txtCPF.Text))
            {
                MessageBox.Show("Campo CPF Vazio.", "Pet Shop", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            ClienteDAO.Remover(c);
            MessageBox.Show($"Cliente \"{c.Nome}\" Removido Com Sucesso.", "Pet Shop", MessageBoxButton.OK, MessageBoxImage.Information);
            LimparFormulario();
            BuscarCliente();
        }
        #endregion

        #region Funcionario
        private bool FuncionarioCamposVazios()
        {
            if (String.IsNullOrWhiteSpace(txtCPF.Text) || String.IsNullOrWhiteSpace(txtNome.Text) || String.IsNullOrWhiteSpace(txtSobrenome.Text) || String.IsNullOrWhiteSpace(txtMatricula.Text) || !dtpData.SelectedDate.HasValue)
            {
                MessageBox.Show("Um ou Mais Campos Vazios.", "Pet Shop", MessageBoxButton.OK, MessageBoxImage.Error);
                return true;
            }
            return false;
        }

        private void CadastrarFuncionario()
        {
            if (FuncionarioCamposVazios())
                return;

            f = new Funcionario
            {
                Cpf = txtCPF.Text,
                DataNasc = (DateTime)dtpData.SelectedDate,
                Nome = txtNome.Text,
                Sobrenome = txtSobrenome.Text,
                Matricula = Convert.ToInt32(txtMatricula.Text)
            };

            if (FuncionarioDAO.Cadastrar(f))
            {
                MessageBox.Show($"Funcionario \"{f.Nome} {f.Sobrenome}\" Cadastrato Com Sucesso.", "Pet Shop", MessageBoxButton.OK, MessageBoxImage.Information);
                LimparFormulario();
                BuscarFuncionario();
            }
            else
                MessageBox.Show("CPF e/ou Matricula já estão sendo utilizados.", "Pet Shop", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        private void BuscarFuncionario()
        {
            if (String.IsNullOrWhiteSpace(txtCPF.Text) && String.IsNullOrWhiteSpace(txtMatricula.Text))
            {
                itens.Clear();
                foreach (Funcionario funcionario in FuncionarioDAO.Listar)
                {
                    itens.Add(new
                    {
                        Nome = funcionario.Nome,
                        Sobrenome = funcionario.Sobrenome,
                        DataNasc = funcionario.DataNasc,
                        Cpf = funcionario.Cpf
                    });
                }
                dtaPessoas.ItemsSource = itens;
                dtaPessoas.Items.Refresh();
                return;
            }

            f = FuncionarioDAO.BuscarPorCPF(txtCPF.Text);

            if (f == null && !String.IsNullOrWhiteSpace(txtMatricula.Text))
                f = FuncionarioDAO.BuscarPorMatricula(Convert.ToInt32(txtMatricula.Text));


            if (f == null)
            {
                MessageBox.Show("Funcionario Não Encontrado.", "Pet Shop", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            txtCPF.Text = f.Cpf;
            txtID.Text = f.Id.ToString();
            txtNome.Text = f.Nome;
            txtSobrenome.Text = f.Sobrenome;
            txtMatricula.Text = f.Matricula.ToString();

            dtpData.SelectedDate = f.DataNasc;

            btnAlterar.IsEnabled = btnRemover.IsEnabled = true;
            btnCadastrar.IsEnabled = btnBuscar.IsEnabled = txtCPF.IsEnabled = false;
        }

        private void AlterarFuncionario()
        {
            if (FuncionarioCamposVazios())
                return;

            f.DataNasc = (DateTime)dtpData.SelectedDate;
            f.Matricula = Convert.ToInt32(txtMatricula.Text);
            f.Nome = txtNome.Text;
            f.Sobrenome = txtSobrenome.Text;

            FuncionarioDAO.Alterar(f);
            MessageBox.Show($"Funcionário \"{f.Nome} {f.Sobrenome}\" Alterado Com Sucesso.", "Pet Shop", MessageBoxButton.OK, MessageBoxImage.Information);
            LimparFormulario();
            BuscarFuncionario();
        }

        private void RemoverFuncionario()
        {
            if (String.IsNullOrWhiteSpace(txtCPF.Text))
            {
                MessageBox.Show("Campo CPF Vazio.", "Pet Shop", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (FuncionarioDAO.Remover(f))
            {
                MessageBox.Show($"Funcionário \"{f.Nome} {f.Sobrenome}\" Removido Com Sucesso.", "Pet Shop", MessageBoxButton.OK, MessageBoxImage.Information);
                LimparFormulario();
                BuscarFuncionario();
            }
            else
                MessageBox.Show($"Funcionário \"{f.Nome} {f.Sobrenome}\" Não pode ser removido pois está em uso.", "Pet Shop", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        #endregion

        private void btnCadastrar_Click(object sender, RoutedEventArgs e)
        {
            if (Cliente)
                CadastrarCliente();
            else
                CadastrarFuncionario();
        }

        private void btnBuscar_Click(object sender, RoutedEventArgs e)
        {

            if (Cliente)
                BuscarCliente();
            else
                BuscarFuncionario();
        }

        private void btnAlterar_Click(object sender, RoutedEventArgs e)
        {
            if (Cliente)
                AlterarCliente();
            else
                AlterarFuncionario();
        }

        private void btnRemover_Click(object sender, RoutedEventArgs e)
        {
            if (Cliente)
                RemoverCliente();
            else
                RemoverFuncionario();
        }

        private void btnLimpar_Click(object sender, RoutedEventArgs e)
        {
            LimparFormulario();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (cboAnimais.SelectedIndex == -1)
            {
                MessageBox.Show("Selecione um Animal.", "Pet Shop", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Animal a = AnimalDAO.BuscarPorID((int)cboAnimais.SelectedValue);

            if (a == null)
            {
                MessageBox.Show("Animal não Encontrado.", "Pet Shop", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            c.Animais.Add(a);
            dtaAnimais.ItemsSource = c.Animais;
            dtaAnimais.Items.Refresh();
        }

        private void dtaPessoas_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            txtCPF.Text = (String)((dynamic)((DataGrid)sender).CurrentItem).Cpf;
            btnBuscar_Click(new object(), new RoutedEventArgs());
        }
    }
}
